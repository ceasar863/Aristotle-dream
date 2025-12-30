using Badtime;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ammo : Weapon_Component
{
    private Player_Aristotle player;
    private GameObject bullet_prefab;
    private Bullet current_bullet;
    private Vector2 start_point;
    public GameObject crosshair;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        player = Player_Aristotle.instance;
       
        crosshair = weapon.Data.Get_Data<Ammo_Component_Data>().cross_hair;
        crosshair = GameObject.Instantiate(crosshair);
        crosshair.gameObject.SetActive(false);
        
        current_bullet = null;
        anim_handler.On_Shoot_Bullet += Handle_Shoot;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Aim, Map_Type_Enum.Performed, Perform_Aiming);
        Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Aim, Map_Type_Enum.Cancel, Cancel_Aiming);

        Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Grab, Map_Type_Enum.Performed, Perform_Domain_Visible);
        Player_Input_System.instance.Bind_Key_Map(Operation_Mapping_Enum.Grab, Map_Type_Enum.Cancel, Cancel_Domain_Visible);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Player_Input_System.instance.Unbind_Key_Map(Operation_Mapping_Enum.Aim, Perform_Aiming);
        Player_Input_System.instance.Unbind_Key_Map(Operation_Mapping_Enum.Aim, Cancel_Aiming);

        Player_Input_System.instance.Unbind_Key_Map(Operation_Mapping_Enum.Grab, Perform_Domain_Visible);
        Player_Input_System.instance.Unbind_Key_Map(Operation_Mapping_Enum.Grab, Cancel_Domain_Visible);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        anim_handler.On_Shoot_Bullet -= Handle_Shoot;
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

    private void Handle_Shoot()
    {
        if (current_bullet == null) return;

        start_point = player.shoot_point.transform.position;
        Shoot_Bullet(start_point);
        Clear_Bullet();
    }

    public void Shoot_Bullet(Vector2 start_point)
    {
        current_bullet.gameObject.SetActive(true);
        current_bullet.transform.position = start_point;
        current_bullet.Set_Parameter();
    }


    [ContextMenu("---Give_Bullet---")]//Debug用的
    public void Give_Bullet()
    {
        Generate_Bullet(bullet_prefab);
    }

    public void Generate_Bullet(GameObject bullet)
    {
        if (bullet == null) return;

        current_bullet = Bullet_Pool_Manager.instance.Get_Item_Object_From_Pool(bullet, player.transform.position);
        current_bullet.gameObject.SetActive(false);
    }

    public void Set_Bullet(GameObject bullet)//拾取时实例化
    {
        bullet_prefab = bullet;
        if (bullet_prefab != null && current_bullet == null)
            Generate_Bullet(bullet);
    }

    public void Clear_Bullet()
    {
        current_bullet = null;
    }

    public bool Try_Grab_Monster()
    {
        if (Get_Current_Bullet() != null)
        {
            Debug.Log("弹匣内已有子弹");
            return false;
        }

        bool target_valid = false;
        RaycastHit2D target = Physics2D.Raycast(player.transform.position, player.Direction_To_Mouse(), player.grab_radius, player.what_is_grab_target | player.what_is_ground);


        if (target.collider != null)
        {
            if ((1 << target.collider.gameObject.layer) != player.what_is_ground)
            {
                crosshair.transform.position = target.collider.transform.position;
                target_valid = true;
            }
        }
        crosshair.SetActive(target_valid);

        //潜在隐患：和ground的shoot判断可能有冲突！
        if (Player_Input_System.instance.player_input.Player.Shoot.WasPressedThisFrame())
        {
            if (target.collider == null)
            {
                Debug.Log("无可抓取目标");
                return false;
            }

            if ((1 << target.collider.gameObject.layer) == player.what_is_ground)
            {
                Debug.Log("中间有障碍物！");
                return false;
            }

            //播放抓捕成功动画ing...

            //成功抓取转化为弹药补给给玩家
            //Bullet bullet = Bullet_Pool_Manager.instance.Spawn_Item_Object() 
            GameObject bullet = target.collider.gameObject.GetComponentInParent<Enemy>().Was_Grabed();
            Set_Bullet(bullet);

            //被捕获后销毁目标物体
            Enemy_Pool_Manager.instance.Recycle_Item_To_Pool(target.collider.GetComponentInParent<Enemy>());
            //Destroy(target.collider.GetComponentInParent<Enemy>().gameObject);
            crosshair.SetActive(false);
            return true;
        }

        return false;
    }

    private void Perform_Aiming(InputAction.CallbackContext ctx)
    {
        Player_Aristotle.instance.is_aiming = true;
    }

    private void Cancel_Aiming(InputAction.CallbackContext ctx)
    {
        Player_Aristotle.instance.is_aiming = false;
    }

    private void Perform_Domain_Visible(InputAction.CallbackContext ctx)
    {
        Set_Grab_Domain_Visible(true);
    }

    private void Cancel_Domain_Visible(InputAction.CallbackContext ctx)
    {
        Set_Grab_Domain_Visible(false);
    }

    private void Set_Grab_Domain_Visible(bool flag)
    {
        player.entity_center.GetComponent<SpriteRenderer>().enabled = flag;
    }
}
