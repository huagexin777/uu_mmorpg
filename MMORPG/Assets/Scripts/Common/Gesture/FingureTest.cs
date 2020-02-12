using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FingureTest : MonoBehaviour {


    private Vector2 dragStartPos;
    private Vector2 dragEndPos;


    void Start () {
		
	}

    void OnEnable()
    {
        FingerGestures.OnFingerDown += OnFingerDown;
        FingerGestures.OnDragBegin += OnDragBegin;
        FingerGestures.OnDragEnd += OnDragEnd;

        //手指拖拽
        FingerGestures.OnFingerDragBegin += OnFingerDragBegin;

    }



    private void OnDestroy()
    {
        FingerGestures.OnFingerDown -= OnFingerDown;
        FingerGestures.OnDragBegin -= OnDragBegin;
    }

    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        Debug.Log("[点击]  手指id" + fingerIndex + "--位置 = " + fingerPos);
    }

    void OnDragBegin(Vector2 fingerPos, Vector2 startPos)
    {
        this.dragStartPos = startPos;
        Debug.Log("[开始拖拽] 位置 = " + fingerPos + "--开始位置 = " + startPos);
    }
    void OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
    {
        Debug.Log(" 手指id " + fingerIndex + "[开始拖拽] 手指位置 = " + fingerPos + "--开始位置 = " + startPos);
    }

    private void OnDragEnd(Vector2 fingerPos)
    {
        this.dragEndPos = fingerPos;
        Debug.Log("[结束拖拽] 手指位置 = " + fingerPos );
    }

    void MouseDown()
    {


    }


    void Update () {
		
	}



}
