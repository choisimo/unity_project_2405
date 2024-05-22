using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public enum weaponType {Melee, Range};
    public weaponType weapontype;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public Transform ammoPos;
    public Transform ammoCasePos;
    public GameObject ammo;
    public GameObject ammoCase;
    public TrailRenderer trailEffect;

    public void Use(){
        if (weapontype == weaponType.Melee){
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing(){
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }
}
