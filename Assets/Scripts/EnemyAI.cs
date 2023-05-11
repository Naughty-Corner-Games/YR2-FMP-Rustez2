using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 PlayerTrans;
    public GameObject Player;
    private float distance;
    private Transform WayPoint;
    private Animator anim;
    private PlayerMovement Pm;
    public Stats EnemyStats;
    public float Health;
    private float Damage;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim= GetComponent<Animator>();
        Health = EnemyStats.Health;
        Damage = EnemyStats.Damage;
    }

    public void Patrol()
    {
        if (distance >= 10)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
    public void Follow()
    {
        if (distance < 10)
        {
            agent.SetDestination(PlayerTrans);
        }
    }

    public void Attack()
    {
        if (distance < 2)
        {
            //anim.SetTrigger("");
        
        }
    }

    public void Hit()
    {
        Health -= Damage;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerTrans = Player.transform.position;
        
        distance = Vector3.Distance(agent.transform.position, Player.transform.position);
        Patrol();
        Follow();





        if (Health <= 0)
            Destroy(this.gameObject);
            



    }
}
