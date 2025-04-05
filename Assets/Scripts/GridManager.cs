using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public GridSpawner gridSpawner;
    public float scrollDuration;
    public int depthStep;

    private int currentDepth = 0;
    private bool isScrolling = false;


    private void Start()
    {
        gridSpawner.UpdateGrid();
    }

    public void NextDepth()
    {
        if (isScrolling) return;

        currentDepth++;

        StartCoroutine(ScrollCameraDown());
    }

    IEnumerator ScrollCameraDown()
    {
        isScrolling = true;

        Camera cam = Camera.main;
        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = startPos + Vector3.down * depthStep;

        float elapsed = 0f;

        while (elapsed < scrollDuration)
        {
            cam.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / scrollDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPos;
        isScrolling = false;

        gridSpawner.UpdateGrid();
    }
}