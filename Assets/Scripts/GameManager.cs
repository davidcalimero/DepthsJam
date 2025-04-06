using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int availablePieces;
    public Button nextDepthButton;
    public int scoreToIncreaseDepth;
    public GameObject panel;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text endScoreText;

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

    private void LateUpdate()
    {
        panel.SetActive(availablePieces <= 0);
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        endScoreText.text = $"{score}";
        nextDepthButton.gameObject.SetActive((score - lastDepthScore) >= scoreToIncreaseDepth);
    }

    public void ResetButton()
    {
        lastDepthScore = score;
        UpdateUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
