using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag.Equals("bottom") ||
        collision.gameObject.tag.Equals("Ground")){
            Destroy(gameObject, 10f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("wall"))
        {
            Destroy(gameObject);
        } else if (other.tag.Equals("enemy"))
        {
            Destroy(gameObject);
        }
    }

    
}
