using UnityEngine;

namespace Badtime
{
    public class Poise_Damage_Data : Component_Data<Attack_Poise_Damage>
    {
        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Poise_Damage);
        }
    }
}
