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
            ++cellsColliding;
            currentTile = other.GetComponent<Tile>();
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GridCell"))
            --cellsColliding;
    }
}