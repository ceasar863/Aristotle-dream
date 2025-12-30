using UnityEngine;
using System;

namespace Badtime
{
    public class Attack_Movement_Component_Data : Component_Data<Attack_Movements>
    {

        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Movement);
        }
    }
}
