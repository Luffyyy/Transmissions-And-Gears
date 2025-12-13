using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public QuizData data;

    private int currentPageIndex;
    private QuizPage currentPage => data.pages[currentPageIndex];

    public GameObject title;
    public GameObject text1;
    public GameObject text2;
    public GameObject answersHolder;
    public GameObject continueButton;

    void Start()
    {
        SetCurrentPage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPage()
    {
        currentPageIndex++;
        print("Page: " + currentPageIndex);
        SetCurrentPage();
    }

    public void CheckAnswer(int answer)
    {
        if (currentPage.correctAnswer == answer)
        {
            NextPage();
        } else
        {
            print("Wrong answer!");
        }
    }

    void SetCurrentPage()
    {
        title.GetComponent<TextMeshProUGUI>().SetText(currentPage.title);
        text1.GetComponent<TextMeshProUGUI>().SetText(currentPage.text1);
        text2.GetComponent<TextMeshProUGUI>().SetText(currentPage.text2);

        answersHolder.SetActive(currentPage.pageType == PageType.Question);
        continueButton.SetActive(currentPage.pageType == PageType.Explanation);

        if (currentPage.pageType == PageType.Question)
        {
            answersHolder.GetNamedChild("Answer1").GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[0]);
            answersHolder.GetNamedChild("Answer2").GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[1]);
            answersHolder.GetNamedChild("Answer3").GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[2]);
            answersHolder.GetNamedChild("Answer4").GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[3]);
        }
    }
}
