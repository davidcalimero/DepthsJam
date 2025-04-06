using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridTilePrefab;
    public GameObject connectorPrefab;
    public GameObject startPiece;
    public int columns;
    public int rows;
    public float initialY;
    public Vector2Int startPosition = new Vector2Int(0, 4);
    private bool startExists = false;

    public static readonly Vector2Int[] Directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private Dictionary<Vector2Int, BlockNode> _piecesMap = new Dictionary<Vector2Int, BlockNode>();
    public Dictionary<Vector2Int, BlockNode> piecesMap { get { return _piecesMap; } }


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
        GameManager.Instance.ResetButton();
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

                if(row == startPosition.y && col == startPosition.x && !startExists)
                {
                    GameObject piece = Instantiate(startPiece, transform);
                    piece.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                    piece.transform.position = new Vector3(0, 0.1f, 0);
                    tile.GetComponent<Tile>().filled = true;
                    startExists = true;
                }
            }

            lastGridBottomY = yPos;
        }
    }

    public void PlaceBlock(Vector2Int gridPos, BlockNode node)
    {
        piecesMap.Add(gridPos, node);
        UpdateConnectionsForBlock(node);
        UpdateConnectedGroupFrom(node);
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
        connector.GetComponent<ConnectorNode>().direction = DirectionToAngle(direction);
        fromNode.transform.GetChild(2).gameObject.SetActive(true);
        fromNode.transform.GetChild(3).gameObject.SetActive(true);
        fromNode.transform.GetChild(1).localScale = Vector3.one;

    }

    private void UpdateConnectedGroupFrom(BlockNode startingNode)
    {
        HashSet<Vector2Int> visited = new();
        Queue<Vector2Int> toVisit = new();
        List<BuildingNode> buildingsInChain = new();

        visited.Add(startingNode.gridPosition);
        toVisit.Enqueue(startingNode.gridPosition);

        int count = 0;

        while (toVisit.Count > 0)
        {
            Vector2Int pos = toVisit.Dequeue();

            if (!piecesMap.TryGetValue(pos, out BlockNode node)) continue;
            if (node.type != startingNode.type) continue;

            if (node is BuildingNode building)
            {
                buildingsInChain.Add(building);
            }
            else
            {
                count++;
            }

            foreach (Vector2Int dir in Directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (visited.Contains(neighborPos)) continue;

                if (piecesMap.TryGetValue(neighborPos, out BlockNode neighbor) &&
                    neighbor.type == startingNode.type)
                {
                    visited.Add(neighborPos);
                    toVisit.Enqueue(neighborPos);
                }
            }
        }

        // Update all buildings in this group
        foreach (var building in buildingsInChain)
        {
            building.currentAmount = count;
        }
    }
}
