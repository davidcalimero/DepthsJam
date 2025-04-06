using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public Tile currentTile;
    public int cellsColliding = 0;
    public bool isOverGrid
    {
        get { return cellsColliding > 0; }
    }

    private BlockNode blockNode;

    void Start()
    {
        blockNode = GetComponentInChildren<BlockNode>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GridCell"))
        {
            Tile tile = other.GetComponent<Tile>();
            if(!tile.filled)
            {
                ++cellsColliding;
                currentTile = tile;
                blockNode.gridPosition = currentTile.gridPos;
            }
        }  
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GridCell"))
        {
            Tile tile = other.GetComponent<Tile>();
            if (!tile.filled)
            {
                --cellsColliding;
                if (cellsColliding <= 0)
                {
                    currentTile = null;
                }
            }
        }
    }

    public void PlaceInGrid()
    {
        if (currentTile != null)
        {
            currentTile.filled = true;
            FindFirstObjectByType<GridSpawner>().PlaceBlock(blockNode.gridPosition, blockNode);
        }
    }
}