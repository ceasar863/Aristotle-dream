using UnityEngine;
using System;

namespace Badtime
{
    [Serializable]
    public class Attack_Movements : Attack_Data
    {
        [field: SerializeField] public Vector2 direction { get; private set; }
        [field: SerializeField] public float velocity { get; private set; }
    }
}
