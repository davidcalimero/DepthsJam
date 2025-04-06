using TMPro;

public class BuildingNode : BlockNode
{
    public TextMeshPro text;
    public int targetAmount = 0;
    public int currentAmount = 0;
    public int points = 0;

    private bool objectiveReached = false;

    private void Start()
    {
        text.color  = colors[(int)type];
    }

    void OnGUI()
    {
        if(!objectiveReached && currentAmount >= targetAmount)
        {
            objectiveReached = true;
            GameManager.Instance.AddPoints(points * currentAmount);
            ++GameManager.Instance.availablePieces;

        }
        if (text.gameObject.activeSelf && targetAmount <= currentAmount)
        {
            text.gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        text.text =  (targetAmount - currentAmount).ToString();

    }
}