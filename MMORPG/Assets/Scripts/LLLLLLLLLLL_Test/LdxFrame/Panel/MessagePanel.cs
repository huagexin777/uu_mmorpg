using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    private Text _txt;
    private Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
        _txt = transform.Find("content").GetComponent<Text>();
        _txt.enabled = false;

    }

    void Update()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartCoroutine("Efx_MessageBox");
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public void ShowMessage(string str)
    {

    }


    /// <summary>
    /// 消息盒的特效
    /// </summary>
    /// <returns></returns>
    IEnumerator Efx_MessageBox()
    {
        float timer = 0;
        while (timer <= 1)
        {
            _img.fillAmount = timer / 1f;
            timer += Time.deltaTime;
            yield return null;  
        }
        _img.fillAmount = 1;
        _txt.enabled = true;
        StopCoroutine("Efx_MessageBox");
    }

}
