using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIthroughWalls : MonoBehaviour {

    public List<GameObject> players;

    public NavMeshAgent agent;
    public GameObject aggroedPlayer;

    public bool chasing = false;
    public bool attacking = false;

    public float aggroRange;
    public float losingAggroRange;
    public float attackRange;
    public float attackRotationSpeed;

    public float fovAngle;


	
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	
	void Update () {

        foreach(GameObject player in players)
        {
            if(Vector3.Distance(player.transform.position, gameObject.transform.position) <= aggroRange)
            {
                float angle = Vector3.Angle(player.transform.position - gameObject.transform.position, gameObject.transform.forward);
                if(angle <= fovAngle/2)
                {
                    aggroedPlayer = player;
                    chasing = true;
                }                
                
            }
        }
        if (aggroedPlayer)
        {
            float distance = Vector3.Distance(aggroedPlayer.transform.position, gameObject.transform.position);
            if (distance >= losingAggroRange)
            {
                aggroedPlayer = null;
            }
            else chasing = true;

            if (distance <= attackRange)
            {
                attacking = true;
            }
            else attacking = false;
        }
        else
        {
            chasing = false;
            agent.isStopped = true;
        }

        if(attacking == true)
        {
            FaceTarget();
            Attack();
        }

        if (chasing == true && aggroedPlayer != null)
        {
            Chase();
        }
	}

    public void Attack()
    {
        

    }

    public void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(aggroedPlayer.transform.position);
    }

    public void FaceTarget()
    {
        Vector3 direction = aggroedPlayer.transform.position - gameObject.transform.position;
        direction.y = 0;
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * attackRotationSpeed);
    }
}
