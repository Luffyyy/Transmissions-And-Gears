using UnityEngine;

[CreateAssetMenu(fileName = "QuizQuestion", menuName = "Scriptable Objects/QuizQuestion")]
public class QuizQuestion : QuizSlide
{
    [TextArea] public string hintText;

    public string[] answers;
    public int correctAnswer;
}
