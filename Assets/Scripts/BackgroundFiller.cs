using UnityEngine;

public class BackgroundFiller : MonoBehaviour
{
    public GameObject[] backgroundPrefabs;

    private void Start()
    {
        GenerateBackground(-1, -1, 1, 1, 0.05f, 0.01f);
    }
    public void GenerateBackground(float minX, float maxX, float minY, float maxY, float piecesScale, float distanceBetween)
    {

    }
}
