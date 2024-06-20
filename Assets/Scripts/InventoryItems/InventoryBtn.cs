using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBtn : MonoBehaviour
{
    public Button useBtn;
    public Button removeBtn;
    public Image InventoryImage;
    public Button thisBtn;
    private void OnDisable()
    {
        useBtn.gameObject.SetActive(false);
        removeBtn.gameObject.SetActive(false);
    }
}
