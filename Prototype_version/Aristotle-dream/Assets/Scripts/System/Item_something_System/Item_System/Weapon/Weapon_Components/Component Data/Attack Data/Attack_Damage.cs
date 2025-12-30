using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Attack_Damage : Attack_Data
    {
        [field: SerializeField] public float Amount { get; private set; }
        
    }
}
