using TMPro;

public class BuildingNode : BlockNode
{
    public TextMeshPro text;
    public int targetAmount = 0;
    public int currentAmount = 0;
    public int points = 0;

    private bool objectiveReached = false;

    void OnGUI()
    {
        if(!objectiveReached && currentAmount >= targetAmount)
        {
            objectiveReached = true;
            GameManager.Instance.AddPoints(points * currentAmount);
            ++GameManager.Instance.availablePieces;

        }
        text.text = currentAmount + "/" + targetAmount;
    }
}