using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public Tile currentTile;
    public int cellsColliding = 0;
    public bool isOverGrid
    {
        get { return cellsColliding > 0; }
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
            GridSpawner.piecesMap.Add(currentTile.gridPos, GetComponent<BlockNode>());
        }
    }
}