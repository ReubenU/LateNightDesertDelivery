using UnityEngine;
using UnityEngine.InputSystem;

public class QuadScript : MonoBehaviour
{
    // Main Rigidbody variable
    Rigidbody rigid;

    bool isQuadColliding = false;

    // Quad's center of mass
    public Transform centerOfMass;

    // Camera control transform
    public Transform camControl;

    // Quad controls
    public QuadControls quadControls;

    private InputAction move;
    private InputAction brake;
    private InputAction keyMove;
    private InputAction keyBrake;

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

    // ATV Quad Animator
    public Animator quadAnimator;

    // Girl's animation settings
    public Animator girlAnimator;

    // Girl's Ragdoll settings
    public GirlStates girlRagdoller;

    public Transform girlPelvis;

    // Quad sound
    public AudioSource quadAudio;


    bool useController = true;

    [HideInInspector]
    // Quad States and State Machine
    public enum QuadStates : int
    {
        Active = 0,
        Crash = 1
    }
    [HideInInspector]
    public QuadStates state;



    private void Awake()
    {
        useController = (PlayerPrefs.GetInt("UseController") == 1)? true : false;

        rigid = GetComponent<Rigidbody>();

        rigid.centerOfMass = centerOfMass.localPosition;

        quadControls = new QuadControls();

        state = QuadStates.Active;
    }

    // Enable all quad controls
    private void OnEnable()
    {
        quadControls.Enable();

        move = quadControls.Player.Move;
        brake = quadControls.Player.Brake;
        keyMove = quadControls.Player.KeyMove;
        keyBrake = quadControls.Player.KeyBrake;

        move.Enable();
        brake.Enable();

        keyMove.Enable();
        keyBrake.Enable();
    }

