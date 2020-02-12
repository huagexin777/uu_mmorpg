using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerPanel : BasePanel {

    public GameObject _serverItem;

    private Transform _content;
    
    private Button BackBtn;

    void Awake()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = new Vector3(0, 1000, 0);
        _content = transform.Find("bg/Scroll View/Viewport/Content").transform;
        
        BackBtn = transform.Find("BackBtn").GetComponent<Button>();
        BackBtn.onClick.AddListener(BackClick);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        ShowPanel();
    }

    public override void OnPause()
    {
        base.OnPause();

        HidePanel();
    }

    public override void OnExit()
    {
        base.OnExit();

        HidePanel();
    }

    public override void OnResume()
    {
        base.OnResume();

        ShowPanel();
    }
    

    public void BackClick()
    {
        base.uiMng.PopPanel();
    }

    public void GetServersRequest()
    {
        //TODO 请求获得 服务器面板
    }

    void ShowPanel()
    {
        this.gameObject.SetActive(true);
        transform.DOLocalMoveY(-75f, 1);
    }

    void HidePanel()
    {
        transform.DOLocalMoveY(1000, 1).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
