using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour
{
    [SerializeField] StoryEventManager m_storyEventManager;
    [SerializeField] StoryDialogue     m_storyDialogue;
    // Start is called before the first frame update
    void Start()
    {
        if(m_storyEventManager == null)
        {
            m_storyEventManager = GameObject.FindObjectOfType<StoryEventManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision enter!");
        if (collision.gameObject.tag == ("Player"))
        {
            Debug.Log("Player Enter story event!");
            //FirstBossFight storyEvent = new FirstBossFight("firstBossFight", m_storyDialogue);
            FirstStory storyEvent = new FirstStory("firstStory", m_storyDialogue);
            m_storyEventManager.SetStoryEvent(storyEvent);
        }
    }
}
