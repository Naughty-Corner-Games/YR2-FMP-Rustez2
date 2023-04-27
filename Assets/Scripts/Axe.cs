using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Axe : MonoBehaviour
{

    public TreeScript tree;
    public InventoryManager inv;
    public bool axeSwing;
    public LayerMask TreeMask;
    public int damage = 20;
    public ItemClass Wood;
    public ItemClass Apple;
    
    //public int appleFreq;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AxeSwung()
    {
        axeSwing = true;
        Debug.Log(axeSwing);

    }




    private void OnCollisionStay(Collision collision)
    {
        tree = collision.collider.gameObject.GetComponent<TreeScript>();
        if (collision.gameObject.CompareTag("Tree") && axeSwing)
        {
            axeSwing = false;
            Debug.Log(axeSwing);
            tree.anim.SetTrigger("Hit");
            tree.health -= damage;



            //Debug.Log(axeSwing);
            Debug.Log(tree.health);

            if (tree.health <= 0)
            {
                var appleFreq = Random.Range(0, 4);
                Destroy(tree.gameObject);
                inv.Add(Wood, 3, "wood");
                if(appleFreq > 0)
                    inv.Add(Apple, appleFreq, "apple");
            }



        }
        else
            return;

    }

    public void AxeSwingFalse()
    {
      
    }

}
