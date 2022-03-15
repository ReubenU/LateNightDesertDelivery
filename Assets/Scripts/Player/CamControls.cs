using UnityEngine;
using UnityEngine.InputSystem;

public class CamControls : MonoBehaviour
{
    private QuadControls quadControls;
    private InputAction look;
    private InputAction mouseLook;

    public float mouse_look_sens = 3f;
    public float gamepad_look_sens = 30f;

    public Transform targetVehicle;

    private void Awake()
    {
        quadControls = new QuadControls();
    }

    private void OnEnable()
    {
        quadControls.Enable();

        look = quadControls.Player.Look;
        look.Enable();

        mouseLook = quadControls.Player.Mouselook;
        mouseLook.Enable();
    }

    private void OnDisable()
    {
        quadControls.Disable();
        look.Disable();
        mouseLook.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        LockOnVehicle();
    }

    void LookAround()
    {
        float gamepadLookX = look.ReadValue<Vector2>().x * gamepad_look_sens;
        float mouseLookX = mouseLook.ReadValue<Vector2>().x * mouse_look_sens;

        float bestLookInput = (Mathf.Abs(gamepadLookX) > Mathf.Abs(mouseLookX)) ? gamepadLookX : mouseLookX;

        float rotY =  bestLookInput * Time.deltaTime;

        transform.Rotate(rotY * Vector3.up, Space.Self);
    }

    void LockOnVehicle()
    {
        transform.position = targetVehicle.position;
    }
}
