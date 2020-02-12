using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 80;
    public string servername = "";

    private Text _serverName;
    private Button _enterBtn;

    void Start()
    {
        _serverName = transform.Find("servername").GetComponent<Text>();
        _enterBtn = transform.Find("Enter").GetComponent<Button>();
        _enterBtn.onClick.AddListener(ServerClick);
    }

    void Update()
    {

    }

    void ServerClick()
    {


        ProgressBarTool.Instance.LoadScene(SceneType.MainCity);
    }


}
