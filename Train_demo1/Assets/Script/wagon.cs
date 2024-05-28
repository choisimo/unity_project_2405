using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wagon : MonoBehaviour
{
    public float speed;
    public GameObject tofollow;

    private Rigidbody rig;
    public float TurningSpeed = 1f;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
 
    void Update()
    {
        Vector3 look = tofollow.transform.position - transform.position;
        look.y = 0;
        Quaternion rotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, tofollow.transform.position, step);

        if(Vector3.Distance(transform.position, tofollow.transform.position) < 0.001f)
        {
            tofollow.transform.position *= -1.0f;
        }
    }
}
