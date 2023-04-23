using UnityEngine;

[CreateAssetMenu]
public class ToolClass : ItemClass
{
    [SerializeField] private int damageDealt = 5;
    [SerializeField] private ToolType toolType;
    public override void Use(PlayerMovement caller)
    {
       
        //base.Use();
        Debug.Log(toolType + " " + damageDealt);
    }

    public override ToolClass GetTool() { return this; }
}

public enum ToolType
{
    Sword,
    Pickaxe,
    Axe
};
