using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//要寫這段才能用ScebeManager

public class MenuManager : MonoBehaviour
{
    #region
    //事件
    #endregion


    #region
    //方法

    /// <summary>
    /// 延遲開始遊戲，為讓音效跑完
    /// </summary>
    public void Delaystartgame()
    {
        //語法：延遲呼叫("方法名稱",延遲秒數);
        //Invoke();
        Invoke("startgame", 0.9f);
    }

    /// <summary>
    /// 延遲離開遊戲，為讓音效跑完
    /// </summary>
    public void Delayleavegame()
    {
        //語法：延遲呼叫("方法名稱",延遲秒數);
        //Invoke();
        Invoke("leavegame", 0.9f);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void startgame()//開始遊戲
    {
        SceneManager.LoadScene("step2_Game");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void leavegame()
    {
        Application.Quit();//離開遊戲
    }
    #endregion
}
