using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameView : BaseView
{
    public Button createNewMap;
    public Button chooseMap;
    public Button setting;
    public Button startGame;

    public override void Close()
    {
        this.gameObject.SetActive(false);
    }

    public override void Open()
    {
        this.gameObject.SetActive(true);
    }

    private void Awake()
    {
        createNewMap.onClick.AddListener(()=> { });
        startGame.onClick.AddListener(() => {
            Close();
        });

    }
}
