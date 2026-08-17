using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 speedAxis;
    [SerializeField]
    CinemachineOrbitalFollow orbitalFollowControls;
    [SerializeField]
    private Transform rotationTarget;
    HeadRotationHandler headRotationHandler;

    public bool MoveCamera = true;

    private static readonly float SPEEDMODIFIER = 100;

    private void Start()
    {
        speedAxis *= SPEEDMODIFIER;
        headRotationHandler = FindFirstObjectByType<HeadRotationHandler>();
    }

    public void Rotate(Vector2 movementAxis)
    {
        if (MoveCamera)
        {
            orbitalFollowControls.HorizontalAxis.Value += movementAxis.x * speedAxis.x;
            orbitalFollowControls.VerticalAxis.Value = Mathf.Clamp(
                orbitalFollowControls.VerticalAxis.Value - movementAxis.y * speedAxis.y
            , orbitalFollowControls.VerticalAxis.Range.x, orbitalFollowControls.VerticalAxis.Range.y);

        }
        headRotationHandler.rotationRef = new(Mathf.Repeat(orbitalFollowControls.VerticalAxis.Value - orbitalFollowControls.VerticalAxis.Center + 180, 360) - 180,
            (orbitalFollowControls.HorizontalAxis.Value - orbitalFollowControls.HorizontalAxis.Center) % 360);
        rotationTarget.eulerAngles = new(0, orbitalFollowControls.HorizontalAxis.Value, 0);
    }
    public Transform GetRotationTarget()
    {
        return rotationTarget.transform;
    }
}
