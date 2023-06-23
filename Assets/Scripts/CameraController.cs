using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed;
    public float movementTime;
    private Vector3 newPosition;

    // Start is called before the first frame update
    private void Start()
    {
        newPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        bool isUpPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDownPressed = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isRightPressed = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool isLeftPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        Vector3 horizontalPositionChange = transform.forward * movementSpeed;
        Vector3 verticalPositionChange = transform.right * movementSpeed;

        if (isUpPressed)
        {
            newPosition += horizontalPositionChange;
        }

        if (isDownPressed)
        {
            newPosition -= horizontalPositionChange;
        }

        if (isRightPressed)
        {
            newPosition += verticalPositionChange;
        }

        if (isLeftPressed)
        {
            newPosition -= verticalPositionChange;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
