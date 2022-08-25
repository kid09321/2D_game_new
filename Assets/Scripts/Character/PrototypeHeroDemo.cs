using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrototypeHeroDemo : MonoBehaviour {

    [Header("Variables")]
    [SerializeField] float      m_maxSpeed = 4.5f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_dashMultiplier = 10f;
    [SerializeField] float      m_dashCoolDown = 3f;
    [SerializeField] float      m_dashDuration = 0.2f;
    [SerializeField] bool       m_hideSword = false;
    [Header("Effects")]
    [SerializeField] GameObject m_RunStopDust;
    [SerializeField] GameObject m_JumpDust;
    [SerializeField] GameObject m_LandingDust;
    [Header("Test for changing appearance")]
    [SerializeField] Creature   m_creatureData;
    [Header("WallSlide")]
    [SerializeField] Transform  m_wallCheck;
    [SerializeField] float      m_radius = 10f;
    [SerializeField] LayerMask  m_groundLayer;
    [SerializeField] float      m_wallSlideSpeed = 3f;
    [SerializeField] float      m_wallJumpForce = 3f;
    [SerializeField] float      m_wallSlideJumpDuration = 0.1f;
    [SerializeField] List<TrailRenderer> m_trailRenderers;
    [Header("Properties")]
    [SerializeField] int      m_maxHealth = 1000;
    [Header("UI")]
    [SerializeField] HealthBar  m_healthBar;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Prototype    m_groundSensor;
    private AudioSource         m_audioSource;
    private AudioManager_PrototypeHero m_audioManager;
    private bool                m_grounded = false;
    private bool                m_moving = false;
    private bool                m_dashing = false;
    private bool                m_attacking = false;
    private bool                m_movementLocked = false;
    private int                 m_facingDirection = 1;
    private float               m_disableMovementTimer = 0.0f;
    private float               m_dashCoolDownTimer = 0.0f;
    private float               m_dashDurationTimer = 0.0f;
    private int                 m_defaultFacingDirection = 1;

    // Used for change appearance
    private bool                m_changingAppearance = false;
    private SpriteRenderer      m_spriteRenderer;

    // Used for wall slide
    private bool                m_canWallSlide = true;
    private bool                m_wallSlide = false;
    private bool                m_wallJump = false;
    private float               m_wallJumpDuration = 0.5f;
    private float               m_wallSlideJumpDurationTimer = 0.0f; // Use to have to time to jump against wall

    // Used for character properties
    private int               m_currentHealth;
    private bool              m_canDoubleJump = false;

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = AudioManager_PrototypeHero.instance;
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();

        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_currentHealth = m_maxHealth;

    }

    // Update is called once per frame
    void Update ()
    {
        if (!GameManager.Instance.m_canControlPlayer)
        {
            //Stop character;
            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
            m_animator.SetBool("Grounded", m_grounded);
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
            m_animator.SetInteger("AnimState", 0);
            return;
        }
        // Decrease timer that disables input movement. Used when attacking
        m_disableMovementTimer -= Time.deltaTime;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        MovementHandler();

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Set Animation layer for hiding sword
        int boolInt = m_hideSword ? 1 : 0;
        m_animator.SetLayerWeight(1, boolInt);      

        if (!m_movementLocked)
        {                        
            // -- Handle Animations --
            //Jump
            if (Input.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
            {
                Jump();
            }
            else if(Input.GetButtonDown("Jump") && !m_grounded && m_canDoubleJump)
            {
                DoubleJump();
            }

            //Run
            else if (m_moving)
                m_animator.SetInteger("AnimState", 1);

            //Idle
            else
                m_animator.SetInteger("AnimState", 0);

            if (m_dashing)
                m_animator.SetInteger("AnimState", 2);
        }

        // Handle Wall Slide
        WallSlide();
        WallJump();

        // Handle appearance changing
        if (Input.GetKeyDown(KeyCode.C) && !m_changingAppearance)
        {
            ChangeAppearance(m_creatureData);
        }

    }
    // Set functions
    public void SetMovementLock(bool lockMovement)
    {
        m_movementLocked = lockMovement;
    }
    // Handlers
    
    // Movement input handler
    void MovementHandler()
    {
        float inputX = 0.0f;
        float inputRaw = 0.0f;
        if (!m_movementLocked && m_disableMovementTimer < 0.0f)
        {
            inputX = Input.GetAxis("Horizontal");
        }
        // GetAxisRaw returns either -1, 0 or 1
        inputRaw = Input.GetAxisRaw("Horizontal");
        // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == m_facingDirection)
            m_moving = true;

        else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRaw > 0)
        {
            //GetComponent<SpriteRenderer>().flipX = true;
            transform.localScale = new Vector3(m_defaultFacingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            m_facingDirection = 1;
        }

        else if (inputRaw < 0)
        {
            //GetComponent<SpriteRenderer>().flipX = false;
            transform.localScale = new Vector3(-m_defaultFacingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            m_facingDirection = -1;
        }

        // SlowDownSpeed helps decelerate the characters when stopping
        float SlowDownSpeed = m_moving ? 1.0f : 0.5f;
        // Set movement
        m_body2d.velocity = new Vector2(inputX * m_maxSpeed * SlowDownSpeed, m_body2d.velocity.y);
        // Dash
        Dash();
    }
    // Function used to spawn a dust effect
    // All dust effects spawns on the floor
    // dustXoffset controls how far from the player the effects spawns.
    // Default dustXoffset is zero
    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, -0.5f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_facingDirection, 1, 1);
        }
    }

    // Movements
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!m_dashing && m_dashCoolDownTimer <= 0)
            {
                AE_Dash();
                Debug.Log("is dashing");
                m_dashing = true;
                m_dashCoolDownTimer = m_dashCoolDown;
                m_dashDurationTimer = m_dashDuration;
            }
        }
        if (m_dashing)
        {
            if (m_dashDurationTimer > 0)
            {
                m_dashDurationTimer -= Time.deltaTime;
                m_body2d.velocity = new Vector2(m_facingDirection * m_maxSpeed * m_dashMultiplier, 0);
                m_body2d.gravityScale = 0;
                for(int i = 0; i < m_trailRenderers.Count; i++)
                {
                    m_trailRenderers[i].emitting = true;
                }

            }
            else
            {
                m_dashing = false;
                m_body2d.gravityScale = 2;
                for (int i = 0; i < m_trailRenderers.Count; i++)
                {
                    m_trailRenderers[i].emitting = false;
                }
            }
        }
        else
        {
            if (m_dashCoolDownTimer > 0)
            {
                m_dashCoolDownTimer -= Time.deltaTime;
            }
        }
    }

    // Handle Wall Slide and Jump.
    void WallSlide()
    {
        if (m_canWallSlide)
        {
            bool isTouchingWall = Physics2D.OverlapCircle(m_wallCheck.position, m_radius, m_groundLayer);
            if (isTouchingWall && !m_grounded && Input.GetAxisRaw("Horizontal") * m_facingDirection > 0)
            {
                m_wallSlide = true;
                m_wallSlideJumpDurationTimer = m_wallJumpDuration;
            }
            else
            {
                m_wallSlide = false;
                m_animator.SetBool("WallHolding", false);
                m_wallSlideJumpDurationTimer -= Time.fixedDeltaTime;
            }

            if (m_wallSlide)
            {
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, Mathf.Clamp(m_body2d.velocity.y, -m_wallSlideSpeed, float.MaxValue));
                m_animator.SetBool("WallHolding", true);
            }
        }
    }

    void WallJump()
    {
        if(Input.GetButtonDown("Jump") && m_wallSlideJumpDurationTimer > 0 && !m_wallJump)
        {
            m_wallJump = true;
            m_animator.SetBool("WallJumping", true);
            m_animator.SetTrigger("WallJump");
            Invoke("setWallJumpingToFalse", m_wallJumpDuration);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_wallJumpForce);
            m_canDoubleJump = true;
        }
        else if (Input.GetButtonDown("Jump") && m_wallJump && m_canDoubleJump)
        {
            DoubleJump();
        }

    }

    private void setWallJumpingToFalse()
    {
        m_wallJump = false;
        m_animator.SetBool("WallJumping", false);
    }

    private void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_groundSensor.Disable(0.2f);
        m_canDoubleJump = true;
    }
    private void DoubleJump()
    {
        Debug.Log("DoubleJump");
        m_animator.SetTrigger("Jump");
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_groundSensor.Disable(0.2f);
        m_canDoubleJump = false;
    }

    public void Damaged(int damageValue, GameObject attacker)
    {
        // Damage calculation
        m_currentHealth -= damageValue;
        // Damaged Animation
        //m_animator.SetTrigger("Hurt");
        m_healthBar.UpdateHealthBar(m_currentHealth, m_maxHealth);
        //m_rigidBody.velocity = new Vector2(0, m_rigidBody.velocity.y);
        Debug.Log("Player: " + this.gameObject.name + "get hurt!");
        Debug.Log("Player: " + this.gameObject.name + "Current Health: " + m_currentHealth);
    }

    // Change Appearance
    void ChangeAppearance(Creature creatureData)
    {
        Debug.Log("Change Appearance");
        m_changingAppearance = true;
        StartCoroutine(SpriteFadeOut());
        m_changingAppearance = false;
    }

    IEnumerator SpriteFadeOut()
    {
        float secPerLoop = 1.5f / 400f;
        Color tmpColor = m_spriteRenderer.color;
        Debug.Log("Sprite FadeOut");
        for (int i = 255; i >= 0; i--)
        {
            tmpColor.a = i / 255f;
            m_spriteRenderer.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
        m_spriteRenderer.sprite = m_creatureData.Sprite;
        m_animator.runtimeAnimatorController = m_creatureData.AnimatorController;
        m_defaultFacingDirection = m_creatureData.DefaultFacingDirection;
        // Set m_grounded to false to re-detect grounded for animator,
        // or the grounded for animator will be false at the beginning untill
        // jump or other action that off the ground.
        m_grounded = false;
        for (int i = 0; i <= 255; i++)
        {
            tmpColor.a = i / 255f;
            m_spriteRenderer.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
    }

    // Animation Events
    // These functions are called inside the animation files
    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
        // Spawn Dust
        float dustXOffset = 0.6f;
        SpawnDustEffect(m_RunStopDust, dustXOffset);
    }

    void AE_footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");
        // Spawn Dust
        SpawnDustEffect(m_JumpDust);
    }

    void AE_Landing()
    {
        m_audioManager.PlaySound("Landing");
        // Spawn Dust
        SpawnDustEffect(m_LandingDust);
    }

    void AE_AttackEnd()
    {
        Debug.Log("EndAttackEvent Called");
        //m_attackTimeBetweenTimer = m_attackTimeBetween;       
    }

    void AE_Attack1()
    {
        m_audioManager.PlaySound("Swing1");
    }

    void AE_Attack2()
    {
        m_audioManager.PlaySound("Swing1");
    }

    void AE_Attack3()
    {
        m_audioManager.PlaySound("Swing2");
    }

    void AE_Dash()
    {
        m_audioManager.PlaySound("Dash");
    }
}
