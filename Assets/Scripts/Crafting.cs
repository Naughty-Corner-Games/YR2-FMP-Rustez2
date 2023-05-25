using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Crafting : MonoBehaviour
{
  public  InventoryManager inventoryManager;
  public  ItemClass Spear;
  public  ItemClass Wood;


    public void CraftSpear()
    {
        for (int i = 0; i < inventoryManager.items.Length; i++)
        {
            if(inventoryManager.items[i].GetItem() == Wood && inventoryManager.items[i].quantity >= 5)
            {
                inventoryManager.items[i].SubQuantity(5);
                inventoryManager.Add(Spear, 1, "Spear");
                Debug.Log("work");
                break;
            }
        }

        Debug.Log("no work");
        
    }

    void Start()
    {
      
    }










    // Update is called once per frame
    void Update()
    {

    }






















}