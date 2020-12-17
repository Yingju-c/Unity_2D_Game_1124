using UnityEngine;

public class APInostatic : MonoBehaviour
{
    //動態屬性
    //語法：類型 欄位名稱

    //屬性
    public Transform traA;
    public Transform traB;

    public GameObject myobj;

    //方法
    public Transform myTra;

    private void Start()
    {
        //一般屬性的取得與設定
    
        //取得
        //語法：類型欄位名稱 的 一般屬性
        print("物件A的座標：" + traA.position);

        //設定
        //語法：類型欄位名稱 的 一般屬性 = 對應的值;
        traB.position = new Vector3(1, 2, 3);

        //練習取得與設定
        print("我的物件圖層：" + myobj.layer);
        myobj.layer = 2;

    }

    private void Update()
    {
        //一般方法
        //使用
        //語法：類型欄位名稱 的 方法(對應的參數)
        myTra.Rotate(0, 0, 3);
        myTra.Translate(1, 0, 0);//位移
    }
}
