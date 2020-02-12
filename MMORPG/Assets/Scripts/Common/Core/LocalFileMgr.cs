using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �����ļ�����
/// </summary>
public class LocalFileMgr : Singleton<LocalFileMgr>
{


#if UNITY_EDITOR

    #if UNITY_STANDALONE_WIN
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/Window/";
    #elif UNITY_IPHONE
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/iOS/";
    #elif UNITY_ANDROID
        /// <summary>
        /// ����ƽ̨�����õ���Ӧ�İ�·����
        /// <para>�Զ�����</para>
        /// </summary>
        public readonly string LocalFilePath = Application.dataPath + "/../AssetBundle/Android/";
    #endif

#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN
    public readonly string LocalFilePath = Application.persistentDataPath + "/../AssetBundle/";
#endif


    void Start()
    {
        
    }


    void Update()
    {

    }

    /// <summary>
    /// ��ȡ�����ļ���byte[]
    /// </summary>
    /// <returns></returns>
    public byte[] GetBuffer(string path)
    {
        byte[] buffer = null;

        //���ڶ���һ����Χ���ڴ˷�Χ��ĩβ���ͷŶ���
        //using �����ʹ�õĶ������ʵ�� IDisposable �ӿ�.

        //(����̳еĸ���)Singleton������ʵ���� IDisposable �ӿ�.
        using (FileStream fs = new FileStream(path,FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }

}
