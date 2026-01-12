using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float _timerValue;
    [SerializeField] private float _timeToCompleteQuestion = 5f;
    [SerializeField] private float _timeToShowCorrectAnswer = 2f;

    [SerializeField] Image _timerImage;

    private bool _isAnsweringQuestion = false;
    private float _fillFraction;
    private float _activeTimer = 1f;
    private bool _canLoadNextQuestion = false;

    public bool CanLoadNextQuestion
    {
        get => _canLoadNextQuestion;
        set => _canLoadNextQuestion = value;
    }

    public bool IsAnsweringQuestion => _isAnsweringQuestion;

    private void Update()
    {
        UpdateTimer();
        _timerImage.fillAmount = _fillFraction;


    }

    public void CancelTimer()
    {
        _timerValue = 0f;
    }

    private void UpdateTimer()
    {
        _timerValue -= Time.deltaTime;

        if (_isAnsweringQuestion)
        {
            if (_timerValue <= 0)
            {
                _timerValue = _timeToShowCorrectAnswer;
                _activeTimer = _timeToShowCorrectAnswer;
                _isAnsweringQuestion = false;
                
            }

        }
        else
        {
            if (_timerValue <= 0)
            {
                _isAnsweringQuestion = true;
                _canLoadNextQuestion = true; // should I decouple with an event? not sure
                _timerValue = _timeToCompleteQuestion;
                _activeTimer = _timeToCompleteQuestion;
            }
        }

        if (_timerValue > 0)
        {
            _fillFraction = _timerValue / _activeTimer;
        }

        // This isnt really needed no?
        //    if (_timerValue <= 0)
        //    {
        //       _timerValue = _timeToCompleteQuestion;
        //    }

    }
}
