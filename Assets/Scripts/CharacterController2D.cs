using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private int ANIMATION_SPEED;
    private int ANIMATION_FORCE;
    private int ANIMATION_FALL;
    private int ANIMATION_ATTACK;
    private int ANIMATION_SUPER;
    private int ANIMATION_DEAD;

    [Header("Movement")]
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float gravityMultiplier;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Vector2 groundCheckSize;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    bool isFacingRight;

    [Header("Attack")]
    [SerializeField]
    Transform punchpoint;

    [SerializeField]
    float punchRadius;

    [SerializeField]
    LayerMask attackMask;

    [Header("Death")]
    [SerializeField]
    float dieDelay;

    [SerializeField]
    int playerHealth = 100; // Suponiendo que el jugador tiene una salud inicial de 100

    [SerializeField]
    string gameOverSceneName = "Last-Scene"; // Nombre de la escena de Game Over

    [Header("Projectile")]
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    Transform projectilePoint;

    [SerializeField]
    float projectileLifeTime;

    Rigidbody2D _rigidbody;
    Animator _animator;

    float _inputX;
    float _gravityY;
    float _velocityY;

    bool _isGrounded;
    bool _isJumpPressed;
    bool _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _gravityY = Physics2D.gravity.y;

        ANIMATION_SPEED = Animator.StringToHash("speed");
        ANIMATION_FORCE = Animator.StringToHash("force");
        ANIMATION_FALL = Animator.StringToHash("fall");
        ANIMATION_ATTACK = Animator.StringToHash("attack");
        ANIMATION_SUPER = Animator.StringToHash("super");
        ANIMATION_DEAD = Animator.StringToHash("dead");
    }

    private void Start()
    {
        HandleGrounded();
    }

    private void Update()
    {
        HandleGravity();
        HandleInputMove();

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleRotate();
        HandleMove();
    }

    private void HandleGrounded()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded)
        {
            StartCoroutine(WaitForGroundedCoroutine());
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_velocityY < -1.0F)
            {
                _velocityY = -1.0F;
            }

            HandleInputJump();
        }
    }

    private void HandleInputJump()
    {
        _isJumpPressed = Input.GetButton("Jump");
    }

    private void HandleInputMove()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }

    private void HandleJump()
    {
        if (_isJumpPressed)
        {
            _isJumpPressed = false;
            _isGrounded = false;
            _isJumping = true;

            _velocityY = jumpForce;

            _animator.SetTrigger(ANIMATION_FORCE);

            StartCoroutine(WaitForGroundedCoroutine());
        }
        else if (!_isGrounded)
        {
            _velocityY += _gravityY * gravityMultiplier * Time.fixedDeltaTime;
            if (!_isJumping)
            {
                _animator.SetTrigger(ANIMATION_FALL);
            }
        }
        else if (_isGrounded)
        {
            if (_velocityY >= 0.0F)
            {
                _velocityY = -1.0F;
            }
            else
            {
                HandleGrounded();
            }

            _isJumping = false;
        }
    }

    private void HandleMove()
    {
        float speed = _inputX != 0.0F ? 1.0F : 0.0F;
        float animatorSpeed = _animator.GetFloat(ANIMATION_SPEED);

        if (speed != animatorSpeed)
        {
            _animator.SetFloat(ANIMATION_SPEED, speed);
        }

        Vector2 velocity = new Vector2(_inputX, 0.0F) * walkSpeed * Time.fixedDeltaTime;
        velocity.y = _velocityY;

        _rigidbody.velocity = velocity;
    }

    private void HandleRotate()
    {
        if (_inputX == 0.0F)
        {
            return;
        }

        bool facingRight = _inputX > 0.0F;
        if (isFacingRight != facingRight)
        {
            isFacingRight = facingRight;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }
    }

    private bool IsGrounded()
    {
        Collider2D collider2D =
            Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.0F, groundMask);
        return collider2D != null;
    }

    private IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        _isGrounded = true;
    }

    public void Punch()
    {
        _animator.SetTrigger(ANIMATION_ATTACK);
    }

    public void Punch(float damage, bool isPercentage)
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(punchpoint.position, punchRadius, attackMask);

        foreach (Collider2D collider in colliders)
        {
            DamageableController controller = collider.GetComponent<DamageableController>();
            if (controller != null)
            {
                continue;
            }

            controller.TakeDamage(damage, isPercentage);
        }
    }

    public void Super()
    {
        _animator.SetTrigger(ANIMATION_SUPER);
    }

    public void Super(float damage, bool isPercentage)
    {
        GameObject projectile =
            Instantiate(projectilePrefab, projectilePoint.position, transform.rotation);
        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        controller.Go(damage, isPercentage);
        Destroy(projectile, projectileLifeTime);
    }

    // Método para manejar la muerte del personaje
    public void Die()
    {
        // Reproducir la animación de muerte
        _animator.SetTrigger(ANIMATION_DEAD);

        // Desactivar el control del personaje
        this.enabled = false;

        // Iniciar la corutina para esperar un tiempo antes de cargar la escena de Game Over
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        // Esperar un tiempo antes de cargar la escena de Game Over
        yield return new WaitForSeconds(dieDelay);

        // Cargar la escena de Game Over
        SceneManager.LoadScene(gameOverSceneName);
    }

    // Ejemplo de reducción de salud (llamar este método cuando el personaje reciba daño)
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            Die();
        }
    }
}
