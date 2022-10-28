using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCharacterMove : EventUnit
{

    private GameObject   m_playerGO;
    private Transform    m_playerCameraTrans;
    private Transform    m_playerTrans;
    private Vector2      m_targetPos = new Vector2(52f, -0.04f);
    private CharacterMovement m_playerMovement;

    public IntroCharacterMove(StoryEvent storyEvent) : base(storyEvent)
    {
        m_playerGO = GameObject.FindGameObjectWithTag("Player");
        m_playerTrans = m_playerGO.transform;
        m_playerMovement = m_playerGO.GetComponent<CharacterMovement>();
        m_playerCameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public override void StartEvent()
    {
    

    }

    public override void Update(float deltaTime)
    {
        if (m_finished) return;
        if (Vector2.Distance(m_playerCameraTrans.position, m_playerGO.transform.position) < 1f)
        {
            GameManager.Instance.m_canMoveCamera = true;
        }

        if (Vector2.Distance(m_playerCameraTrans.position, m_playerTrans.position) < 1f)
        {
            GameManager.Instance.m_canMoveCamera = true;
        }
        GameManager.Instance.m_canControlPlayer = true;

        if (Vector2.Distance(m_targetPos, m_playerTrans.position) > 2f)
        {
            m_playerMovement.MovementHandler(-1.0f, -1.0f);
        }
        else
        {
            m_storyEvent.ExecuteNext();
            m_finished = true;
        }
    }
}
