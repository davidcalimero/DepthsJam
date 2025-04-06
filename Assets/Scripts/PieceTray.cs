using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PieceTrayUI : MonoBehaviour
{
    public Image previewImage;
    public List<GameObject> availablePieces;
    public GameObject buildingNodePrefab;
    public GameObject elementNodePrefab;
    public float buildingProbability = 0.3f;
    public Vector2Int buildingObjective;
    public Camera renderCamera;
    public RenderTexture renderTexture;

    private GameObject currentPiecePreview;

    void Start()
    {
        LoadRandomPiece();
    }

    void LoadRandomPiece()
    {
        GameObject basePrefab = availablePieces[Random.Range(0, availablePieces.Count)];
        currentPiecePreview = Instantiate(basePrefab);
        currentPiecePreview.transform.position = Vector3.zero;
        AssignRandomNodes(currentPiecePreview);
        SetLayerRecursively(currentPiecePreview, LayerMask.NameToLayer("Water"));

        previewImage.sprite = RenderToSprite();

        currentPiecePreview.SetActive(false);
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void OnBeginDrag()
    {
        Vector2 position = GetMouseWorldPosition();
        currentPiecePreview.transform.position = new Vector3(position.x, position.y, -1);
        currentPiecePreview.SetActive(true);
        SetLayerRecursively(currentPiecePreview, LayerMask.NameToLayer("Default"));

        DraggablePiece dp = currentPiecePreview.GetComponent<DraggablePiece>();
        dp.onPiecePlaced = OnPiecePlaced;
        dp.StartDragging();
    }

    void OnPiecePlaced(bool successful)
    {
        if (successful)
        {
            LoadRandomPiece();
        }
        else
        {
            SetLayerRecursively(currentPiecePreview, LayerMask.NameToLayer("Water"));
            currentPiecePreview.SetActive(false);
        }
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector2 mouse = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    void AssignRandomNodes(GameObject piece)
    {
        BlockCollider[] blocks = piece.GetComponentsInChildren<BlockCollider>();

        int total = blocks.Length;
        int guaranteedBuildingIndex = Random.Range(0, total);

        for (int i = 0; i < total; i++)
        {
            bool isBuilding = (i == guaranteedBuildingIndex) || (Random.value < buildingProbability);

            ElementType randomType = (ElementType)Random.Range(0, System.Enum.GetNames(typeof(ElementType)).Length);

            GameObject prefab = isBuilding ? buildingNodePrefab : elementNodePrefab;
            GameObject nodeObj = Instantiate(prefab, blocks[i].transform);
            nodeObj.transform.localPosition = Vector3.zero;

            BlockNode node = nodeObj.GetComponent<BlockNode>();
            node.type = randomType;
            node.GetComponent<SpriteRenderer>().sprite = node.icons[(int)randomType];

            if (isBuilding)
            {
                BuildingNode bn = (BuildingNode)node;
                bn.targetAmount = Random.Range(buildingObjective.x, buildingObjective.y + 1);
            }
        }
    }

    Bounds GetCombinedBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.one * 0.1f); // fallback

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }

    Sprite RenderToSprite()
    {
        Bounds bounds = GetCombinedBounds(currentPiecePreview);

        float boundsAspect = bounds.size.x / bounds.size.y;
        float renderAspect = (float)renderTexture.width / renderTexture.height;

        if (renderAspect > boundsAspect)
        {
            renderCamera.orthographicSize = bounds.extents.y;
        }
        else
        {
            renderCamera.orthographicSize = bounds.size.x / renderAspect / 2f;
        }

        renderCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        renderCamera.Render();

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        renderCamera.targetTexture = null;
        RenderTexture.active = null;

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}