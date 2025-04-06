using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int availablePieces;
    public Button nextDepthButton;
    public int scoreToIncreaseDepth;

    [Header("UI")]
    public TMP_Text scoreText;

    private int lastDepthScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        nextDepthButton.gameObject.SetActive((score - lastDepthScore) >= scoreToIncreaseDepth);
    }

    public void ResetButton()
    {
        lastDepthScore = score;
        UpdateUI();
    }
}
