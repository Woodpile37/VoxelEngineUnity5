using UnityEngine;
using System.Collections;

[SerializeField]
public class ItemStack : SerializableClass {

    string itemName = "Null";

    string description = "I'm a description";

    int damage = 1;

    bool isBlock = true;

    string texture;

    string placeSkyMaterial = "DIRT";

    int amount = 1;

    public ItemStack(string name) {

    }
    public void setAmount(int amount) {
        this.amount = amount;
    }
    public int getAmount() {
        return amount;
    }
}
