using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class ControlsChangePackage : MonoBehaviour
{
    public CameraMovementType CurrentControls;
    private TMP_Dropdown dropdown;


    private void OnEnable()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnChange);
    }
    void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(OnChange);
    }
    private void OnChange(int newValue)
    {
        CurrentControls = (CameraMovementType)newValue;
    }
}
