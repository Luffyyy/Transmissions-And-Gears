using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Drawing;

public class QuizManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<QuizSlide> slides;
    public RotateUI rotateUI;
    public int correctAnswers = 0;
    private int currentSlideIndex;
    private QuizSlide currentSlide => slides[currentSlideIndex];
    private Coroutine badgeRoutine;

    public GameObject topPanel;
    public GameObject title;
    public GameObject text;
    public GameObject hintText;
    public GameObject answersHolder;
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject restartButton;
    public GameObject image;
    public GameObject gear;

    public AudioSource audioSource;
    public AudioResource correctSound;
    public AudioResource incorrectSound;
    public Animator gearAnimator;
    public Animator gearAnimatorAlt;
    public TMP_Text uiText; 

    private GameObject[] answers = new GameObject[4];

    private enum QuestionState
    {
        UNANSWERED,
        INCORRECT,
        CORRECT
    }

    private QuestionState[] questionsState;

    private GameObject currentARPrefab;
    public Transform arAnchor;

    void Start()
    {
        GameObject.Find("Welcome")?.SetActive(false);

        answers[0] = answersHolder.GetNamedChild("Answer1");
        answers[1] = answersHolder.GetNamedChild("Answer2");
        answers[2] = answersHolder.GetNamedChild("Answer3");
        answers[3] = answersHolder.GetNamedChild("Answer4");
        gear.SetActive(false);
        if (uiText != null)
            uiText.gameObject.SetActive(false);

        questionsState = new QuestionState[slides.Count];

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

    public void RestartSlide()
    {
        currentSlideIndex = 0;
        correctAnswers = 0;
        questionsState = new QuestionState[slides.Count];

        SetcurrentSlide();
    }

    public void CheckAnswer(int answer)
    {
        QuizQuestion question = (QuizQuestion)currentSlide;
        // Check answer
        if (question.correctAnswer == answer)
        {   
            if (questionsState[currentSlideIndex] == 0)
            {
                questionsState[currentSlideIndex] = QuestionState.CORRECT;
                // Increment counter
                correctAnswers++;

                if(badgeRoutine != null)
                    StopCoroutine(badgeRoutine);
                gear.SetActive(true);
                badgeRoutine = StartCoroutine(HideBadgeAfterSeconds(4f));
                gearAnimator.Play("Badge", 0, 0f);
                gearAnimatorAlt.Play("Text (TMP)", 0, 0f);

                // Update text
                rotateUI.uiText.text = correctAnswers.ToString();
                audioSource.resource = correctSound;
                audioSource.Play();
            }

            NextSlide();
        } else
        {
            questionsState[currentSlideIndex] = QuestionState.INCORRECT;
            answers[answer].GetComponent<Animator>().Play("Incorrect"); // Show incorrect button animation
            audioSource.resource = incorrectSound;
            hintText.SetActive(true);
            audioSource.Play();
        }
    }
    private IEnumerator HideBadgeAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gear.SetActive(false);
        rotateUI.StopParticles();
        rotateUI.HideText();
    }

    /**
        Updates the quiz UI to the current page, either explanation or question
    */
    void SetcurrentSlide()
    {
        hintText.SetActive(false);
        title.GetComponent<TextMeshProUGUI>().SetText(currentSlide.title);
        image.GetComponent<Image>().sprite = currentSlide.image;
        image.SetActive(currentSlide.image != null);

        var isQuiz = currentSlide is QuizQuestion;
        var lastSlide = currentSlideIndex == (slides.Count-1);
        answersHolder.SetActive(isQuiz);
        prevButton.SetActive(currentSlideIndex != 0);
        nextButton.SetActive(!isQuiz && !lastSlide);
        restartButton.SetActive(lastSlide);
        hintText.SetActive(false);

        var textStr = currentSlide.text;

        if (lastSlide)
        {
            textStr += $"\nקיבלתם: {correctAnswers} נקודות!";
        }

        text.GetComponent<TextMeshProUGUI>().SetText(textStr);

        // Force update (fixes a bug with Vertical Layout)
        LayoutRebuilder.ForceRebuildLayoutImmediate(topPanel.GetComponent<RectTransform>());

        if (currentARPrefab)
        {
            Destroy(currentARPrefab);
        }

        if (currentSlide.arPrefab != null)
        {
            currentARPrefab = Instantiate(currentSlide.arPrefab);
            currentARPrefab.AddComponent<GearTouchControls>(); // ALlow the user to rotate it in the Y rotation axis
            currentARPrefab.transform.SetParent(arAnchor, true);

            currentARPrefab.transform.localPosition = new Vector3(-0.1f, 0.25f, 0);
            currentARPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
            currentARPrefab.transform.localScale = Vector3.one * 0.15f;
        }

        // If question then we may update answers based on how many there are
        if (isQuiz)
        {
            var currentQuestion = currentSlide as QuizQuestion;

            if (questionsState[currentSlideIndex] != QuestionState.UNANSWERED)
            {
                nextButton.SetActive(true);
            }

            hintText.GetComponent<TextMeshProUGUI>().SetText(currentQuestion.hintText);

            for (int i = 0; i < answers.Length; i++)
            {
                var answer = answers[i];
                answer.GetComponent<Button>().interactable = questionsState[currentSlideIndex] == QuestionState.UNANSWERED;

                answer.SetActive(true);                
                answer.GetComponent<Animator>().Play("Empty");
                answer.SetActive((i+1) <= currentQuestion.answers.Length);
                if (answer.activeSelf)
                {
                    answer.GetComponentInChildren<TextMeshProUGUI>().SetText(currentQuestion.answers[i]);
                }
            }
        }
    }
}
