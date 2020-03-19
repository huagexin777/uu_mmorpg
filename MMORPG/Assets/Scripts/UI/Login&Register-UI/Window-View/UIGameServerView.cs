using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerView : UIWindowViewBase
{
    [SerializeField, Header("页码Item")] GameObject serverPageItemPref;
    [SerializeField, Header("服务器Item")] GameObject serverItemPref;


    //当前-服务器list
    private List<ServerItem> currentServerList = new List<ServerItem>();
    //当前-选中的服务器Item (返回类)
    private RetGameServerEntity curRetServerEntity;

    public override UIWindowCtrl.WindowType CurrentWindowType
    {
        get { return UIWindowCtrl.WindowType.GameServer; }
    }


    //属性
    private Transform content_page;
    private Transform content_list;

    //选中的服务器
    private Text selectingTxt;
    private Image selectingPoint;

    public override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < allTransList.Count; i++)
        {
            if (allTransList[i].name == "Content-Page")
            {
                content_page = allTransList[i];
            }
            else if (allTransList[i].name == "Content-List")
            {
                content_list = allTransList[i];
            }
            else if (allTransList[i].name == "selectingTxt")
            {
                selectingTxt = allTransList[i].GetComponent<Text>();
            }
            else if (allTransList[i].name == "selectingPoint")
            {
                selectingPoint = allTransList[i].GetComponent<Image>();
            }
        }
    }


    public override void OnStart()
    {
        base.OnStart();

        //默认,启动组.
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["type"] = 1;
        dict["pageIndex"] = -1;
        NetWorkHttp.Instance.SendData("api/gameserver", OnGameServerList, true, dict);
    }

    public override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        if (go.name == "SelectBtn")
        {
            object[] objs = new object[1];
            objs[0] = curRetServerEntity;
            UIDispatcher.Instance.Dispatcher(ConstDefine.UIGameServerView_SelectBtn, objs);
        }

        //如果是,关闭按钮.
        if (go.name.Equals("BackBtn", System.StringComparison.CurrentCultureIgnoreCase) ||
            go.name.Equals("CloseBtn", System.StringComparison.CurrentCultureIgnoreCase) ||
            go.name.Equals("GoToRegisterBtn", System.StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }


    #region 》》》》更新页码

    /// <summary>
    /// 更新页码
    /// </summary>
    public void UpdateServerPage(List<RetGameServerPageEntity> list)
    {
        if (list == null || serverPageItemPref == null) { return; }
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = Instantiate(serverPageItemPref);
            go.transform.SetParent(content_page);
            go.GetComponent<ServerPageItem>().UpdateInfo(list[i]);
            go.GetComponent<ServerPageItem>().OnClickEvent = ServerPageCallBack;
        }
    }


    #endregion


    #region 》》》》页码item-点击


    /// <summary>
    /// 页码点击-回调
    /// </summary>
    void ServerPageCallBack(RetGameServerPageEntity ret)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["type"] = 1;
        dict["pageIndex"] = ret.PageIndex;
        NetWorkHttp.Instance.SendData("api/gameserver", OnGameServerList, true, dict);
    }

    /// <summary>
    /// 单个页码点击响应后的事件-回调
    /// </summary>
    private void OnGameServerList(RetValue obj)
    {
        List<RetGameServerEntity> retServerList = JsonMapper.ToObject<List<RetGameServerEntity>>(obj.Value.ToString());
        //先清除
        ClearGameServerItem();
        //添加
        for (int i = 0; i < retServerList.Count; i++)
        {
            InitGameServerItem(retServerList[i]);
        }
    }


    /// <summary>
    /// 清除serverItemList
    /// </summary>
    void ClearGameServerItem()
    {
        for (int i = 0; i < currentServerList.Count; i++)
        {
            Destroy(currentServerList[i].gameObject);
        }
        currentServerList.Clear();
    }

    /// <summary>
    /// 生成serverItem
    /// </summary>
    void InitGameServerItem(RetGameServerEntity entity)
    {
        GameObject serverItem = Instantiate(serverItemPref);
        serverItem.transform.SetParent(content_list);
        serverItem.GetComponent<ServerItem>().UpdateInfo(entity, this);
        if (!currentServerList.Contains(serverItem.GetComponent<ServerItem>()))
        {
            currentServerList.Add(serverItem.GetComponent<ServerItem>());
        }
    }

    #endregion





    public override void Close()
    {
        base.Close();
    }

    //----------------------------- 公开方法 ---------------------------------

    /// <summary>
    /// 更新选中的服务器
    /// </summary>
    public void UpdateSelectingServer(ServerItem item) 
    {
        GlobalInit.currentServer.ip = item.ip;
        GlobalInit.currentServer.port = item.port;

        this.curRetServerEntity = item.serverEntity;
        this.selectingPoint.sprite = item.ServerPoint.sprite;
        this.selectingTxt.text = item.Txt.text;
    }


}
