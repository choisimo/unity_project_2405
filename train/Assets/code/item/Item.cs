using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {
        Ammo, fuel, Grenade, life , weapon
    };

    public ItemType itemtype;
    public int itemIndex;
    public string itemName;
    public int num;
    
    void Update()
    {
        //transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

}
