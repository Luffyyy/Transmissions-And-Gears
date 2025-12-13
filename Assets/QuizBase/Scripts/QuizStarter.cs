using UnityEngine;

public class QuizStarter : MonoBehaviour
{
    public GameObject quiz;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(quiz, GameObject.Find("UI").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
