using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health_System : MonoBehaviour
{
    [SerializeField] private Slider red_bar;
    [SerializeField] private Slider white_bar;

    [Header("Health Details")]
    [SerializeField] private float white_bar_wait;
    [SerializeField] private float white_bar_change_speed;

    private RectTransform red_bar_rect;
    private RectTransform white_bar_rect;
    private Vector3 red_bar_infor;
    private Vector3 white_bar_infor;

    private Coroutine white_bar_changeCo;

    private void OnEnable()
    {
        Event_Center.Add_Listener<float>(Event_Type.Change_Health_Bar, Change_Health_Bar);
    }

    private void OnDisable()
    {
        Event_Center.Remove_Listener<float>(Event_Type.Change_Health_Bar, Change_Health_Bar);   
    }

    private void Awake()
    {
        red_bar_rect = red_bar.GetComponent<RectTransform>();
        white_bar_rect = white_bar.GetComponent<RectTransform>();

        red_bar_infor = red_bar_rect.localScale;
        white_bar_infor = white_bar_rect.localScale;
    }

    private void LateUpdate()
    {
        //防止血条左右翻转
        red_bar_rect.localScale = red_bar_infor;
        white_bar_rect.localScale = white_bar_infor;

        //防止血条旋转
        red_bar_rect.localRotation = Quaternion.identity;
        white_bar_rect.localRotation = Quaternion.identity;

        red_bar_rect.rotation = Quaternion.identity;
        white_bar_rect.rotation = Quaternion.identity;
    }

    public void Change_Health_Bar(float target_percent)
    {
        Change_Red_Bar(target_percent);
        Change_White_Bar(target_percent);
    }

    public void Change_Red_Bar(float target_percent)
    {
        red_bar.value = target_percent;
    }

    public void Change_White_Bar(float target_percent)
    {
        if (white_bar_changeCo != null) StopCoroutine(white_bar_changeCo);
        StartCoroutine(White_Bar_ChangeCo(target_percent));
    }

    private IEnumerator White_Bar_ChangeCo(float target_percent)
    {
        yield return new WaitForSeconds(white_bar_wait);

        while (Mathf.Abs(white_bar.value - target_percent) > 0.01f)
        {
            float delta_value = Time.deltaTime * white_bar_change_speed;
            white_bar.value = Mathf.Lerp(white_bar.value, target_percent, delta_value);
            yield return null;//防止在一帧内无限循环
        }
        white_bar.value = target_percent;
    }
}
