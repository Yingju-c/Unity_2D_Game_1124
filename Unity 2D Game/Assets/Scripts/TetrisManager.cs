using System.Collections; //引用系統.集合API-協同程序
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

    [Header("生成俄羅斯方塊的父物件")]
    public Transform traTetrisParent;//告訴程式有一個畫布

    [Header("生成的起始位置")]
    public Vector2[] posSpawn = //[]為陣列的寫法
    {
        new Vector2(0,206),
        new Vector2(18,206),
        new Vector2(0,206),
        new Vector2(16,224),
        new Vector2(0,224),
        new Vector2(0,224),
        new Vector2(0,224),
        new Vector2(0,224),
        new Vector2(0,224),
        new Vector2(0,260)
    };

    private int indexNext;//下一個俄羅斯方塊編號

    private RectTransform currentTetris;//目前俄羅斯方塊

    private float timer;//計時器
    
    #endregion



    #region
    //事件
    #endregion



    #region
    //方法


    private void Start()
    {
        addbricks();
    }

    private void Update()
    {
        ControlTertis();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //語法：啟動協同程序(協同方法());
            StartCoroutine(ShakeEffect());
        }

    }

    private void ControlTertis()
    {
        if(currentTetris)//當有現在的俄羅斯方塊才會執行該段程式
        {
            timer += Time.deltaTime;//timer累加時間

            if (timer >= falltime) //寫判斷式
            {
                timer = 0;
                currentTetris.anchoredPosition -= new Vector2(0, 36);//x,y
            }

            #region 按鍵盤控制俄羅斯方塊上下及旋轉與加速

            //取得 目前俄羅斯方塊的 Tetris腳本
            Tetris tetris = currentTetris.GetComponent<Tetris>();

            //右邊範圍限制：以座標控制
            //if (currentTetris.anchoredPosition.x < 190)
            //改成 如果 目前俄羅斯方塊 沒有 碰到右邊牆壁，!為沒有
            if(!tetris.wallRight)
            {
                //按下鍵盤D或右，往右50，||代表或者
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentTetris.anchoredPosition += new Vector2(36, 0);
                    //36=30方塊+6間距
                }
            }

            //左邊範圍限制
            //if(currentTetris.anchoredPosition.x > -190)
            //改成 如果 目前俄羅斯方塊 沒有 碰到右邊牆壁，!為沒有
            if (!tetris.wallLeft)
            {
                //按下鍵盤A或左，往左50
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentTetris.anchoredPosition -= new Vector2(36, 0);
                }
            }

            if (tetris.canRotate) //如果俄羅斯方塊可旋轉
            {
                //按下鍵盤w或上，逆時針旋轉90度
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {   //在Unity要抓rotation用以eulerAngles控制,單位才是角度
                    currentTetris.eulerAngles += new Vector3(0, 0, 90);

                    tetris.Offset();
                }
            }
            

            //按下鍵盤S或下，加速，沒按的話恢復
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                falltime = 0.1f;
            }
            else
            {
                falltime = 1.5f;
            }


            #endregion

            //if (currentTetris.anchoredPosition.y==-173.5)
            if(tetris.wallBottom)
            {
                StartGame();
            }

        }

    }

    /// <summary>
    /// 生成俄羅斯方塊
    /// 1.隨機顯示一個下一顆俄羅斯方塊0-10
    /// </summary>
    private void addbricks()//生成俄羅斯方塊
    {
        //語法：下一顆編號=隨機 的 範圍(最小，最大)
        indexNext =Random.Range(0, 10);//整數最大不包括
        //indexNext = 9;//測試用
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
        GameObject current = Instantiate(tetris, traTetrisParent);
        //API:GetComponent取得元件
        //API:anchoredPosition座標
        //語法：GetComponent<任何元件>()
        //<>指<T>泛型，指所有類型
        //語法：目前俄羅斯方塊，取得元件<介面變形>().座標=二維向量(Vx,Vy);
        //以Unity給的面板數字為主
        current.GetComponent<RectTransform>().anchoredPosition = posSpawn[indexNext];

        //2.上一顆隱藏
        tetris.SetActive(false);

        //3.隨機取下一個
        addbricks();

        //將生成的俄羅斯方塊 RectTransform元件儲存
        currentTetris = current.GetComponent<RectTransform>();

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

    /// <summary>
    /// 震動效果，協同程序-傳回類型-時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShakeEffect()
    {
        //print("start");
        //可重複寫等待幾秒
        //yield return new WaitForSeconds(0.1f);
        //print("one second later...");

        RectTransform rect = traTetrisParent.GetComponent<RectTransform>();

        //等待秒數0.05秒
        float interval = 0.05f;

        //晃動效果：向上30 0 20 0
        rect.anchoredPosition += Vector2.up * 30;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition += Vector2.up * 20;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);

    }

    #endregion

}

