using UnityEngine;

public class DraggablePiece : MonoBehaviour
{
    public PieceShape shape;
    public float snapSpeed = 15f;
    public float maxSnapDistance = 1.5f;

    private SpriteRenderer sprite;
    public float snapThreshold = 0.4f;
    private bool isDragging;

    private Color validColor = new Color(0.6f, 1f, 0.6f, 0.8f);
    private Color invalidColor = new Color(1f, 0.4f, 0.4f, 0.8f);
    private Color placedColor = Color.white;
    private BlockCollider[] blockColliders;

    void Awake()
    {
        blockColliders = GetComponentsInChildren<BlockCollider>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isDragging) return;

        Vector2 mousePos = GetMouseWorldPosition();
        Vector2 targetPos = mousePos;

        if (AllBlocksOverGrid())
        {
            Vector2 snappedPos = GetSnappedPosition();
            float dist = Vector2.Distance(mousePos, snappedPos);

            bool shouldSnap = dist < snapThreshold;

            if (shouldSnap)
            {
                targetPos = snappedPos;
            }

            UpdatePreviewColor(shouldSnap);
        }
        else
        {
            UpdatePreviewColor(false);
        }

        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }

    void OnMouseDown() => isDragging = true;

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        sprite.color = placedColor;

        //if (isSnapping && CanPlaceAt(targetGridOrigin))
        //{
        //    PlaceAt(targetGridOrigin);
        //    sprite.color = placedColor;
        //}
        //else
        //{
        //    //Destroy(gameObject);
        //}
    }

    void UpdatePreviewColor(bool valid)
    {
        sprite.color = valid ? validColor : invalidColor;
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 result = Camera.main.ScreenToWorldPoint(mouse);
        return result;
    }

    bool AllBlocksOverGrid()
    {
        foreach (var block in blockColliders)
        {
            if (!block.isOverGrid)
                return false;
        }
        return true;
    }

    Vector2 GetSnappedPosition()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        bool found = false;

        foreach (var block in blockColliders)
        {
            if (block.isOverGrid)
            {
                Vector3 pos = block.currentTile.worldPos;
                minX = Mathf.Min(minX, pos.x);
                maxX = Mathf.Max(maxX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                maxY = Mathf.Max(maxY, pos.y);
                found = true;
            }
        }

        if (!found)
            return transform.position;

        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;

        return new Vector2(centerX, centerY);
    }
}