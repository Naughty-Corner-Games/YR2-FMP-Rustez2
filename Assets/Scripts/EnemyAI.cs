using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 PlayerTrans;
    public GameObject Player;
    private Vector3 distance;
    private Transform WayPoint;
    private Animator anim;
    private PlayerMovement Pm;
    public Stats EnemyStats;
    public float Health;
    private float Damage;
    public bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim= GetComponent<Animator>();
        Health = EnemyStats.Health;
        Damage = EnemyStats.Damage;
        Pm = Player.gameObject.GetComponent<PlayerMovement>();
    }

    public void Patrol()
    {
        if (distance.magnitude >= 10)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
    public void Follow()
    {
        if (distance.magnitude < 10)
        {
            agent.SetDestination(PlayerTrans);
        }
    }

    public void Attack()
    {
        if (distance.magnitude < 2 && canAttack)
        {
            StartCoroutine(Pm.Cooldown());
            Pm.TakeDamage(10);
            Debug.Log("Damage");
            
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

        //distance = Vector3.Distance(agent.transform.position, Player.transform.position);
        //distance = (Vector3)Vector3.Distance(agent.transform.position, PlayerTrans.transform.position);

        distance = transform.position - Player.transform.position;
        Patrol();
        Follow();
        Attack();
        Debug.Log(distance.magnitude);



        if (Health <= 0)
            Destroy(this.gameObject);
            



    }


    
}
