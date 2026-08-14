using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField]
    private CameraMovementType camType;
    [Header("Mobile Joysticks Dependencies")]
    [SerializeField]
    private JoystickBase movementJoystick;
    [SerializeField]
    private JoystickBase cameraJoystick;
    [Header("Mouse Input Controller")]
    [SerializeField]
    private CinemachineInputAxisController mouseRotationController;
    private CameraMovement cameraMovement;

    public CameraMovementType ControlsType => camType;
    void Start()
    {
        //TODO: Move this onto a GameContext. Do DependencyInjection.
        cameraMovement = FindFirstObjectByType<CameraMovement>();
    }
    private void OnEnable()
    {
        //If Camera Type is (MOUSE), enable the Preset Cinemachine Controller, otherwise disable it.
        mouseRotationController.enabled = camType == CameraMovementType.MOUSE;
    }
    private void OnDisable()
    {
        mouseRotationController.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (camType != CameraMovementType.MOUSE)
        {
            Vector2 camRotationAxis = new();
            switch (camType)
            {
                case CameraMovementType.MOBILE:
                    camRotationAxis.x = cameraJoystick.Horizontal;
                    camRotationAxis.y = cameraJoystick.Vertical;
                    break;
            }
            camRotationAxis *= Time.deltaTime;
            cameraMovement.Rotate(camRotationAxis);
        }
    }
}
