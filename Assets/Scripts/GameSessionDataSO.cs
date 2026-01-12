using UnityEngine;

//in the past I would use a singleton with dontdestroyonload() for this, but trying alternatives in order to learn
//saves a session's score

[CreateAssetMenu(menuName = "Session Data")]
public class GameSessionDataSO : ScriptableObject
{
    public int FinalScore { get; private set; }
    public int CorrectAnswers { get; private set; }
    public int TotalQuestions { get; private set; }

    public void SetSessionData(int score, int correct, int total)
    {
        FinalScore = score;
        CorrectAnswers = correct;
        TotalQuestions = total;
    }

    public void Clear()
    {
        FinalScore = 0;
        CorrectAnswers = 0;
        TotalQuestions = 0;
    }
}