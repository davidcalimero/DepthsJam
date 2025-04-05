using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public int columns;
    public int rows;
    public float initialY;

    public static Dictionary<Vector2Int, GameObject> tileMap = new Dictionary<Vector2Int, GameObject>();

    public float lastGridBottomY;
    private float tileSize;

    private void Awake()
    {
        tileSize = GetTileSize();
        lastGridBottomY = initialY;
    }

    private void Start()
    {
        SpawnRowsFrom(lastGridBottomY);
    }

    public float GetTileSize()
    {
        return gridTilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
    }


   public void SpawnMoreRows()
    {
        float newStartY = lastGridBottomY - tileSize;
        SpawnRowsFrom(newStartY);
    }

    private void SpawnRowsFrom(float startY)
    {
        float totalWidth = columns * tileSize;
        float startX = -totalWidth / 2f + tileSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            float yPos = startY - row * tileSize;
            int yGrid = Mathf.RoundToInt(yPos / tileSize);

            for (int col = 0; col < columns; col++)
            {
                float xPos = startX + col * tileSize;
                int xGrid = col;

                Vector2 spawnPos = new Vector2(xPos, yPos);
                GameObject tile = Instantiate(gridTilePrefab, spawnPos, Quaternion.identity, transform);
                tile.GetComponent<Tile>().gridPos = new Vector2Int(xGrid, yGrid);
                tile.GetComponent<Tile>().worldPos = tile.transform.position;

                tileMap[new Vector2Int(xGrid, yGrid)] = tile;
            }

            lastGridBottomY = yPos;
        }
    }
}
