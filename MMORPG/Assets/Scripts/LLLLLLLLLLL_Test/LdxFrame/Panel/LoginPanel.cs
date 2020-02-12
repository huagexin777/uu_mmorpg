using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoginPanel : BasePanel {

    private InputField loginInput;
    private InputField passwordInput;
    private Button LoginBtn;
    private Button RegisterBtn;

    void Awake()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = new Vector3(0, 1000, 0);

        loginInput = transform.Find("bg/LoginUser").GetComponent<InputField>();
        passwordInput = transform.Find("bg/Password").GetComponent<InputField>();

        LoginBtn = transform.Find("bg/LoginBtn").GetComponent<Button>();
        LoginBtn.onClick.AddListener(LoginClick);
        RegisterBtn = transform.Find("bg/RegisterBtn").GetComponent<Button>();
        RegisterBtn.onClick.AddListener(RegisterClick);
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

    public void LoginClick()
    {
        //TODO 登录面板 登录按键还没做
        base.uiMng.PushPanel(PanelType.Server); 
    }

    public void RegisterClick()
    {
        //base.uiMng.PopPanel();
        base.uiMng.PushPanel(PanelType.Register);
    }

    void ShowPanel()
    {
        this.gameObject.SetActive(true);
        transform.DOLocalMoveY(-75f, 1);
    }

    void HidePanel()
    {
        transform.DOLocalMoveY(1000, 1).OnComplete(()=> 
        {
            this.gameObject.SetActive(false);
        });
    }
}
