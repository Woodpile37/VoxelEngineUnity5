using UnityEngine;
using System.Collections;
using System;

public class InterfaceHandler : MonoBehaviour {

    public static GameObject itemSlotPrefab;

    public static GameObject DefaultInventoryCanvas;

    public GameObject defaultInventoryCanvas;

    public static GameObject SlotPanel;

    public GameObject slotPanel;

    public static Slot[] slots = new Slot[20];


    void Start() {

        itemSlotPrefab = GameManager.getPrefabGameObject("Slot");

        DefaultInventoryCanvas = defaultInventoryCanvas;

        SlotPanel = slotPanel;

        for (int i = SlotPanel.transform.childCount - 1; i >= 0; i--) {
            slots[i] = SlotPanel.transform.GetChild(i).GetComponent<Slot>();
        }
    }
    public static void showInventory(Inventory inv) {
        closeInventory();
        DefaultInventoryCanvas.SetActive(true);
        updateInventory(inv);

    }
    public static void updateInventory(Inventory inv) {
        int index = 0;
        foreach (ItemStack item in inv.getContains()) {
            Slot slot = slots[index];
            if (slot == null) {
                GameObject newSlotObject = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                newSlotObject.transform.SetParent(SlotPanel.transform);
                Slot s = newSlotObject.GetComponent<Slot>();
                s.setCurrentInventory(inv);
                s.id = index;
                slots[index] = s;
                slot = s;
            }
            slot.setItem(item);

            index++;
        }
    }
    public static void closeInventory() {
        if (DefaultInventoryCanvas.activeSelf) {
            DefaultInventoryCanvas.SetActive(false);
            foreach (Slot slot in slots) {
                if (slot != null) {
                    slot.clear();
                }
            }
        }
    }
}
