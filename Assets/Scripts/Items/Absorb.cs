using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    [SerializeField] float      m_maxSpeed = 5f;
    [SerializeField] float      m_instantiateDuration = 0.5f;
    [SerializeField] float      m_dropSpeedY = 0.1f;
    [SerializeField] GameObject m_hitEffect;

    private Rigidbody2D m_body2d;
    private int         m_direction = 1;
    private bool        m_absorbing = false;
    private Transform   m_moveTarget;
    private float       m_instantiateTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_body2d = GetComponent<Rigidbody2D>();
        m_moveTarget = GameObject.FindGameObjectWithTag("Player").transform;
        m_body2d.velocity = new Vector2(0, m_dropSpeedY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_instantiateTimer < m_instantiateDuration)
        {
            m_instantiateTimer += Time.fixedDeltaTime;
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(m_moveTarget.position.x, m_moveTarget.position.y)) < 3f)
        {          
            m_absorbing = true;
        }

        if (m_absorbing && m_instantiateTimer >= m_instantiateDuration)
        {
            m_body2d.simulated = false;
            AbsorbMovement();
        }
    }

    void AbsorbMovement()
    {
        if(Mathf.Sign(m_moveTarget.position.x - transform.position.x) < 0){
            m_direction = -1;
        }
        else
        {
            m_direction = 1;
        }
        transform.position = Vector2.MoveTowards(transform.position, m_moveTarget.position, m_maxSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, m_moveTarget.position) < 0.1f)
        {
            gameObject.SetActive(false);
            Instantiate(m_hitEffect, m_moveTarget);
        }
    }
}
