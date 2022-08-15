using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    enum EnemyStates
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }
    [Header("Variables")]
    [SerializeField] int        m_maxHealth = 10;
    [SerializeField] float      m_maxSpeed = 30f;
    [SerializeField] float      m_strength = 10f;
    [SerializeField] Creature   m_creatureData;
    [SerializeField] LayerMask  m_groundLayer;
    [SerializeField] Transform  m_groundCheck;
    [SerializeField] Transform  m_groundSensor;
    [SerializeField] float      m_radius = 10f;
    [SerializeField] Collider2D m_bodyCollider;

    [SerializeField] float      m_maxDetectDistance = 5f;
    [SerializeField] float      m_maxChaseSpeed = 60f;
    [SerializeField] Transform  m_playerTransform;

    [SerializeField] GameObject m_blueCrystal;

    [Header("Attack")]
    [SerializeField] Collider2D m_attackRegion;
    [SerializeField] int  m_maxEnemiesToAttack;
    [SerializeField] float m_attackDuration = 0.7f;
    [SerializeField] float m_attackCoolDown = 0.5f;

    private Animator     m_animator;
    private int          m_currentHealth;
    private float        m_currentStrength;
    private EnemyStates  m_currentState;
    private HealthBar    m_healthBar;
    private Rigidbody2D  m_rigidBody;
    private int          m_faceDirection = -1;
    private bool         m_grounded = false;

    private int          m_numberOfBlueCrystal = 5;

    // Used for combat
    private List<Collider2D> m_damagedColliders;
    private bool             m_attacking = false;
    private float            m_attackCoolDownTimer = 0f;
    private float            m_attackDurationTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_healthBar = GetComponentInChildren<HealthBar>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        InitializeProperties();
    }

    // Update is called once per frame
    void Update()
    {
        // Death check first.
        if (DeathCheck()) return;
        //Debug.Log("Current State: " + m_currentState);
        switch (m_currentState)
        {
            case EnemyStates.Patrol:
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
                {
                    Debug.Log("Hurt!");
                }
                if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
                {
                    Patrol();
                }
                break;
            case EnemyStates.Chase:
                ChasePlayer();
                break;
            case EnemyStates.Attack:
                AttackState();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (m_currentState)
        {
            case EnemyStates.Patrol:
                if (!Physics2D.OverlapCircle(m_groundCheck.position, m_radius, m_groundLayer) ||
                    m_bodyCollider.IsTouchingLayers(m_groundLayer))
                {
                    Flip();
                }
                break;
        }
    }
    public void Damaged(int damageValue, GameObject attacker)
    {
        if (m_currentState == EnemyStates.Dead) return;
        // Damage calculation
        m_currentHealth -= damageValue;
        // Damaged Animation
        m_animator.SetTrigger("Hurt");
        m_healthBar.UpdateHealthBar(m_currentHealth, m_maxHealth);
        //m_rigidBody.velocity = new Vector2(0, m_rigidBody.velocity.y);
        Debug.Log("Enemy: " + this.gameObject.name + "get hurt!");
        Debug.Log("Enemy: " + this.gameObject.name + "Current Health: " + m_currentHealth);
    }

    void InitializeProperties()
    {
        m_currentHealth = m_maxHealth;
        m_currentStrength = m_strength;
        m_currentState = EnemyStates.Patrol;

        m_damagedColliders = new List<Collider2D>();
    }

    bool DeathCheck()
    {
        if (m_currentHealth <= 0)
        {
            if (m_currentState != EnemyStates.Dead)
            {
                m_animator.SetTrigger("Death");
                m_currentState = EnemyStates.Dead;
                m_healthBar.gameObject.SetActive(false);
                Drop();
            }
            return true;
        }
        return false;
    }
    void Patrol()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor)
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        m_rigidBody.velocity = new Vector2(m_faceDirection * m_maxSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);
        m_animator.SetInteger("AnimState", 1);
           
        DetectPlayer();
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        m_faceDirection = -m_faceDirection;
    }

    void DetectPlayer()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(m_playerTransform.position.x, m_playerTransform.position.y);
        if (Vector2.Distance(position, playerPosition) < m_maxDetectDistance)
        {
            if(Vector2.Angle(playerPosition - position, new Vector2(m_faceDirection, 0)) < 90f){
                m_currentState = EnemyStates.Chase;
            }
        }
    }

    void ChasePlayer()
    {
        if ((m_playerTransform.position.x - transform.position.x ) * m_faceDirection < 0)
        {
            Flip();
        }
        
        m_rigidBody.velocity = new Vector2(m_faceDirection * m_maxChaseSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);
        m_animator.SetInteger("AnimState", 1);

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(m_playerTransform.position.x, m_playerTransform.position.y);
        if (Vector2.Distance(position, playerPosition) < 3f)
        {
            if (Vector2.Angle(playerPosition - position, new Vector2(m_faceDirection, 0)) < 90f)
            {
                m_currentState = EnemyStates.Attack;
            }
        }
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor)
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (!Physics2D.OverlapCircle(m_groundCheck.position, m_radius, m_groundLayer) ||
            m_bodyCollider.IsTouchingLayers(m_groundLayer))
        {
            m_rigidBody.velocity = new Vector2(0, m_rigidBody.velocity.y);
        }
    }

    void AttackState()
    {
        // Back to Chase State if distance to player is too large.
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(m_playerTransform.position.x, m_playerTransform.position.y);
     
        //Attack
        if (!m_attacking && m_attackCoolDownTimer <= 0)
        {
            Debug.Log("AttackState");
            m_attacking = true;
            m_damagedColliders.Clear();
            m_animator.SetTrigger("Attack");
            m_animator.SetBool("Attacking", true);             
            m_attackDurationTimer = 0f;
        }

        if (m_animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
            Debug.Log("Attack");
        }
        if (m_attacking)
        {
            m_attackDurationTimer += Time.fixedDeltaTime;
        }
        else
        {
            if ((m_playerTransform.position.x - transform.position.x) * m_faceDirection < 0)
            {
                Flip();
            }
            m_animator.SetInteger("AnimState", 0);
            if (Vector2.Distance(position, playerPosition) > 3f)
            {
                m_currentState = EnemyStates.Chase;
            }
        }
        if (m_attackDurationTimer >= m_attackDuration && m_attacking)
        {
            m_attacking = false;
            m_animator.SetBool("Attacking", false);
            m_attackCoolDownTimer = m_attackCoolDown;
        }
        m_attackCoolDownTimer -= Time.fixedDeltaTime;
    }

    void Attack()
    {
        Debug.Log("Actual Attack");
        Collider2D[] attackCandidates = new Collider2D[m_maxEnemiesToAttack];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        int collidersCount = Physics2D.OverlapCollider(m_attackRegion, filter, attackCandidates);
        for (int i = 0; i < collidersCount; i++)
        {
            if (m_damagedColliders.Contains(attackCandidates[i])) continue;
            PrototypeHeroDemo playerbehavior = attackCandidates[i].GetComponent<PrototypeHeroDemo>();
            if (playerbehavior != null)
            {
                playerbehavior.Damaged(5, this.gameObject);
                m_damagedColliders.Add(attackCandidates[i]);
                Debug.Log("Attack enemy: " + playerbehavior.gameObject.name);
            }
        }
    }

    void SetState(EnemyStates enemyState)
    {
        m_currentState = enemyState;
        //Invoke EnemyStateChanged Event.
        //EnemyStateChanged.Invoke();
    }

    // Drop items when enemy is dead.
    void Drop()
    {
        float y = 0.5f;
        for (int i = 0; i < m_numberOfBlueCrystal; i++)
        {
            y *= -1;
            Instantiate(m_blueCrystal, transform.position + new Vector3(-i / 4f, y, 0),Quaternion.identity, null);
        }
    }


}
