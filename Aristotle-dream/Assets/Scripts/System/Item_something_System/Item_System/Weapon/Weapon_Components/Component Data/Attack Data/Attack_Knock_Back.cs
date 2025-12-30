using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Attack_Knock_Back : Attack_Data
    {
        [field: SerializeField] public Vector2 angle { get;private set; }
        [field: SerializeField] public float strength { get; private set; }

        
    }
}
