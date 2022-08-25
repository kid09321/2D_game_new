using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] float      m_attackDuration;
    [SerializeField] float      m_attackCoolDown;
    [SerializeField] float      m_attackTimeBetween;
    [SerializeField] Collider2D m_attackRegion;

    private PrototypeHeroDemo m_player;
    private Animator          m_animator;
    private bool              m_attacking = false;
    private int               m_maxEnemiesToAttack = 10;
    private List<Collider2D>  m_damagedColliders;
    private int               m_currentAttackState = 0;

    // Timers
    // - Attack
    private float m_attackDurationTimer;
    private float m_attackCoolDownTimer;
    private float m_attackTimeBetweenTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_damagedColliders = new List<Collider2D>();
        m_player = GetComponent<PrototypeHeroDemo>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack
        HandleAttack();
        if (m_attacking)
        {
            m_animator.SetInteger("AnimState", 3);
            m_attackDurationTimer += Time.deltaTime;
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !m_attacking && m_attackCoolDownTimer <= 0)
        {
            m_attacking = true;
            m_player.SetMovementLock(true);
            m_damagedColliders.Clear();
            switch (m_currentAttackState % 3)
            {
                case 0:
                    m_animator.SetTrigger("Attack1");
                    break;
                case 1:
                    m_animator.SetTrigger("Attack2");
                    break;
                case 2:
                    m_animator.SetTrigger("Attack3");
                    m_attackCoolDownTimer = m_attackCoolDown;
                    break;

            }
            m_currentAttackState += 1;
            m_animator.SetInteger("AnimState", 3);
            m_attackDurationTimer = 0.0f;
            m_attackTimeBetweenTimer = 0.0f;
        }


        if (m_animator.GetFloat("Weapon.Active") > 0f)
        {
            ActualAttack();
        }
        // To lock the movement when comboing.
        // After one attack duration is over, attacking set to false first.
        // Then check if there is next attack in m_attackTimeBetween, if not, unlock the movement.
        if (m_attackDurationTimer >= m_attackDuration)
        {
            m_attacking = false;
            if (m_attackTimeBetweenTimer > m_attackTimeBetween)
            {
                m_player.SetMovementLock(false);
            }
            m_attackTimeBetweenTimer += Time.deltaTime;
        }
        m_attackCoolDownTimer -= Time.deltaTime;
        
    }

    void ActualAttack()
    {
        Collider2D[] attackCandidates = new Collider2D[m_maxEnemiesToAttack];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int collidersCount = Physics2D.OverlapCollider(m_attackRegion, filter, attackCandidates);

        for (int i = 0; i < collidersCount; i++)
        {
            if (m_damagedColliders.Contains(attackCandidates[i])) continue;
            EnemyBehavior enemyBehavior = attackCandidates[i].GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.Damaged(5, this.gameObject);
                m_damagedColliders.Add(attackCandidates[i]);
                Debug.Log("Attack enemy: " + enemyBehavior.gameObject.name);
            }
        }
    }
}
