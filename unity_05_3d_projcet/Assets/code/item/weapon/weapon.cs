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
        Debug.Log("Fire coroutine start");
        if (weapontype == weaponType.Melee){
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        } else if (weapontype == weaponType.Range) 
        {
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing(){
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }

    IEnumerator Shot(){
        Debug.Log("shooting");
        // ammo fire
        GameObject instantBullet = Instantiate(ammo, ammoPos.position, ammoPos.rotation);    
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = ammoPos.forward * 50;
        
        Debug.Log("Bullet instantiated at " + ammoPos.position);

        yield return null;

        // ammo case 
        GameObject instantCase = Instantiate(ammoCase, ammoCasePos.position, ammoCasePos.rotation);    
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = ammoCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    Debug.Log("Ammo case instantiated at " + ammoCasePos.position);
    }
}
