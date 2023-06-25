using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 dragCurrentPosition;
    public Vector3 dragStartPosition;
    public float fastSpeed;
    public float mouseZoomSpeed;
    public float movementSpeed;
    public float movementTime;
    public float normalSpeed;
    public Vector3 rotateCurrentPosition;
    public Vector3 rotateStartPosition;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    // Start is called before the first frame update
    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            // TODO: pass the camera to controller
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * mouseZoomSpeed * zoomAmount;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * -difference.x / 5f);
        }
    }

    private void HandleMovementInput()
    {
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
        Vector3 horizontalPositionChange = transform.forward * movementSpeed;
        Vector3 verticalPositionChange = transform.right * movementSpeed;
        Quaternion rotationChange = Quaternion.Euler(Vector3.up * rotationAmount);

        DirectionKeyManager[] directionKeyManagers = {
            // TODO: create interface for the keys
            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.W, KeyCode.UpArrow },
                () => newPosition += horizontalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.S, KeyCode.DownArrow },
                () => newPosition -= horizontalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.D, KeyCode.RightArrow },
                () => newPosition += verticalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[2] { KeyCode.A, KeyCode.LeftArrow },
                () => newPosition -= verticalPositionChange
            ),

            new DirectionKeyManager(
                new KeyCode[1] { KeyCode.Q },
                () => newRotation *= Quaternion.Euler(Vector3.up * rotationAmount)
            ),

            new DirectionKeyManager(
                new KeyCode[1] { KeyCode.E },
                () => newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount)
            ),

            new DirectionKeyManager(
                new KeyCode[1] { KeyCode.R },
                () => newZoom += zoomAmount
            ),

            new DirectionKeyManager(
                new KeyCode[1] { KeyCode.F },
                () => newZoom -= zoomAmount
            ),
        };

        foreach (DirectionKeyManager directionKeyManager in directionKeyManagers)
        {
            ShiftPositionIfKeyPressed(directionKeyManager);
        }

        float time = Time.deltaTime * movementTime;
        transform.position = Vector3.Lerp(transform.position, newPosition, time);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, time);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, time);
    }

    private void ShiftPositionIfKeyPressed(DirectionKeyManager directionKeyManager)
    {
        bool isKeyPressed = Array.Exists(directionKeyManager.keys, key => Input.GetKey(key));

        if (isKeyPressed)
        {
            directionKeyManager.callback();
        }
    }

    private class DirectionKeyManager
    {
        public KeyCode[] keys;
        public Action callback;

        public DirectionKeyManager(KeyCode[] keys, Action callback)
        {
            this.keys = keys;
            this.callback = callback;
        }
    }
}
