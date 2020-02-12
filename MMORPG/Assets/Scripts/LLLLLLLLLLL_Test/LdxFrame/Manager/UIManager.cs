using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class UIManager : BaseManager
{

    private Stack<BasePanel> _panels;
    private Dictionary<PanelType, string> panelPath;     //储存面板panel的路径
    private Dictionary<PanelType, BasePanel> panelGo;   //储存面板实例
    private Canvas canvas;

    public UIManager(LdxFacade ldxFacade) : base(ldxFacade)
    {
        ParseJsonForPanel();
    }

    public override void OnInit()
    {
        base.OnInit();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        

        PushPanel(PanelType.Message);
        PushPanel(PanelType.Login);

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void PushPanel(PanelType panelType)
    {
        BasePanel newPanel = GetPanel(panelType);

        if (_panels.Count > 0) //栈里面存在多个Panel
        {
            BasePanel panel = _panels.Peek();
            panel.OnPause();

            _panels.Push(newPanel);
            newPanel.OnEnter();
        }
        else
        {
            _panels.Push(newPanel);
            newPanel.OnEnter();
        }
    }

    public void PopPanel()
    {
        if (_panels.Count > 0)
        {
            BasePanel oldPanel = _panels.Pop();
            oldPanel.OnExit();

            BasePanel nowPanel = _panels.Peek();
            nowPanel.OnResume();
        }
    }

    //获得Panel面板
    BasePanel GetPanel(PanelType panelType)
    {
        if (_panels == null) { _panels = new Stack<BasePanel>(); }
        if (panelGo == null) { panelGo = new Dictionary<PanelType, BasePanel>(); }
        
        BasePanel basePanel = null;
        if (panelGo.ContainsKey(panelType))
        {
            panelGo.TryGetValue(panelType,out basePanel);
            return basePanel;
        }
        else
        {
            string path;
            panelPath.TryGetValue(panelType, out path);
            Debug.Log(path);
            GameObject panelGO = (GameObject)GameObject.Instantiate(Resources.Load(path));
            basePanel = panelGO.GetComponent<BasePanel>();
            panelGO.transform.SetParent(canvas.transform, false);
            panelGo.Add(panelType, basePanel);
            basePanel.uiMng = this;
            basePanel.facade = ldxFacade;
        }
        return basePanel;
    }

    [Serializable]
    public class Panel : ISerializationCallbackReceiver
    {
        [NonSerialized]
        public PanelType PanelType;

        public string paneltype;
        public string pathstr;

        //反序列化
        public void OnAfterDeserialize()
        {
            PanelType = (PanelType)Enum.Parse(typeof(PanelType), paneltype);
        }


        public void OnBeforeSerialize()
        {

        }
    }

    [Serializable]
    class JsonPanel
    {
        public List<Panel> PanelList;
    }

    /// <summary>
    /// 解析面板 Json路径
    /// </summary>
    void ParseJsonForPanel()
    {
        panelPath = new Dictionary<PanelType, string>();
        string txt = Resources.Load<TextAsset>("Panel").text;
        JsonPanel jsonPanel = JsonUtility.FromJson<JsonPanel>(txt);

        foreach (Panel p in jsonPanel.PanelList)
        {
            //Debug.Log(p.PanelType +"," + p.pathstr);
            panelPath.Add(p.PanelType, p.pathstr);
        }

    }

}
