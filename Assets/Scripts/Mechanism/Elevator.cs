using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float m_maxSpeed = 5f;
    [SerializeField] float m_stayDuration = 0.5f;
    [SerializeField] Transform m_upperGoal;
    [SerializeField] Transform m_lowerGoal;

    private Rigidbody2D m_body2d;
    private int         m_moveDirection = 1;
    private bool        m_arrivedOneEnd = false;
    private float       m_stayDurationTimer = 0f;
    private Vector2     m_currentGoalPosition;
  

    // Start is called before the first frame update
    void Start()
    {
        m_body2d = GetComponent<Rigidbody2D>();
        m_currentGoalPosition = m_upperGoal.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        StartElevator();
    }

    void StartElevator()
    {
        // Set the current goal position according to current move direction.
        if (Mathf.Sign(m_moveDirection) > 0)
        {
            m_currentGoalPosition = m_upperGoal.position;
        }
        else
        {
            m_currentGoalPosition = m_lowerGoal.position;
        }

        // Check if arrived, set the arrived flag to true and reset timer. 
        // Else if it's not arrived to goal and it's not stop after arrived then move.
        if (Vector2.Distance(m_currentGoalPosition, transform.position) < 0.1f && !m_arrivedOneEnd)
        {
            m_arrivedOneEnd = true;
            m_stayDurationTimer = m_stayDuration;

        }else if (m_stayDurationTimer <= 0 && !m_arrivedOneEnd)
        {

            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), m_currentGoalPosition, m_maxSpeed);

        }

        // If arrived, reduce the stay timer, once timer is over then flip the goal position.
        if (m_arrivedOneEnd)
        {
            m_stayDurationTimer -= Time.fixedDeltaTime;
            if(m_stayDurationTimer <= 0f)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        m_moveDirection *= -1;
        m_arrivedOneEnd = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
