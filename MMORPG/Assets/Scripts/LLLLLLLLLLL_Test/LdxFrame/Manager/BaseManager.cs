using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager {

    protected LdxFacade ldxFacade;

    public BaseManager(LdxFacade ldxFacade)
    {
        this.ldxFacade = ldxFacade;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnDestroy()
    {

    }



}
