using UnityEngine;

public class GridTile : MonoBehaviour
{
    private SpriteRenderer sr;

    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Vector2Int gridPos;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = defaultColor;
    }

    void OnMouseEnter()
    {
        sr.color = highlightColor;
    }

    void OnMouseExit()
    {
        sr.color = defaultColor;
    }

    void OnMouseDown()
    {
        if (IsOnSurface())
        {
            GridSystem.tileMap.Remove(gridPos);
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
            if (!GridSystem.tileMap.ContainsKey(neighbor))
            {
                return true;
            }
        }

        return false;
    }
}
