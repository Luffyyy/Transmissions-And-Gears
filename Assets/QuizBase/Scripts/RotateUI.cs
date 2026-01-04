using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.Audio;
public class RotateUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 100f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.05f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.8f;
    public ParticleSystem uiParticles;
    public TMP_Text uiText;
    public AudioSource audioSource;
    Outline outline;
    Color baseColor;
    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
        outline = GetComponent<Outline>();
        baseColor = outline.effectColor;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * scale;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        outline.effectColor = new Color(
            baseColor.r,
            baseColor.g,
            baseColor.b,
            alpha);
    }
    public void PlayParticles()
    {
        uiParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        uiParticles.gameObject.SetActive(true);
        audioSource.Play();
        uiParticles.Play();
    }
    public void StopParticles()
    {
        uiParticles.Stop();
        uiParticles.gameObject.SetActive(false);
    }
    public void ShowText()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(true); // show
            uiText.alpha = 1f; // reset alpha if using fade
        }
    }
    public void HideText()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false); // hide
        }
    }
}
