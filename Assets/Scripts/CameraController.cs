using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float normalSpeed;
    public float rotationAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;

    // Start is called before the first frame update
    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
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
            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.W, KeyCode.UpArrow },
                horizontalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.S, KeyCode.DownArrow },
                -horizontalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.D, KeyCode.RightArrow },
                verticalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.A, KeyCode.LeftArrow },
                -verticalPositionChange
            )
        };

        foreach (DirectionKeyManager directionKeyManager in directionKeyManagers)
        {
            ShiftPositionIfKeyPressed(directionKeyManager);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }

    private void ShiftPositionIfKeyPressed(DirectionKeyManager directionKeyManager)
    {
        bool isKeyPressed = Array.Exists(directionKeyManager.keys, key => Input.GetKey(key));

        if (isKeyPressed)
        {
            newPosition += directionKeyManager.changePosition;
        }
    }

    private class DirectionKeyManager
    {
        public KeyCode[] keys;
        public Vector3 changePosition;

        public DirectionKeyManager(KeyCode[] keys, Vector3 changePosition)
        {
            this.keys = keys;
            this.changePosition = changePosition;
        }
    }
}
