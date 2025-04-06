using UnityEngine;
using System.Collections.Generic;

public class BackgroundFiller : MonoBehaviour
{
    public GameObject backgroundTilePrefab;
    public float tileHeight;

    private List<GameObject> spawnedTiles = new();
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        GameObject first = Instantiate(backgroundTilePrefab, Vector3.zero, Quaternion.identity, transform);
        spawnedTiles.Add(first);
        tileHeight = CalculateBackgroundHeight(first);
    }

    void Update()
    {
        CheckBackground();
    }

    void CheckBackground()
    {
        GameObject lastTile = spawnedTiles[spawnedTiles.Count - 1];
        float cameraBottom = cam.transform.position.y - cam.orthographicSize;
        float lastTileBottom = lastTile.transform.position.y - tileHeight / 2f;

        if (cameraBottom < lastTileBottom + tileHeight)
        {
            Vector3 newPos = lastTile.transform.position - new Vector3(0, tileHeight, 0);
            newPos.y += 3;
            GameObject newTile = Instantiate(backgroundTilePrefab, newPos, Quaternion.identity, transform);
            spawnedTiles.Add(newTile);
        }
    }

    float CalculateBackgroundHeight(GameObject prefab)
    {
        SpriteRenderer[] renderers = prefab.GetComponentsInChildren<SpriteRenderer>(true);

        if (renderers.Length == 0)
            return 0;

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        return bounds.size.y;
    }
}
