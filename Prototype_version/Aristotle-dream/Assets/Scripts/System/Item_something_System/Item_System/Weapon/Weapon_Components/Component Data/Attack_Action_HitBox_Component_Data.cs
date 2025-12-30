using UnityEngine;

namespace Badtime
{
    public class Attack_Action_HitBox_Component_Data : Component_Data<Attack_Action_HitBox>
    {
        [field: SerializeField] public LayerMask Detectable_LayerMasks { get; private set; }


        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Action_Hit_Box);
        }
    }
}
