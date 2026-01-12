using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] private QuestionSO _question;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private List<GameObject> _choiceObjectsList = new List<GameObject>(); //since im going with questions being able to have more or less than 4
    [SerializeField] private GameObject _choicesObjectsPrefab;
    [SerializeField] private VerticalLayoutGroup _buttonsGroup;

    [SerializeField] private Sprite _defaultAnswerSprite;
    [SerializeField] private Sprite _correctAnswerSprite;

    private List<TextMeshProUGUI> _choiceTextsList = new List<TextMeshProUGUI>();
    private List<Image> _choiceImagesList = new List<Image>();
    private List<Button> _choiceButtonsList = new List<Button>();
    private int _correctChoiceIndex;


    private void Start()
    {
        LoadNextQuestion();
    }

    //creates buttons for each choice for the current question SO, and their OnClick listener
    //sets question text
    //caches everything relevant
    private void DisplayQuestions()
    {
        _questionText.text = _question.Question;
        int numberOfChoices = _question.ChoiceCount;
        for (int i = 0; i < numberOfChoices; i++)
        {
            GameObject newButtonObj = Instantiate(_choicesObjectsPrefab, _buttonsGroup.transform);
            _choiceObjectsList.Add(newButtonObj);
            _choiceTextsList.Add(_choiceObjectsList[i].GetComponentInChildren<TextMeshProUGUI>());

            _choiceTextsList[i].text = _question.GetChoiceFromIndex(i);

            _choiceImagesList.Add(_choiceObjectsList[i].GetComponentInChildren<Image>());

            Button btn = newButtonObj.GetComponent<Button>();

            int index = i;

            btn.onClick.AddListener(() => OnAnswerSelected(index));


            _choiceButtonsList.Add(btn);
        }
    }

    public void OnAnswerSelected(int index)
    {
        if (index == _question.CorrectChoiceIndex)
        {
            _questionText.text = "Correct!";
            _choiceImagesList[index].sprite = _correctAnswerSprite;
        }
        else
        {
            _correctChoiceIndex = _question.CorrectChoiceIndex;
            string correctChoice = _question.GetChoiceFromIndex(_correctChoiceIndex);
            _questionText.text = "Sorry, the correct answer was: " + correctChoice;
            _choiceImagesList[_correctChoiceIndex].sprite = _correctAnswerSprite;
        }

        SetButtonState(false);
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
        SetButtonState(true);
        ClearPreviousQuestion();
        DisplayQuestions();
        SetDefaultChoiceSprites();
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
