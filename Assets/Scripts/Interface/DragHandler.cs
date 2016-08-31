using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler {
    public CanvasGroup cavasGroup;
    Transform originalParent;
    Vector2 originalPosition;
    Slot thisSlot;
    void Start() {
        originalParent = transform.parent;
        originalPosition = transform.position;
        thisSlot = originalParent.GetComponent<Slot>();
    }
    public void OnBeginDrag(PointerEventData eventData) {
        transform.position = eventData.position;
        transform.SetParent(originalParent.parent);
        cavasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }
    public void OnDrop(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(originalParent);
        transform.position = originalParent.transform.position;
        cavasGroup.blocksRaycasts = true;

        GameObject hit = eventData.pointerCurrentRaycast.gameObject;

        Inventory inv = thisSlot.getCurrentInventory();
        if (hit != null) {
            Slot slot = hit.GetComponent<Slot>();
            if (slot != null) {
                ItemStack slotItem = slot.getItem();
                inv.setItem(slot.id, thisSlot.getItem());
                inv.setItem(thisSlot.id, slotItem);
                slot.setItem(thisSlot.getItem());
                thisSlot.setItem(null);
            }
        }
        InterfaceHandler.updateInventory(inv);
    }
}
