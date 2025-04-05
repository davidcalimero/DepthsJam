using UnityEngine;

public class GridTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Vector2Int gridPos;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }

    void OnMouseDown()
    {
        if (IsOnSurface())
        {
            GridSpawner.tileMap.Remove(gridPos);
            Destroy(gameObject);
        }
    }

    bool IsOnSurface()
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = gridPos + dir;
            if (!GridSpawner.tileMap.ContainsKey(neighbor))
            {
                return true;
            }
        }

        return false;
    }
}
