using UnityEngine;

namespace Badtime
{
    public class Entity_KnockBack : MonoBehaviour,IKnockBackable
    {
        [SerializeField] private float max_knockback_time = 0.2f;

        private bool is_knockback_active;
        private float knockback_start_time;
        private Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity>();
        }

        private void Update()
        {
            Check_KnockBack();
        }

        public void KnockBack(Vector2 angle, float strength, int direction)
        {
            if (is_knockback_active) return;

            entity.Set_Velocity(strength, angle , direction);
            is_knockback_active = true;
            knockback_start_time = Time.time;
        }

        private void Check_KnockBack()
        {
            if(is_knockback_active && 
               ((entity.Get_Parameter<bool>(Entity_Attribute.is_on_ground) && entity.rb.linearVelocity.y<=0.01f) || 
                Time.time >= knockback_start_time + max_knockback_time)
               )
            {
                is_knockback_active = false;
            }
        }
    }
}
