using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class startSceneWeapon : MonoBehaviour
{
    public enum weaponType { Melee, Range };
    public weaponType weapontype;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public Transform ammoPos;
    public Transform ammoCasePos;
    public GameObject ammo;
    public GameObject ammoCase;
    public TrailRenderer trailEffect;
    public Transform character;
    public Camera mainCamera;

    public Transform aimPoint;
    // 적 상태 UI
    public RectTransform enemyStatusUIRect;

    private bool isFiring;

    public void Use()
    {
        Debug.Log("Fire coroutine start");
        if (weapontype == weaponType.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (weapontype == weaponType.Range)
        {
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        isFiring = false;
    }

    IEnumerator Shot()
    {
        while (Input.GetButton("Fire1"))
        {
            Vector3 targetPoint;

            // Using the position of aimPoint as the direction
            if (aimPoint != null)
            {
                targetPoint = aimPoint.position;
            }
            else
            {
                Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 화면 중앙으로부터 레이 쏘기
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(100); // Assume the target is far away if nothing is hit
                }
            }

            // ammo fire
            if (ammo != null && ammoPos != null)
            {
                GameObject instantBullet = Instantiate(ammo, ammoPos.position, ammoPos.rotation);
                if (instantBullet != null)
                {
                    Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                    if (bulletRigid != null)
                    {
                        Vector3 direction = (targetPoint - ammoPos.position).normalized;
                        bulletRigid.velocity = direction * 50;

                        if (character != null)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10);
                        }
                    }
                }
            }

            // ammo case 
            if (ammoCase != null && ammoCasePos != null)
            {
                GameObject instantCase = Instantiate(ammoCase, ammoCasePos.position, ammoCasePos.rotation);
                if (instantCase != null)
                {
                    Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
                    if (caseRigid != null)
                    {
                        Vector3 caseVec = ammoCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
                        caseRigid.AddForce(caseVec, ForceMode.Impulse);
                        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
                    }
                }
            }

            yield return new WaitForSeconds(rate);
        }

        isFiring = false;
    }

}
