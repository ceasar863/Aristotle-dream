using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrent_ValueZero;
        [field: SerializeField] public float max_value { get; private set; }
       
        public float current_value
        {
            get => Current_Value;
            private set
            {
                Current_Value = Mathf.Clamp(value, 0f, max_value);

                if(Current_Value<=0f)
                {
                    OnCurrent_ValueZero?.Invoke();
                }
            }
        }
        private float Current_Value;

        public void Init() => current_value = max_value;
        public void Increase(float amount) => current_value += amount;
        public void Decrease(float amount) => current_value -= amount;
    }
}
