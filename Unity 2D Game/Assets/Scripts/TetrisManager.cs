﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    
    [Header("fall duration"),Range(0.1f,3)]
    public float fallduration = 1.5f;//掉落時間
    
    [Header("Current scores")]
    public int scores;//目前分數
    
    [Header("Best scores")]
    public int bestscores;//最高分數
    
    [Header("Level")]
    public int Level =1;//等級
    
    [Header("Go end")]
    public GameObject goend;//結束畫面
    
    [Header("Sound")]
    public AudioClip brickdropsound;//方塊掉落音效
    
    public AudioClip movedsound;//方塊移動與旋轉音效
    
    public AudioClip removedsound;//方塊消除音效
    
    public AudioClip gameoversound;//遊戲結束音效

}
