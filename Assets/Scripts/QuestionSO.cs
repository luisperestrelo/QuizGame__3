using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quiz Questions", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] private string _question = "Enter question here";
    //[SerializeField] private string[] _choicesArray = new string[4] {"A","B","C","D"};
    //Doing List instead so we can let designers have questions with different sizes of answers
    [SerializeField] private List<string> _choicesList = new List<string> { "A", "B", "C", "D" };



    [SerializeField] private int _correctChoiceIndex = -1;
    private const int MIN_CHOICES = 2;
    private const int _MAX_CHOICES = 5;



    private void OnValidate()
    {
        while (_choicesList.Count < MIN_CHOICES)
        {
            _choicesList.Add(((char)('A' + _choicesList.Count)).ToString());
            Debug.LogWarning($"Question '{name}' must have at least {MIN_CHOICES} choices!");
        }

        while (_choicesList.Count > _MAX_CHOICES)
        {
            _choicesList.RemoveAt(_choicesList.Count - 1);
            Debug.LogWarning($"Question '{name}' cannot have more than {_MAX_CHOICES} choices!");
        }

        if (_correctChoiceIndex == -1 || _correctChoiceIndex > _choicesList.Count)
        {
            Debug.LogError($"<color=red><b>ACTION REQUIRED:</b></color> Question '{name}' does not have a valid index for the correct answer", this);
        }
    }
    public string Question => _question;
    public int CorrectChoiceIndex => _correctChoiceIndex;

    public string GetChoiceFromIndex(int index)
    {
        return _choicesList[index];
    }

    public int ChoiceCount => _choicesList.Count;

}
