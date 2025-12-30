using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Badtime
{
    public class Action_Hit_Box : Weapon_Component<Attack_Action_HitBox_Component_Data , Attack_Action_HitBox>
    {
        public event Action<Collider2D[]> On_Detected_Collider2D;

        private Vector2 offset;
        private Collider2D[] detected;

        private void Handle_Attack_Action()
        {
            offset.Set
                (
                    transform.position.x + (current_attack_data.HitBox.center.x * Player_Aristotle.instance.Get_Parameter<int>(Entity_Attribute.facing_dir)),
                    transform.position.y + current_attack_data.HitBox.center.y
                );

            detected = Physics2D.OverlapBoxAll(offset, current_attack_data.HitBox.size, 0f, data.Detectable_LayerMasks);

            if (detected.Length == 0) return;
            On_Detected_Collider2D?.Invoke(detected);
        }

        protected override void Start()
        {
            base.Start();
            anim_handler.On_Attack_Action += Handle_Attack_Action;
        }

        protected override void OnEnable()
        {
           
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            anim_handler.On_Attack_Action -= Handle_Attack_Action;
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null) return;

            foreach (var item in data.Attack_Data)
            {
                if (item.debug == false) continue;

                Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
            }
        }
    }
}
