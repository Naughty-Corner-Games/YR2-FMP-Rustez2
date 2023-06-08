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


    public IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1);
        Pm.TakeDamage(10);
        canAttack = true;
    }
    public void Attack()
    {


        if (distance.magnitude < 3 && canAttack)
        {
            StartCoroutine(Cooldown());
          
         
            
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
     



        if (Health <= 0)
            Destroy(this.gameObject);
            



    }


    
}