    // Disable all controls
    private void OnDisable()
    {
        quadControls.Disable();

        move.Disable();
        brake.Disable();

        keyMove.Disable();
        keyBrake.Disable();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case QuadStates.Active:
                Accelerate();
                break;
            case QuadStates.Crash:
                CrashState();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case QuadStates.Active:
                NormalState();
                break;
            case QuadStates.Crash:
                CrashState();
                break;
            default:
                break;
        }
    }


    Vector3 deltaVelocity = Vector3.zero;
    private void LateUpdate()
    {
        RagdollGirl();
    }


    void NormalState()
    {
        // This should be the normal state.
        Steer();
        Brake();
        AnimateGirl();
        AnimateQuad();
        QuadEngineAudio();
    }

    void CrashState()
    {

    }


    // All wheel steering
    private float currentAngle = 0f;
    private float angleVel = 0f;
    private float steerSmooth = .5f;
    private float leftStickDeadZone = 0.15f;

    // Girl steering animation variables
    private float girlAnimSteer = 0.5f; // 0 is left, .5 is center, 1 is right
    private float animSpeed = 0f;
    private float animSmooth = 3f;
    void Steer()
    {
        // Get the general direction we want to move towards...
        Vector3 moveStickVal = new Vector3(0, 0, Mathf.Abs(move.ReadValue<Vector2>().y));
        Vector3 moveKeyVal = new Vector3(0, 0, Mathf.Abs(keyMove.ReadValue<Vector2>().y));

        Vector3 bestMoveInput = (moveStickVal.magnitude > moveKeyVal.magnitude) ? moveStickVal : moveKeyVal;

        Vector3 moveDirection = camControl.TransformDirection(bestMoveInput);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;

        // The quad's facing direction...
        Vector3 quadDir = transform.TransformDirection(Vector3.forward);
        quadDir.y = 0;
        quadDir = quadDir.normalized;

        // Find the angle between two vectors using a custom function
        // because Unity is too retarded to do precise calculations.
        // Thanks u/ActionScripter9109.
        float bestDir = (Mathf.Abs(move.ReadValue<Vector2>().y) > Mathf.Abs(keyMove.ReadValue<Vector2>().y)) ? move.ReadValue<Vector2>().y : keyMove.ReadValue<Vector2>().y;
        Debug.Log(bestDir);
        float angle = AngleBetweenVectors(quadDir, moveDirection, transform.TransformDirection(Vector3.up)) * Mathf.Rad2Deg * bestDir;

        angle = Loop2Range(angle, -180, 180);

        // Check if left stick is let go:
        if (bestMoveInput.magnitude <= leftStickDeadZone)
        {
            currentAngle = 0;
        }

        // Smooth the steering angle so the quad isn't so spastic.
        currentAngle = Mathf.SmoothDampAngle(currentAngle, angle, ref angleVel, steerSmooth * Time.deltaTime);
        currentAngle = Map2Range(currentAngle, -360, 360, -180, 180);

        // Apply steering only when accelerating.
        float finalSteerAngle = currentAngle * bestMoveInput.magnitude;

        // Set girl's steering frame...
        float animAngle = Map2Range(angle/180, -1f, 1f, 0, 1f);
        girlAnimSteer = Mathf.SmoothDamp(girlAnimSteer, animAngle, ref animSpeed, animSmooth * Time.deltaTime);

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

        //if (test < 0f) angle = -angle;
        angle *= Mathf.Sign(Vector3.Dot(up, cross));


        return angle; // radians
    }


    void Accelerate()
    {
        float gamepadGas = move.ReadValue<Vector2>().y;
        float keyboardGas = keyMove.ReadValue<Vector2>().y;

        float bestGas = (Mathf.Abs(gamepadGas) > Mathf.Abs(keyboardGas)) ? gamepadGas : keyboardGas;

        Vector3 localSpeed = transform.InverseTransformDirection(rigid.velocity);

        float speedDelta = Mathf.Clamp(Mathf.Abs(localSpeed.z), 1, Mathf.Infinity);

        float horsepower = (torque-speedDelta) * bestGas;

        frontLeft.motorTorque = horsepower;
        frontRight.motorTorque = horsepower;
        rearLeft.motorTorque = horsepower;
        rearRight.motorTorque = horsepower;
    }

    void Brake()
    {
        float triggerBrake = brake.ReadValue<float>();
        float keyboardBrake = keyBrake.ReadValue<float>();

        float bestBrakeInput = (Mathf.Abs(triggerBrake) > Mathf.Abs(keyboardBrake)) ? triggerBrake : keyboardBrake;

        float braking = 2*torque * bestBrakeInput;

        frontLeft.brakeTorque = braking;
        frontRight.brakeTorque = braking;
        rearLeft.brakeTorque = braking;
        rearRight.brakeTorque = braking;
    }

    // Animate girl steering
    void AnimateGirl()
    {
        girlAnimator.SetFloat("SteerPercent", girlAnimSteer);
    }

    // Ragdoll girl when crashing at high speed
    int maxCrashForce = 10;
    void RagdollGirl()
    {
        if (!isQuadColliding)
        {
            deltaVelocity = rigid.velocity;
        }
        else
        {
            if (deltaVelocity.magnitude > rigid.velocity.magnitude + maxCrashForce)
            {
                girlRagdoller.activateRagdoll = true;
                girlRagdoller.inheritedVelocity = deltaVelocity * .5f;
                girlRagdoller.RagdollModeOn();

                // Change state:
                state = QuadStates.Crash;

                camControl.GetComponent<CamControls>().targetVehicle = girlPelvis;
                camControl.GetComponentInChildren<Light>().enabled = true;
            }
        }
    }

    // Penalty function


    // Animate quad steering
    void AnimateQuad()
    {
        quadAnimator.SetFloat("Steering", girlAnimSteer);
    }

    // Adjust engine audio
    float maxSpeed = 100;
    void QuadEngineAudio()
    {
        Vector3 localSpeed = transform.InverseTransformDirection(rigid.velocity);
        float speed = Mathf.Abs(localSpeed.z * 2.23694f);

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        float pitchThrottle = Map2Range(speed, 0, maxSpeed, 1, 3);

        pitchThrottle = (Mathf.Round(brake.ReadValue<float>()) > 0) ? 1 : pitchThrottle;
        quadAudio.pitch = pitchThrottle;
    }

    float Map2Range(float inpVal, float frmMin, float frmMax, float toMin, float toMax)
    {
        return (inpVal - frmMin) / (frmMax - frmMin) * (toMax - toMin) + toMin;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isQuadColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isQuadColliding = false;
    }

    // Helper functions
    float Loop2Range(float inputVal, float minVal, float maxVal)
    {
        float isMaxxed = (inputVal > maxVal) ? minVal : 0;
        float isMinned = (inputVal < minVal) ? maxVal : 0;
        float Default = (inputVal < maxVal && inputVal > minVal) ? inputVal : 0;

        return isMaxxed + isMinned + Default;
    }
}
