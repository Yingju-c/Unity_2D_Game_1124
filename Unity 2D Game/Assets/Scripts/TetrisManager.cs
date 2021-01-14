using System.Collections; //引用系統.集合API-協同程序
using System.Collections.Generic;
using System.Linq; //查詢語言
using UnityEngine.UI; //引用介面
using UnityEngine.SceneManagement;
using UnityEngine;


public class TetrisManager : MonoBehaviour
{
    #region
    //field欄位
    [Header("fall duration"), Range(0.1f, 3)]
    public float falltime = 3f;//掉落時間

    [Header("Current scores")]
    public int scores;//目前分數

    [Header("Best scores")]
    public int bestscores;//最高分數

    [Header("Level")]
    public int Level = 1;//等級

    [Header("Go end")]
    public GameObject goend;//結束畫面

    [Header("Sound")]
    public AudioClip fallsound;//方塊掉落音效

    public AudioClip movedsound;//方塊移動音效

    public AudioClip rotatesound; //方塊旋轉音效

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

    private bool gameover; //是否遊戲結束

    private AudioSource aud;

    #endregion



    #region
    //事件
    #endregion



    #region
    //方法


    private void Start()
    {
        aud = GetComponent<AudioSource>();

        addbricks();
    }

    private void Update()
    {
        if (gameover) return; //如果遊戲結束就跳出

        ControlTertis();
        FastDown();
    }

    private void ControlTertis()
    {
        if (currentTetris)//當有現在的俄羅斯方塊才會執行該段程式
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
            if (!tetris.wallRight && !tetris.smallRight)
            {
                //按下鍵盤D或右，往右50，||代表或者
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    aud.PlayOneShot(movedsound, Random.Range(0.8f, 1.2f));

                    currentTetris.anchoredPosition += new Vector2(36, 0);
                    //36=30方塊+6間距
                }
            }

