using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Appearance")]
    [SerializeField] Creature m_creatureData;
    [SerializeField] Creature m_originalCreatureData;

    private Animator          m_animator;
    private CharacterMovement m_playerMovement;
    private CharacterCombat   m_playerCombat;
    // Used for change appearance
    private bool              m_changingAppearance = false;
    private SpriteRenderer    m_spriteRenderer;
    private bool              m_isMorph = false;

    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = GetComponent<CharacterMovement>();
        m_playerCombat = GetComponent<CharacterCombat>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle appearance changing
        if (Input.GetKeyDown(KeyCode.C) && !m_changingAppearance)
        {
            // if current is morph, than morph back to original apperance,
            // else morph to the target creature.
            if (m_isMorph)
            {
                ChangeAppearance(m_originalCreatureData);
                m_playerCombat.SetCombat(new KnightCombat(m_playerCombat.GetAttackRegion(), this.gameObject));
                m_isMorph = false;
            }
            else
            {
                ChangeAppearance(m_creatureData);
                Debug.Log("m_attackRegion:" + m_playerCombat.GetAttackRegion());
                m_playerCombat.SetCombat(new BringerOfDeathCombat(m_playerCombat.GetAttackRegion(), this.gameObject));
                m_isMorph = true;
            }
        }
    }

    // Change Appearance
    void ChangeAppearance(Creature creatureData)
    {
        Debug.Log("Change Appearance");
        m_changingAppearance = true;
        StartCoroutine(SpriteFadeOut(creatureData));
        m_changingAppearance = false;
    }

    IEnumerator SpriteFadeOut(Creature creatureData)
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
        m_spriteRenderer.sprite = creatureData.Sprite;
        m_animator.runtimeAnimatorController = creatureData.AnimatorController;
        m_playerMovement.SetDefaultFaceDirection(creatureData.DefaultFacingDirection);
        //Destroy(m_playerCombat.GetAttackRegion().gameObject);
        //GameObject attackRegion = Instantiate(creatureData.AttackRegion, this.transform);
        //attackRegion.name = creatureData.AttackRegion.name;
        //m_playerCombat.SetAttackRegion(attackRegion.GetComponent<Collider2D>());
        //m_playerCombat.SetCombat(new BringerOfDeathCombat(attackRegion.GetComponent<Collider2D>(), this.gameObject));
        // Set m_grounded to false to re-detect grounded for animator,
        // or the grounded for animator will be false at the beginning untill
        // jump or other action that off the ground.
        m_playerMovement.SetGrounded(false);
        for (int i = 0; i <= 255; i++)
        {
            tmpColor.a = i / 255f;
            m_spriteRenderer.color = tmpColor;
            yield return new WaitForSeconds(secPerLoop);
        }
    }
}
