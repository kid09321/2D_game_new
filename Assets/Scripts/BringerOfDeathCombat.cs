using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathCombat : Combat
{

    public BringerOfDeathCombat(Collider2D attackRegion, GameObject playerGameObject)
    {
        m_attackDuration = 1f;
        m_attackCoolDown = 0.5f;
        m_attackTimeBetween = 0.1f;
        m_attackRegion = attackRegion;
        m_player = playerGameObject.GetComponent<CharacterMovement>();
        m_animator = playerGameObject.GetComponent<Animator>();
        m_maxEnemiesToAttack = 10;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }
    
    public override void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !m_attacking && m_attackCoolDownTimer <= 0)
        {
            m_attacking = true;
            m_player.SetMovementLock(true);
            m_damagedColliders.Clear();
            m_animator.SetTrigger("Attack");
            m_animator.SetBool("Attacking", true);
            m_attackCoolDownTimer = m_attackCoolDown;
            m_attackDurationTimer = 0.0f;
            m_attackTimeBetweenTimer = 0.0f;

        }
        if (m_animator.GetFloat("Weapon.Active") > 0f)
        {         
            ActualAttack();
        }

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
            m_attackDurationTimer += Time.deltaTime;
        }
    }
    
    void ActualAttack()
    {
        Collider2D[] attackCandidates = new Collider2D[m_maxEnemiesToAttack];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Debug.Log("m_attackRegion:" + m_attackRegion);
        int collidersCount = Physics2D.OverlapCollider(m_attackRegion, filter, attackCandidates);
        Debug.Log("Candidates Count:" + collidersCount);
        for (int i = 0; i < collidersCount; i++)
        {
            Debug.Log("Candidates:" + attackCandidates[i]);
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
