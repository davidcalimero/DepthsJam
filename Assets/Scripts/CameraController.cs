using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float mouseScrollSpeed = 2;

    public float minZoom = 5;
    public float maxZoom = 10;

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

    void Start()
    {
        cam = Camera.main;
        canUseMouse = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Input.mousePresent;
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
            if (ignoreUI || !IsPointerOverUIObject() && !IsPointerOverDraggable())
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
            ZoomCamera(-Input.mouseScrollDelta.y);
        }
    }

    void MoveCamera(Vector2 deltaPosition)
    {
        if (cam == null) cam = Camera.main;

        cam.transform.position -= (cam.ScreenToWorldPoint(deltaPosition) - cam.ScreenToWorldPoint(Vector2.zero));

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
            cam.orthographicSize = Mathf.Min(cam.orthographicSize, ((boundMaxY - boundMinY) / 2) - 0.001f);
            cam.orthographicSize = Mathf.Min(cam.orthographicSize, (Screen.height * (boundMaxX - boundMinX) / (2 * Screen.width)) - 0.001f);

            Vector2 margin = cam.ScreenToWorldPoint((Vector2.up * Screen.height / 2) + (Vector2.right * Screen.width / 2)) - cam.ScreenToWorldPoint(Vector2.zero);

            float marginX = margin.x;
            float marginY = margin.y;

            float camMaxX = boundMaxX - marginX;
            float camMaxY = boundMaxY - marginY;
            float camMinX = boundMinX + marginX;
            float camMinY = boundMinY + marginY;

            float camX = Mathf.Clamp(cam.transform.position.x, camMinX, camMaxX);
            float camY = Mathf.Clamp(cam.transform.position.y, camMinY, camMaxY);

            cam.transform.position = new Vector3(camX, camY, cam.transform.position.z);
        }
    }

    bool IsPointerOverDraggable()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (hit.collider != null)
        {
            // You can filter by tag or component
            if (hit.collider.GetComponent<DraggablePiece>() != null)
                return true;

            // OR if you're clicking on a block:
            if (hit.collider.GetComponent<BlockCollider>() != null)
                return true;
        }

        return false;
    }
}

