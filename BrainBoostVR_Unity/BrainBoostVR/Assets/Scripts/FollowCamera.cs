using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Main Camera
    public Vector3 offset = new Vector3(0, -0.2f, 1.5f); // devant les yeux

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        transform.position = cameraTransform.position + 
                             cameraTransform.forward * offset.z + 
                             cameraTransform.up * offset.y + 
                             cameraTransform.right * offset.x;

        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
