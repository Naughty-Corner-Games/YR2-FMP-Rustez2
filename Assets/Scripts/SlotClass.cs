using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [field: SerializeField] public string name { get; private set;} = null;
    [field: SerializeField] public ItemClass item { get; private set;} = null;
    [field: SerializeField] public int quantity { get; private set;} = 0;

    public SlotClass()
    {
        item = null;
        quantity = 0;
        name = null;
    }

    public SlotClass(ItemClass _item, int _quantity, string _name)
    {
        name = _name;
        item = _item;
        quantity = _quantity;
    }

    public SlotClass(SlotClass slot)
    {
        this.item = slot.item;
        this.quantity = slot.quantity;
        this.name = slot.name;
    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
        this.name = null;
    }

    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public string GetName() { return name; }

    public void AddQuantity(int _quantity) { quantity += _quantity; }
    public void SubQuantity(int _quantity)
    {
        quantity -= _quantity; 
        
        if(quantity <= 0)
            Clear();
    }

    public void AddItem(ItemClass item, int quantity, string name)
    {
        this.item = item;
        this.quantity = quantity;
        this.name = name;
    }
}
