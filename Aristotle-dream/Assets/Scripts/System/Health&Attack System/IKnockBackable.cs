using UnityEngine;

namespace Badtime
{
    public interface IKnockBackable
    {
        void KnockBack(Vector2 angle , float strength , int direction);
    }
}
