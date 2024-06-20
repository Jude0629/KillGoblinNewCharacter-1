using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IItem
{
    int _ItemID;
    string _ItemName;
    string _ItemDescription;
    public int ItemID
    {
        get { return _ItemID; }

        set
        {
            _ItemID = value;
        }
    }
    public string ItemName { get { return _ItemName; } set { _ItemName = value; } }
    public string ItemDescription { get { return _ItemDescription; } set { _ItemDescription = value; } }

    public void UseItem()
    {
     
    }
}
public interface IItem
{
   
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public void UseItem();
}