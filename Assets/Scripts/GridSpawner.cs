using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public GameObject connectorPrefab;
    public int columns;
    public int rows;
    public float initialY;

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private Dictionary<Vector2Int, BlockNode> piecesMap = new Dictionary<Vector2Int, BlockNode>();

    public float lastGridBottomY;
    private float tileSize;

    private void Awake()
    {
        tileSize = GetTileSize();
        lastGridBottomY = initialY;
        piecesMap.Clear();
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
            }

            lastGridBottomY = yPos;
        }
    }

    public void PlaceBlock(Vector2Int gridPos, BlockNode node)
    {
        piecesMap.Add(gridPos, node);
        UpdateConnectionsForBlock(node);
    }

    private void UpdateConnectionsForBlock(BlockNode node)
    {
        foreach (Vector2Int dir in Directions)
        {
            Vector2Int neighborPos = node.gridPosition + dir;

            if (piecesMap.TryGetValue(neighborPos, out BlockNode neighbor))
            {
                if (node.type == neighbor.type && node != neighbor)
                {
                    AddConnectorTo(node, dir);
                    AddConnectorTo(neighbor, -dir);
                }
            }
        }
    }

    private float DirectionToAngle(Vector2Int dir)
    {
        if (dir == Vector2Int.left) return 0f;
        if (dir == Vector2Int.up) return -90f;
        if (dir == Vector2Int.right) return 180f;
        if (dir == Vector2Int.down) return -270f;
        return 0f;
    }

    private void AddConnectorTo(BlockNode fromNode, Vector2Int direction)
    {
        GameObject connector = Instantiate(connectorPrefab, fromNode.transform);
        connector.GetComponent<ConnectorNode>().type = fromNode.type;
        connector.transform.localPosition = Vector3.zero;
        connector.transform.rotation = Quaternion.Euler(0, 0, DirectionToAngle(direction));
    }
}
