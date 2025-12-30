using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Attack_Poise_Damage : Attack_Data
    {
        [field: SerializeField] public float amount { get; private set; }
    
    }
}
