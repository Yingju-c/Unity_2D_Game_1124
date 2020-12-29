using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("角度為0或180，線條的長度")]
    public float length0;
    [Header("角度為90或270，線條的長度")]
    public float length90;

    [Header("旋轉位移左右")]
    public int offsetX;
    [Header("旋轉位移上下")]
    public int offsetY;

    /// <summary>
    /// 紀錄目前射線長度
    /// </summary>
    private float length;
    private float lengthdown;

    /// <summary>
    /// ODG為繪製圖飾，繪製一條線讓其判定牆在哪
    /// </summary>
    private void OnDrawGizmos()
    {

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
        }
        
    }

    private void Start()
    {
        //儲存遊戲開始角度是0的長度
        length = length0;
    }

    private void Update()
    {
        CheckWall();
    }

    /// <summary>
    /// 是否碰到右邊牆壁，是打勾，否取消打勾
    /// </summary>
    public bool wallRight;
    public bool wallLeft;
    public bool wallBottom;

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

}
