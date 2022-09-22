using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCombat : Combat
{
    private int m_currentAttackState = 0;

    public KnightCombat(Collider2D attackRegion, GameObject playerGameObject)
    {
        m_attackDuration = 0.4f;
        m_attackCoolDown = 0.1f;
        m_attackTimeBetween = 0.1f;
        m_attackRegion = attackRegion;
        m_player = playerGameObject.GetComponent<CharacterMovement>();
        m_animator = playerGameObject.GetComponent<Animator>();
        m_maxEnemiesToAttack = 10;

    }
    public override void Update()
    {
        
    }

    public override void HandleAttack()
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

        if (m_attacking)
        {
            m_animator.SetInteger("AnimState", 3);
            m_attackDurationTimer += Time.deltaTime;
        }
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
                enemyBehavior.Damaged(5, m_player.gameObject);
                m_damagedColliders.Add(attackCandidates[i]);
                Debug.Log("Attack enemy: " + enemyBehavior.gameObject.name);
            }
        }
    }
}
