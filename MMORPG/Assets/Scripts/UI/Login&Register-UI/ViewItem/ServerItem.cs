using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour
{

    private UIGameServerView serverView;

    #region ����

    private Text txt;
    public Text Txt { get { return GetComponentInChildren<Text>(); } }

    private Image serverPoint;
    public Image ServerPoint { get { return transform.Find("serverPoint").GetComponent<Image>(); } }

    private Button btn;
    public Button Btn { get { return GetComponentInChildren<Button>(); } }

    #endregion


    public RetGameServerEntity serverEntity;

    public string ip { set; get; }
    public int port { set; get; }


    private void Start()
    {
        Btn.onClick.AddListener(OnClick);
    }

    void OnClick() 
    {
        serverView.UpdateSelectingServer(this);
    }

    /// <summary>
    /// ���·�����Item��Ϣ
    /// </summary>
    /// <param name="entity"></param>
    public void UpdateInfo(RetGameServerEntity entity, UIGameServerView view) 
    {
        //��ַ���˿ں�
        this.ip = entity.Ip;
        this.port = entity.Port;

        this.serverView = view;


        this.serverEntity = entity;
        Txt.text = entity.Name;
        if (entity.RunStatus == 0)
        {

        }
        else if (entity.RunStatus == 1)
        {

        }
        else if (entity.RunStatus == 1)
        {

        }
    }

}
