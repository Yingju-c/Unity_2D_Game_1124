using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIstatic : MonoBehaviour
{
    //程式主要就兩個：取得、設定
    
    /// <summary>
    /// 開始事件：播放後執行一次
    /// </summary>
    private void Start()
    {
        //靜態屬性
        //取得
        //語法：類別名稱.靜態屬性名稱
        print(Mathf.PI);

        //設定
        //語法：類別名稱.靜態屬性名稱=相同類型的值

        Time.timeScale = 0.5f;
        print(Time.timeScale = 0.5f);
        print(Time.time);

        //靜態屬性練習
        print("所有攝影機的數量："+Camera.allCamerasCount);
        Cursor.visible = false;

        //靜態方法
        int number = Mathf.Abs(-999);
        print("取得絕對值：" + number);

        print("隨機數字3-20.5：" + Random.Range(3,20.5f));

        //靜態方法練習
        //Application.OpenURL("http://google.com");
        print("7.7去小數點：" + Mathf.Floor(7.7f));

    }

    /// <summary>
    /// 更新事件：一秒執行約60次(60FPS)
    /// </summary>
    private void Update()
    {
        //靜態屬性練習
        //print("是否按任意鍵："+Input.anyKey);
        //print("遊戲時間："+Time.time);

        //靜態方法練習
        //print("是否按下空白鍵：" + Input.GetKeyDown("space"));

    }


}
