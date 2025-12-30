using UnityEngine;

namespace Badtime
{
    public class Knock_Back : Weapon_Component<Knock_Back_Component_Data , Attack_Knock_Back>
    {
        private Action_Hit_Box hitbox;
        private void Handle_Detect_Collider2D(Collider2D[] colliders)
        {
            foreach(Collider2D item in colliders)
            {
                if(item.TryGetComponent(out IKnockBackable knockable))
                {
                    knockable.KnockBack(current_attack_data.angle, current_attack_data.strength, item.GetComponent<Entity>().Get_Parameter<int>(Entity_Attribute.facing_dir));
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            hitbox = GetComponent<Action_Hit_Box>();

            hitbox.On_Detected_Collider2D += Handle_Detect_Collider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            hitbox.On_Detected_Collider2D -= Handle_Detect_Collider2D;
        }
    }
}
