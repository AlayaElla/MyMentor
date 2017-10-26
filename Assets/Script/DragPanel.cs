using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform dragObject;
    private RectTransform dragArea;

    void Awake()
    {
        dragObject = transform as RectTransform;
        dragArea = dragObject.parent as RectTransform;
    }

    public void OnPointerDown(PointerEventData data)
    {
        originalPanelLocalPosition = dragObject.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out originalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (dragObject == null || dragArea == null)
        {
            Debug.Log("找不到拖动物体，或者拖动区域！");
            return;
        }


        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Debug.Log(localPointerPosition);
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            dragObject.localPosition = originalPanelLocalPosition + offsetToOriginal;
        }

        ClampToWindow();
    }

    // Clamp panel to area of parent
    void ClampToWindow()
    {
        Vector3 pos = dragObject.localPosition;

        Vector3 minPosition = dragArea.rect.min - dragObject.rect.min;
        Vector3 maxPosition = dragArea.rect.max - dragObject.rect.max;

        pos.x = Mathf.Clamp(dragObject.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(dragObject.localPosition.y, minPosition.y, maxPosition.y);

        dragObject.localPosition = pos;
    }
}
