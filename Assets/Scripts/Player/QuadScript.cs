using UnityEngine;
using UnityEngine.InputSystem;

public class QuadScript : MonoBehaviour
{
    // Main Rigidbody variable
    Rigidbody rigid;

    // Quad's center of mass
    public Transform centerOfMass;

    // Camera control transform
    public Transform camControl;

    // Quad controls
    public QuadControls quadControls;

    private InputAction move;

    // Wheels for us to control.
    // REMEMBER: 
    // Front wheel steer angle: 22.5 min, 30 max
    // Rear wheel steer angle: 12 min, 15 max
    public float minFrontSteer = 22.5f;
    public float maxFrontSteer = 30f;
    public float minRearSteer = 12f;
    public float maxRearSteer = 15f;

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    // ATV Quad Engine Settings
    public float torque;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.centerOfMass = centerOfMass.localPosition;

        quadControls = new QuadControls();
    }

    // Enable all quad controls
    private void OnEnable()
    {
        quadControls.Enable();

        move = quadControls.Player.Move;
        move.Enable();
    }

    // Disable all controls
    private void OnDisable()
    {
        quadControls.Disable();

        move.Disable();
    }

    private void FixedUpdate()
    {
        Accelerate();
    }

    private void Update()
    {
        Steer();
    }


    // All wheel steering
    private float currentAngle = 0f;
    private float angleVel = 0f;
    void Steer()
    {
        // Get the general direction we want to move towards...
        Vector3 moveStickVal = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
        Vector3 moveDirection = camControl.TransformDirection(moveStickVal);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;

        // The quad's facing direction...
        Vector3 quadDir = transform.TransformDirection(Vector3.forward);
        quadDir.y = 0;
        quadDir = quadDir.normalized;

        // Find the angle between two vectors using a custom function
        // because Unity is too retarded to do precise calculations.
        // Thanks u/ActionScripter9109.
        float angle = AngleBetweenVectors(quadDir, moveDirection, transform.TransformDirection(Vector3.up)) * Mathf.Rad2Deg;

        // Smooth the steering angle so the quad isn't so spastic.
        currentAngle = Mathf.SmoothDampAngle(currentAngle, angle, ref angleVel, 2f * Time.deltaTime);

        // Apply steering only when accelerating.
        float finalSteerAngle = currentAngle * moveStickVal.magnitude;

        // Apply the steering angle to each and every wheel with
        // their custom min and max steering angles.
        float frontLeftSteer = Mathf.Clamp(finalSteerAngle, -maxFrontSteer, minFrontSteer);
        float frontRightSteer = Mathf.Clamp(finalSteerAngle, -minFrontSteer, maxFrontSteer);
        float rearLeftSteer = Mathf.Clamp(-finalSteerAngle, -maxRearSteer, minRearSteer);
        float rearRightSteer = Mathf.Clamp(-finalSteerAngle, -minRearSteer, maxRearSteer);

        frontLeft.steerAngle = frontLeftSteer;
        frontRight.steerAngle = frontRightSteer;
        rearLeft.steerAngle = rearLeftSteer;
        rearRight.steerAngle = rearRightSteer;
    }

    // Steer helper function
    // Special thanks to u/ActionScripter9109
    float AngleBetweenVectors(Vector3 v1, Vector3 v2, Vector3 up)
    {
        // provides a cleaner result at low angles than Vector3.Angle()
        Vector3 cross = Vector3.Cross(v1, v2);
        float dot = Vector3.Dot(v1, v2);
        float angle = Mathf.Atan2(cross.magnitude, dot);
        //float test = Vector3.Dot(up, cross);

        //if (test < 0f) angle = -angle;
        angle *= Mathf.Sign(Vector3.Dot(up, cross));


        return angle; // radians
    }


    void Accelerate()
    {
        float horsepower = torque * move.ReadValue<Vector2>().magnitude;
        float braking = torque * (1f - move.ReadValue<Vector2>().magnitude);

        frontLeft.motorTorque = horsepower;
        frontRight.motorTorque = horsepower;
        rearLeft.motorTorque = horsepower;
        rearRight.motorTorque = horsepower;

        //frontLeft.brakeTorque = braking;
        //frontRight.brakeTorque = braking;
        //rearLeft.brakeTorque = braking;
        //rearRight.brakeTorque = braking;
    }
}
