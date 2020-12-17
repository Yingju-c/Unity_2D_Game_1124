using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetrisManager : MonoBehaviour
{
    #region
    //field欄位
    [Header("fall duration"), Range(0.1f, 3)]
    public float falltime = 1.5f;//掉落時間

    [Header("Current scores")]
    public int scores;//目前分數

    [Header("Best scores")]
    public int bestscores;//最高分數

    [Header("Level")]
    public int Level = 1;//等級

    [Header("Go end")]
    public GameObject goend;//結束畫面

    [Header("Sound")]
    public AudioClip brickdropsound;//方塊掉落音效

    public AudioClip movedsound;//方塊移動與旋轉音效

    public AudioClip removedsound;//方塊消除音效

    public AudioClip gameoversound;//遊戲結束音效

    [Header("下一個俄羅斯方塊區域")]
    public Transform traNextArea;//Transform非靜態寫法

    [Header("畫布")]
    public Transform traCanvas;//告訴程式有一個畫布
    #endregion

    #region
    //事件
    #endregion



    #region
    //方法

    public int indexNext;
    private void Start()
    {
        addbricks();
    }

    /// <summary>
    /// 生成俄羅斯方塊
    /// 1.隨機顯示一個下一顆俄羅斯方塊0-10
    /// </summary>
    private void addbricks()//生成俄羅斯方塊
    {
        //語法：下一顆編號=隨機 的 範圍(最小，最大)
        indexNext =Random.Range(0, 11);//整數最大不包括
        //語法：下一顆俄羅斯方塊的區域 的子物件轉成遊戲物件 的狀態 打勾
        traNextArea.GetChild(indexNext).gameObject.SetActive(true);
    }

    /// <summary>
    /// 開始遊戲
    /// 1.生成俄羅斯方塊
    /// 2.上一顆隱藏
    /// 3.隨機取下一個
    /// </summary>
    public void StartGame()//開始遊戲
    {
        //1.生成俄羅斯方塊
        //告訴程式有一個遊戲物件
        GameObject tetris = traNextArea.GetChild(indexNext).gameObject;
        //並生成物件
        //語法：Instantiate(生成命名，置入哪個父物件)
        //告訴程式這是目前的俄羅斯方塊
        GameObject current = Instantiate(tetris, traCanvas);
        //API:GetComponent取得元件
        //API:anchoredPosition座標
        //語法：GetComponent<任何元件>()
        //<>指<T>泛型，指所有類型
        //語法：目前俄羅斯方塊，取得元件<介面變形>().座標=二維向量(Vx,Vy);
        current.GetComponent<RectTransform>().anchoredPosition = new Vector2(-648, 221);

        //2.上一顆隱藏
        tetris.SetActive(false);

        //3.隨機取下一個
        addbricks();

    }



    public void addscores(int Scores)//添加分數
    {

    }

    private void gameduration()//遊戲時間
    {

    }

    private void gameover()//遊戲結束
    {

    }

    public void restart()//重新遊戲
    {

    }

    public void leavegame()//離開遊戲
    {

    }

    #endregion

}

