using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerPageItem : MonoBehaviour
{
    #region 属性

    private Text txt;
    public Text Txt { get { return GetComponentInChildren<Text>(); } }

    private Button btn;
    public Button Btn { get { return GetComponentInChildren<Button>(); } }


    #endregion


    public RetGameServerPageEntity serverPageEntity;

    public Action<RetGameServerPageEntity> OnClickEvent;


    private void Start()
    {
        Btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        OnClickEvent(serverPageEntity);
    }

    /// <summary>
    /// 更新服务器PageItem信息
    /// </summary>
    /// <param name="entity"></param>
    public void UpdateInfo(RetGameServerPageEntity entity)
    {

        this.serverPageEntity = entity;
        Txt.text = entity.Name;
    }
}
