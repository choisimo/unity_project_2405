using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform targetPlayer;
    public Vector3 offset;


    private void Update()
    {
        transform.position = targetPlayer.position + offset;
    }
}
