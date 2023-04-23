using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventorySlotHolder;
    [SerializeField] private SlotClass[] starterItems;
    private SlotClass[] items;
    private GameObject[] inventorySlots;
    private bool isInventoryActive;

    [Header("Hotbar")]
    [SerializeField] private TMP_Text currentItemName;
    [SerializeField] private GameObject hotbarSlotHolder;
    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int currentSlotIndex = 0;
    private GameObject[] hotbarSlots;
    public ItemClass selectedItem;

    [Header("Moving Data")]
    [SerializeField] private GameObject cursor;
    [SerializeField] private bool isMovingItem;

    [SerializeField] private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originSlot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventorySlots = new GameObject[inventorySlotHolder.transform.childCount];
        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];
        
        items = new SlotClass[inventorySlots.Length];

        for (int i = 0; i < hotbarSlots.Length; i++)
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;

        for (int i = 0; i < inventorySlotHolder.transform.childCount; i++)
            inventorySlots[i] = inventorySlotHolder.transform.GetChild(i).gameObject;

        for (int i = 0; i < items.Length; i++)
            items[i] = new SlotClass();

        for (int i = 0; i < starterItems.Length; i++)
            Add(starterItems[i].item, starterItems[i].quantity, starterItems[i].item.itemName);

        RefreshInventoryUI();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) //left click
        {
            if(isInventoryActive)
            {
                if(isMovingItem)
                    EndItemMove();
                else
                    BeginItemMove();
            }
        }
        else if(Input.GetMouseButtonDown(1)) //right click
        {
            if(isInventoryActive)
            {
                if(isMovingItem)
                    EndItemMove_Seperate();
                else
                    BeginItemMove_Split();
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {

            //set to opposite of current state
            inventory.SetActive(!inventory.activeSelf);
            //hotbar.SetActive(!hotbar.activeSelf); 

            isInventoryActive = inventory.activeSelf; 
        }

        if(isInventoryActive)
        { //Show mouse when inventory open
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        { //Hide mouse when inventory closed
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //manage what happens when you pick up an item
        cursor.SetActive(isMovingItem); //if moving item them set cursor active
        cursor.transform.position = Input.mousePosition;
        if(isMovingItem)
        {
            cursor.GetComponent<Image>().sprite = movingSlot.item.itemIcon; //Line 88
            cursor.GetComponentInChildren<TMP_Text>().text = movingSlot.item.itemName + " - " + movingSlot.quantity; //get name and item amount 
        }

        //manage hotbar item selection / scrolling
        if(Input.GetAxis("Mouse ScrollWheel") < 0) //scrolling up
            currentSlotIndex = Mathf.Clamp(currentSlotIndex + 1, 0, hotbarSlots.Length - 1);
        else if(Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling down
            currentSlotIndex = Mathf.Clamp(currentSlotIndex - 1, 0, hotbarSlots.Length - 1);

        hotbarSelector.transform.position = hotbarSlots[currentSlotIndex].transform.position;
        selectedItem = items[currentSlotIndex].item;
        
        if(selectedItem != null)
            currentItemName.transform.GetComponent<TMP_Text>().text = selectedItem.itemName;
        else 
            currentItemName.transform.GetComponent<TMP_Text>().text = "";
    

    }

    #region Inventory UItils
    private void RefreshInventoryUI()
    {
        //loop through every item and determine if an item is stored, if true then set image, item count and everything else
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            try
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].item.itemIcon;
                
                if(items[i].item.isStackable)
                    inventorySlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = items[i].quantity + "";
                else
                    inventorySlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            catch
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                inventorySlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }

        RefreshHotbarUI();
    }

    private void RefreshHotbarUI()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].item.itemIcon;
                if(items[i].item.isStackable)
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = items[i].quantity + "";
                else
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public bool Add(ItemClass item, int quantity, string name)
    {
        //check if inventory contains item
        SlotClass slot = Contains(item);

        if (slot != null && slot.item.isStackable && slot.quantity < item.maxStackSize)
        {
            // going to add 20 = quantity
            // there is already 5 = slot.quantity;
            var quantityCanAdd = slot.item.maxStackSize - slot.quantity; //16 - 5 = 11
            var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);
                
            var remainder = quantity - quantityCanAdd; // = 9
            
            slot.AddQuantity(quantityToAdd);
            if (remainder > 0) Add(item, remainder, item.itemName);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == null) //this is an empty slot
                { 
                    var quantityCanAdd = item.maxStackSize - items[i].quantity; //16 - 5 = 11
                    var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);
                
                    var remainder = quantity - quantityCanAdd; // = 9
            
                    items[i].AddItem(item, quantityToAdd, item.itemName);
                    if (remainder > 0) Add(item, remainder, item.itemName);
                    break;
                }
            }
        }

        RefreshInventoryUI();
        return true;
    }  

    public bool Remove(ItemClass item)
    {
        // items.Remove(item);
        SlotClass temp = Contains(item);
        if(temp != null)
        {
            if(temp.quantity > 1)
                temp.SubQuantity(1);
            else
            {
                int slotToRemove = 0;
                for(int i = 0; i < items.Length; i++)
                {
                    if(items[i].item == item)
                    {
                        slotToRemove = i;
                        break;
                    }
                }

                items[slotToRemove].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshInventoryUI();
        return true;
    }

    public void UseItem()
    {
        items[currentSlotIndex].SubQuantity(1);
        RefreshInventoryUI();
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item)
                return items[i];
        }

        return null;
    }
    #endregion

    #region Moving Data
    private bool BeginItemMove()
    {
        originSlot = GetClosestSlot();
        if(originSlot is null || originSlot.item is null)
            return false; //no item to move

        movingSlot = new SlotClass(originSlot); //create new slotclass taking in originSlot data
        originSlot.Clear(); //then clear originSlot data

        isMovingItem = true;
        RefreshInventoryUI();
        return true;
    }

    private bool EndItemMove()
    {   
        originSlot = GetClosestSlot();
        if(originSlot is null)
        {
            Add(movingSlot.item, movingSlot.quantity, movingSlot.item.itemName);
            movingSlot.Clear();
        }
        else
        {
            if(originSlot.item != null) //if item exists
            {
                if(originSlot.item == movingSlot.item && originSlot.item.isStackable && 
                    originSlot.quantity < originSlot.item.maxStackSize) //theyre the same
                {
                    //stack items
                    var quantityCanAdd = originSlot.item.maxStackSize - originSlot.quantity;
                    var quantityToAdd = Mathf.Clamp(movingSlot.quantity, 0, quantityCanAdd);
                    
                    var remainder = movingSlot.quantity - quantityToAdd;

                    //items can stack so add together
                    originSlot.AddQuantity(quantityToAdd);
                    
                    if(remainder == 0) movingSlot.Clear();
                    else 
                    {
                        movingSlot.SubQuantity(quantityCanAdd);
                        RefreshInventoryUI();
                        return false;
                    }
                }
                else
                {
                    //not the same so swap items
                    tempSlot = new SlotClass(originSlot);                               //a = b
                    originSlot.AddItem(movingSlot.item, movingSlot.quantity, movingSlot.item.itemName); //b = c
                    movingSlot.AddItem(tempSlot.item, tempSlot.quantity, tempSlot.item.itemName);     //c = a

                    RefreshInventoryUI();
                    return true;
                }
            }
            else
            {
                //place item as usual
                originSlot.AddItem(movingSlot.item, movingSlot.quantity, movingSlot.item.itemName); //Line 307
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshInventoryUI();
        return true;
    }

    private bool BeginItemMove_Split()
    {
        originSlot = GetClosestSlot();
        if(originSlot is null || originSlot.item is null)
            return false; //no item to move

        movingSlot = new SlotClass(originSlot.item, Mathf.CeilToInt(originSlot.quantity / 2), originSlot.item.itemName); //create new slotclass taking in originSlot data but half the quantity
        originSlot.SubQuantity(Mathf.CeilToInt(originSlot.quantity / 2));

        if(originSlot.quantity == 0)
        {
            originSlot.Clear();
            isMovingItem = false;
            return false;
        }

        if(movingSlot.quantity < 1)
        {
            movingSlot.Clear();
            isMovingItem = false;
            return false;
        }

        isMovingItem = true;
        RefreshInventoryUI();
        return true;
    }

    private bool EndItemMove_Seperate()
    {   
        originSlot = GetClosestSlot();
        if(originSlot is null)
            return false; //no item to move
        if(originSlot.item != null && (originSlot.item != movingSlot.item || originSlot.quantity >= originSlot.item.maxStackSize))
            return false;

        movingSlot.SubQuantity(1);
        if(originSlot.item != null && originSlot.item == movingSlot.item)
            originSlot.AddQuantity(1);
        else
            originSlot.AddItem(movingSlot.item, 1, movingSlot.item.itemName); //Line 355

        if(movingSlot.quantity < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
            isMovingItem = true;

        RefreshInventoryUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if(Vector2.Distance(inventorySlots[i].transform.position, Input.mousePosition) <= 65)
                return items[i];
        }
        return null;
    }
    #endregion
}