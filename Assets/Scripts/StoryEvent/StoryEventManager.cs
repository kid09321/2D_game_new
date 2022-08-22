using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventManager : MonoBehaviour
{
    public StoryEvent CurrentStoryEvent { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentStoryEvent == null) return;
        CurrentStoryEvent.Update(Time.deltaTime);
    }

    public void SetStoryEvent(StoryEvent se)
    {
        CurrentStoryEvent = se;
        if (CurrentStoryEvent != null)
            CurrentStoryEvent.OnEnter();
    }
}
