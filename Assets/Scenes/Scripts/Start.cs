using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public SpriteRenderer textSprite; // Arraste o SpriteRenderer do texto no Inspector
    public string sceneToLoad = ""; // Substitua pelo nome da cena a carregar
    public float blinkDuration = 1f; // Tempo total do efeito de piscar
    public float blinkInterval = 0.1f; // Velocidade do piscar
    public AudioSource audioSource; // Componente de som
    public AudioClip startSound; // Som ao pressionar enter

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter para iniciar
        {
            StartCoroutine(BlinkText());
        }
    }

    IEnumerator BlinkText()
    {
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound); // Toca o som uma vez
        }

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < blinkDuration)
        {
            isVisible = !isVisible;
            textSprite.enabled = isVisible;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        textSprite.enabled = true; // Garante que fique visível antes de trocar de cena
        SceneManager.LoadScene(sceneToLoad);
    }
}

