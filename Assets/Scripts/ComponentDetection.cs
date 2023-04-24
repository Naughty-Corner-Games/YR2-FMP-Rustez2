using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDetection : MonoBehaviour
{

    public Camera cam;
    public LayerMask tree;
    public Collider treeCol;
    public TreeScript treeScript;
    public Axe axe;
    public Vector3 offset;
    

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
   

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition + offset);

        if (Physics.Raycast(ray, out RaycastHit TreeHit, float.MaxValue, tree))
        {
            transform.position = TreeHit.point;

            axe.tree = TreeHit.collider.gameObject.GetComponent<TreeScript>();
            

        }
    }
}
