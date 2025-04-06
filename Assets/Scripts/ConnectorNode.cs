using UnityEngine;
using System.Collections.Generic;

public class ConnectorNode : MonoBehaviour
{
    public ElementType type;
    public List<Sprite> iconsLeft;
    public List<Sprite> iconsRight;
    public List<Sprite> iconsUp;
    public List<Sprite> iconsDown;

    public float direction = 0;

    void Start()
    {
        switch(direction)
        {
            case -90:
                GetComponent<SpriteRenderer>().sprite = iconsUp[(int)type];
                break;
            case 180:
                GetComponent<SpriteRenderer>().sprite = iconsRight[(int)type];
                break;
            case -270:
                GetComponent<SpriteRenderer>().sprite = iconsDown[(int)type];
                break;
            case 0:
                GetComponent<SpriteRenderer>().sprite = iconsLeft[(int)type];
                break;
        }
    }
}
