using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {
        Ammo, fuel, Grenade, hear, weapon
    };

    public ItemType type;
    public int itemIndex;
    public string itemName;
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

}
