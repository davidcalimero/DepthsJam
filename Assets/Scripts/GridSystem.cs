using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public int columns;
    public float startY;
    public float bottomGap;

    public static Dictionary<Vector2Int, GameObject> tileMap = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        Camera cam = Camera.main;

        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        float tileSize = screenWidth / columns;

        float bottomY = cam.transform.position.y - cam.orthographicSize;
        float usableHeight = startY - bottomY - bottomGap;
        int rows = Mathf.FloorToInt(usableHeight / tileSize);

        float startX = -screenWidth / 2f + tileSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            float yPos = startY - row * tileSize;
            int yGrid = row;

            for (int col = 0; col < columns; col++)
            {
                float xPos = startX + col * tileSize;
                int xGrid = col;

                Vector2 spawnPos = new Vector2(xPos, yPos);
                GameObject tile = Instantiate(gridTilePrefab, spawnPos, Quaternion.identity, transform);

                // Scale tile
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Vector2 spriteSize = sr.bounds.size;
                    float scaleX = tileSize / spriteSize.x;
                    float scaleY = tileSize / spriteSize.y;
                    tile.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }

                // Set grid position in tile
                GridTile gridTile = tile.GetComponent<GridTile>();
                if (gridTile != null)
                {
                    gridTile.gridPos = new Vector2Int(xGrid, yGrid);
                }

                tileMap[new Vector2Int(xGrid, yGrid)] = tile;
            }
        }
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 snappedPos = GetSnappedPosition(mouseWorldPos);
        //    //Instantiate(objectToPlace, snappedPos, Quaternion.identity);
        //}
    }

    //Vector2 GetSnappedPosition(Vector2 worldPos)
    //{
    //    float x = Mathf.Floor(worldPos.x / gridCellSize.x) * gridCellSize.x;
    //    float y = Mathf.Floor(worldPos.y / gridCellSize.y) * gridCellSize.y;
    //    return new Vector2(x, y);
    //}

    //void OnDrawGizmos()
    //{
    //    int width = 20;
    //    int height = 20;
    //    Gizmos.color = Color.gray;

    //    for (int x = 0; x <= width; x++)
    //    {
    //        Gizmos.DrawLine(new Vector3(x * gridCellSize.x, 0, 0), new Vector3(x * gridCellSize.x, height * gridCellSize.y, 0));
    //    }

    //    for (int y = 0; y <= height; y++)
    //    {
    //        Gizmos.DrawLine(new Vector3(0, y * gridCellSize.y, 0), new Vector3(width * gridCellSize.x, y * gridCellSize.y, 0));
    //    }
    //}
}
