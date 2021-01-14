using UnityEngine;
using System.Linq; //引用系統.查詢語言API-偵測陣列.清單內的資料


public class Tetris : MonoBehaviour
{
    #region 欄位
    [Header("角度為0或180，線條的長度")]
    public float length0;
    [Header("角度為90或270，線條的長度")]
    public float length90;

    [Header("旋轉位移左右")]
    public int offsetX;
    [Header("旋轉位移上下")]
    public int offsetY;

    [Header("是否能旋轉")]
    public float lengthRotate0l;
    public float lengthRotate0r;
    public float lengthRotate90l;
    public float lengthRotate90r;

    /// <summary>
    /// 紀錄目前射線長度，呼叫長度
    /// </summary>
    private float length;
    private float lengthdown;

    private float lengthRotateR;
    private float lengthRotateL;

    /// <summary>
    /// 是否碰到右邊牆壁，是打勾，否取消打勾
    /// </summary>
    public bool wallRight;
    public bool wallLeft;
    public bool wallBottom;

    /// <summary>
    /// 是否能旋轉
    /// </summary>
    public bool canRotate = true;

    /// <summary>
    /// 寫位移用的
    /// </summary>
    private RectTransform rect;

    [Header("每一顆小方塊的射線長度"), Range(0f, 2f)]
    public float smallLength = 0.5f;

    #endregion


    #region 事件


    private void settingLength()
    {
        #region 判定牆壁地板
        //將角度原設定浮點數，去小數點轉換成整數
        int Z = (int)transform.eulerAngles.z;

        //因應不同角度的方塊，會有不同長度的線條以做判定
        if (Z == 0 || Z == 180)
        {
            length = length0;//左右線條的初始值，是角度0的長度

            lengthdown = length90;//向下的初始值，是角度90的長度

            //設定旋轉
            lengthRotateL = lengthRotate0l;
            lengthRotateR = lengthRotate0r;

        }

        else if (Z == 90 || Z == 270)
        {
            length = length90;//左右線條的初始值，是角度90的長度

            lengthdown = length0;//向下的初始值，是角度0的長度
           
            //設定旋轉
            lengthRotateL = lengthRotate90l;
            lengthRotateR = lengthRotate90r;
            
        }

        #endregion
    }

    /// <summary>
    /// ODG為繪製圖飾，繪製一條線讓其判定牆在哪
    /// </summary>
    private void OnDrawGizmos()
    {
        #region 判定牆壁地板
        //將角度原設定浮點數，去小數點轉換成整數
        int Z = (int)transform.eulerAngles.z;

        //因應不同角度的方塊，會有不同長度的線條以做判定
        if (Z==0 || Z==180)
        {
            length = length0;//左右線條的初始值，是角度0的長度

            Gizmos.color = Color.red;//圖示的顏色
            Gizmos.DrawRay(transform.position, Vector3.right * length0);//圖示的繪製射線(起點，方向)

            //設定左邊
            Gizmos.color = Color.yellow;//圖示的顏色
            Gizmos.DrawRay(transform.position, -Vector3.right * length0);//圖示的繪製射線(起點，方向)

            lengthdown = length90;//向下的初始值，是角度90的長度

            //設定下方
            Gizmos.color = Color.magenta;//圖示的顏色
            Gizmos.DrawRay(transform.position, -Vector3.up * length90);//圖示的繪製射線(起點，方向)

            //設定旋轉
            lengthRotateL = lengthRotate0l;
            lengthRotateR = lengthRotate0r;
            Gizmos.color = Color.blue;//圖示的顏色
            Gizmos.DrawRay(transform.position, Vector3.right * lengthRotate0l);//圖示的繪製射線(起點，方向)
            Gizmos.DrawRay(transform.position, Vector3.right * lengthRotate0r);//圖示的繪製射線(起點，方向)

        }

        else if (Z == 90 || Z == 270)
        {
            length = length90;//左右線條的初始值，是角度90的長度
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.right * length90);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -Vector3.right * length90);

