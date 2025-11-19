using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Health_System : UI_Health_System
{
    public TextMeshProUGUI health_text;

    private void Start()
    {
        Update_Health_Text();
    }
    public override void Change_Health_Bar(float target_percent, Transform target)
    {
        base.Change_Health_Bar(target_percent, target);
        Update_Health_Text();
    }

    public void Update_Health_Text()
    {
        float current_health = entity_health.Get_Current_Health();
        float max_health = entity_health.Get_Max_Health();

        health_text.text = $"{current_health}/{max_health}";
    }
}
