using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateMapView : MonoBehaviour
{
    public Transform center;
    public Transform pool;
    public HorizontalLayoutGroup caoGroup;
    public GameObject item;
    public Transform[] caos;
    private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> collectedItems = new Dictionary<string, GameObject>();
    private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    private Dictionary<GameObject, MapBlockData> itemsInCao = new Dictionary<GameObject, MapBlockData>();
    public MapViewSetting mapViewSetting;
    private bool inited = false;
    private Action<List<object>> addBlockCallBack;
    private Action<List<object>> removeBlockCallBack;
    private Action<List<object>> changeBlockStateCallBack;
    private Action<List<object>> sizeCallBack;
    private Action<List<object>> blockSizeChangeCallBack;
    private Action<List<object>> intervalChangeCallBack;
    private Action<List<object>> showWhiteBlockChangeCallBack;

    public void Open()
    {
        this.gameObject.SetActive(true);
        if (!inited)
        {
            PreloadTextrue();
            CreateMap();
        }
        else
        {
            RefreshMap();
        }
        KindInfo kindInfo = MapController.Instance.GetKindInfo();
        //注册监听事件
        addBlockCallBack = (objs) =>
        {
            MapBlockData block = objs[0] as MapBlockData;
            ChangeOneBlockTexture(kindInfo.info[block.kind], block.layer, block.coord);
        };
        Event.AddEvent(ModelEvents.AddBlock, addBlockCallBack);
        removeBlockCallBack = (objs) =>
        {
            int layer = (int)objs[0];
            Vector2 coord = (Vector2)objs[1];
            ChangeOneBlockTexture(string.Empty, layer, coord);
        };
        Event.AddEvent(ModelEvents.RemoveBlock, removeBlockCallBack);
        changeBlockStateCallBack = (objs) =>
        {
            int layer = (int)objs[0];
            Vector2 coord = (Vector2)objs[1];
            int state = (int)objs[2];
            if (state == 0)
            {
                PutBlockOnCoord(items[(new Vector3(coord.x, coord.y, layer)).ToString()], layer, coord);
            }
            if (state == 1)
            {
                PutBlockOnCao(layer, coord);
            }
            if (state == 2)
            {
                CollectBlock(layer, coord);
            }
        };
        Event.AddEvent(ModelEvents.ChangeBlockState, changeBlockStateCallBack);
        changeBlockStateCallBack = (objs) =>
        {
            RefreshMap();
        };
        Event.AddEvent(ModelEvents.SizeChange, sizeCallBack);
        //
        blockSizeChangeCallBack = (objs) => { RefreshMap(); };
        Event.AddEvent(MapViewEvents.BlockSizeChange, blockSizeChangeCallBack);
        intervalChangeCallBack = (objs) => { RefreshMap(); };
        Event.AddEvent(MapViewEvents.IntervalChange, intervalChangeCallBack);
        showWhiteBlockChangeCallBack = (objs) => { RefreshShowWhiteBlock(); };
        Event.AddEvent(MapViewEvents.showWhiteBlockChange, showWhiteBlockChangeCallBack);
    }
    public void Close()
    {
        //注销监听事件
        Event.RemoveEvent(ModelEvents.AddBlock, addBlockCallBack);
        Event.RemoveEvent(ModelEvents.RemoveBlock, removeBlockCallBack);
        Event.RemoveEvent(ModelEvents.ChangeBlockState, changeBlockStateCallBack);
        Event.RemoveEvent(ModelEvents.SizeChange, sizeCallBack);
        Event.RemoveEvent(MapViewEvents.BlockSizeChange, blockSizeChangeCallBack);
        Event.RemoveEvent(MapViewEvents.IntervalChange, intervalChangeCallBack);
        Event.RemoveEvent(MapViewEvents.showWhiteBlockChange, showWhiteBlockChangeCallBack);
        //
        this.gameObject.SetActive(false);
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
                if (!items.ContainsKey((new Vector3(it.coord.x, it.coord.y, it.layer)).ToString()))
                {
                    GameObject obj = CreateOneBlock();
                    PutBlockOnCoord(obj, it.layer, it.coord);
                }
                ChangeOneBlockTexture(kindInfo.info[it.kind], item.Key, it.coord);
                if (it.state == 0)
                {
                    PutBlockOnCoord(items[(new Vector3(it.coord.x, it.coord.y, it.layer)).ToString()], it.layer, it.coord);
                }
                if(it.state == 1)
                {
                    PutBlockOnCao(it.layer, it.coord);
                }
                if (it.state == 2)
                {
                    CollectBlock(it.layer, it.coord);
                }
            }
        }
        RefreshCao();
    }
    public void RefreshMap()
    {
        KindInfo kindInfo = MapController.Instance.GetKindInfo();
        foreach (var item in MapController.Instance.GetMapInfo())
        {
            foreach (var it in item.Value)
            {
                if (!items.ContainsKey((new Vector3(it.coord.x, it.coord.y, it.layer)).ToString()))
                {
                    GameObject obj = CreateOneBlock();
                    PutBlockOnCoord(obj, it.layer, it.coord);
                }
                ChangeOneBlockTexture(kindInfo.info[it.kind], item.Key, it.coord);
                if (it.state == 0)
                {
                    PutBlockOnCoord(items[(new Vector3(it.coord.x, it.coord.y, it.layer)).ToString()], it.layer, it.coord);
                }
                if (it.state == 1)
                {
                    PutBlockOnCao(it.layer, it.coord);
                }
                if (it.state == 2)
                {
                    CollectBlock(it.layer, it.coord);
                }
            }
        }
        RefreshCao();
        RefreshShowWhiteBlock();
    }
    public void RefreshCao()
    {
        foreach (var item in caos)
        {
            item.GetComponent<RectTransform>().sizeDelta = Vector2.one * mapViewSetting.oneBlockSize;
            if(item.GetChild(0) != null)
            {
                item.GetChild(0).localScale = Vector3.one * mapViewSetting.oneBlockSize;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(caoGroup.GetComponent<RectTransform>());
    }
    public void RefreshShowWhiteBlock()
    {
        Vector3 size = MapController.Instance.GetMapSize();
        KindInfo kindInfo = MapController.Instance.GetKindInfo();
        if (mapViewSetting.showWhiteBlock)
        {
            for (int i = 0; i < size.z; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    for (int k = 0; k < size.x; k++)
                    {
                        string key = (new Vector3(k, j, i)).ToString();
                        if (!items.ContainsKey(key))
                        {
                            GameObject item = CreateOneBlock();
                            PutBlockOnCoord(item, i, new Vector2(k, j));
                        }
                        else
                        {
                            items[key].SetActive(true);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < size.z; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    for (int k = 0; k < size.x; k++)
                    {
                        string key = (new Vector3(k, j, i)).ToString();
                        if (items.ContainsKey(key))
                        {
                            items[key].SetActive(false);
                        }
                    }
                }
            }
            foreach (var item in MapController.Instance.GetMapInfo())
            {
                foreach (var it in item.Value)
                {
                    string key = (new Vector3(it.coord.x, it.coord.y, it.layer)).ToString();
                    items[key].SetActive(true);
                }
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
            if (textureName == string.Empty)
            {
                obj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", null);
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[textureName]);
            }
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
    public void PutBlockOnCao(int layer, Vector2 coord)
    {
        string key = (new Vector3(coord.x, coord.y, layer)).ToString();
        if (!items.ContainsKey(key))
        {
            return;
        }
        itemsInCao.Add(items[key], MapController.Instance.GetMapBlockInfo(layer, coord));
        items[key].transform.SetParent(caos[itemsInCao.Count - 1]);
        items[key].transform.localScale = Vector3.one * 100 * mapViewSetting.oneBlockSize;
        items[key].transform.localEulerAngles = Vector3.zero;
        items[key].transform.localPosition = Vector3.zero;
    }
    public void CollectBlock(int layer, Vector2 coord)
    {
        string key = (new Vector3(coord.x, coord.y, layer)).ToString();
        if (!items.ContainsKey(key))
        {
            return;
        }
        collectedItems.Add(key, items[key]);
        items[key].gameObject.SetActive(false);
        items[key].transform.SetParent(pool);
    }
    public GameObject GetCollectedBlock(int layer, Vector2 coord)
    {
        string key = (new Vector3(coord.x, coord.y, layer)).ToString();
        if (!collectedItems.ContainsKey(key))
        {
            return null;
        }
        collectedItems.Remove(key);
        collectedItems[key].gameObject.SetActive(true);
        return collectedItems[key];
    }
}
