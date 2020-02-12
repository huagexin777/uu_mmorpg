using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel:MonoBehaviour
{

    public UIManager uiMng
    {
        set;get;
    }
    public LdxFacade facade
    {
        get;set;
    }


    public virtual void OnEnter()
    {

    }

    public virtual void OnPause()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnResume()
    {

    }
}
