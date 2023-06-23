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
        Vector3 horizontalPositionChange = transform.forward * movementSpeed;
        Vector3 verticalPositionChange = transform.right * movementSpeed;

        DirectionKeyManager[] keysAndShifts = {
            new DirectionKeyManager(KeyCode.W, KeyCode.UpArrow, horizontalPositionChange),
            new DirectionKeyManager(KeyCode.S, KeyCode.DownArrow, -horizontalPositionChange),
            new DirectionKeyManager(KeyCode.D, KeyCode.RightArrow, verticalPositionChange),
            new DirectionKeyManager(KeyCode.A, KeyCode.LeftArrow, -verticalPositionChange)
        };

        foreach (DirectionKeyManager keysAndShift in keysAndShifts)
        {
            ShiftPositionIfKeyPressed(keysAndShift);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    private void ShiftPositionIfKeyPressed(DirectionKeyManager keysAndShift)
    {
        bool isKeyPressed = Input.GetKey(keysAndShift.key1) || Input.GetKey(keysAndShift.key2);

        if (isKeyPressed)
        {
            newPosition += keysAndShift.changePosition;
        }
    }

    private class DirectionKeyManager
    {
        public KeyCode key1;
        public KeyCode key2;
        public Vector3 changePosition;

        public DirectionKeyManager(KeyCode key1Value, KeyCode key2Value, Vector3 changePositionValue)
        {
            key1 = key1Value;
            key2 = key2Value;
            changePosition = changePositionValue;
        }
    }
}
