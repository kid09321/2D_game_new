using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat
{
    protected float m_attackDuration;
    protected float m_attackCoolDown;
    protected float m_attackTimeBetween;
    protected Collider2D m_attackRegion;
    protected int m_maxEnemiesToAttack = 10;

    protected CharacterMovement m_player;
    protected Animator m_animator;

    protected bool m_attacking = false;
    protected List<Collider2D> m_damagedColliders;

    protected float m_attackDurationTimer;
    protected float m_attackCoolDownTimer;
    protected float m_attackTimeBetweenTimer;

    public Combat()
    {
        m_attackDurationTimer = 0f;
        m_attackCoolDownTimer = 0f;
        m_attackTimeBetweenTimer = 0f;
        m_attacking = false;
        m_damagedColliders = new List<Collider2D>();
    }

    public virtual void Update()
    {

    }

    public virtual void HandleAttack()
    {

    }
}
