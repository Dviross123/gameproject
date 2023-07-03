using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 5f, -10f);   // Offset from the player

    private void LateUpdate()
    {
        if (target != null)
        {
            // Set the camera's position to the player's position with the offset
            transform.position = target.position + offset;
        }
    }
}
