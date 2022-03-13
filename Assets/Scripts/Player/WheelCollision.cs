
using UnityEngine;

public class WheelCollision : MonoBehaviour
{
    public WheelCollider parentWheelCollider;
    public Transform wheelSpinner;

    WheelHit hitInfo;


    public bool negateRotation = false;
    float wheelRPM = 0;
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


            wheelRPM += ((negateRotation)?-1:1) * (parentWheelCollider.rpm * 6) * Time.deltaTime;
            wheelRPM = Loop2Range(wheelRPM, 0, 360f);
            wheelSpinner.localRotation = Quaternion.Euler(Vector3.up * wheelRPM);
        }
        else
        {
            Vector3 downVec = parentWheelCollider.transform.TransformDirection(Vector3.down) * .25f;
            transform.position = parentWheelCollider.transform.position + downVec;
            transform.rotation = parentWheelCollider.transform.rotation;
        }
    }

    float Loop2Range(float inputVal, float minVal, float maxVal)
    {
        float isMaxxed = (inputVal > maxVal) ? minVal : 0;
        float isMinned = (inputVal < minVal) ? maxVal : 0;
        float Default = (inputVal < maxVal && inputVal > minVal) ? inputVal : 0;

        return isMaxxed + isMinned + Default;
    }
}
