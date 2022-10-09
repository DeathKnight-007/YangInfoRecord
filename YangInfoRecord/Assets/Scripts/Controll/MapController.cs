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
        return mapDataModel.mapData.size;
    }
    public KindInfo GetKindInfo()
    {
        return mapDataModel.kindInfo;
    }
    public Dictionary<int, List<MapBlockData>> GetMapInfo()
    {
        return mapDataModel.mapData.info;
    }
    public void SaveData()
    {
        mapDataModel.SaveMapData();
    }
}
