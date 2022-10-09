using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 块种类对应的信息
/// </summary>
[Serializable]
public class KindInfo : ScriptableObject
{
    public KindInfo()
    {
        info = new Dictionary<int, string>();
    }
    [SerializeField]
    private List<int> kind;
    [SerializeField]
    private List<string> textureName;
    public Dictionary<int, string> info;
    public void DicToList()
    {
        kind = new List<int>();
        textureName = new List<string>();
        foreach (var item in info)
        {
            kind.Add(item.Key);
            textureName.Add(item.Value);
        }
    }
    public void ListToDic()
    {
        info = new Dictionary<int, string>();
        for (int i = 0; i < kind.Count; i++)
        {
            info.Add(kind[i], textureName[i]);
        }
    }
}
