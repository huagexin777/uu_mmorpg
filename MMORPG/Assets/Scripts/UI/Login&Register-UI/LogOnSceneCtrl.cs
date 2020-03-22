using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 启动场景-脚本
/// </summary>
public class LogOnSceneCtrl : MonoBehaviour
{

    
    void Awake()
    {
         

        UISceneCtrl.Instance.LoadSceneUI(SceneUIType.LogOn);

        ////测试
        //List<JobEntity> jobEntities = JobDBModel.Instance.GetEntityList();
        //for (int i = 0; i < jobEntities.Count; i++)
        //{
        //    Debug.LogError("名称: " + jobEntities[i].Name);
        //}

        
    }

    
    void Update()
    {

    }
}
  