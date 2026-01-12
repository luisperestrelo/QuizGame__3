using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private GameSessionDataSO _sessionData;

    private TextMeshProUGUI _endText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _endText = GetComponentInChildren<TextMeshProUGUI>();
        _endText.text = "Congrats! Here are your results:\n" +
                        "Correct answers: " + _sessionData.CorrectAnswers + "\n" +
                        "Total Questions: " + _sessionData.TotalQuestions + "\n" +
                        "Final Score: " + _sessionData.FinalScore;

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("QuizScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {

    }
}
