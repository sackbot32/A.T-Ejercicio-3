using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.IO;
public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] 
    private int currentLife;
    [SerializeField] 
    private int maxsLife;
    [SerializeField] 
    private int enemyScorePoints;
    [SerializeField]
    private Transform patrolPointsParent;
    private List<Transform> patrolPointList;
    private int currentPatrolPoint;
    private NavMeshAgent agent;

    private Transform playerTrans;

    private WeaponController wController;

    [SerializeField]
    private bool playerSeen;


    private void Start()
    {
        patrolPointList = new List<Transform>();
        foreach (Transform item in patrolPointsParent)
        {
            patrolPointList.Add(item);
        }
        wController = GetComponent<WeaponController>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        SearchForPlayer();
        if(!playerSeen)
        {
            PatrolWithChild();
            //Patrol();
        }
    }
    /// <summary>
    /// Enemy search and go towards player
    /// </summary>
    private void SearchForPlayer()
    {
        NavMeshHit hit;

        playerSeen = !agent.Raycast(playerTrans.position, out hit) && (hit.distance <= 10f);
        if(!agent.Raycast(playerTrans.position, out hit))
        {
            if(hit.distance <= 10f)
            {
                agent.SetDestination(playerTrans.position);
                agent.stoppingDistance = 5f;
                transform.GetChild(0).transform.LookAt(playerTrans.position);
                if (hit.distance <= 6f)
                {
                    if (wController.CanShoot())
                    {
                        wController.Shoot();
                    }
                }
            }
        } else
        {
            if(!agent.pathPending)
            {
                agent.stoppingDistance = 0f;
                agent.SetDestination(patrolPointList[currentPatrolPoint].position);
            }
        }

    }
    /// <summary>
    /// Goes to patrol points and goes to the next one when reached
    /// </summary>
    private void Patrol()
    {  
        if(patrolPointList.Count > 0)
        {

            if(agent.destination == null || !agent.pathPending)
            {
                agent.stoppingDistance = 0f;
                agent.SetDestination(patrolPointList[currentPatrolPoint].position);
            }
            if(agent.remainingDistance < 1f)
            {
                currentPatrolPoint++;
                currentPatrolPoint = currentPatrolPoint >= patrolPointList.Count ? 0 : currentPatrolPoint;
                agent.SetDestination(patrolPointList[currentPatrolPoint].position);
                agent.stoppingDistance = 0f;
            } 
        }
    }
    /// <summary>
    /// Same as Patrol() but with GetChild()
    /// </summary>
    private void PatrolWithChild()
    {
        if (patrolPointsParent.childCount > 0)
        {

            if (agent.destination == null || !agent.pathPending)
            {
                agent.stoppingDistance = 0f;
                agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
            }
            if (agent.remainingDistance < 1f)
            {
                currentPatrolPoint++;
                currentPatrolPoint = currentPatrolPoint >= patrolPointList.Count ? 0 : currentPatrolPoint;
                agent.SetDestination(patrolPointsParent.GetChild(currentPatrolPoint).position);
                agent.stoppingDistance = 0f;
            }
        }
    }

    /// <summary>
    /// Handle when the enemy receive a bullet
    /// </summary>
    /// <param name="quantity"></param>
    public void DamageEnemy(int quantity)
    {
        currentLife -= quantity;
        if(currentLife <= 0 )
        {
            currentLife = 0;
            Destroy(gameObject);
        }
    }



}
