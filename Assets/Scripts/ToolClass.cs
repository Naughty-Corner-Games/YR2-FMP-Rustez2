using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu]
public class ToolClass : ItemClass
{
    [SerializeField] private int damageDealt = 5;
    public ToolType toolType;
    public override void Use(PlayerMovement caller)
    {
       
        //base.Use();
        //Debug.Log(toolType + " " + damageDealt);
    }

    public AnimatorController AnimCont;

    public override ToolClass GetTool() { return this; }
}



public enum ToolType
{
    Sword,
    Pickaxe,
    Axe,
    Spear
};
