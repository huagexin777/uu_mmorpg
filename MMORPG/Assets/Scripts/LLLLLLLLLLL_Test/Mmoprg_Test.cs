using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mmoprg_Test : MonoBehaviour
{


    void Start()
    {

        #region ���� Web����������

        NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress +"?id=1", GetWebEvent);

        #endregion

        #region ���� �������ݶ�ȡ


        //List<ProductEntity> products = ProductDBModel.Instance.GetEntityList();
        //for (int i = 0; i < products.Count; i++)
        //{
        //    Debug.LogError(products[i].ID + "-" + products[i].Name);
        //}

        #endregion


        #region ����,Http����

        //if (!NetWorkHttp.Instance.IsBusy)
        //{
        //    NetWorkHttp.Instance.SendData(GlobalInit.HttpIPAdress + )
        //}



        #endregion


        #region ����,Socket����

        //string ip = "192.168.0.116";
        //int port = 7777;
        ////1.���ӵ�������.
        //NetWorkSocket.Instance.ConnetionServer(ip, port);
        ////2.����������


        #endregion
    }
    void Update()
    {

    }

    private void GetWebEvent(CallBackArgs obj)
    {
        if (obj.IsError)
        {
            Debug.LogError("Web������Ϣ: " + obj.ErrorInfo);
        }

        try
        {
            AccountEntity account = JsonMapper.ToObject<AccountEntity>(obj.Json);

            Debug.LogError(account.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("�����л���ʱ��,���ִ���: " + e );
            throw;
        }
    }


}
