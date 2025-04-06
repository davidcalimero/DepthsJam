using UnityEngine;
using TMPro;

public class BuildingNode : BlockNode
{
    public TextMeshPro text;
    public int targetAmount = 0;
    public int currentAmount = 0;

    void OnGUI()
    {
        text.text = currentAmount + "/" + targetAmount;
    }
}