using UnityEngine;

namespace Badtime
{
    public class Knock_Back_Component_Data :Component_Data<Attack_Knock_Back>
    {
        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Knock_Back);
        }
    }
}
