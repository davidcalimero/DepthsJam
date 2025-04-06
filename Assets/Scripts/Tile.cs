using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPos;
    public Vector2 worldPos;
    public bool _filled = false;
    public bool filled
    {
        get { return _filled; }
        set {
            GetComponent<SpriteRenderer>().color = value ? new Color(0, 0, 0, 0): Color.white; //Debug: if seen, there is a bug
            _filled = value;
        }
    }


}
