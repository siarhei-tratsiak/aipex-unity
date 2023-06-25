using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float fastSpeed = 3f;
    public float mouseZoomSpeed = 10f;
    public float movementSpeed = 1f;
    public float movementTime = 5f;
    public float normalSpeed = 0.5f;
    public float rotationAmount = 1f;
    public Vector3 zoomAmount = new(0, -1, 1);

    private Vector3 dragCurrentPosition;
    private Vector3 dragStartPosition;
    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;
    private Vector3 rotateCurrentPosition;
    private Vector3 rotateStartPosition;

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

            updateRotation(-difference.x / 5f);
        }
    }

    private void HandleMovementInput()
    {
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;
        Vector3 horizontalPositionChange = transform.forward * movementSpeed;
        Vector3 verticalPositionChange = transform.right * movementSpeed;
        Quaternion rotationChange = Quaternion.Euler(Vector3.up * rotationAmount);

        KeyManager[] keyManagers = {
            // TODO: create interface for the keys
            new KeyManager(
                new KeyCode[2] { KeyCode.W, KeyCode.UpArrow },
                () => newPosition += horizontalPositionChange
            ),

            new KeyManager(
                new KeyCode[2] { KeyCode.S, KeyCode.DownArrow },
                () => newPosition -= horizontalPositionChange
            ),

            new KeyManager(
                new KeyCode[2] { KeyCode.D, KeyCode.RightArrow },
                () => newPosition += verticalPositionChange
            ),

            new KeyManager(
                new KeyCode[2] { KeyCode.A, KeyCode.LeftArrow },
                () => newPosition -= verticalPositionChange
            ),

            new KeyManager(
                new KeyCode[1] { KeyCode.Q },
                () => updateRotation(rotationAmount)
            ),

            new KeyManager(
                new KeyCode[1] { KeyCode.E },
                () => updateRotation(-rotationAmount)
            ),

            new KeyManager(
                new KeyCode[1] { KeyCode.R },
                () => newZoom += zoomAmount
            ),

            new KeyManager(
                new KeyCode[1] { KeyCode.F },
                () => newZoom -= zoomAmount
            ),
        };

        foreach (KeyManager keyManager in keyManagers)
        {
            ShiftPositionIfKeyPressed(keyManager);
        }

        float time = Time.deltaTime * movementTime;
        transform.position = Vector3.Lerp(transform.position, newPosition, time);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, time);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, time);
    }

    private void ShiftPositionIfKeyPressed(KeyManager keyManager)
    {
        bool isKeyPressed = Array.Exists(keyManager.keys, key => Input.GetKey(key));

        if (isKeyPressed)
        {
            keyManager.callback();
        }
    }

    private void updateRotation(float amount)
    {
        newRotation *= Quaternion.Euler(Vector3.up * amount);
    }

    private struct KeyManager
    {
        public KeyCode[] keys;
        public Action callback;

        public KeyManager(KeyCode[] keys, Action callback)
        {
            this.keys = keys;
            this.callback = callback;
        }
    }
}
