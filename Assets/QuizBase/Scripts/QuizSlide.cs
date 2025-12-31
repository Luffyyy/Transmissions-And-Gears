using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizSlide", menuName = "Scriptable Objects/QuizSlide")]
public class QuizSlide : ScriptableObject
{
    [TextArea] public string title;

    public Sprite image;

    [TextArea] public string text;

    public GameObject arPrefab;
}

[CreateAssetMenu(fileName = "QuizQuestion", menuName = "Scriptable Objects/QuizQuestion")]
public class QuizQuestion : QuizSlide
{
    [TextArea] public string hintText;

    public string[] answers;
    public int correctAnswer;
}
