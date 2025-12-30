using System.Threading;
using UnityEngine;

public class Ammo_System : MonoBehaviour
{
    private Player_Aristotle player_aristotle;
    [SerializeField] private GameObject bullet_prefab;
    [SerializeField] private Bullet current_bullet;
    public GameObject crosshair;

    private void Awake()
    {
        
    }

    private void Start()
    {
        player_aristotle = Player_Aristotle.instance;
        current_bullet = null;
        Generate_Bullet(bullet_prefab);
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

    public void Generate_Bullet(GameObject bullet)
    {
        if (bullet == null) return;

        current_bullet = Bullet_Pool_Manager.instance.Get_Item_Object_From_Pool(bullet, player_aristotle.transform.position);
        current_bullet.gameObject.SetActive(false);
    }

    public void Set_Bullet(GameObject bullet)//拾取时实例化
    {
        if (bullet_prefab != null && current_bullet == null)
            Generate_Bullet(bullet);
    }

    public void Clear_Bullet()
    {
        current_bullet = null;
    }

    [ContextMenu("---Give_Bullet---")]
    public void Give_Bullet()
    {
        Generate_Bullet(bullet_prefab);
    }
}
