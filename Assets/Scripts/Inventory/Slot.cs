using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour {

    Inventory currentInventory;
    public Image image;
    ItemStack item;
    public Sprite defaultImage;
    public int id;
    public Text amountText;

    public void clear() {
        image.sprite = null;
        image.enabled = false;
    }
    public void setItem(ItemStack item) {
        if (item != null) {
            image.sprite = defaultImage;
            amountText.text = item.getAmount() + "";
            image.enabled = true;
            amountText.enabled = true;
        } else {
            image.enabled = false;
            amountText.enabled = false;
        }
        this.item = item;
    }
    public ItemStack getItem() {
        return item;
    }
    public Inventory getCurrentInventory() {
        return currentInventory;
    }
    public void setCurrentInventory(Inventory inv) {
        currentInventory = inv;
    }
}
