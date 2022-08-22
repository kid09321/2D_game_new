using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    private Animator m_animator;
    private bool     m_attacking;
    private float    m_attackDurationTimer;
    // Start is called before the first frame update
    void Start()
    {
        m_animator.GetComponent<Animator>();
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
        /*if (Input.GetKeyDown(KeyCode.Z) && !m_attacking && m_attackCoolDownTimer <= 0)
        {
            m_attacking = true;
            m_movementLocked = true;
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
            Attack();
        }
        // To lock the movement when comboing.
        // After one attack duration is over, attacking set to false first.
        // Then check if there is next attack in m_attackTimeBetween, if not, unlock the movement.
        if (m_attackDurationTimer >= m_attackDuration)
        {
            m_attacking = false;
            if (m_attackTimeBetweenTimer > m_attackTimeBetween)
            {
                m_movementLocked = false;
            }
            m_attackTimeBetweenTimer += Time.deltaTime;
        }
        m_attackCoolDownTimer -= Time.deltaTime;
        */
    }
}
