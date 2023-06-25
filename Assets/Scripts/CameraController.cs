using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float fastSpeed;
    public float normalSpeed;
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
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
        Vector3 horizontalPositionChange = transform.forward * movementSpeed;
        Vector3 verticalPositionChange = transform.right * movementSpeed;

        DirectionKeyManager[] directionKeyManagers = {
            new DirectionKeyManager(KeyCode.W, KeyCode.UpArrow, horizontalPositionChange),
            new DirectionKeyManager(KeyCode.S, KeyCode.DownArrow, -horizontalPositionChange),
            new DirectionKeyManager(KeyCode.D, KeyCode.RightArrow, verticalPositionChange),
            new DirectionKeyManager(KeyCode.A, KeyCode.LeftArrow, -verticalPositionChange)
        };

        foreach (DirectionKeyManager directionKeyManager in directionKeyManagers)
        {
            ShiftPositionIfKeyPressed(directionKeyManager);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    private void ShiftPositionIfKeyPressed(DirectionKeyManager directionKeyManager)
    {
        bool isKeyPressed = Input.GetKey(directionKeyManager.key1) || Input.GetKey(directionKeyManager.key2);

        if (isKeyPressed)
        {
            newPosition += directionKeyManager.changePosition;
        }
    }

    private class DirectionKeyManager
    {
        public KeyCode key1;
        public KeyCode key2;
        public Vector3 changePosition;

        public DirectionKeyManager(KeyCode key1, KeyCode key2, Vector3 changePosition)
        {
            this.key1 = key1;
            this.key2 = key2;
            this.changePosition = changePosition;
        }
    }
}
