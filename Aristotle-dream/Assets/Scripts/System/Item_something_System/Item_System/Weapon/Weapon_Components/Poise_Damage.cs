using UnityEngine;

namespace Badtime
{
    public class Poise_Damage : Weapon_Component<Poise_Damage_Data , Attack_Poise_Damage>
    {
        private Action_Hit_Box hitbox;

        private void Handle_Detect_Collider2D(Collider2D[] colliders)
        {
            foreach(Collider2D item in colliders)
            {
                if(item.TryGetComponent(out IPoiseDamageable poise_damageable))
                {
                    poise_damageable.Damage_Poise(current_attack_data.amount);
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
