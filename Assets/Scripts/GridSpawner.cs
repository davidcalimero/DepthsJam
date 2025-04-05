using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public int columns;
    public float initialY;
    public float bottomGap;

    public static Dictionary<Vector2Int, GameObject> tileMap = new Dictionary<Vector2Int, GameObject>();

    private float lastGridBottomY;

    private void Awake()
    {
        lastGridBottomY = initialY;
    }

    public void UpdateGrid()
    {
        Camera cam = Camera.main;

        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        float tileSize = screenWidth / columns;

        float startY = lastGridBottomY - tileSize;

        float bottomY = cam.transform.position.y - cam.orthographicSize;
        float usableHeight = startY - bottomY - bottomGap;
        int rows = Mathf.FloorToInt(usableHeight / tileSize);

        float startX = -screenWidth / 2f + tileSize / 2f;

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

                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Vector2 spriteSize = sr.bounds.size;
                    float scaleX = tileSize / spriteSize.x;
                    float scaleY = tileSize / spriteSize.y;
                    tile.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }

                GridTile gridTile = tile.GetComponent<GridTile>();
                if (gridTile != null)
                {
                    gridTile.gridPos = new Vector2Int(xGrid, yGrid);
                }

                tileMap[new Vector2Int(xGrid, yGrid)] = tile;
            }

            lastGridBottomY = yPos;
        }
    }
}
