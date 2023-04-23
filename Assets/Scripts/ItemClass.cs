using UnityEngine;

[CreateAssetMenu]
public class ItemClass : ScriptableObject
{
    public string itemName = "New Item Name";
    public Sprite itemIcon = null;
    public bool isStackable = true;
    public int maxStackSize = 64;
    public ItemType itemType;
    //public Animator itemAnimation;

    public virtual void Use(PlayerMovement caller)
    {
        Debug.Log("Used");
    }

    public virtual ItemClass GetItem() { return this; }
    public virtual ConsumableClass GetConsumable() { return null; }
    public virtual ToolClass GetTool() { return null; }
}

public enum ItemType
{
    Item,
    Consumable,
    Tool
};
