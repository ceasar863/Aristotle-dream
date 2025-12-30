using UnityEngine;

namespace Badtime
{
    public class Attack_Data
    {
        [SerializeField , HideInInspector] private string name;
        public void Set_Attack_Name(int i) => name = $"Attack {i}";
    }
}
