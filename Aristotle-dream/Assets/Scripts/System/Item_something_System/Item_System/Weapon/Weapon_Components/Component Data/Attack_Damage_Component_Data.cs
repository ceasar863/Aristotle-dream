using UnityEngine;

namespace Badtime
{
    public class Attack_Damage_Component_Data : Component_Data<Attack_Damage>
    {

        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Damage);
        }
    }
}
