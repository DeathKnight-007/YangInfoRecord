using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager instance;
    public GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    public List<string> inames;
    public List<BaseView> iviews;
    public Dictionary<string, BaseView> views = new Dictionary<string, BaseView>();
    private void Awake()
    {
        instance = this;
        for(int i = 0; i < inames.Count; i++)
        {
            views.Add(inames[i], iviews[i]);
        }
    }
    public void OpenView(string name)
    {
        views[name].Open();
        views[name].transform.SetAsLastSibling();
    }
    public void CloseView(string name)
    {
        views[name].Close();
    }
}
