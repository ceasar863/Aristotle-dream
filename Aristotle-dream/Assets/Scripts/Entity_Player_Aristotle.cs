using UnityEngine;
using UnityEngine.InputSystem;

public class Entity_Player_Aristotle : MonoBehaviour
{

    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>(); 
    }

   
}
