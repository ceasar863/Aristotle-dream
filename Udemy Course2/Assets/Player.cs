using UnityEngine;

public class Player : MonoBehaviour
{
    private float xInput;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpforce = 8f;
    [SerializeField] private bool isMoving;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
   
    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleAnimations();
    }


    private void HandleAnimations()
    {
        isMoving = rb.linearVelocity.x != 0;
        
        anim.SetBool("isMoving", isMoving);
        Debug.Log($"XËÙ¶È£º{rb.linearVelocity.x} | isMoving£º{isMoving}");
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2 (xInput*moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
    }


}

