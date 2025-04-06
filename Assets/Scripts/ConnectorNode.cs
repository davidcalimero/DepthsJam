using UnityEngine;
using System.Collections.Generic;

public class ConnectorNode : MonoBehaviour
{
    public ElementType type;
    public List<Sprite> icons;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = icons[(int)type];
    }
}
