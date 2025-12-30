using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Badtime;

public class Weapon : Item
{
    #region 暂时别管
    protected List<Opera_Action> weapon_band; //需要绑定的函数集合
    #endregion

    //专门的数据存储类
    [Space(20)]
    [SerializeField] private float attack_count_reset_cooldown;

    public event Action on_enter;//与Weapon_Sprite互动通知是否处于攻击状态
    public event Action on_exit;//与攻击状态节点互动通知动画结束
    public Weapon_Data_So Data { get; private set; }
    
    public GameObject base_game_object { get; private set; }//其上有动画控制器相关组件
    public GameObject weapon_sprite_gameobject { get; private set; }//其上有渲染相关组件

    public Animation_Trigger anim_handler { get; private set; }
    private Animator anim;
    private Timer attack_count_timer;
    private int Current_attack_counter;
    private bool Current_Input;

    public event Action<bool> On_Current_Input_Change;
    public bool current_input//输入来源于玩家类 
    {
        get => Current_Input;
        set
        {
            if(Current_Input != value)
            {
                Current_Input = value;
                On_Current_Input_Change?.Invoke(Current_Input);
            }
        }
    }

    public int current_attack_counter
    {
        get => Current_attack_counter;

        //value是赋值的时候传进来的数，比如a=5，这个5就是value!
        private set=>Current_attack_counter = (value >= Data.number_of_attacks)? 0 : value;   
    }

    protected override void Awake()
    {
        base.Awake();
        
        base_game_object = transform.Find("Base").gameObject;
        weapon_sprite_gameobject = transform.Find("Weapon_Sprite").gameObject;

        anim = base_game_object.GetComponent<Animator>();
        anim_handler = base_game_object.GetComponent<Animation_Trigger>();

        attack_count_timer = new Timer(attack_count_reset_cooldown);
    }

    public void Enter()
    {
        //Debug.Log("Use Weapon!");
        attack_count_timer.Set_Timer_Active(false);//进入攻击状态禁用定时器回调功能,有的攻击动画可能会比计时器间隔要长
       
        anim.SetBool("Active", true);
        anim.SetInteger("Count", current_attack_counter);

        on_enter?.Invoke();
    }

    protected override void Update()
    {
        base.Update();
        attack_count_timer.Tick();
    }

    public void Set_Data(Weapon_Data_So data)
    {
        Data = data;
    }

    //疑问：教程为什么不直接绑on_exit，我暂且蒙在鼓里
    //好吧我知道了，离开前也有一些必要的变量设置
    private void Exit()
    {
        anim.SetBool("Active", false);
        current_attack_counter++;
        attack_count_timer.Start_Timer();//武器攻击结束后重新启用定时器
        on_exit?.Invoke();
    }

    private void OnEnable()
    {
        anim_handler.On_Finished += Exit;
        attack_count_timer.on_timer_done += Reset_Attack_Counter;
    }

    private void OnDisable()
    {
        anim_handler.On_Finished -= Exit;
        attack_count_timer.on_timer_done -= Reset_Attack_Counter;
    }

    private void Reset_Attack_Counter() => current_attack_counter = 0;

    #region 暂时别管:p
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public virtual void Equip()
    {
        foreach (Opera_Action item in weapon_band)
            Player_Input_System.instance.Bind_Key_Map(item.opera_type, item.map_type, item.action);
    }

    public virtual void Unequip()
    {
        foreach (Opera_Action item in weapon_band)
            Player_Input_System.instance.Unbind_Key_Map(item.opera_type, item.action);
    }
    #endregion
}

public class Opera_Action
{
    public Operation_Mapping_Enum opera_type;
    public Map_Type_Enum map_type;
    public Action<InputAction.CallbackContext> action;
}