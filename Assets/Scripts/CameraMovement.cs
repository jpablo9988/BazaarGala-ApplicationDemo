using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 speedAxis;
    [SerializeField]
    CinemachineOrbitalFollow orbitalFollowControls;
    public void Rotate(Vector2 movementAxis)
    {
        orbitalFollowControls.HorizontalAxis.Value += movementAxis.x * speedAxis.x;
        orbitalFollowControls.VerticalAxis.Value += movementAxis.y * speedAxis.y;
    }
}
