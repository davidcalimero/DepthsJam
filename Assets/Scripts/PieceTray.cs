using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PieceTrayUI : MonoBehaviour
{
    public Image previewImage;
    public List<GameObject> availablePieces;
    public GameObject draggablePiecePrefab;

    void Start()
    {
        LoadRandomPiece();
    }

    void LoadRandomPiece()
    {
        draggablePiecePrefab = availablePieces[Random.Range(0, availablePieces.Count)];
        previewImage.sprite = draggablePiecePrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public void OnBeginDrag()
    {
        GameObject piece = Instantiate(draggablePiecePrefab);
        Vector2 position = GetMouseWorldPosition();
        piece.transform.position = new Vector3(position.x, position.y, draggablePiecePrefab.transform.position.z); 

        DraggablePiece dp = piece.GetComponent<DraggablePiece>();
        dp.onPiecePlaced = OnPiecePlaced;
        dp.StartDragging();
    }

    void OnPiecePlaced(bool successful)
    {
        if (successful)
        {
            LoadRandomPiece();
        }
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector2 mouse = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}