using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapViewSetting
{
    private Vector3 _interval;
    /// <summary>
    /// block之间的间距 x 代表横向间距 y 代表竖向间距 z 代表高向间距
    /// </summary>
    public Vector3 interval {
        get
        {
            return _interval;
        }
        set
        {
            _interval = value;
            Event.BroadcastEvent(MapViewEvents.IntervalChange, value);
        }
    }
    private bool _showWhiteBlock;
    /// <summary>
    /// 是否显示没有数据的白块
    /// </summary>
    public bool showWhiteBlock
    {
        get
        {
            return _showWhiteBlock;
        }
        set
        {
            _showWhiteBlock = value;
            Event.BroadcastEvent(MapViewEvents.showWhiteBlockChange, value);
        }
    }
    private float _oneBlockSize;
    /// <summary>
    /// 一个方块边长
    /// </summary>
    public float oneBlockSize
    {
        get
        {
            return _oneBlockSize;
        }
        set
        {
            _oneBlockSize = value;
            Event.BroadcastEvent(MapViewEvents.BlockSizeChange, value);
        }
    }
}
