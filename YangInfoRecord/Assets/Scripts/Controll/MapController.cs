using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController
{
    private static MapController instance;
    public static MapController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MapController();
                instance.mapDataModel = new MapDataModel();
            }
            return instance;
        }
    }
    private MapDataModel mapDataModel;

    public void AddBlock(MapBlockData block)
    {
        mapDataModel.AddBlock(block);
    }
    public void RemoveBlock(int layer, Vector2 coord)
    {
        mapDataModel.RemoveBlock(layer, coord);
    }
    public void ChangeBlockState(int layer, Vector2 coord, int state)
    {
        mapDataModel.ChangeBlockState(layer, coord, state);
    }
    public void ChangeBlockState(MapBlockData block)
    {
        mapDataModel.ChangeBlockState(block.layer, block.coord, block.state);
    }
    public Vector3 GetMapSize()
    {
        return mapDataModel.currentMapData.size;
    }
    public KindInfo GetKindInfo()
    {
        return mapDataModel.kindInfo;
    }
    public Dictionary<int, List<MapBlockData>> GetMapInfo()
    {
        return mapDataModel.currentMapData.info;
    }
    public MapBlockData GetMapBlockInfo(int layer, Vector2 coord)
    {
        foreach(var item in mapDataModel.currentMapData.info[layer])
        {
            if (item.coord == coord)
            {
                return item;
            }
        }
        return null;
    }
    public void SaveData()
    {
        mapDataModel.SaveMapData();
    }
    public void CreateOneMap(string name)
    {
        mapDataModel.CreateOneMap(name);
    }
    public void SetCurrentMap(MapData mapData)
    {
        mapDataModel.currentMapData = mapData;
    }
}
