using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField]
    private CameraMovementType camType;
    [Header("Movement Dependencies")]
    [SerializeField]
    private PlayerMovement playerMovement;
    [Header("Mobile Joysticks Dependencies")]
    [SerializeField]
    private JoystickBase movementJoystick;
    [SerializeField]
    private JoystickBase cameraJoystick;
    [Header("Mouse Input Controller")]
    [SerializeField]
    private CinemachineInputAxisController mouseRotationController;
    [SerializeField]
    private CameraMovementType initialControls = CameraMovementType.KEYBOARD;
    private CameraMovement cameraMovement;
    private InputAction move;
    private InputAction look;

    public CameraMovementType ControlsType
    {
        set
        {
            if (move == null || look == null)
            {
                move = InputSystem.actions.FindAction("Move");
                look = InputSystem.actions.FindAction("Look");
            }
            if (camType == CameraMovementType.MOUSE)
            {
                Cursor.lockState = CursorLockMode.Locked;

                look.Disable();
                move.Enable();

            }
            if (camType == CameraMovementType.KEYBOARD)
            {
                Cursor.lockState = CursorLockMode.None;

                look.Enable();
                move.Enable();

            }
            if (camType == CameraMovementType.MOBILE)
            {
                Cursor.lockState = CursorLockMode.None;

                look.Disable();
                move.Disable();

            }
            mouseRotationController.enabled = camType == CameraMovementType.MOUSE;
        }
    }
    void Start()
    {
        //TODO: Move this onto a GameContext. Do DependencyInjection.
        cameraMovement = FindFirstObjectByType<CameraMovement>();
        if (Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer)
        {
            camType = CameraMovementType.MOBILE;
        }
        else
        {
            camType = CameraMovementType.KEYBOARD;
        }
    }
    private void OnEnable()
    {
        //If Camera Type is (MOUSE), enable the Preset Cinemachine Controller, otherwise disable it.
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        ControlsType = initialControls;
    }
    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 movementAxis = new(0, 0);

        Vector2 camRotationAxis = new();
        switch (camType)
        {
            case CameraMovementType.MOBILE:
                camRotationAxis.x = cameraJoystick.Horizontal;
                camRotationAxis.y = cameraJoystick.Vertical;
                //Axis are mirrored. Use Vertical for X, Horizontal for Y
                movementAxis.x = movementJoystick.Vertical;
                movementAxis.y = movementJoystick.Horizontal;
                break;
            case CameraMovementType.KEYBOARD:
                Vector2 auxMovement = move.ReadValue<Vector2>();
                movementAxis.y = auxMovement.x;
                movementAxis.x = auxMovement.y;
                camRotationAxis = look.ReadValue<Vector2>();
                break;

        }
        camRotationAxis *= Time.deltaTime;
        if (camType != CameraMovementType.MOUSE)
        { cameraMovement.Rotate(camRotationAxis); }
        playerMovement.MovementAxis = movementAxis;

    }
}
