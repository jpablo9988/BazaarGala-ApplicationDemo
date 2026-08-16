using UnityEngine;

public class HeadRotationHandler : MonoBehaviour
{
    [SerializeField]
    private Transform bodyReference;
    [SerializeField]
    private Vector2 headLimits;

    public Vector3 rotationRef;
    private Vector3 initialRotation;

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
    }


    void Update()
    {
        Quaternion localRotationRef = Quaternion.Euler(rotationRef + initialRotation);
        /* if (Mathf.Abs(rotationRef.x - bodyReference.rotation.eulerAngles.x) > headLimits.x)
        {
            localRotationRef.eulerAngles = new(0, 0, 0);
        }
        if (Mathf.Abs(rotationRef.y - bodyReference.rotation.eulerAngles.y) > headLimits.y)
        {
            localRotationRef.eulerAngles = new(0, 0, 0);
        } */
        transform.rotation = localRotationRef;
    }
}
