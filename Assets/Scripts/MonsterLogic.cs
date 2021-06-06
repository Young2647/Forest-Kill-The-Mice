using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class MonsterLogic : MonoBehaviour
{
   GameObject m_player;
    PlayerLogic m_playerLogic;

    NavMeshAgent m_navMeshAgent;

    [SerializeField]
    Transform m_patrolDestinationTransform;

    Vector3 m_patrolOrigin;
    Vector3 m_patrolDestination;

    [SerializeField]
    EnemyState m_enemyState;

    float m_offset = 2.0f;
    float m_searchRadius = 8.0f;

    int m_health = 100;
    const float MAX_ATTACK_COOLDOWN = 3.0f;
    float m_attackCooldown = MAX_ATTACK_COOLDOWN;


    [SerializeField]
    AudioClip m_zombieAttack;

    [SerializeField]
    AudioClip m_bloodSplat;

    AudioSource m_audioSource;

    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        if(m_player)
        {
            m_playerLogic = m_player.GetComponent<PlayerLogic>();
        }

        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.speed = 1.7f;
        m_patrolOrigin = transform.position;

        SetState(m_enemyState);

        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_enemyState)
        {
            case (EnemyState.Idle):
                SearchForPlayer();
                break;

            case (EnemyState.Patrol):
                SearchForPlayer();
                Patrol();
                break;

            case (EnemyState.Chase):
                Chase();
                break;

            case (EnemyState.Attack):
                Attack();
                break;
        }

        if(m_enemyState == EnemyState.Patrol || m_enemyState == EnemyState.Chase){
            m_animator.SetBool("IfWalking",true);
        }else{
            m_animator.SetBool("IfWalking",false);
        }

        if(m_attackCooldown > 0.0f)
        {
            m_attackCooldown -= Time.deltaTime;
        }

        // Debug Draw Lines
        /*Debug.DrawLine(transform.position, transform.position + transform.forward * m_searchRadius, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.right * m_searchRadius, Color.red);
        Debug.DrawLine(transform.position, transform.position - transform.right * m_searchRadius, Color.red);
        Debug.DrawLine(transform.position, transform.position - transform.forward * m_searchRadius, Color.red);*/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, m_searchRadius);
    }

    void SetState(EnemyState enemyState)
    {
        m_enemyState = enemyState;

        switch (m_enemyState)
        {
            case (EnemyState.Idle):
                // Do nothing
                break;

            case (EnemyState.Patrol):
                if (m_patrolDestinationTransform && m_patrolDestinationTransform.position != Vector3.zero)
                {
                    m_patrolDestination = m_patrolDestinationTransform.position;
                    m_navMeshAgent.SetDestination(m_patrolDestination);
                }
                break;

            case (EnemyState.Chase):
                // Play Chase Sound Effect

                m_navMeshAgent.Resume();
                break;

            case (EnemyState.Attack):
                m_navMeshAgent.Stop();
                break;
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(m_navMeshAgent.destination, transform.position) < m_offset)
        {
            // Swap Destination between Origin and Destination, remove Y coordinates
            Vector3 navMeshDestination = new Vector3(m_navMeshAgent.destination.x, 0, m_navMeshAgent.destination.z);
            Vector3 patrolOrigin = new Vector3(m_patrolOrigin.x, 0, m_patrolOrigin.z);
            Vector3 patrolDestination = new Vector3(m_patrolDestination.x, 0, m_patrolDestination.z);

            if (navMeshDestination == patrolDestination)
            {
                m_navMeshAgent.SetDestination(m_patrolOrigin);
            }
            else if (navMeshDestination == patrolOrigin)
            {
                m_navMeshAgent.SetDestination(m_patrolDestination);
            }
        }
    }

    void SearchForPlayer()
    {
        if(Vector3.Distance(transform.position, m_player.transform.position) < m_searchRadius)
        {
            SetState(EnemyState.Chase);
        }
    }

    void Chase()
    {
        m_navMeshAgent.SetDestination(m_player.transform.position);
        if (Vector3.Distance(m_navMeshAgent.destination, transform.position) < m_offset)
        {
            SetState(EnemyState.Attack);
        }
    }

    void Attack()
    {
        if (Vector3.Distance(m_player.transform.position, transform.position) < m_offset)
        {
            // Check if we can Attack
            if(m_attackCooldown < 0)
            {
                // Play Attack Sound Effect
                Debug.Log("Attack");
                // Deal damage to Player
                m_animator.SetTrigger("Attack");

                // Reset Attack Cooldown
                m_attackCooldown = MAX_ATTACK_COOLDOWN;
            }
        }
        else
        {
            // If we are too far away chase the player again
            SetState(EnemyState.Chase);
        }
    }

    public void MakeDamage(){
        m_playerLogic.Damage(10);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Arrow")
        {
            ArrowLogic arrowLogic = collision.collider.GetComponent<ArrowLogic>();
            if(arrowLogic)
            {
                arrowLogic.IsHit();
            }

            m_animator.SetTrigger("Hit");
            if(m_enemyState == EnemyState.Idle)
                SetState(EnemyState.Chase);
            // Play Blood Splat Sound Effect
            PlaySound(m_bloodSplat);

            m_health -= 35;

            if(m_health <= 0)
            {
                m_animator.SetTrigger("Dead");
            }
        }
    }

    void PlaySound(AudioClip sound)
    {
        // Play Sound effect
        if (m_audioSource && sound)
        {
            m_audioSource.PlayOneShot(sound);
        }
    }

    void MonsterDie(){
        Destroy(gameObject);
        GameManager m_gameManager = FindObjectOfType<GameManager>();
        m_gameManager.miceNum--;
    }

    void PlayAttackSound(){
        PlaySound(m_zombieAttack);
    }
}
