using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCollision : MonoBehaviour
{
    public WheelCollider parentWheelCollider;

    WheelHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = parentWheelCollider.GetGroundHit(out hitInfo);

        if (isGrounded)
        {
            Vector3 hitPosition = hitInfo.point;
            Vector3 suspensionDir = (parentWheelCollider.transform.position - hitPosition).normalized;

            transform.position = hitPosition + (suspensionDir * parentWheelCollider.radius);

            transform.localRotation = Quaternion.Euler(Vector3.up * parentWheelCollider.steerAngle);
        }
    }
}
