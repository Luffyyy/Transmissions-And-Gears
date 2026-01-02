using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<QuizSlide> slides;

    private int currentSlideIndex;
    private QuizSlide currentSlide => slides[currentSlideIndex];

    public GameObject topPanel;
    public GameObject title;
    public GameObject text;
    public GameObject hintText;
    public GameObject answersHolder;
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject image;

    public AudioSource audioSource;
    public AudioResource correctSound;
    public AudioResource incorrectSound;

    private GameObject[] answers = new GameObject[4];

    private GameObject currentARPrefab;
    public Transform arAnchor;

    void Start()
    {
        answers[0] = answersHolder.GetNamedChild("Answer1");
        answers[1] = answersHolder.GetNamedChild("Answer2");
        answers[2] = answersHolder.GetNamedChild("Answer3");
        answers[3] = answersHolder.GetNamedChild("Answer4");
        SetcurrentSlide();
    }

    public void NextSlide()
    {
        currentSlideIndex++;
        SetcurrentSlide();
    }

    public void PrevSlide()
    {
        currentSlideIndex--;
        SetcurrentSlide();
    }

    public void CheckAnswer(int answer)
    {
        // Check answer
        if ((currentSlide as QuizQuestion).correctAnswer == answer)
        {
            audioSource.resource = correctSound;
            NextSlide();
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
    void SetcurrentSlide()
    {
        hintText.SetActive(false);
        title.GetComponent<TextMeshProUGUI>().SetText(currentSlide.title);
        text.GetComponent<TextMeshProUGUI>().SetText(currentSlide.text);
        image.GetComponent<Image>().sprite = currentSlide.image;
        image.SetActive(currentSlide.image != null);

        var isQuiz = currentSlide is QuizQuestion;
        answersHolder.SetActive(isQuiz);
        prevButton.SetActive(currentSlideIndex != 0);
        nextButton.SetActive(!isQuiz && currentSlideIndex != (slides.Count-1));
        hintText.SetActive(false);

        // Force update (fixes a bug with Vertical Layout)
        LayoutRebuilder.ForceRebuildLayoutImmediate(topPanel.GetComponent<RectTransform>());

        if (currentARPrefab)
        {
            Destroy(currentARPrefab);
        }

        if (currentSlide.arPrefab != null)
        {
            currentARPrefab = Instantiate(currentSlide.arPrefab);
            currentARPrefab.transform.SetParent(arAnchor, true);

            currentARPrefab.transform.localPosition = new Vector3(-0.1f, 0.25f, 0);
            currentARPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
            currentARPrefab.transform.localScale = Vector3.one * 0.15f;
        }

        // If question then we may update answers based on how many there are
        if (isQuiz)
        {
            var currentQuestion = currentSlide as QuizQuestion;

            hintText.GetComponent<TextMeshProUGUI>().SetText(currentQuestion.hintText);

            answers[0].GetComponentInChildren<TextMeshProUGUI>().SetText(currentQuestion.answers[0]);
            answers[1].GetComponentInChildren<TextMeshProUGUI>().SetText(currentQuestion.answers[1]);
            if (currentQuestion.answers.Length > 2)
            {
                answers[2].SetActive(true);
                answers[2].GetComponentInChildren<TextMeshProUGUI>().SetText(currentQuestion.answers[2]);
            } else
            {
                answers[2].SetActive(false);
            }
            
            if (currentQuestion.answers.Length > 3)
            {
                answers[3].SetActive(true);
                answers[3].GetComponentInChildren<TextMeshProUGUI>().SetText(currentQuestion.answers[3]);
            } else
            {
                answers[3].SetActive(false);
            }
        }
    }
}
