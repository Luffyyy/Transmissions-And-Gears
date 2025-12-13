using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public GameObject welcomePanel;

    public GameObject GreetingText;

    private int step = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseWelcomeScreen()
    {
        welcomePanel.SetActive(false);
    }
}
