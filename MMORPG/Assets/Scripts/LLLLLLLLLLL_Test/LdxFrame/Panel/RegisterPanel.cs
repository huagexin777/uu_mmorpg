using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{

    private InputField loginInput;
    private InputField passwordInput;
    private InputField repasswordInput;
    private Button RegisterBtn;
    private Button BackBtn;

    void Awake()
    {
        this.gameObject.SetActive(false);
        this.gameObject.transform.position = new Vector3(0, 1000, 0);

        loginInput = transform.Find("bg/LoginUser").GetComponent<InputField>();
        passwordInput = transform.Find("bg/Password").GetComponent<InputField>();
        repasswordInput = transform.Find("bg/RePassword").GetComponent<InputField>();

        RegisterBtn = transform.Find("bg/RegisterBtn").GetComponent<Button>();
        RegisterBtn.onClick.AddListener(RegisterClick);
        BackBtn = transform.Find("bg/BackBtn").GetComponent<Button>();
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

    public void RegisterClick()
    {
        //TODO 注册面板 注册按键还没做
    }

    public void BackClick()
    {
        base.uiMng.PopPanel();
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
