using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    const float ARROW_LIFE_TIME = 2.5f;
    Rigidbody m_rigidBody;
    const float SPEED = 15.0f;
    Collider m_collider;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.velocity = transform.forward * SPEED;
        m_collider = GetComponent<Collider>();
        Destroy(gameObject,ARROW_LIFE_TIME);
    }

    // Update is called once per frame

    public void IsHit()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision){
        Destroy(gameObject);
    }
}
