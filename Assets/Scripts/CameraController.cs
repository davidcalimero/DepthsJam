using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float mouseScrollSpeed = 2;

    public float minZoom = 5;
    public float maxZoom = 10;
    public float cameraExtraMargin = 5f;

    [Header("UI")]
    [Tooltip("Are touch motions listened to if they are over UI elements?")]
    public bool ignoreUI = false;

    public bool useBounds;

    public float boundMinX = -1500;
    public float boundMaxX = 1500;
    public float boundMinY = -1500;
    public float boundMaxY = 1500;

    private bool canUseMouse;
    private Camera cam;
    private bool isTouching;
    private Vector2 lastPosition;

    private GridSpawner spawner;

    void Start()
    {
        cam = Camera.main;
        canUseMouse = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Input.mousePresent;
        spawner = FindFirstObjectByType<GridSpawner>();
    }

    void Update()
    {
        if (canUseMouse)
        {
            UpdateWithMouse();
        }
        else
        {
            //UpdateWithTouch();
        }
    }

    void LateUpdate()
    {
        CameraInBounds();
    }

    void UpdateWithMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ignoreUI || !IsPointerOverUIObject())
            {
                isTouching = true;
                lastPosition = (Vector2)Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0) && isTouching)
        {
            Vector2 move = (Vector2)Input.mousePosition - lastPosition;

            if (move != Vector2.zero)
            {
                MoveCamera(move);
            }

            lastPosition = (Vector2)Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            isTouching = false;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            //ZoomCamera(-Input.mouseScrollDelta.y);
        }
    }

    void MoveCamera(Vector2 deltaPosition)
    {
        if (cam == null) cam = Camera.main;

        Vector3 position = (cam.ScreenToWorldPoint(deltaPosition) - cam.ScreenToWorldPoint(Vector2.zero));

        cam.transform.position -= new Vector3(0, position.y, 0);

    }
    void ZoomCamera(float distance)
    {
        if (cam == null) cam = Camera.main;

        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + (mouseScrollSpeed * distance), minZoom, maxZoom);
        }
    }

    public bool IsPointerOverUIObject()
    {

        if (EventSystem.current == null) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void CameraInBounds()
    {
        if (useBounds && cam != null && cam.orthographic)
        {
            // Get the current lowest block Y + margin
            float dynamicMinY = spawner.lastGridBottomY - cameraExtraMargin;

            cam.orthographicSize = Mathf.Min(cam.orthographicSize, ((boundMaxY - boundMinY) / 2) - 0.001f);
            cam.orthographicSize = Mathf.Min(cam.orthographicSize, (Screen.height * (boundMaxX - boundMinX) / (2 * Screen.width)) - 0.001f);

            Vector2 margin = cam.ScreenToWorldPoint((Vector2.up * Screen.height / 2) + (Vector2.right * Screen.width / 2)) - cam.ScreenToWorldPoint(Vector2.zero);

            float marginX = margin.x;
            float marginY = margin.y;

            float camMaxX = boundMaxX - marginX;
            float camMaxY = boundMaxY - marginY;
            float camMinX = boundMinX + marginX;
            float camMinY = Mathf.Max(boundMinY + marginY, dynamicMinY);

            float camX = Mathf.Clamp(cam.transform.position.x, camMinX, camMaxX);
            float camY = Mathf.Clamp(cam.transform.position.y, camMinY, camMaxY);

            cam.transform.position = new Vector3(camX, camY, cam.transform.position.z);
        }
    }
}

