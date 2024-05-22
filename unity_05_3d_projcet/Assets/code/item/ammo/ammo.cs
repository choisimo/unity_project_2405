using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag.Equals("wall") || 
        collision.gameObject.tag.Equals("bottom") ||
        collision.gameObject.tag.Equals("Ground")){
            Destroy(gameObject);
        }
    }
}
