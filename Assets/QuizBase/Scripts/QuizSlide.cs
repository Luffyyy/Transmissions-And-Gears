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