using UnityEngine;

namespace Badtime
{
    public class Ammo_Component_Data : Component_Data
    {
        [field:SerializeField] public GameObject cross_hair { get; private set; }
        protected override void Set_Component_Depencency()
        {
            component_dependency = typeof(Ammo);
        }
    }
}
