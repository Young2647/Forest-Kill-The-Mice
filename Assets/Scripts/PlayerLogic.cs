using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    const float m_forwardMovementSpeed = 3.0f;
    const float m_backwardMovementSpeed = 2.0f;
    const float m_strafeMovementSpeed = 3.0f;
    float m_shootPressure = 0f;
    const float MIN_PRESS = 3f;
    const float MAX_PRESS = 10f;

    int m_health = 100;
    float m_currentAngleY = 0f;

    float m_rotateAngle = 0f;
    CharacterController m_characterController;
    Animator m_animator;
    bool _ifshooting = false;
    float m_horizontalInput;
    float m_verticalInput;

    Vector3 m_movementInput;

    Vector3 m_verticalMovement;

    Vector3 m_horizontalMovement;
    Vector3 m_movement;

    CameraLogic m_cameraLogic;

    [SerializeField]

    GameObject m_arrowPrefab;

    [SerializeField]

    Transform m_arrowSpawn;

    [SerializeField]
    Text m_healthTMP;

    [SerializeField]
    Text m_DieUI;

    [SerializeField]
    AudioClip m_shootSound;

    [SerializeField]
    AudioClip m_hurtSound;

    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_cameraLogic = FindObjectOfType<CameraLogic>();
        m_audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
        UpdateDieUI("");
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        m_verticalInput = Input.GetAxisRaw("Vertical");

        m_animator.SetFloat("HorizontalInput", m_horizontalInput);
        m_animator.SetFloat("VerticalInput", m_verticalInput);

        m_movementInput = new Vector3(m_horizontalInput, 0, m_verticalInput);

        if (Input.GetButton("Fire1")){
            m_animator.SetBool("Shooting",true);
            _ifshooting = true;
            if(m_shootPressure < MAX_PRESS){
                 m_shootPressure += Time.deltaTime * 10f;
            }
            else{
                m_shootPressure = MAX_PRESS;
            }
        }
        else{
            if (m_shootPressure > 0f){
                m_shootPressure += MIN_PRESS;
                ShootArrow();
                m_shootPressure = 0f;
                m_animator.SetBool("Shooting",false);
            }
        }

        if(m_health <= 0){
            Die();
        }

        Debug.Log(transform.forward);
    }


    void FixedUpdate()
    {
        
        // Apply Movement
        if (m_verticalInput >= 0.1f || m_verticalInput <= -0.1f || m_horizontalInput >= 0.1f || m_horizontalInput <= -0.1f)
        {
            if (m_cameraLogic)
            {
                if (_ifshooting){
                    ChangeTransform();
                }
                else{
                    transform.forward = m_cameraLogic.GetForwardVector();
                }
            }
        }else{
            if (m_animator.GetBool("Shooting")){
                ChangeTransform();
            }
        }

        m_verticalMovement = transform.forward * m_verticalInput * GetMovementSpeed() * Time.deltaTime;
        m_horizontalMovement = Camera.main.transform.right * m_horizontalInput * m_strafeMovementSpeed * Time.deltaTime;
        m_characterController.Move(m_horizontalMovement + m_verticalMovement);
    }

    void ShootArrow(){
        m_animator.SetTrigger("StopShooting");
    }

    public void SpawnArrow(){
        Instantiate(m_arrowPrefab, m_arrowSpawn.position, m_arrowSpawn.rotation);
        PlayShootSound();
    }

    float GetMovementSpeed()
    {
        if(m_verticalInput >= 0.1f)
        {
            return m_forwardMovementSpeed;
        }
        else
        {
            return m_backwardMovementSpeed;
        }
    }

    void StopShooting(){
        _ifshooting = false;
    }

    public void Damage(int damage){
        m_health -= damage;
        m_animator.SetTrigger("Hit");
        UpdateHealthUI();
    }
    void UpdateHealthUI(){
        if(m_healthTMP){
            if(m_health >= 0)
                m_healthTMP.text = "❤ :" + m_health;
            else
                m_healthTMP.text = "❤:" + 0;
        }
    }

    void Die(){
        m_animator.SetTrigger("Die");
    }

    void Dead(){
        if (m_health <= 0){
            Time.timeScale = 0;
            UpdateDieUI("Dead");
        }
    }
    void UpdateDieUI(string name){
        if (name == "Dead"){
            m_DieUI.gameObject.SetActive(true);
        }else{
            m_DieUI.gameObject.SetActive(false);
        }
    }

    void PlayShootSound(){
        m_audioSource.PlayOneShot(m_shootSound);
    }

    void PlayHurtSound(){
        m_audioSource.PlayOneShot(m_hurtSound);
    }

    void ChangeTransform(){
        Vector3 pos = m_cameraLogic.GetForwardVector();
        transform.forward = new Vector3(pos.z,pos.y,-pos.x);
    }
}
