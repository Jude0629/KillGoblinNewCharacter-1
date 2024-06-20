using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySystem : MonoBehaviour
{
    public InventoryData inventoryData;

    public int maxitems;
    public GameObject[] inventoryBtns;
    public Sprite[] inventorySprites;
    public GameObject inventoryPanel;

    private void Start()
    {

       // LoadInventory();
    }
    public void LoadInventory()
    {

        foreach (var vv in inventoryBtns)
        {
            vv.gameObject.SetActive(false);
        }

        int i = 0;
        foreach (var v in inventoryData.itemsList)
        {
            InventoryBtn iBtn = inventoryBtns[i].GetComponent<InventoryBtn>();
            Button useBtn = iBtn.useBtn;
            Button removeBtn = iBtn.removeBtn;
            Button thisBtn = iBtn.thisBtn;
            Image invImg = iBtn.InventoryImage;

            invImg.sprite = inventorySprites[v.ItemID];
            iBtn.gameObject.SetActive(true);
            useBtn.onClick.RemoveAllListeners();
            removeBtn.onClick.RemoveAllListeners();
            thisBtn.onClick.RemoveAllListeners();

            useBtn.onClick.AddListener(v.UseItem);
            useBtn.onClick.AddListener(() => RemoveItem(v));
            removeBtn.onClick.AddListener(() => RemoveItem(v));
            thisBtn.onClick.AddListener(HideAllInventoryBtns);
            thisBtn.onClick.AddListener(() =>
            {

                useBtn.gameObject.SetActive(true);
                removeBtn.gameObject.SetActive(true);
            });

            i++;
        }


    }
    public void HideAllInventoryBtns()
    {
        foreach (var vv in inventoryBtns)
        {
            InventoryBtn iBtn = vv.GetComponent<InventoryBtn>();
            iBtn.useBtn.gameObject.SetActive(false);
            iBtn.removeBtn.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IItem item = other.GetComponent<IItem>();

        if (item != null)
        {
            // The object has an IItem component, you can call its methods
            if(inventoryData.itemsList.Count<maxitems)
            {
                if (!inventoryData.itemsList.Contains(item))
                {
                    
               
                    inventoryData.itemsList.Add(item);
                    other.gameObject.SetActive(false);
                    LoadInventory();
                }
            }
            Debug.Log("Damage applied to " + other.name);
        }
        else
        {
            Debug.Log(other.name + " does not implement IItem");
        }
    }
  
    public void RemoveItem(IItem item)
    {
        inventoryData.itemsList.Remove(item);
        LoadInventory();
    }
 
}

[System.Serializable]
public class InventoryData
{
    public List<IItem> itemsList;

    InventoryData()
    {
        itemsList = new List<IItem>();
    }
    
}