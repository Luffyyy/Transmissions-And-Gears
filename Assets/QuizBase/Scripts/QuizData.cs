using System.Collections.Generic;
using UnityEngine;

public enum PageType { Explanation, Question }

[System.Serializable]
public class QuizPage
{
    public PageType pageType;

    // The question
    [TextArea] public string title;

    [TextArea] public string text1;

    [TextArea] public string text2;

    public string[] answers;
    public int correctAnswer;
}

[CreateAssetMenu(fileName = "QuizData", menuName = "Scriptable Objects/QuizData")]
public class QuizData : ScriptableObject
{
    public List<QuizPage> pages;
}
