using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private Transform lookDirectionTransform;
    [SerializeField]
    private Transform bodyRotation;
    private Vector2 movementAxis;
    private static readonly float SPEEDMODIFIER = 100;
    private Rigidbody rb;
    private Vector3 direction = new(0, 0);
    public Vector2 MovementAxis
    {
        get
        {
            return movementAxis;
        }
        set
        {
            movementAxis = value;
        }
    }
    void Start()
    {
        movementSpeed *= SPEEDMODIFIER;
        rb = GetComponent<Rigidbody>();
        movementAxis = new(0, 0);
    }

    private void Update()
    {
        Vector3 forwardDirection = movementAxis.x * lookDirectionTransform.forward;
        Vector3 horizontalDirection = movementAxis.y * lookDirectionTransform.right;
        if (movementAxis.magnitude > 0) bodyRotation.rotation = lookDirectionTransform.rotation;
        direction = Vector3.ClampMagnitude(forwardDirection + horizontalDirection, 1);
        if (movementAxis.magnitude > 0) bodyRotation.rotation = Quaternion.LookRotation(direction);

    }
    void FixedUpdate()
    {
        rb.linearVelocity = movementSpeed * Time.fixedDeltaTime * direction;
    }
}
