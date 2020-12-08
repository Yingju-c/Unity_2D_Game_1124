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
    }

}
