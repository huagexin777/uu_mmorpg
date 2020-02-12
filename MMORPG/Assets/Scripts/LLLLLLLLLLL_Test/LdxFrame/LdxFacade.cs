using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LdxFacade : MonoBehaviour
{
    private static LdxFacade _instance;
    public static LdxFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("LdxFacade").GetComponent<LdxFacade>();
            }
            return _instance;
        }
    }

    private UIManager uIManager;
    private KnapscakManager knapscakManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        uIManager = new UIManager(this);
        knapscakManager = new KnapscakManager(this);

        uIManager.OnInit();
        knapscakManager.OnInit();
    }

    void OnDestroy()
    {
        uIManager.OnDestroy();
        knapscakManager.OnDestroy();
    }

    void Update()
    {
        uIManager.OnUpdate();
        knapscakManager.OnUpdate();
    }

    #region 外观模式 调用机制

    //载入面板 panel
    public void ShowPanelType(PanelType panelType)
    {
        uIManager.PushPanel(panelType);
    }

    #endregion
}
