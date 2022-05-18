using UnityEngine;

public class NicknameBoard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Quaternion cameraRotation = _cameraTransform.rotation;
        transform.LookAt(transform.position + cameraRotation * Vector3.forward, cameraRotation * Vector3.up);
    }
}