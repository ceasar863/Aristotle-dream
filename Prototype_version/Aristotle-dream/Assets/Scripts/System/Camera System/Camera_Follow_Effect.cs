using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Camera_Follow_Effect : MonoBehaviour
{
    public static Camera_Follow_Effect instance;

    [SerializeField] private CinemachineCamera[] cameras;

    [Header("Follow Object")]
    [SerializeField] private Transform follow_target;

    [Header("Follow Velocity")]
    [SerializeField] private float follow_speed_time;

    [Header("Falling And Uping Follow Effect Velocity")]
    [SerializeField] private float falling_damping;
    [SerializeField] private float normal_damping;
    [SerializeField] private float conversion_duration;

    public CinemachineCamera current_cinemachine_camera;
    
    private Player_Aristotle player_aristotle;
    private CinemachinePositionComposer position_composer;

    private Coroutine turn_follow_co;
    private Coroutine falling_and_uping_adjust_co;
    private Coroutine pan_camera_co;

    private Vector2 starting_tracked_object_offset;

    private bool is_facing_right;
    private bool is_falling;
    private bool is_uping;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        player_aristotle = follow_target.GetComponent<Player_Aristotle>();
        position_composer = current_cinemachine_camera.GetComponent<CinemachinePositionComposer>();
        is_facing_right = player_aristotle.Get_Parameter<bool>(Entity_Attribute.is_facing_right);
    }

    private void Start()
    {
        starting_tracked_object_offset = position_composer.Composition.ScreenPosition;
    }

    private void Update()
    {
        transform.position = follow_target.position;
        is_falling = player_aristotle.Get_Parameter<bool>(Entity_Attribute.is_falling);
        is_uping = player_aristotle.Get_Parameter<bool>(Entity_Attribute.is_uping);
    }

    #region 上下移动时候的镜头偏移，往上的时候平滑增加阻尼，往下的时候阻尼减小紧跟人物增加“坠落感”
    public void Falling_And_Uping_Adjust(bool is_player_falling)
    {
        if (falling_and_uping_adjust_co != null) StopCoroutine(falling_and_uping_adjust_co);
        falling_and_uping_adjust_co = StartCoroutine(Falling_And_Uping_Adjust_Co(is_player_falling));
    }

    private IEnumerator Falling_And_Uping_Adjust_Co(bool is_player_falling)
    {
        float start_damp = position_composer.Damping.y;
        float end_damp = 0f;

        if (is_player_falling) end_damp = falling_damping;
        else end_damp = normal_damping;

        float elapsed_time = 0f;
        while(elapsed_time < conversion_duration)
        {
            elapsed_time += Time.deltaTime;

            float y_damp = Mathf.Lerp(start_damp, end_damp, (elapsed_time / conversion_duration));
            position_composer.Damping.y = y_damp;
            yield return null;
        }
    }
    #endregion

    #region 把镜头往人物面朝方向偏移一部分
    public void Start_Follow()
    {
        if (turn_follow_co != null) StopCoroutine(turn_follow_co);
        turn_follow_co = StartCoroutine(Follow_Object());
    }

    private IEnumerator Follow_Object()
    {
        float start_rotation_amount = transform.localEulerAngles.y;
        float end_rotation_amount = Determine_End_Rotation();
        float y_rotation;

        float elapsed_time = 0f;
        while(elapsed_time<follow_speed_time)
        {
            elapsed_time += Time.deltaTime;

            y_rotation = Mathf.Lerp(start_rotation_amount, end_rotation_amount, (elapsed_time / follow_speed_time));
            transform.rotation = Quaternion.Euler(0f, y_rotation, 0f);//直接赋值不行
            yield return null;
        }
    }

    private float Determine_End_Rotation()
    {
        is_facing_right = !is_facing_right;

        if (!is_facing_right) return 180f;
        else return 0f;
    }
    #endregion

    #region 人物到悬崖边上的时候镜头会自动往下移动[下面的功能上下左右移动镜头都可以办到的喵:>]
    public void Pan_Camera_On_Contact(float pan_distance , float pan_time , Pan_Direction pan_direction , bool pan_to_starting_pos)
    {
        if (pan_camera_co != null) StopCoroutine(pan_camera_co);
        pan_camera_co = StartCoroutine(Pan_Camera(pan_distance , pan_time , pan_direction , pan_to_starting_pos));
    }

    private IEnumerator Pan_Camera(float pan_distance, float pan_time, Pan_Direction pan_direction, bool pan_to_starting_pos)
    {
        Vector2 end_pos = Vector2.zero;
        Vector2 start_pos = Vector2.zero;

        if(!pan_to_starting_pos)
        {
            switch(pan_direction)
            {
                case Pan_Direction.Up:
                    end_pos = Vector2.up;
                    break;
                case Pan_Direction.Down:
                    end_pos = Vector2.down;
                    break;
                case Pan_Direction.Left:
                    end_pos = Vector2.left;
                    break;
                case Pan_Direction.Right:
                    end_pos = Vector2.right;
                    break;
                default:
                    break;
            }

            end_pos *= pan_distance;
            start_pos = starting_tracked_object_offset;

            end_pos += start_pos;
        }
        else
        {
            start_pos = position_composer.Composition.ScreenPosition;
            end_pos = starting_tracked_object_offset;
        }

        float elapsed_time = 0f;
        while(elapsed_time<pan_time)
        {
            elapsed_time += Time.deltaTime;

            Vector3 pan_lerp = Vector3.Lerp(start_pos, end_pos, (elapsed_time / pan_time));
            position_composer.Composition.ScreenPosition = pan_lerp;
            yield return null;
        }
    }
    #endregion

    #region 切换摄像头
    public void Swap_Camera(CinemachineCamera camera_from_left, CinemachineCamera camera_from_right, Vector2 trigger_exit_direction)
    {
        if (current_cinemachine_camera == camera_from_left && trigger_exit_direction.x > 0f)
        {
            camera_from_left.enabled = false;
            camera_from_right.enabled = true;
            current_cinemachine_camera = camera_from_right;

            position_composer = current_cinemachine_camera.GetComponent<CinemachinePositionComposer>();
        }
        else if(current_cinemachine_camera == camera_from_right && trigger_exit_direction.x<0f)
        {
            camera_from_right.enabled = false;
            camera_from_left.enabled = true;
            current_cinemachine_camera = camera_from_left ;

            position_composer = current_cinemachine_camera.GetComponent<CinemachinePositionComposer>();
        }
    }
    #endregion
}
