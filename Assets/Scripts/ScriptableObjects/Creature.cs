using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Creature", menuName = "ScriptableObjects/Creature")]
public class Creature : ScriptableObject
{
    [SerializeField] Sprite   m_sprite;  
    [SerializeField] float    m_maxHealth;
    [SerializeField] float    m_maxSpeed;
    [SerializeField] int      m_defaultFacingDirection;
    [SerializeField] GameObject m_attackRegion;

    [SerializeField] RuntimeAnimatorController m_animatorController;    
    public Sprite Sprite
    {
        get
        {
            return m_sprite;
        }
    }

    public float MaxHealth
    {
        get
        {
            return m_maxHealth;
        }
    }

    public float MaxSpeed
    {
        get
        {
            return m_maxSpeed;
        }
    }

    public RuntimeAnimatorController AnimatorController
    {
        get
        {
            return m_animatorController;
        }
    }

    public int DefaultFacingDirection
    {
        get
        {
            return m_defaultFacingDirection;
        }
    }

    public GameObject AttackRegion
    {
        get
        {
            return m_attackRegion;
        }
    }

}
