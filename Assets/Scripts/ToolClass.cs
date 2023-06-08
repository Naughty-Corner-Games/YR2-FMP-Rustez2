
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class ToolClass : ItemClass
{
   // [SerializeField] private int damageDealt = 5;
    public ToolType toolType;
    public override void Use(PlayerMovement caller)
    {
       
        //base.Use();
        //Debug.Log(toolType + " " + damageDealt);
    }

    public RuntimeAnimatorController AnimCont;

    public override ToolClass GetTool() { return this; }
}



public enum ToolType
{
    Sword,
    Pickaxe,
    Axe,
    Spear
};
