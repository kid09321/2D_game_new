using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Camera m_playerCamera;
    [SerializeField] float m_parallaxEffect;
    [SerializeField] Transform m_rightBackground;
    [SerializeField] Transform m_leftBackground;

    private float m_length;
    private float m_startPos;
    private float m_camStartPos;


    // Start is called before the first frame update
    void Start()
    {
        m_startPos = transform.position.x;
        m_camStartPos = m_playerCamera.transform.position.x;
        m_length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("m_length:" + m_length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float dist = ((m_playerCamera.transform.position.x - m_camStartPos) * m_parallaxEffect);
        float temp = ((m_playerCamera.transform.position.x - m_camStartPos) * (1 - m_parallaxEffect));
        transform.position = new Vector3(m_startPos + dist, transform.position.y, transform.position.z);
        //Debug.Log("temp: " +temp);
        if (temp >= m_length)
        {
            Debug.Log("temp larger");
            m_startPos += (m_length+dist);
            
            /*if(m_leftBackground.transform == transform)
            {
                m_startPos += (3 * m_length + dist);
            }*/

            //m_startPos = transform.position.x + m_length;
            m_camStartPos = m_playerCamera.transform.position.x;
        }
        else if(temp <= -m_length)
        {
            Debug.Log("temp smaller");
            m_startPos += (dist - m_length);

            /*if (m_rightBackground.transform == transform)
            {
                m_startPos += (-3 * m_length + dist);
                Debug.Log("m_startPos: " + m_startPos);
            }*/
            //m_startPos = transform.position.x - m_length;
            m_camStartPos = m_playerCamera.transform.position.x;
        }
    }
}
