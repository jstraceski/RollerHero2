using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates {
        Idle, Patrol, Chase, Attack, Flee, Dead
    }

    public float attackDistance = 5;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public FSMStates currentState;
    public GameObject player;
    public GameObject[] spellProjectile;
    public GameObject wandTip;
    public float shootRate = 2.0f;
    public GameObject deadVFX;
    
    private GameObject[] wanderPoints;
    private Vector3 nextDestination;
    private int currentDestinationIndex = 0;
    private Animator animator;
    private float distanceToPlayer;
    private float elapsedTime = 0;
    
    // private EnemyHealth _enemyHealth;
    private int health;
    private Transform deadTransform;
    private bool isDead;
    private int randInt = 101;

    private NavMeshAgent agent;
    public Transform enemyEyes;
    public float fov = 45f;
    
    // Start is called before the first frame update
    void Start() {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        // wandTip = GameObject.FindGameObjectWithTag("WandTip");

        // _enemyHealth = GetComponent<EnemyHealth>();
        // health = _enemyHealth.currentHealth;
        isDead = false;
        agent = GetComponent<NavMeshAgent>();
        
        Initalize();
        FindNextPoint();
    }

    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        // health = _enemyHealth.currentHealth;
        switch (currentState) {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

        if (currentState != FSMStates.Dead && currentState != FSMStates.Idle) {
            agent.SetDestination(nextDestination);
            FaceTaget(nextDestination);
        }

        // if (health <= 0) {
        //     currentState = FSMStates.Dead;
        // }
    }

    void Initalize() {
        currentState = FSMStates.Patrol;
        isDead = false;
        FindNextPoint();
    }
    
    void UpdatePatrolState() {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        if (Vector3.Distance(transform.position, nextDestination) < 3) {
            FindNextPoint();
        }

        if (distanceToPlayer <= chaseDistance && IsPlayerInClearFOV()) {
            currentState = FSMStates.Chase;
        }
        
        print("patrolling");
        animator.SetInteger("animState", 0);
        agent.stoppingDistance = 0;
        agent.speed = 3.5f;
    }
    
    void UpdateChaseState() {
        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }
        
        print("chasing");
        animator.SetInteger("animState", 1);
        nextDestination = player.transform.position;
        agent.stoppingDistance = attackDistance;
        agent.speed = 5.0f;
    }

    void UpdateAttackState() {

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }

        print("attacking");
        animator.SetInteger("animState", 2);
        nextDestination = player.transform.position;
        EnemySpellCast();
    }

    void UpdateDeadState() {
        animator.SetInteger("animState", 4);
        deadTransform = gameObject.transform;
        isDead = true;
        Destroy(gameObject, 3);
    }

    void EnemySpellCast() {
        if (isDead || elapsedTime < shootRate) {
            return;
        }

        elapsedTime = 0;
        animator.SetInteger("animState", 3);
        var animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(SpellCasting), animDuration);
    }

    void SpellCasting() {
        Instantiate(spellProjectile[Random.Range(0, spellProjectile.Length)], 
            wandTip.transform.position, wandTip.transform.rotation);
    }

    private void OnDestroy() {
        // Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    }

    void FaceTaget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, enemySpeed * 3 * Time.deltaTime);
    }

    void FindNextPoint() {
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 lookVector = enemyEyes.forward * chaseDistance;
        
        Vector3 frontRayPoint = enemyEyes.position + lookVector;
        Vector3 leftRayPoint = enemyEyes.position + Quaternion.Euler(0, fov * 0.5f, 0) * lookVector;
        Vector3 rightRayPoint =  enemyEyes.position + Quaternion.Euler(0, -fov * 0.5f, 0) * lookVector;
        
        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV() {
        Vector3 dirToPlayer = player.transform.position - enemyEyes.position;
        if (Vector3.Angle(dirToPlayer, enemyEyes.forward) <= fov) {
            RaycastHit hit;
            if (Physics.Raycast(enemyEyes.position, dirToPlayer, out hit, chaseDistance)) {
                if (hit.collider.CompareTag("Player")) {
                    return true;
                }
            }
        }

        return false;
    }
}
