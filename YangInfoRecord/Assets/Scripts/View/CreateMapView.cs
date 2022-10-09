using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapView : MonoBehaviour
{
    public Transform center;
    public GameObject item;
    private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    public MapViewSetting mapViewSetting;
    private bool inited = false;
    public void Open()
    {
        if(!inited)
        {
            CreateMap();
        }
        //注册监听事件
    }
    public void Close()
    {
        //注销监听事件
    }
    public void CreateMap()
    {
        Vector3 size = MapController.Instance.GetMapSize();
        KindInfo kindInfo = MapController.Instance.GetKindInfo();
        if (mapViewSetting.showWhiteBlock)
        {
            for(int i = 0; i < size.z; i++)
            {
                for(int j = 0; j < size.y; j++)
                {
                    for(int k = 0; k < size.x; k++)
                    {
                        GameObject item = CreateOneBlock();
                        PutBlockOnCoord(item, i, new Vector2(k, j));
                    }
                }
            }
        }
        foreach(var item in MapController.Instance.GetMapInfo())
        {
            foreach(var it in item.Value)
            {
                ChangeOneBlockTexture(kindInfo.info[it.kind], item.Key, it.coord);
            }
        }
    }
    public void RefreshMap()
    {
        if (mapViewSetting.showWhiteBlock)
        {
            for (int i = 0; i < size.z; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    for (int k = 0; k < size.x; k++)
                    {
                        GameObject item = CreateOneBlock();
                        PutBlockOnCoord(item, i, new Vector2(k, j));
                    }
                }
            }
        }
        foreach (var item in MapController.Instance.GetMapInfo())
        {
            foreach (var it in item.Value)
            {
                ChangeOneBlockTexture(kindInfo.info[it.kind], item.Key, it.coord);
            }
        }
    }
    public void PreloadTextrue()
    {
        foreach(var item in MapController.Instance.GetKindInfo().info)
        {
            textures.Add(item.Value, Resources.Load<Texture>(item.Value));
        }
    }
    public GameObject CreateOneBlock(string textureName)
    {
        GameObject obj = GameObject.Instantiate(item);
        obj.transform.SetParent(center);
        obj.GetComponent<MeshRenderer>().material = new Material(item.GetComponent<MeshRenderer>().material);
        obj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[textureName]);
        return obj;
    }
    public GameObject CreateOneBlock()
    {
        GameObject obj = GameObject.Instantiate(item);
        obj.transform.SetParent(center);
        obj.GetComponent<MeshRenderer>().material = new Material(item.GetComponent<MeshRenderer>().material);
        return obj;
    }
    public bool ChangeOneBlockTexture(string textureName, int layer, Vector2 coord)
    {
        string key = (new Vector3(coord.x, coord.y, layer)).ToString();
        if (!items.ContainsKey(key))
        {
            return false;
        }
        else
        {
            GameObject obj = items[key];
            obj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[textureName]);
            return true;
        }
    }
    public void PutBlockOnCoord(GameObject block, int layer, Vector2 coord)
    {
        string key = (new Vector3(coord.x, coord.y, layer)).ToString();
        if (!items.ContainsKey(key))
        {
            items.Add(key, block);
        }
        Vector3 size = MapController.Instance.GetMapSize();
        float z = mapViewSetting.interval.z * (layer - (size.z - 1) / 2);
        float x = (mapViewSetting.interval.x + mapViewSetting.oneBlockSize) * (coord.x - (size.x - 1) / 2);
        float y = (mapViewSetting.interval.y + mapViewSetting.oneBlockSize) * (coord.y - (size.y - 1) / 2);
        block.transform.localPosition = new Vector3(x, y, z);
    }
}
