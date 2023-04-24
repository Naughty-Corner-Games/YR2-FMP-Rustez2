using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Axe : MonoBehaviour
{

    public TreeScript tree;
    public bool axeSwing;
    public LayerMask TreeMask;
    public int damage = 20;



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




    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Tree") && axeSwing)
        {

            tree.health -= damage;



            //Debug.Log(axeSwing);
            Debug.Log(tree.health);

            if (tree.health == 0)
            {
                Destroy(tree.gameObject);
            }



        }
        else
            return;

    }

    public void AxeSwingFalse()
    {
        axeSwing = false;
        Debug.Log(axeSwing);
    }

}
