using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Animations;
using UnityEngine;

public class Axe : MonoBehaviour
{

    public TreeScript tree;
    public InventoryManager inv;
    public bool axeSwing;
    public LayerMask TreeMask;
    public int damage = 10;
    public ItemClass Wood;
    public ItemClass Apple;
    public EnemyAI EnemyAI;
  
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
    

    }


    private void OnTriggerStay(Collider other)
    {
        tree = other.gameObject.GetComponent<TreeScript>();
        if (other.gameObject.CompareTag("Tree") && axeSwing && inv.selectedItem.GetTool().toolType == ToolType.Axe)
        {
            axeSwing = false;
            Debug.Log(axeSwing);
            tree.anim.SetTrigger("Hit");
            tree.Health -= tree.Damage;


            //Debug.Log(axeSwing);
            Debug.Log(tree.Health);

        }

        EnemyAI = other.gameObject.GetComponent<EnemyAI>();
        if (other.gameObject.CompareTag("Enemy") && axeSwing && inv.selectedItem.GetTool().toolType == ToolType.Spear)
        {

            axeSwing = false;
            EnemyAI.Hit();
            Debug.Log("Hitting Enemy");
            Debug.Log(EnemyAI.Health);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    public void AxeSwingFalse()
    {
        axeSwing = false;
    }

}
