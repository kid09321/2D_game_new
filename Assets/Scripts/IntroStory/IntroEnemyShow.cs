using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnemyShow : EventUnit
{
    private EnemyBehavior m_enemyBehavior;
    private bool m_instantiated = false;
    private float m_timerCounter = 0f;
    public IntroEnemyShow(StoryEvent storyEvent) : base(storyEvent)
    {
        m_storyEvent = storyEvent;
    }

    public override void StartEvent()
    {
        GameObject enemy = GameObject.Instantiate(EnemyManager.Instance.BoD, new Vector2(59.1f, 1.1f), Quaternion.identity);
        m_enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        m_enemyBehavior.enabled = false;
        m_instantiated = true;
    }

    public override void Update(float deltaTime)
    {
        if (m_finished) return;
        if (m_instantiated)
        {
            if(m_timerCounter < 1)
            {
                m_timerCounter += deltaTime;
            }
            else if (!m_finished)
            {
                m_finished = true;
                m_storyEvent.ExecuteNext();
            }
        }
    }   
    
}
