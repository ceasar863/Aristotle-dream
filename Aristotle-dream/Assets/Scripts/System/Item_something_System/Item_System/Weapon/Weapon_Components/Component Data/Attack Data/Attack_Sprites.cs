using System;
using UnityEngine;

namespace Badtime
{
    [Serializable]
    public class Attack_Sprites : Attack_Data
    {
        [field: SerializeField] public Phase_Sprites[] phase_sprites {get; private set;}
    }

    [Serializable]
    public struct Phase_Sprites
    {
        [field: SerializeField] public Attack_Phases Phase { get; private set; }

        //field: → 序列化自动属性的隐藏 backing field；private set → 外部不可改，数据安全
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}
