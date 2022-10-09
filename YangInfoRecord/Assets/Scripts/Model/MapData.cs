﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    public MapData()
    {
        info = new Dictionary<int, List<MapBlockData>>();
    }
    public Vector3 size;
    [SerializeField]
    private List<int> layers;
    [SerializeField]
    private List<List<MapBlockData>> layersInfo;
    public Dictionary<int, List<MapBlockData>> info;
    public void DicToList()
    {
        layers = new List<int>();
        layersInfo = new List<List<MapBlockData>>();
        foreach(var item in info)
        {
            layers.Add(item.Key);
            layersInfo.Add(item.Value);
        }
    }
    public void ListToDic()
    {
        info = new Dictionary<int, List<MapBlockData>>();
        for(int i = 0; i < layers.Count; i++)
        {
            info.Add(layers[i], layersInfo[i]);
        }
    }
}

public class MapBlockData : ScriptableObject
{
    public int layer; // 在第几层
    public Vector2 coord; // 在层面的坐标
    public int kind; // 种类
    public int state; // 0 在地图上 1 在下面仓库里 2 被消除了
}