            lengthdown = length0;//向下的初始值，是角度0的長度
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, -Vector3.up * length0);

            //設定旋轉
            lengthRotateL = lengthRotate90l;
            lengthRotateR = lengthRotate90r;
            Gizmos.color = Color.blue;//圖示的顏色
            Gizmos.DrawRay(transform.position, Vector3.right * lengthRotate90l);//圖示的繪製射線(起點，方向)
            Gizmos.DrawRay(transform.position, Vector3.right * lengthRotate90r);//圖示的繪製射線(起點，方向)

        }

        #endregion

        #region 小方塊判定

        for (int i = 0; i < transform.childCount; i++) //下方射線
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.down * smallLength);
        }

        for (int i = 0; i < transform.childCount; i++) //左方射線
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.left * smallLength);
        }

        for (int i = 0; i < transform.childCount; i++) //右方射線 +左方射線
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.right * smallLength);
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.left * smallLength);
        }

        #endregion

    }


    #endregion

    

    private void Start()
    {
        //儲存遊戲開始角度是0的長度
        length = length0;

        rect = GetComponent<RectTransform>();

        smallRightAll = new bool[transform.childCount]; //用陣列寫
        smallLeftAll = new bool[transform.childCount]; //用陣列寫
        
    }

    private void Update()
    {
        CheckWall();
        CheckBottom();
        CheckLeftAndRight();

        settingLength();
    }

    #region 方法

    /// <summary>
    /// 小方塊底部碰撞
    /// </summary>
    public bool smallBottom;

    public bool smallLeft;
    public bool smallRight;

    /// <summary>
    /// 所有方塊左右邊是否有其他方塊之設定
    /// </summary>
    public bool[] smallLeftAll;
    public bool[] smallRightAll;

    /// <summary>
    /// 設定每小顆的檢查，下方
    /// </summary>
    private void CheckBottom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(i).position, Vector3.down, smallLength, 1 << 10);
            //print(hitR.collider.name);
            if (hit && hit.collider.name == "方塊") smallBottom = true;
        }
    }

    /// <summary>
    /// 設定每小顆的檢查，左右
    /// </summary>
    private void CheckLeftAndRight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RaycastHit2D hitL = Physics2D.Raycast(transform.GetChild(i).position, Vector3.left, smallLength, 1 << 10);
            //print(hitR.collider.name);
            if (hitL && hitL.collider.name == "方塊")
                smallLeftAll[i] = true;
            else smallLeftAll[i] = false;

            RaycastHit2D hitR = Physics2D.Raycast(transform.GetChild(i).position, Vector3.right, smallLength, 1 << 10);
            //print(hitR.collider.name);

            //用陣列處理每個方塊右邊
            if (hitR && hitR.collider.name == "方塊")
                smallRightAll[i] = true; //右邊有方塊，陣列對應的格子打勾
            else smallRightAll[i] = false;
        }

        //檢查陣列內 等於 true的資料
        //語法：陣列.哪裡(代名詞=>條件)
        //var 無類型
        var allLeft = smallLeftAll.Where(x => x == true);
        //print(allRight.ToArray().Length); //轉為陣列.數量
        smallLeft = allLeft.ToArray().Length > 0;

        
        //檢查陣列內 等於 true的資料
        //語法：陣列.哪裡(代名詞=>條件)
        //var 無類型
        var allRight = smallRightAll.Where(x => x == true);
        //print(allRight.ToArray().Length); //轉為陣列.數量
        smallRight = allRight.ToArray().Length > 0;

    }


    /// <summary>
    /// 設定牆壁檢查
    /// </summary>
    private void CheckWall()
    {
        //為此串命名RaycastHit2D為區域變數hit
        //語法：2D射線碰撞資訊 區域變數名稱=
        //API:2D物理.射線碰撞(起點，方向，長度，圖層(8為在unity設定的圖層))
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, length, 1 << 8);

        //print(hit.transform.name);

        //hit &&並且 hit的名字為wall_R
        if (hit&&hit.transform.name == "wall_R")
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }


        RaycastHit2D hitL = Physics2D.Raycast(transform.position, -Vector3.right, length, 1 << 8);
        if (hitL && hitL.transform.name == "wall_L")
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }

        
        //旋轉判定
        RaycastHit2D hitRotateR = Physics2D.Raycast(transform.position, Vector3.right, lengthRotateR, 1 << 8);
        RaycastHit2D hitRotateL = Physics2D.Raycast(transform.position, -Vector3.right, lengthRotateL, 1 << 8);
        if(hitRotateR && hitRotateR.transform.name == "wall_R"|| hitRotateL && hitRotateL.transform.name == "wall_L")
        {
            canRotate = false;
        }
        else
        {
            canRotate = true;
        }

        RaycastHit2D hitb = Physics2D.Raycast(transform.position, -Vector3.up, lengthdown, 1 << 9);
        if (hitb && hitb.transform.name == "wall_bottom")
        {
            wallBottom = true;
        }
        else
        {
            wallBottom = false;
        }

    }

    public void Offset()
    {
        //將角度原設定浮點數，去小數點轉換成整數
        int Z = (int)transform.eulerAngles.z;

        if (Z==90||Z==270)
        {
            rect.anchoredPosition -= new Vector2(offsetX, offsetY);
        }
        else if (Z == 0 || Z == 180)
        {
            rect.anchoredPosition += new Vector2(offsetX, offsetY);
        }

    }

    #endregion

}
