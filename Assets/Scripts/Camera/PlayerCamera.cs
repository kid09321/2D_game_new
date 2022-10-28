using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform m_characterTransform;

    [SerializeField] float maxHorizontalOffset = 2f;
    [SerializeField] float maxVerticalOffset = 3f;
    [SerializeField] float m_cameraMovingSpeed = 2f;
    [SerializeField] float m_cameraSmoothTime = 0.3f;

    private Vector2 m_cameraCurrentVelocity = Vector2.zero;

    private void Awake()
    {
        //transform.position = m_characterTransform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {       
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.m_canMoveCamera)
        {
            FollowCharacter();
        }
    }

    void FollowCharacter()
    {
        Vector2 playerPosition = new Vector2(m_characterTransform.position.x, m_characterTransform.position.y);
        Vector2 cameraPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        float horizontalDistance = playerPosition.x - cameraPosition.x;
        float verticalDistance = playerPosition.y - cameraPosition.y;

        // Follow player horizontally
        if (horizontalDistance > maxHorizontalOffset || horizontalDistance < -maxHorizontalOffset ||
            verticalDistance > maxVerticalOffset || verticalDistance < -maxVerticalOffset)
        {
            Vector2 target = Vector2.Lerp(cameraPosition, playerPosition, m_cameraMovingSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
        }
        // Follow player vertically
        
        if (verticalDistance > maxVerticalOffset || verticalDistance < -maxVerticalOffset)
        {
            Vector2 target = Vector2.Lerp(cameraPosition, playerPosition, m_cameraMovingSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(transform.position.x, target.y, transform.position.z);            
        }
    }
}
