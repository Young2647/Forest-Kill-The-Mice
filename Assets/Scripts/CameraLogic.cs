using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    Vector3 m_cameraTarget;

    float m_cameraYOffset = 1.5f;
    
    GameObject m_player;

    float m_rotationX;
    float m_rotationY;

    const float MIN_ROT_X = -20.0f;
    const float MAX_ROT_X = 30.0f;

    const float MIN_Z = 5.0f;
    const float MAX_Z = 10.0f;
    float m_cameraZOffset = MIN_Z;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate around character
        m_rotationY += Input.GetAxis("Mouse X");
        m_rotationY += 10 * Input.GetAxis("Stick X");

        m_rotationX -= Input.GetAxis("Mouse Y");
        m_rotationX -= 10 * Input.GetAxis("Stick Y");
        m_rotationX = Mathf.Clamp(m_rotationX, MIN_ROT_X, MAX_ROT_X);

        // Zoom in / out
        m_cameraZOffset -= Input.GetAxis("Mouse ScrollWheel");
        m_cameraZOffset = Mathf.Clamp(m_cameraZOffset, MIN_Z, MAX_Z);

        // This code sets the target of the camera to the player's position
        m_cameraTarget = m_player.transform.position;

        // We want the camera to look slightly above the player so we increase the Y value (using an offset)
        m_cameraTarget.y += m_cameraYOffset;
    }

    void LateUpdate()
    {
        // Add an offset to the camera's position
        Vector3 cameraOffset = new Vector3(0, 0, -m_cameraZOffset);

        // Calculate the rotation
        Quaternion cameraRotation = Quaternion.Euler(m_rotationX, m_rotationY, 0);

        // The position of the camera is the position of the target plus an offset
        transform.position = m_cameraTarget + cameraRotation * cameraOffset;

        // Make the camera look at the camera target
        transform.LookAt(m_cameraTarget);
    }

    public Vector3 GetForwardVector()
    {
        // Calculate the Forward Vector of the Camera without X Axis rotations
        Quaternion rotation = Quaternion.Euler(0, m_rotationY, 0);
        return rotation * Vector3.forward;
    }
}
