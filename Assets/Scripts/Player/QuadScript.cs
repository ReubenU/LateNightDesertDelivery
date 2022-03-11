using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScript : MonoBehaviour
{
    // Main Rigidbody variable
    Rigidbody rigid;

    // Quad's center of mass
    public Transform centerOfMass;

    // Wheels for us to control.
    // REMEMBER: 
    // Front wheel steer angle: 22.5 min, 30 max
    // Rear wheel steer angle: 12 min, 15 max
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    // ATV Quad Settings
    public float horsePower;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        Accelerate();
    }


    void Accelerate()
    { 

    }
}
