using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapsView : BaseView
{
    public RectTransform content;
    public GameObject item;
    public override void Close()
    {
        this.gameObject.SetActive(false);
    }

    public override void Open()
    {
        this.gameObject.SetActive(true);
    }
}
