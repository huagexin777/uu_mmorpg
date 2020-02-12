using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mmoprg_Test : MonoBehaviour
{


    void Start()
    {

        #region 测试 Web服务器连接

        NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress +"?id=1", GetWebEvent);

        #endregion

        #region 测试 本地数据读取


        //List<ProductEntity> products = ProductDBModel.Instance.GetEntityList();
        //for (int i = 0; i < products.Count; i++)
        //{
        //    Debug.LogError(products[i].ID + "-" + products[i].Name);
        //}

        #endregion


        #region 测试,Http链接

        //if (!NetWorkHttp.Instance.IsBusy)
        //{
        //    NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress + )
        //}



        #endregion


        #region 测试,Socket链接

        //string ip = "192.168.0.116";
        //int port = 7777;
        ////1.连接到服务器.
        //NetWorkSocket.Instance.ConnetionServer(ip, port);
        ////2.开启服务器


        #endregion
    }
    void Update()
    {

    }

    private void GetWebEvent(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            Debug.LogError("Web错误信息: " + obj.ErrorInfo);
        }

        try
        {
            AccountEntity account = JsonMapper.ToObject<AccountEntity>(obj.Json);

            Debug.LogError(account.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("反序列化的时候,出现错误: " + e );
            throw;
        }
    }


}
