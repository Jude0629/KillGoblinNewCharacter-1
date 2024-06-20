using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPositionSet : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] int id = 0;
    [SerializeField] RectTransform referenceRectTransform;

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);
        transform.position = canvas.transform.TransformPoint(position);
        if (referenceRectTransform != null)
        {
            referenceRectTransform.position = transform.position;
        }
        PlayerPrefs.SetString("B" + id.ToString(), transform.position.ToString());
      
    }

    
}
