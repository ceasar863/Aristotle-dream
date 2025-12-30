using UnityEngine;

namespace Badtime
{
    public class Movement : Weapon_Component<Attack_Movement_Component_Data , Attack_Movements>
    {
        private void Handle_Start_Movement()
        {
            Player_Aristotle.instance.Set_Velocity(current_attack_data.velocity, current_attack_data.direction , Player_Aristotle.instance.Get_Parameter<int>(Entity_Attribute.facing_dir));
        }

        private void Handle_Stop_Movement()
        {
            Player_Aristotle.instance.Set_Velocity_Zero();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            anim_handler.On_Start_Movement += Handle_Start_Movement;
            anim_handler.On_Stop_Movement += Handle_Stop_Movement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            anim_handler.On_Start_Movement -= Handle_Start_Movement;
            anim_handler.On_Stop_Movement -= Handle_Stop_Movement;
        }
    }
}
