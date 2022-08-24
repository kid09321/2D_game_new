using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private Animator m_animator;
    private bool     m_switch;  // Use to turn on/off the saw.
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_switch = true;
        m_animator.SetBool("Switch", m_switch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
