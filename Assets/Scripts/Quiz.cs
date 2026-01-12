using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions & Choices")]
    [SerializeField] private List<QuestionSO> _questionsList = new List<QuestionSO>();
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private GameObject _choicesObjectsPrefab;
    [SerializeField] private VerticalLayoutGroup _buttonsGroup;
    [SerializeField] private List<GameObject> _choiceObjectsList = new List<GameObject>(); //since im going with questions being able to have more or less than 4
                                                                                           // also I dont actually need to see it in inspector
    [Header("Button Colors")]
    [SerializeField] private Sprite _defaultAnswerSprite;
    [SerializeField] private Sprite _correctAnswerSprite;

    [Header("Scoring")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Slider")]
    [SerializeField] private Slider _progressBar;

    [Header("Game Session Data File")]
    [SerializeField] private GameSessionDataSO _sessionData;

    [Header("Gameplay Stuff")]
    [SerializeField] private int _removeOneAmount = 1;
    [SerializeField] private TextMeshProUGUI _removeOneAmountText;
    [SerializeField] private RemoveOneOracle _removeOneVisual;
    private bool _alreadyRemovedOneThisTurn = false;

    private ScoreKeeper _scoreKeeper;
    private int _correctChoiceIndex;
    private Timer _timer;
    private bool _hasAnsweredEarly;
    private bool _isGameFinished = false;

    //I know all this caching is not necessary for a game like this, just doing it for practice
    //Also I should probably put them all in a wrapper class
    private QuestionSO _currentQuestion;
    private List<TextMeshProUGUI> _choiceTextsList = new List<TextMeshProUGUI>();
    private List<Image> _choiceImagesList = new List<Image>();
    private List<Button> _choiceButtonsList = new List<Button>();



    private bool _firstQuestion = true;
    private void Start()
    {
        _timer = GetComponentInChildren<Timer>();
        _scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        _scoreText.text = "Score: 0%";
        _removeOneAmountText.text = "Power: " + _removeOneAmount;

        _progressBar.maxValue = _questionsList.Count - 1;
        _progressBar.value = 0;
        _sessionData.Clear();
        //LoadNextQuestion(); //first question is loaded in Update
    }

    private void Update()
    {

        if (_timer.CanLoadNextQuestion)
        {
            _hasAnsweredEarly = false;
            LoadNextQuestion();
            _timer.CanLoadNextQuestion = false;
        }
        else if (!_hasAnsweredEarly && !_timer.IsAnsweringQuestion && _currentQuestion != null)
        {
            HandlePlayerChoice(-1);
            SetButtonState(false);

        }
    }

    //creates buttons for each choice for the current question SO, and their OnClick listener
    //sets question text
    //caches everything relevant
    private void DisplayQuestion()
    {
        _questionText.text = _currentQuestion.Question;
        int numberOfChoices = _currentQuestion.ChoiceCount;
        for (int i = 0; i < numberOfChoices; i++)
        {
            GameObject newButtonObj = Instantiate(_choicesObjectsPrefab, _buttonsGroup.transform);
            _choiceObjectsList.Add(newButtonObj);
            _choiceTextsList.Add(_choiceObjectsList[i].GetComponentInChildren<TextMeshProUGUI>());

            _choiceTextsList[i].text = _currentQuestion.GetChoiceFromIndex(i);

            _choiceImagesList.Add(_choiceObjectsList[i].GetComponentInChildren<Image>());

            Button btn = newButtonObj.GetComponent<Button>();

            int index = i;

            btn.onClick.AddListener(() => OnAnswerSelected(index));


            _choiceButtonsList.Add(btn);
        }
    }

    public void OnAnswerSelected(int index)
    {
        _hasAnsweredEarly = true;
        HandlePlayerChoice(index);
        SetButtonState(false);
        _timer.CancelTimer();
    }

    // this is also called if the time runs out
    private void HandlePlayerChoice(int index)
    {
        if (index == _currentQuestion.CorrectChoiceIndex)
        {
            _questionText.text = "Correct!";
            _choiceImagesList[index].sprite = _correctAnswerSprite;
            _scoreKeeper.IncrementNumberOfCorrectAnswers();
        }
        else
        {
            _correctChoiceIndex = _currentQuestion.CorrectChoiceIndex;
            string correctChoice = _currentQuestion.GetChoiceFromIndex(_correctChoiceIndex);
            _questionText.text = "Sorry, the correct answer was: " + correctChoice;
            _choiceImagesList[_correctChoiceIndex].sprite = _correctAnswerSprite;
        }

        _scoreKeeper.CalculateScore();
        _scoreText.text = "Score: " + _scoreKeeper.CurrentScore + "%";
        _isGameFinished = CheckIfGameFinished();

        if (_isGameFinished)
        {
            _sessionData.SetSessionData(
                                        _scoreKeeper.CurrentScore,
                                        _scoreKeeper.NumberOfCorrectAnswers,
                                        _scoreKeeper.NumberOfQuestionsSeen
                                        );

            Invoke(nameof(LoadResultsScene), _timer.TimeToShowCorrectAnswer);

        }
    }

    private void SetupRemoveOne()
    {
        _alreadyRemovedOneThisTurn = false;
    }
    //removes one option

    public void TryRemoveOne()
    {
        
        if (_removeOneAmount <= 0)
        {
            Debug.Log("Out of Remove One!");
            _removeOneVisual.ClickAnimation(false);
            return;
        }

        if (_currentQuestion.ChoiceCount == 2)
        {
            Debug.Log("Can't Remove One when there's only 2 options!");
            _removeOneVisual.ClickAnimation(false);
            return;
        }

        if(_alreadyRemovedOneThisTurn)
        {
            Debug.Log("Can only Remove One once per turn!");
            _removeOneVisual.ClickAnimation(false);
            return;
        }

        _choiceObjectsList[SelectRandomIncorrrectChoiceIndex()].SetActive(false);
        _removeOneAmount--;
        _removeOneAmountText.text = "Power: " + _removeOneAmount;
        _alreadyRemovedOneThisTurn = true;
        _removeOneVisual.ClickAnimation(true);
    }

    private int SelectRandomIncorrrectChoiceIndex()
    {
        int randomIndex = Random.Range(0, _currentQuestion.ChoiceCount - 1);

        if (randomIndex >= _currentQuestion.CorrectChoiceIndex)
        {
            randomIndex++;
        }

        return randomIndex;
    }
    private void LoadResultsScene()
    {
        SceneManager.LoadScene("ResultsScene");

    }

    private bool CheckIfGameFinished()
    {
        return _questionsList.Count <= 0;
    }
    private void SetButtonState(bool state)
    {
        for (int i = 0; i < _choiceButtonsList.Count; i++)
        {
            _choiceButtonsList[i].interactable = state;

        }
    }

    private void LoadNextQuestion()
    {
        if (_questionsList.Count <= 0) return;

        //SetButtonState(true); // since we actually create new buttons every time, this  isn't needed
        ClearPreviousQuestion();
        SetDefaultChoiceSprites();
        GetRandomQuestion();
        DisplayQuestion();
        _scoreKeeper.IncrementNumberOfQuestionsSeen();
        SetupRemoveOne();

        // I prefer to show the bar with no progress when we start out
        if (_firstQuestion)
        {
            _firstQuestion = false;
            return;
        }
        _progressBar.value++;

    }

    private void GetRandomQuestion()
    {
        _currentQuestion = _questionsList[Random.Range(0, _questionsList.Count)];

        if (_questionsList.Contains(_currentQuestion))
        {
            _questionsList.Remove(_currentQuestion);
        }

    }

    private void ClearPreviousQuestion()
    {
        foreach (GameObject obj in _choiceObjectsList)
        {

            Destroy(obj);

        }
        _choiceObjectsList.Clear();
        _choiceTextsList.Clear();
        _choiceImagesList.Clear();
        _choiceButtonsList.Clear();
    }

    private void SetDefaultChoiceSprites()
    {
        foreach (Image img in _choiceImagesList)
        {
            img.sprite = _defaultAnswerSprite;
        }
    }
}
