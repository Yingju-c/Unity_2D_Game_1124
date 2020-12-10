using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{


    //單行註解
    //number=Field
    //Scripts:
    //class name;

    //欄位屬性:標題(Header-字串);提示(Tooltip-字串);範圍(Range-數值)
   
    #region 1.Field

    [Header("尺寸設定"),Tooltip("輸入整數數值"),Range(0,100)]
    public int size=5; //整數
    [Tooltip("輸入小數點數值")]
    public float weight=3.6f; //浮點數 一定要加f或F
    public string brand="BTW"; //字串
    public bool haswindow=true;  //布林值

    //常用的其他類型:color
    public Color colorA = Color.blue;
    public Color colorB = new Color();
    public Color colorC = new Color(0.8f, 0.2f, 0f);
    public Color colorD = new Color(0.8f, 0.3f, 0, 0.5f);

    //常用的其他類型:vector
    public Vector2 v2A = Vector2.zero;
    public Vector2 v2B = new Vector2(0.5f,0.1f);
    public Vector3 v3A = new Vector3(1.5f,1.3f,1.5f);
    public Vector4 v4A = new Vector4(1.5f,1.3f,1.5f,1.2f);

    //常用的其他類型:AudioClip
    public AudioClip sound;

    //常用的其他類型:Sprite
    public Sprite sprite;

    //常用的其他類型:GameObject 預製物及一般物件
    public GameObject goA;
    public GameObject goB;

    //常用的其他類型:元件 屬性面板上的粗體字皆可用
    public Transform Trans;
    public SpriteRenderer SR;
    public Camera cam;

    #endregion


    #region 2.Event

    //事件:Event 開始
    private void Start()
    {
        print("hello world!");

        //取得欄位(抓出資料)
        print(colorA);
        print(sound);

        //設定欄位(修改資料)資料欄位=值;(播放後會變成此設定)
        weight = 1.9f;
        haswindow = false;

        //呼叫自訂方法(void無傳回類型)
        //呼叫方法語法：自訂方法名稱();
        MethodA();
        MethodB();

        int intA=MethodC();
        print("整數:" + intA);

        float floatA=MethodD();
        print("小數:" + floatA);

        Color colorM = MethodE();
        print("顏色:" + colorM);

        MethodF(3.56f);

        float b = BMI(60,1.7f);
        print("BMI:" + b);

        Drive1(150, "後方");
        Drive2(180);
        Drive3("後方");

    }

    #endregion

    #region 3.Method

    //欄位語法：修飾詞 類型 欄位名稱 指定 值;
    //方法語法與欄位語法寫法相似
    //方法語法：修飾詞 類型(傳回類型) 方法名稱 () {}
    //無傳回類型void-沒有任何傳回資料

    private void MethodA()
    {
        print("Here is method");
        print("Test");
    }

    private void MethodB()
    {
        print("Okay");
        print("*-*");
    }

    private int MethodC()
    {
        return 134;
    }

    private float MethodD()
    {
        return 0.5698f;
    }

    private Color MethodE()
    {
    return new Color(0.5f,0.3f,0);
    }

    private void MethodF(float number)
    {
        number -= 0.5f;
        print("累加後的數字:" + number);
    }

    private float BMI(float w, float h)
    {
        float BMI = w / (h * h);
        print("體重(公斤):" + w);
        print("身高(公尺):" + h);
        return BMI;//return後會挑出此框架，所以print要先寫
    }

    /// <summary>
    /// Drive1-flexible
    /// </summary>
    /// <param name="speed">時速</param>
    /// <param name="direction">方向</param>
    private void Drive1(int speed, string direction)
    {
        print("時速:" + speed);
        print("方向:" + direction);
    }

    /// <summary>
    /// Drive2-測試定方位
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    private void Drive2(int speed, string direction="前方")
    {
        print("時速:" + speed);
        print("方向:" + direction);
    }
   
    /// <summary>
    /// Drive3-測試定速
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    private void Drive3(string direction, int speed=100)
    {
        print("時速:" + speed);
        print("方向:" + direction);
    }

    #endregion

}
