using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public int fuel = 0;
    private PlayerController1 player;
    private Rigidbody rb;
    private bool isDriving;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController1>();
        fuel = player.fuel;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        isDriving = false;
    }
    void Update()
    {
        if (isDriving)
        {
            // Add vehicle control logic here
            float move = Input.GetAxis("Vertical") * 10f * Time.deltaTime;
            float turn = Input.GetAxis("Horizontal") * 50f * Time.deltaTime;

            transform.Translate(0, 0, move);
            transform.Rotate(0, turn, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetDriving(bool driving)
    {
        isDriving = driving;
        rb.isKinematic = !driving; 
    }
}
