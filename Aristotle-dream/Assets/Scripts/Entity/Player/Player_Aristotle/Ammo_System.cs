using System.Threading;
using UnityEngine;

public class Ammo_System : MonoBehaviour
{
    private Entity_Player_Aristotle player_aristotle = Entity_Player_Aristotle.instance;
    [SerializeField] private Bullet bullet_prefab;
    [SerializeField] private Bullet current_bullet;
    public GameObject crosshair;

    private void Awake()
    {
        current_bullet = null;
        Generate_Bullet();
    }

    private void Reinforce_Bullet()//加速器机制，随着装在弹匣的时间越长,子弹威力和其特性越强
    {
        if (current_bullet == null) return;
        current_bullet.Reinforce();
    }

    private void Update()
    {
        Reinforce_Bullet();

    }

    public Bullet Get_Current_Bullet()
    {
        if (current_bullet == null)
        {
            //Debug.Log("弹匣内无子弹!");
            return null;
        }
        return current_bullet;
    }

    public void Shoot_Bullet(Vector2 start_point)
    {
        current_bullet.gameObject.SetActive(true);
        current_bullet.transform.position = start_point;
        current_bullet.Set_Parameter();
        
    }

    public void Generate_Bullet()
    {
        current_bullet = GameObject.Instantiate(bullet_prefab);
        current_bullet.gameObject.SetActive(false);
    }

    public void Set_Bullet(Bullet bullet)//拾取时实例化
    {
        if (bullet_prefab != null && current_bullet == null)//Debug用的，正常游戏没这个
        {
            current_bullet = GameObject.Instantiate(bullet);
            current_bullet.gameObject.SetActive(false);
        }
    }

    public void Clear_Bullet()
    {
        current_bullet = null;
    }
}
