using UnityEngine;
using System.Collections.Generic;

public class BlockNode : MonoBehaviour
{
    public List<Sprite> icons;
    public ElementType type;
    public Vector2Int gridPosition;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = icons[(int)type];
    }
}
