using TMPro;
using UnityEngine;

public class InputsUIManager : MonoBehaviour
{
    public GameObject mobileJoysticksGroup;
    public GameObject instructionsPanel;
    public GameObject keyboardInstructions;
    public GameObject mouseInstructions;
    public GameObject dropdown;

    private TMP_Dropdown dropdownRef;

    public TMP_Dropdown Dropdown
    {
        get
        {
            if (dropdownRef == null)
            {
                dropdownRef = dropdown.GetComponent<TMP_Dropdown>();
            }
            return dropdownRef;
        }
    }

    void Awake()
    {
        dropdownRef = dropdown.GetComponent<TMP_Dropdown>();
    }
    public void RefreshUIOnChange(CameraMovementType newControls)
    {
        switch (newControls)
        {
            case CameraMovementType.KEYBOARD:
                instructionsPanel.SetActive(true);
                keyboardInstructions.SetActive(true);
                mouseInstructions.SetActive(false);
                mobileJoysticksGroup.SetActive(false);
                break;
            case CameraMovementType.MOUSE:
                instructionsPanel.SetActive(true);
                keyboardInstructions.SetActive(false);
                mouseInstructions.SetActive(true);
                mobileJoysticksGroup.SetActive(false);
                break;
            case CameraMovementType.MOBILE:
                instructionsPanel.SetActive(false);
                mobileJoysticksGroup.SetActive(true);
                break;
        }
        dropdownRef.value = (int)newControls;
    }
    public void SetDropdownActive(bool isActive)
    {
        dropdown.SetActive(isActive);
    }
}
