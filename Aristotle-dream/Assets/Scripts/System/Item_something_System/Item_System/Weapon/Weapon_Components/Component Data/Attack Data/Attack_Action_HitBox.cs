using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Attack_Action_HitBox : Attack_Data
    {
        public bool debug;
        [field: SerializeField] public Rect HitBox { get; private set; }
    }
}
