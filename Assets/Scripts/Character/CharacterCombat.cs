using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] float      m_attackDuration;
    [SerializeField] float      m_attackCoolDown;
    [SerializeField] float      m_attackTimeBetween;
    [SerializeField] Collider2D m_attackRegion;

    private Combat            m_currentCombat;
    public void SetCombat(Combat combat)
    {
        m_currentCombat = combat;
    }

    public Collider2D GetAttackRegion()
    {
        return m_attackRegion;
    }

    public void SetAttackRegion(Collider2D attackRegion)
    {
        m_attackRegion = attackRegion;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_currentCombat = new KnightCombat(m_attackRegion, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Attack
        HandleAttack();

    }
    
    void HandleAttack()
    {
        m_currentCombat.HandleAttack();
    }
}
