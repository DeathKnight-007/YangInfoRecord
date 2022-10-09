using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class MapDataModel
{
    public MapDataModel()
    {
        this.mapPath = Application.persistentDataPath + storePath + "map";
        this.kindPath = "kind.asset";
        this.ReadData();
    }
    public const string storePath = "/Data/";
    private string mapPath;
    private string kindPath;
    public MapData mapData;
    public KindInfo kindInfo;
    public void ReadData()
    {
        if (File.Exists(mapPath))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Create(mapPath);
                mapData = bf.Deserialize(fs) as MapData;
                mapData.ListToDic();
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("地图数据读取失败: " + e.ToString());
                mapData = new MapData();
            }
        }
        else
        {
            mapData = new MapData();
        }
        try
        {
            kindInfo = Resources.Load<KindInfo>(kindPath);
            kindInfo.ListToDic();
        }
        catch (Exception e)
        {
            Debug.LogError("地图块对应图片信息数据读取失败: " + e.ToString());
            kindInfo = new KindInfo();
        }
    }
    public bool SaveMapData()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Create(mapPath);
            mapData.DicToList();
            bf.Serialize(fs, mapData);
            fs.Close();
        }catch(Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
        Event.BroadcastEvent(ModelEvents.SaveDataSuccess, "map");
        return true;
    }
    public void AddBlock(MapBlockData block)
    {
        if (!mapData.info.ContainsKey(block.layer))
        {
            List<MapBlockData> blocks = new List<MapBlockData>();
            blocks.Add(block);
            mapData.info.Add(block.layer, blocks);
        }
        else
        {
            // 检查是否已经存在这个块,存在则不执行
            foreach (var item in mapData.info[block.layer])
            {
                if (item.coord.x == block.coord.x && item.coord.y == block.coord.y)
                {
                    return;
                }
            }
            mapData.info[block.layer].Add(block);
        }
        Event.BroadcastEvent(ModelEvents.AddBlock, block);
    }

    public void RemoveBlock(int layer, Vector2 coord)
    {
        if (mapData.info.ContainsKey(layer))
        {
            foreach (var item in mapData.info[layer])
            {
                if (item.coord.x == coord.x && item.coord.y == coord.y)
                {
                    mapData.info[layer].Remove(item);
                    Event.BroadcastEvent(ModelEvents.RemoveBlock, layer, coord);
                    break;
                }
            }
        }
    }

    public void ChangeBlockState(int layer, Vector2 coord, int state)
    {
        int index = -1;
        if (mapData.info.ContainsKey(layer))
        {
            foreach (var item in mapData.info[layer])
            {
                index++;
                if (item.coord.x == coord.x && item.coord.y == coord.y)
                {
                    mapData.info[layer][index].state = state;
                    Event.BroadcastEvent(ModelEvents.ChangeBlockState, layer, coord, state);
                    break;
                }
            }
        }
    }
}
