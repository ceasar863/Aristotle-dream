using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Badtime
{
    public class Weapon_Sprite_Component_Data : Component_Data<Attack_Sprites>
    {

        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Weapon_Sprite);
        }
    }
}
