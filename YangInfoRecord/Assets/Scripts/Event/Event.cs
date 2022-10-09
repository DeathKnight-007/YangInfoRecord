using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Event
{
    public static Dictionary<string, List<Action<List<object>>>> events = new Dictionary<string, List<Action<List<object>>>>();
    public static void AddEvent(string name, Action<List<object>> callBack)
    {
        if (!events.ContainsKey(name))
        {
            List<Action<List<object>>> items = new List<Action<List<object>>>();
            items.Add(callBack);
            events.Add(name, items);
        }
        else
        {
            events[name].Add(callBack);
        }
    }
    public static void RemoveEvent(string name)
    {
        if (events.ContainsKey(name))
        {
            events.Remove(name);
        }
    }
    public static void RemoveEvent(string name, Action<List<object>> callBack)
    {
        if (events.ContainsKey(name))
        {
            events[name].Remove(callBack);
        }
    }
    public static void BroadcastEvent(string name, params object[] objs)
    {
        if (events.ContainsKey(name))
        {
           foreach(var item in events[name])
            {
                List<object> para = new List<object>();
                foreach(var i in objs)
                {
                    para.Add(i);
                }
                item(para);
            }
        }
    }
}
