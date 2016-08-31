using UnityEngine;
using System.Collections;

public class Inventory {

    public enum InventoryType {
        DEFAULT_INVENTORY, PLAYER_INVENTORY
    }

    public string title = "Null";

    public int size = 20;

    public ItemStack[] itemStacks;

    public string ownerName;

    public InventoryType inventoryType = InventoryType.DEFAULT_INVENTORY;

    public Inventory() {

    }
    public Inventory(string title, string ownerName, InventoryType type) {
        this.title = title;
        this.ownerName = ownerName;
        this.itemStacks = new ItemStack[size];
        inventoryType = type;
    }
    public Inventory(string title, string ownerName) {
        this.title = title;
        this.ownerName = ownerName;
        this.itemStacks = new ItemStack[size];
    }
    public int getSize() {
        return size;
    }
    public void setItem(int index, ItemStack item) {
        itemStacks[index] = item;
    }
    public void addItem(ItemStack item) {
        int index = 0;
        foreach (ItemStack i in itemStacks) {
            if (i == null) {
                itemStacks[index] = item;
                break;
            }
            index++;
        }
    }
    public void remove(ItemStack item) {
        int index = 0;
        foreach (ItemStack i in itemStacks) {
            if (i.Equals(item)) {
                removeAt(index);
                break;
            }
            index++;
        }
    }
    public void removeAt(int index) {
        itemStacks[index] = null;
    }
    public ItemStack getItem(int index) {
        return itemStacks[index];
    }
    public ItemStack[] getContains() {
        return (ItemStack[])itemStacks.Clone();
    }
    public void setContains(ItemStack[] itemStacks) {
        this.itemStacks = itemStacks;
    }
}