            //左邊範圍限制
            //if(currentTetris.anchoredPosition.x > -190)
            //改成 如果 目前俄羅斯方塊 沒有 碰到右邊牆壁，!為沒有
            if (!tetris.wallLeft && !tetris.smallLeft)
            {
                //按下鍵盤A或左，往左50
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    aud.PlayOneShot(movedsound, Random.Range(0.8f, 1.2f));

                    currentTetris.anchoredPosition -= new Vector2(36, 0);
                }
            }

            if (tetris.canRotate) //如果俄羅斯方塊可旋轉
            {
                //按下鍵盤w或上，逆時針旋轉90度
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    aud.PlayOneShot(rotatesound, Random.Range(0.8f, 1.2f));

                    //在Unity要抓rotation用以eulerAngles控制,單位才是角度
                    currentTetris.eulerAngles += new Vector3(0, 0, 90);

                    tetris.Offset();
                }
            }

            if (!fastDown) //如果沒有快速落下 才能加速
            {
                //按下鍵盤S或下，加速，沒按的話恢復
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    falltime = 0.05f;
                }
                else
                {
                    falltime = timeFallMax;
                }

            }



            #endregion

            //if (currentTetris.anchoredPosition.y==-173.5)
            //如果俄羅斯方塊 碰到了地板 就重新開始 生成下一顆 ||或者 碰到其他方塊
            if (tetris.wallBottom || tetris.smallBottom)
            {
                //StartCoroutine(SetGround());
                SetGround();
                StartCoroutine(CheckTetris());
                StartGame(); //生成下一顆s
                StartCoroutine(ShakeEffect()); //晃動效果
            }

        }

    }

    private void SetGround() //把子物件在碰地後設為方塊
    {
        /*迴圈練習
        for(int i=0; i<10; i++)
        {
            print("迴圈：" + i);
        }
        */

        //yield return new WaitForSeconds(0.05f); //為搭配掉落時間

        aud.PlayOneShot(fallsound, Random.Range(0.8f, 1.2f));

        int count = currentTetris.childCount; //取得目前方塊的子物件數量

        for (int i = 0; i < count; i++) //迴圈執行子物件數量次數
        {
            currentTetris.GetChild(i).name = "方塊"; //名稱改成方塊
            currentTetris.GetChild(i).gameObject.layer = 10;  //圖層改成10
        }

        for (int i = 0; i < count; i++) //把掉下的方塊變成物件包在分數判定區域
        {
            currentTetris.GetChild(0).SetParent(traScoreArea);
        }

        Destroy(currentTetris.gameObject);

            fastDown = false; //碰地後，沒有快速落下
    }

    private bool down;

    /// <summary>
    /// 生成俄羅斯方塊
    /// 1.隨機顯示一個下一顆俄羅斯方塊0-10
    /// </summary>
    private void addbricks()//生成俄羅斯方塊
    {
        //語法：下一顆編號=隨機 的 範圍(最小，最大)
        indexNext = Random.Range(0, 10);//整數最大不包括
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
        fastDown = false; //碰地後，沒有快速落下

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

    [Header("分數文字")]
    public Text textScore;
    [Header("等級文字")]
    public Text textLv;

    private float timeFallMax = 0.5f;


    public void addscores(int add) //添加分數
    {
        scores += add; //分數累加
        textScore.text=""+ scores ; //更新介面

        Level = 1 + scores / 1000; //等級公式
        textLv.text= ""+ Level; //更新介面

        timeFallMax = 1.5f-Level /10;  //速度提升公式

        timeFallMax = Mathf.Clamp(timeFallMax, 0.1f, 99f); //用數值讓時間不要為負數

        falltime = timeFallMax;
    }

    [Header("目前分數")]
    public Text textCurrent;
    [Header("最高分數")]
    public Text textHeight;

    private void Gameover()//遊戲結束
    {
        if (!gameover)
        {
            aud.PlayOneShot(gameoversound, Random.Range(0.8f, 1.2f));

            gameover = true;    //遊戲結束
            StopAllCoroutines();    //停止所有協程
            goend.SetActive(true);  //顯示結束畫面

            textCurrent.text = "Current Scores:" + scores; //目前分數顯示

            //如果分數>本機端紀錄的 最高分數
            if (scores > PlayerPrefs.GetInt("Best Scores"))
            {
                //更新 本機端紀錄的最高分數 與介面
                PlayerPrefs.SetInt("Best Scores", scores);
                textHeight.text = "Best Scores:" + scores;
            }

            //否則 更新最高分數為 本機端紀錄的 最高分數
            else textHeight.text = "Best Scores:" + PlayerPrefs.GetInt("Best Scores");
        }
    }

    public void restart()//重新遊戲
    {
        SceneManager.LoadScene("step2_Game");
    }

    public void leavegame()//離開遊戲
    {
        Application.Quit();
    }

    /// <summary>
    /// 震動效果，協同程序-傳回類型-時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShakeEffect()
    {

        //yield return new WaitForSeconds(0.1f); 為搭配掉落時間

        //print("start");
        //可重複寫等待幾秒
        //yield return new WaitForSeconds(0.1f);
        //print("one second later...");

        RectTransform rect = traTetrisParent.GetComponent<RectTransform>();

        //等待秒數0.05秒
        float interval = 0.05f;

        //晃動效果：向上30 0 20 0
        rect.anchoredPosition += Vector2.up * 15;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition += Vector2.up * 10;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);

    }

    #endregion

    private bool fastDown;
    /// <summary>
    /// 快速掉落時間
    /// </summary>
    private void FastDown() //如果 有 目前方塊
    {
        if(currentTetris && !fastDown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fastDown = true;
                //掉落時間
                falltime = 0.02f;
                //語法：啟動協同程序(協同方法());
            }
        }
        
    }


    [Header("分數判定區域")]
    public Transform traScoreArea;

    /// <summary>
    /// 記錄所有在分數判定區域的小方塊
    /// </summary>
    public RectTransform[] rectSmall;

    /// <summary>
    /// 要刪除的列數
    /// </summary>
    public bool[] destoryRow = new bool[12];

    /// <summary>
    /// 剩下的方塊要掉落的高度
    /// </summary>
    public float[] downHeight;

    /// <summary>
    /// 檢查方塊是否連線
    /// </summary>
    private IEnumerator CheckTetris()
    {
        rectSmall = new RectTransform[traScoreArea.childCount]; //指定數量跟子物件相同

        for (int i = 0; i < traScoreArea.childCount; i++)  //利用迴圈將子物件儲存
        {
            rectSmall[i] = traScoreArea.GetChild(i).GetComponent<RectTransform>();

            float y = rectSmall[i].anchoredPosition.y; //抓到是y的位置
            if (y >= 246 - 10 && y <= 246 + 10) Gameover(); //最高位置遊戲結束
        }

        int row = 12; //總共有幾列

        for (int i = 0; i < row; i++) //用迴圈寫，把每行都包進去
        {
            float bottom = -190; //最底層的位置
            float step = 36; //每顆間隔

            //檢查有幾顆位置在同列，用正負10調整誤差
            var small = rectSmall.Where(x => x.anchoredPosition.y >= bottom+step*i- 5 && x.anchoredPosition.y <= bottom + step * i + 5);
            //print(small.ToArray().Length); 測試用，測現在有幾個在-190

            if (small.ToArray().Length == 12) //整排的長度湊12顆會有閃爍效果
            {
                aud.PlayOneShot(removedsound, Random.Range(0.8f, 1.2f));

                yield return StartCoroutine(Shine(small.ToArray()));//呼叫閃爍效果
                destoryRow[i] = true;

                addscores(100); //呼叫分數系統
            }
        }

        downHeight = new float[traScoreArea.childCount]; //紀錄有幾顆刪除後剩下的方塊
        for (int i = 0; i < downHeight.Length; i++) downHeight[i] = 0; //先將掉落高度歸零

        //計算每顆剩下方塊要掉落的高度
        for (int i = 0; i < destoryRow.Length;i++)
        {
            if (!destoryRow[i]) continue; //!為==false，如果此列沒有要刪除 就跳過繼續下一列

            for (int j = 0; j < rectSmall.Length; j++) //迴圈執行每一顆剩下的方塊
            {
                if(rectSmall[j].anchoredPosition.y>-190+36*i-10) //如果此方塊Y大於要刪除的列
                {
                    downHeight[j] -= 36; //座標遞減40
                }
            }

            destoryRow[i] = false; //恢復為不刪除

        }

        for (int i = 0; i < rectSmall.Length; i++)
        {
            rectSmall[i].anchoredPosition += Vector2.up * downHeight[i];
        }

    }

    /// <summary>
    /// 閃爍效果，閃爍完之後刪除
    /// </summary>
    /// <param name="smalls"></param>
    /// <returns></returns>
    private IEnumerator Shine(RectTransform[] smalls)
    {
        //閃爍
        float interval = 0.05f;
        for (int i = 0; i < 12; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 12; i++) smalls[i].GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 12; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 12; i++) smalls[i].GetComponent<Image>().enabled = true;

        //刪除
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 12; i++) Destroy(smalls[i].gameObject);

        //重新取得小方塊，避免產生Missing導致錯誤
        yield return new WaitForSeconds(interval);

        rectSmall = new RectTransform[traScoreArea.childCount]; //指定數量跟子物件相同

        for (int i = 0; i < traScoreArea.childCount; i++)  //利用迴圈將子物件儲存
        {
            rectSmall[i] = traScoreArea.GetChild(i).GetComponent<RectTransform>();
        }
    }
}

