using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class DoorHandler : MonoBehaviour
{
    public enum ComparisonAxis
    {
        X,
        Y,
        Z
    }
    [SerializeField]
    private ComparisonAxis axisToCompare;
    [SerializeField]
    private Transform centerPoint;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject localHinges;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float yAngleTarget = 0;
    private float yInitialRotation;
    [SerializeField]
    private float absoluteRotationPosition;
    float rotationDirection = 1;
    private bool goingUp = false;
    private void OnEnable()
    {
        yInitialRotation = door.transform.eulerAngles.y;
        yAngleTarget = yInitialRotation;
        absoluteRotationPosition = yInitialRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 objectRelativePos = centerPoint.transform.InverseTransformPoint(other.transform.position);
            Vector3 enterPosition = objectRelativePos - centerPoint.transform.localPosition;
            yAngleTarget = yInitialRotation;
            switch (axisToCompare)
            {
                case ComparisonAxis.X:
                    if (enterPosition.x > centerPoint.transform.localPosition.x)
                    {

                        yAngleTarget -= 90;
                        goingUp = false;

                    }
                    else
                    {
                        yAngleTarget += 90;
                        goingUp = true;
                    }
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (yAngleTarget > yInitialRotation)
            {
                goingUp = false;
            }
            else
            {
                goingUp = true;
            }
            yAngleTarget = yInitialRotation;
        }
    }
    private void Update()
    {
        //Rotate clockwise initially.
        if (absoluteRotationPosition == yAngleTarget) return;
        rotationDirection = 1;
        if (absoluteRotationPosition > yAngleTarget)
        {
            rotationDirection *= -1;
        }
        if ((absoluteRotationPosition > yAngleTarget) && goingUp)
        {
            door.transform.RotateAround(localHinges.transform.position, Vector3.up, -absoluteRotationPosition);
            absoluteRotationPosition = yAngleTarget;
            door.transform.RotateAround(localHinges.transform.position, Vector3.up, yAngleTarget);
            return;
        }
        else if ((absoluteRotationPosition < yAngleTarget) && !goingUp)
        {
            door.transform.RotateAround(localHinges.transform.position, Vector3.up, -absoluteRotationPosition);
            absoluteRotationPosition = yAngleTarget;
            door.transform.RotateAround(localHinges.transform.position, Vector3.up, yAngleTarget);
            return;
        }
        door.transform.RotateAround(localHinges.transform.position, Vector3.up, rotationDirection * Time.deltaTime * rotationSpeed);
        absoluteRotationPosition += rotationDirection * Time.deltaTime * rotationSpeed;


    }
}
