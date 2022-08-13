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

    private Animator     m_animator;
    private int          m_currentHealth;
    private float        m_currentStrength;
    private EnemyStates  m_currentState;
    private HealthBar    m_healthBar;
    private Rigidbody2D  m_rigidBody;
    private int          m_faceDirection = -1;
    private bool         m_grounded = false;

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
        Debug.Log("Current State: " + m_currentState);
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
        if((m_playerTransform.position.x - transform.position.x ) * m_faceDirection < 0)
        {
            Flip();
        }
        
        m_rigidBody.velocity = new Vector2(m_faceDirection * m_maxChaseSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y);
        m_animator.SetInteger("AnimState", 1);

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(m_playerTransform.position.x, m_playerTransform.position.y);
        if (Vector2.Distance(position, playerPosition) < 0.5f)
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
    }

    void AttackState()
    {
        // Back to Chase State if distance to player is too large.
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(m_playerTransform.position.x, m_playerTransform.position.y);
        if (Vector2.Distance(position, playerPosition) > 1f)
        {         
            m_currentState = EnemyStates.Chase;
        }
        else
        {
            //Attack
            Debug.Log("Attack");
        }
    }

    void Attack()
    {
        
    }

    void SetState(EnemyStates enemyState)
    {
        m_currentState = enemyState;
        //Invoke EnemyStateChanged Event.
        //EnemyStateChanged.Invoke();
    }


}
