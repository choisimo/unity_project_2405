using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    /*
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;

    void Start()
    {
        UpdateInventoryUI();
    }

    public void AddItem(InventoryItem item)
    {
        inventoryItems.Add(item);
        UpdateInventoryUI();
    }

    public void RemoveItem(InventoryItem item)
    {
        inventoryItems.Remove(item);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventoryItems)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, inventoryPanel.transform);
            itemSlot.GetComponentInChildren<Image>().sprite = item.itemIcon;
        }
    }
*/
}
