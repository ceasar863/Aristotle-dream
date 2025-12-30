using UnityEngine;

namespace Badtime
{
    public class Input_Hold_Data : Component_Data
    {
        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Input_Hold);
        }
    }
}
