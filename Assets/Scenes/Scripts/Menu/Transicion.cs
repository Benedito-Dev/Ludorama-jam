using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transicion : MonoBehaviour
{
    public GameObject objeto1;
    public GameObject objeto2;
    public GameObject objeto3;
    public AudioClip somClip; // Referência ao clip de áudio que será tocado
    public AudioClip musicaFundo; // Referência ao clip de música de fundo
    public float velocidade = 1.0f; // Velocidade da transição
    public float intervaloPiscar = 0.1f; // Intervalo de tempo entre cada piscada
    public float velocidadeFadeOutMusica = 0.5f; // Velocidade do fade-out da música de fundo
    public Dialogue dialogue; // Referência ao Dialogue

    private Material material1;
    private Material material2;
    private Material material3;
    private float alpha = 1.0f;
    private bool iniciarTransicao = false; // Controla se a transição começa
    private AudioSource audioSource; // Referência ao componente AudioSource do som ao pressionar Enter
    private AudioSource musicaSource; // Referência ao componente AudioSource da música de fundo
    private bool fadeOutMusica = false; // Controla se o fade-out da música deve ser iniciado

    void Start()
    {
        // Obtém os materiais dos objetos
        material1 = objeto1.GetComponent<SpriteRenderer>().material;
        material2 = objeto2.GetComponent<SpriteRenderer>().material;
        material3 = objeto3.GetComponent<SpriteRenderer>().material;

        // Adiciona um componente AudioSource ao objeto para o som ao pressionar Enter
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = somClip;

        // Adiciona um segundo componente AudioSource para a música de fundo
        musicaSource = gameObject.AddComponent<AudioSource>();
        musicaSource.clip = musicaFundo;
        musicaSource.loop = true; // Configura a música para tocar em loop
        musicaSource.Play(); // Inicia a música de fundo
    }

    void Update()
    {
        // Verifica se Enter foi pressionado e inicia o piscar, a transição e o fade-out da música
        if (Input.GetKeyDown(KeyCode.Return) && !iniciarTransicao)
        {
            iniciarTransicao = true;
            fadeOutMusica = true; // Inicia o fade-out da música de fundo
            StartCoroutine(PiscarObjeto3()); // Inicia a corrotina para piscar o objeto 3
            audioSource.Play(); // Toca o som ao pressionar Enter
        }

        // Se a transição estiver ativa, reduz o alpha dos objetos
        if (iniciarTransicao && alpha > 0)
        {
            alpha -= Time.deltaTime * velocidade;
            alpha = Mathf.Clamp01(alpha); // Garante que alpha não fique menor que 0

            Color cor1 = material1.color;
            Color cor2 = material2.color;
            Color cor3 = material3.color;

            material1.color = new Color(cor1.r, cor1.g, cor1.b, alpha);
            material2.color = new Color(cor2.r, cor2.g, cor2.b, alpha);
            material3.color = new Color(cor3.r, cor3.g, cor3.b, alpha);

            // Verifica se a transição terminou (alpha chegou a 0)
            if (alpha <= 0)
            {
                StartCoroutine(NotificarDialogueComDelay()); // Notifica o Dialogue com um delay
            }
        }

        // Se o fade-out da música estiver ativo, reduz o volume gradualmente
        if (fadeOutMusica && musicaSource.volume > 0)
        {
            musicaSource.volume -= Time.deltaTime * velocidadeFadeOutMusica;
            musicaSource.volume = Mathf.Clamp01(musicaSource.volume); // Garante que o volume não fique menor que 0
        }
        else if (fadeOutMusica && musicaSource.volume <= 0)
        {
            musicaSource.Stop(); // Para a música quando o volume chegar a 0
        }
    }

    // Corrotina para piscar o objeto 3 indefinidamente e sumir gradualmente
    IEnumerator PiscarObjeto3()
    {
        bool visivel = true;

        while (alpha > 0) // Continua piscando até o objeto sumir completamente
        {
            // Alterna a visibilidade do objeto 3
            objeto3.SetActive(visivel);
            visivel = !visivel;

            // Espera um pequeno intervalo antes de alternar novamente
            yield return new WaitForSeconds(intervaloPiscar);
        }

        // Desativa o objeto 3 ao final (opcional, para garantir que ele desapareça)
        objeto3.SetActive(false);
    }

    // Método para notificar o Dialogue que a transição terminou, com um delay de 0.5 segundos
    private IEnumerator NotificarDialogueComDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay de 0.5 segundos
        if (dialogue != null)
        {
            dialogue.IniciarDialogo();
        }
    }
}