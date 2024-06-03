using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    private void Update()
    {
        transform.position = player.position + offset;
    }
}
