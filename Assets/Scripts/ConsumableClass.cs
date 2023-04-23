using UnityEngine;

[CreateAssetMenu]
public class ConsumableClass : ItemClass
{
    public int addHealth = 10;

    public override void Use(PlayerMovement caller)
    {
        //base.Use();
        Debug.Log(itemName + " " + addHealth);
    }

    public override ConsumableClass GetConsumable() { return this; }
}
