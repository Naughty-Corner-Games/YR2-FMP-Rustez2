using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour 
{
 
    [SerializeField] private StatsClass StatsClass;
    public ItemClass Wood;
    public ItemClass Apple;
    public InventoryManager inv;


    [HideInInspector]public float Health;
    [HideInInspector] public float Damage;
    [HideInInspector] public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Health = StatsClass.Health;
        Damage = StatsClass.Damage;

    }


    private void Update()
    {
        if (Health <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    private IEnumerator Kill()
    {
        anim.SetTrigger("Kill");
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
      
        yield return new WaitForSeconds(2);

        Destroy(this.gameObject);

        var appleFreq = Random.Range(0, 4);
        if (appleFreq > 0)
            inv.Add(Apple, appleFreq, "apple");

        inv.Add(Wood, 3, "Wood");


       

       
    }




}
