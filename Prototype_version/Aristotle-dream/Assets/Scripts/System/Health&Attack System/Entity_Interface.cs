using UnityEngine;

public interface IEntity_Interface
{ 
    void Take_Damage(float damage , GameObject attacker = null);
    void Die();
}
