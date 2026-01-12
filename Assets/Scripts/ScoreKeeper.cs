using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int _numberOfCorrectAnswers = 0;
    private int _numberOfQuestionsSeen = 0;
    private int _currentScore = 0;

    public int NumberOfCorrectAnswers => _numberOfCorrectAnswers;
    public int NumberOfQuestionsSeen => _numberOfQuestionsSeen;

    public int CurrentScore => _currentScore;

    public void IncrementNumberOfCorrectAnswers() => _numberOfCorrectAnswers++;
    public void IncrementNumberOfQuestionsSeen() => _numberOfQuestionsSeen++;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int CalculateScore()
    {
        _currentScore = Mathf.RoundToInt(_numberOfCorrectAnswers / (float)_numberOfQuestionsSeen * 100);
        return _currentScore;
    }

    
}
