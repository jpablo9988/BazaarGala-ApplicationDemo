using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private InputAction escape;
    private InputsUIManager uiManager;
    private TMP_Dropdown dropdownRef;
    private int UILayer;

    public CameraMovementType ControlsType
    {
        set
        {
            camType = value;
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
        if (uiManager == null) uiManager = FindFirstObjectByType<InputsUIManager>();
        uiManager.SetDropdownActive(true);
        if (Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer)
        {
            camType = CameraMovementType.MOBILE;
            uiManager.RefreshUIOnChange(CameraMovementType.MOBILE);
            uiManager.SetDropdownActive(false);
        }
        else
        {
            camType = initialControls;
            uiManager.RefreshUIOnChange(initialControls);
        }
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void OnEnable()
    {
        //If Camera Type is (MOUSE), enable the Preset Cinemachine Controller, otherwise disable it.
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        escape = InputSystem.actions.FindAction("Escape");
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<InputsUIManager>();
            dropdownRef = uiManager.Dropdown;
            dropdownRef.onValueChanged.AddListener(ChangeControls);
        }
        escape.performed += CheckLockCursor;
        ControlsType = initialControls;
    }

    private void CheckLockCursor(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnDisable()
    {
        escape.performed -= CheckLockCursor;
        move.Disable();
        look.Disable();
        escape.Disable();
        dropdownRef.onValueChanged.RemoveListener(ChangeControls);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ChangeControls(int controlsChange)
    {
        ControlsType = (CameraMovementType)controlsChange;
        uiManager.RefreshUIOnChange((CameraMovementType)controlsChange);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 movementAxis = new(0, 0);
        Vector2 camRotationAxis = new();
        Vector2 auxMovement;
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            CheckIfUIClicked(mouse);
        }
        cameraMovement.MoveCamera = true;
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
                auxMovement = move.ReadValue<Vector2>();
                movementAxis.y = auxMovement.x;
                movementAxis.x = auxMovement.y;
                camRotationAxis = look.ReadValue<Vector2>();
                break;
            case CameraMovementType.MOUSE:
                auxMovement = move.ReadValue<Vector2>();
                movementAxis.y = auxMovement.x;
                movementAxis.x = auxMovement.y;
                cameraMovement.MoveCamera = false;
                break;

        }
        camRotationAxis *= Time.deltaTime;
        cameraMovement.Rotate(camRotationAxis);
        playerMovement.MovementAxis = movementAxis;

    }
    public void CheckIfUIClicked(Mouse mouse)
    {
        Debug.Log("clicked");
        if (Cursor.lockState == CursorLockMode.Locked || camType != CameraMovementType.MOUSE) return;
        PointerEventData eData = new(EventSystem.current)
        {
            position = mouse.position.ReadValue()
        };
        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(eData, raycastResults);
        bool isUnderUI = false;
        foreach (RaycastResult raycast in raycastResults)
        {
            if (raycast.gameObject.layer == UILayer)
            {
                Debug.Log("true");
                isUnderUI = true;
                break;
            }
        }
        if (!isUnderUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
