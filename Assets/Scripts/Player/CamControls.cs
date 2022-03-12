using UnityEngine;
using UnityEngine.InputSystem;

public class CamControls : MonoBehaviour
{
    private QuadControls quadControls;
    private InputAction look;

    public float look_sens = 3f;

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
    }

    private void OnDisable()
    {
        quadControls.Disable();
        look.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        LockOnVehicle();
    }

    void LookAround()
    {
        float rotY = look.ReadValue<Vector2>().x * look_sens * Time.deltaTime;

        transform.Rotate(rotY * Vector3.up, Space.Self);
    }

    void LockOnVehicle()
    {
        transform.position = targetVehicle.position;
    }
}
