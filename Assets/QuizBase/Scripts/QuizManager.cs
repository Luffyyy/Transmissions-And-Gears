using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public QuizData data;

    private int currentPageIndex;
    private QuizPage currentPage => data.pages[currentPageIndex];

    public GameObject title;
    public GameObject text;
    public GameObject hintText;
    public GameObject answersHolder;
    public GameObject continueButton;
    public GameObject image;

    public AudioSource audioSource;
    public AudioResource correctSound;
    public AudioResource incorrectSound;

    private GameObject[] answers = new GameObject[4];

    void Start()
    {
        answers[0] = answersHolder.GetNamedChild("Answer1");
        answers[1] = answersHolder.GetNamedChild("Answer2");
        answers[2] = answersHolder.GetNamedChild("Answer3");
        answers[3] = answersHolder.GetNamedChild("Answer4");
        SetCurrentPage();
    }

    public void NextPage()
    {
        currentPageIndex++;
        if (currentPageIndex == data.pages.Count)
        {
            // Destroy self when the page is done. TODO: allow to revisit the quiz without restart
            Destroy(gameObject);
        } else {
            SetCurrentPage();
        }
    }

    public void CheckAnswer(int answer)
    {
        // Check answer
        if (currentPage.correctAnswer == answer)
        {
            audioSource.resource = correctSound;
            NextPage();
        } else
        {
            answers[answer].GetComponent<Animator>().Play("Incorrect"); // Show incorrect button animation
            audioSource.resource = incorrectSound;
            hintText.SetActive(true);
        }
        audioSource.Play(); // Play audio either correct or incorrect
    }

    /**
        Updates the quiz UI to the current page, either explanation or question
    */
    void SetCurrentPage()
    {
        hintText.SetActive(false);
        title.GetComponent<TextMeshProUGUI>().SetText(currentPage.title);
        text.GetComponent<TextMeshProUGUI>().SetText(currentPage.text);
        hintText.GetComponent<TextMeshProUGUI>().SetText(currentPage.hintText);
        image.GetComponent<Image>().sprite = currentPage.image;
        image.SetActive(currentPage.image != null);

        answersHolder.SetActive(currentPage.pageType == PageType.Question);
        continueButton.SetActive(currentPage.pageType == PageType.Explanation);

        // If question then we may update answers based on how many there are
        if (currentPage.pageType == PageType.Question)
        {
            answers[0].GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[0]);
            answers[1].GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[1]);
            if (currentPage.answers.Length > 2)
            {
                answers[2].SetActive(true);
                answers[2].GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[2]);
            } else
            {
                answers[2].SetActive(false);
            }
            
            if (currentPage.answers.Length > 3)
            {
                answers[3].SetActive(true);
                answers[3].GetComponentInChildren<TextMeshProUGUI>().SetText(currentPage.answers[3]);
            } else
            {
                answers[3].SetActive(false);
            }
        }
    }
}
