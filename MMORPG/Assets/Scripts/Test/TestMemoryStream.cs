using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestMemoryStream : MonoBehaviour
{


    void Start()
    {

        TT t1 = new TT()
        {
            id = 1,
            age = 18,
            name = "我哎你我哎你我哎你啊我哎你我哎你我哎你啊我哎你我哎你我哎你啊"
        };
        byte[] buffer = null;


        using (MMO_MemoryStream mmo = new MMO_MemoryStream())
        {
            mmo.WriteInt(t1.id);
            mmo.WriteInt(t1.age);
            mmo.WriteUTF8String(t1.name);
            buffer = mmo.ToArray();
        }

        TT t2 = new TT();
        using (MMO_MemoryStream mmo = new MMO_MemoryStream(buffer))
        {
            //储存原理: 队列queue.
            t2.id = mmo.ReadInt();
            t2.age = mmo.ReadInt();
            t2.name = mmo.ReadUTF8String();
        }

        Debug.LogFormat("{0},{1},{2}",t2.id,t2.age,t2.name);
    }


    void Update()
    {

    }
}

public class TT
{
    public int id;
    public int age;
    public string name;

}
