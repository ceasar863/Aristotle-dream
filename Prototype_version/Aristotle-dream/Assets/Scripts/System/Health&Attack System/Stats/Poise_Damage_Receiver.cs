using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Badtime
{
    public class Poise_Damage_Receiver : MonoBehaviour,IPoiseDamageable
    {
        [SerializeField] private Entity_Stats stats;

        public void Damage_Poise(float amount)
        {
            stats.Poise.Decrease(amount);
        }

        protected void Awake()
        {
            stats = GetComponentInChildren<Entity_Stats>();
        }
    }
}
