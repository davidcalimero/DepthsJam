using UnityEngine;
using System.Collections.Generic;

public class BlockNode : MonoBehaviour
{
    public List<Sprite> icons;
    public ElementType type;
    public List<Color> colors = new List<Color>() { Color.yellow, new Color(255, 0, 255), Color.green, Color.blue};
    public List<Sprite> circles;
    public Vector2Int gridPosition;
}
