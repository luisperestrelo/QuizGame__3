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
    private void Update()
    {
        UpdateTimer();
        _timerImage.fillAmount = _fillFraction;
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
                _isAnsweringQuestion=false;
            }

        }
        else
        {
            if (_timerValue <= 0)
            {
                _isAnsweringQuestion = true;
                _timerValue = _timeToCompleteQuestion;
                _activeTimer = _timeToCompleteQuestion;
            }
        }

        if (_timerValue > 0 )
        {
            _fillFraction = _timerValue / _activeTimer;
        }

        // This isnt really needed no?
    //    if (_timerValue <= 0)
    //    {
     //       _timerValue = _timeToCompleteQuestion;
    //    }

        Debug.Log(_timerValue);
    }
}
