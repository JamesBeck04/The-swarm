using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    [SerializeField] private int EnemyDamage = 15;

    public NavMeshAgent agent;

    public Transform player;

    public GameObject playerhp;

    

    public LayerMask WhatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRage, playerInAttackRange;

    private void Awake()
    {
        
        player = GameObject.Find("Player").transform;
        playerhp = GameObject.Find("Player");

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRage = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRage && !playerInAttackRange) Patroling();
        if (playerInSightRage && !playerInAttackRange) ChasePlayer();
        if (playerInSightRage && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomz = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomz);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround)) 
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //putattackcodehere dont foget
            print("hit player");
            playerhp.GetComponent<PlayerMovement>().TakeDamagePlayer(EnemyDamage);
            

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }



    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
          
            Destroy(gameObject);
        }
        else
        {
          
        }
    }
}
