using UnityEngine;

namespace Badtime
{
    public class Damage : Weapon_Component<Attack_Damage_Component_Data , Attack_Damage>
    {
        private Action_Hit_Box hitbox;
        private void Handle_Detect_Collider2D(Collider2D[] colliders)
        {
            foreach(var item in colliders)
            {
                if(item.TryGetComponent(out IEntity_Interface damageable))
                {
                    damageable.Take_Damage(current_attack_data.Amount);
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
