using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPowerItem : MonoBehaviour, IItem
{
    [SerializeField] int _ItemID;
    [SerializeField] string _ItemName;
    [SerializeField] string _ItemDescription;
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
     
        GamePlayHandler.Instance.AddHealth(10);
    }
}
