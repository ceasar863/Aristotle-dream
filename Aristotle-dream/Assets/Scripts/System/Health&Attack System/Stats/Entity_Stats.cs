using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Entity_Stats : MonoBehaviour,IPoiseDamageable
    {
        [field : SerializeField] public Stat Health { get; private set; }
        [field: SerializeField] public Stat Poise { get; private set; }

        [SerializeField] private float poise_recovery_rate;

        protected virtual void Awake()
        {
            Health.Init();
            Poise.Init();
        }

        private void Update()
        {
            if (Poise.current_value.Equals(Poise.max_value))
                return;

            Poise.Increase(poise_recovery_rate * Time.deltaTime);
        }

        public void Damage_Poise(float amount)
        {
  
            Poise.Decrease(amount);
        }
    }
}
