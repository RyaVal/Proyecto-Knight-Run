using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    bool isFacingRight;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundMask;

    private Rigidbody2D _rigidbody;
    public Animator animator;

    private void Start()
    {
        animator.SetBool("Corriendo", true);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit =
            Physics2D.Raycast(groundCheck.position, Vector2.down, 0.40F, groundMask);

        if (!raycastHit)
        {
            speed *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }

        Vector2 velocity = Vector2.right * speed * Time.fixedDeltaTime;
        velocity.y = _rigidbody.position.y;
        _rigidbody.velocity = velocity;


    }
}
