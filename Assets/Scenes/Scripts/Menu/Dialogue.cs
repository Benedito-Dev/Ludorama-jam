using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para gerenciamento de cenas
using UnityEngine.UI; // Para componentes de UI padrão
using TMPro; // Para TextMeshPro

public class Dialogue : MonoBehaviour
{
    public GameObject Guia;
    public GameObject objetoComAlpha; // Objeto que terá o alpha controlado (dentro de um Canvas)
    public GameObject TitleBox; // Caixa de título que também terá o alpha controlado
    public Vector3 posicaoFinal;
    public float velocidade = 2.0f;
    public float velocidadeFadeInObjeto = 1.0f; // Velocidade do fade-in do objetoComAlpha
    public float velocidadeFadeInTexto = 0.5f; // Velocidade do fade-in do TitleBox (mais lento)
    public float velocidadeFadeOut = 2.0f; // Velocidade do fade-out (ajuste no Inspector)

    // Variáveis para controle de diálogo
    public TextMeshProUGUI falaPersonagem; // Referência ao componente TextMeshPro da fala do personagem
    public string[] frasesIniciais; // Frases iniciais obrigatórias (configure no Inspector)
    public float velocidadeDigitacao = 0.05f; // Velocidade da digitação (ajuste no Inspector)
    public string nomeDaProximaCena; // Nome da próxima cena a ser carregada (configure no Inspector)

    private int indiceFrase = 0; // Índice da frase atual
    private bool dialogoAtivo = false; // Controla se o diálogo está ativo
    private bool digitando = false; // Controla se o texto está sendo digitado

    private Vector3 posicaoInicial;
    private float tempo;
    private bool iniciarMovimento = false; // Controla se o movimento da Guia deve começar
    private bool iniciarFadeIn = false; // Controla se o fade-in dos objetos deve começar
    private CanvasGroup canvasGroupObjeto; // Referência ao CanvasGroup do objetoComAlpha
    private Image imagemObjeto; // Referência ao componente Image do objetoComAlpha
    private TMP_Text textMeshProObjeto; // Referência ao componente TMP_Text do objetoComAlpha
    private CanvasGroup canvasGroupTitleBox; // Referência ao CanvasGroup do TitleBox
    private Image imagemTitleBox; // Referência ao componente Image do TitleBox
    private TMP_Text textMeshProTitleBox; // Referência ao componente TMP_Text do TitleBox
    private CanvasGroup canvasGroupFala; // Referência ao CanvasGroup da fala do personagem

    void Start()
    {
        posicaoInicial = Guia.transform.position;
        tempo = 0f;
        Guia.SetActive(false); // Garante que a Guia comece invisível

        // Configura o alpha inicial do objetoComAlpha
        if (objetoComAlpha != null)
        {
            canvasGroupObjeto = objetoComAlpha.GetComponent<CanvasGroup>();
            imagemObjeto = objetoComAlpha.GetComponent<Image>();
            textMeshProObjeto = objetoComAlpha.GetComponent<TMP_Text>();

            // Define o alpha inicial como 0 (totalmente transparente)
            if (canvasGroupObjeto != null)
            {
                canvasGroupObjeto.alpha = 0;
                Debug.Log("CanvasGroup do objetoComAlpha configurado com alpha = 0");
            }
            else if (imagemObjeto != null)
            {
                imagemObjeto.color = new Color(imagemObjeto.color.r, imagemObjeto.color.g, imagemObjeto.color.b, 0);
                Debug.Log("Image do objetoComAlpha configurado com alpha = 0");
            }
            else if (textMeshProObjeto != null)
            {
                textMeshProObjeto.color = new Color(textMeshProObjeto.color.r, textMeshProObjeto.color.g, textMeshProObjeto.color.b, 0);
                Debug.Log("TextMeshPro do objetoComAlpha configurado com alpha = 0");
            }
        }
        else
        {
            Debug.LogError("objetoComAlpha não está atribuído no Inspector!");
        }

        // Configura o alpha inicial do TitleBox
        if (TitleBox != null)
        {
            canvasGroupTitleBox = TitleBox.GetComponent<CanvasGroup>();
            imagemTitleBox = TitleBox.GetComponent<Image>();
            textMeshProTitleBox = TitleBox.GetComponent<TMP_Text>();

            // Define o alpha inicial como 0 (totalmente transparente)
            if (canvasGroupTitleBox != null)
            {
                canvasGroupTitleBox.alpha = 0;
                Debug.Log("CanvasGroup do TitleBox configurado com alpha = 0");
            }
            else if (imagemTitleBox != null)
            {
                imagemTitleBox.color = new Color(imagemTitleBox.color.r, imagemTitleBox.color.g, imagemTitleBox.color.b, 0);
                Debug.Log("Image do TitleBox configurado com alpha = 0");
            }
            else if (textMeshProTitleBox != null)
            {
                textMeshProTitleBox.color = new Color(textMeshProTitleBox.color.r, textMeshProTitleBox.color.g, textMeshProTitleBox.color.b, 0);
                Debug.Log("TextMeshPro do TitleBox configurado com alpha = 0");
            }
        }
        else
        {
            Debug.LogError("TitleBox não está atribuído no Inspector!");
        }

        // Configura o alpha inicial da fala do personagem
        if (falaPersonagem != null)
        {
            canvasGroupFala = falaPersonagem.GetComponent<CanvasGroup>();
            if (canvasGroupFala == null)
            {
                canvasGroupFala = falaPersonagem.gameObject.AddComponent<CanvasGroup>(); // Adiciona CanvasGroup se não existir
            }
            canvasGroupFala.alpha = 0; // Define o alpha inicial como 0
            Debug.Log("CanvasGroup da fala do personagem configurado com alpha = 0");
        }
        else
        {
            Debug.LogError("falaPersonagem não está atribuído no Inspector!");
        }

        // Adiciona a frase inicial "Olá Usuário" ao início da lista de frases
        if (frasesIniciais != null && frasesIniciais.Length > 0)
        {
            List<string> frases = new List<string>(frasesIniciais);
            frases.Insert(0, "Olá Usuário"); // Adiciona a frase inicial
            frasesIniciais = frases.ToArray(); // Atualiza a lista de frases
        }
    }

    void Update()
    {
        // Controla o movimento da Guia
        if (iniciarMovimento)
        {
            tempo += Time.deltaTime * velocidade;
            Guia.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, tempo);

            // Se a Guia chegou à posição final, inicia o fade-in dos objetos
            if (tempo >= 1.0f)
            {
                iniciarMovimento = false;
                iniciarFadeIn = true; // Inicia o fade-in dos objetos
                dialogoAtivo = true; // Ativa o diálogo
                Debug.Log("Fade-in dos objetos iniciado");
                IniciarDialogo(); // Inicia o diálogo
            }
        }

        // Controla o fade-in dos objetos
        if (iniciarFadeIn)
        {
            // Fade-in do objetoComAlpha
            if (canvasGroupObjeto != null)
            {
                canvasGroupObjeto.alpha += Time.deltaTime * velocidadeFadeInObjeto;
                canvasGroupObjeto.alpha = Mathf.Clamp01(canvasGroupObjeto.alpha);
            }
            else if (imagemObjeto != null)
            {
                Color cor = imagemObjeto.color;
                cor.a += Time.deltaTime * velocidadeFadeInObjeto;
                cor.a = Mathf.Clamp01(cor.a);
                imagemObjeto.color = cor;
            }
            else if (textMeshProObjeto != null)
            {
                Color cor = textMeshProObjeto.color;
                cor.a += Time.deltaTime * velocidadeFadeInObjeto;
                cor.a = Mathf.Clamp01(cor.a);
                textMeshProObjeto.color = cor;
            }

            // Fade-in do TitleBox (mais lento)
            if (canvasGroupTitleBox != null)
            {
                canvasGroupTitleBox.alpha += Time.deltaTime * velocidadeFadeInTexto;
                canvasGroupTitleBox.alpha = Mathf.Clamp01(canvasGroupTitleBox.alpha);
            }
            else if (imagemTitleBox != null)
            {
                Color cor = imagemTitleBox.color;
                cor.a += Time.deltaTime * velocidadeFadeInTexto;
                cor.a = Mathf.Clamp01(cor.a);
                imagemTitleBox.color = cor;
            }
            else if (textMeshProTitleBox != null)
            {
                Color cor = textMeshProTitleBox.color;
                cor.a += Time.deltaTime * velocidadeFadeInTexto;
                cor.a = Mathf.Clamp01(cor.a);
                textMeshProTitleBox.color = cor;
            }

            // Fade-in da fala do personagem
            if (canvasGroupFala != null)
            {
                canvasGroupFala.alpha += Time.deltaTime * velocidadeFadeInTexto;
                canvasGroupFala.alpha = Mathf.Clamp01(canvasGroupFala.alpha);
            }

            // Verifica se todos os objetos atingiram alpha 1
            if ((canvasGroupObjeto != null && canvasGroupObjeto.alpha >= 1.0f) ||
                (imagemObjeto != null && imagemObjeto.color.a >= 1.0f) ||
                (textMeshProObjeto != null && textMeshProObjeto.color.a >= 1.0f) ||
                (canvasGroupTitleBox != null && canvasGroupTitleBox.alpha >= 1.0f) ||
                (imagemTitleBox != null && imagemTitleBox.color.a >= 1.0f) ||
                (textMeshProTitleBox != null && textMeshProTitleBox.color.a >= 1.0f) ||
                (canvasGroupFala != null && canvasGroupFala.alpha >= 1.0f))
            {
                iniciarFadeIn = false; // Desativa o fade-in
                Debug.Log("Fade-in dos objetos concluído");
            }
        }

        // Avança para a próxima frase ao clicar
        if (dialogoAtivo && Input.GetMouseButtonDown(0)) // Verifica se o botão esquerdo do mouse foi clicado
        {
            if (digitando)
            {
                // Se o texto ainda estiver sendo digitado, completa imediatamente
                StopAllCoroutines();
                falaPersonagem.text = frasesIniciais[indiceFrase]; // Exibe o texto completo
                digitando = false;
            }
            else
            {
                if (indiceFrase < frasesIniciais.Length - 1)
                {
                    // Exibe a próxima frase inicial
                    indiceFrase++;
                    StartCoroutine(DigitarTexto(frasesIniciais[indiceFrase])); // Inicia o efeito de digitação
                }
                else
                {
                    // Desativa o diálogo quando todas as frases forem exibidas
                    dialogoAtivo = false;
                    // Inicia o fade-out antes de trocar de cena
                    StartCoroutine(FadeOutECarregaCena());
                }
            }
        }
    }

    // Método público para iniciar o movimento da Guia
    public void IniciarDialogo()
    {
        Guia.SetActive(true); // Torna a Guia visível
        iniciarMovimento = true; // Inicia o movimento
        Debug.Log("Movimento da Guia iniciado");
    }

    // Corrotina para o efeito de digitação
    IEnumerator DigitarTexto(string frase)
    {
        digitando = true;
        falaPersonagem.text = ""; // Limpa o texto atual
        foreach (char letra in frase.ToCharArray()) // Itera sobre cada caractere da frase
        {
            falaPersonagem.text += letra; // Adiciona o caractere ao texto
            yield return new WaitForSeconds(velocidadeDigitacao); // Aguarda um pequeno intervalo
        }
        digitando = false;
    }

    // Corrotina para o efeito de fade-out e carregamento da cena
    IEnumerator FadeOutECarregaCena()
    {
        // Fade-out de todos os objetos
        while ((canvasGroupObjeto != null && canvasGroupObjeto.alpha > 0) ||
               (imagemObjeto != null && imagemObjeto.color.a > 0) ||
               (textMeshProObjeto != null && textMeshProObjeto.color.a > 0) ||
               (canvasGroupTitleBox != null && canvasGroupTitleBox.alpha > 0) ||
               (imagemTitleBox != null && imagemTitleBox.color.a > 0) ||
               (textMeshProTitleBox != null && textMeshProTitleBox.color.a > 0) ||
               (canvasGroupFala != null && canvasGroupFala.alpha > 0))
        {
            // Fade-out do objetoComAlpha
            if (canvasGroupObjeto != null)
            {
                canvasGroupObjeto.alpha -= Time.deltaTime * velocidadeFadeOut;
                canvasGroupObjeto.alpha = Mathf.Clamp01(canvasGroupObjeto.alpha);
            }
            else if (imagemObjeto != null)
            {
                Color cor = imagemObjeto.color;
                cor.a -= Time.deltaTime * velocidadeFadeOut;
                cor.a = Mathf.Clamp01(cor.a);
                imagemObjeto.color = cor;
            }
            else if (textMeshProObjeto != null)
            {
                Color cor = textMeshProObjeto.color;
                cor.a -= Time.deltaTime * velocidadeFadeOut;
                cor.a = Mathf.Clamp01(cor.a);
                textMeshProObjeto.color = cor;
            }

            // Fade-out do TitleBox
            if (canvasGroupTitleBox != null)
            {
                canvasGroupTitleBox.alpha -= Time.deltaTime * velocidadeFadeOut;
                canvasGroupTitleBox.alpha = Mathf.Clamp01(canvasGroupTitleBox.alpha);
            }
            else if (imagemTitleBox != null)
            {
                Color cor = imagemTitleBox.color;
                cor.a -= Time.deltaTime * velocidadeFadeOut;
                cor.a = Mathf.Clamp01(cor.a);
                imagemTitleBox.color = cor;
            }
            else if (textMeshProTitleBox != null)
            {
                Color cor = textMeshProTitleBox.color;
                cor.a -= Time.deltaTime * velocidadeFadeOut;
                cor.a = Mathf.Clamp01(cor.a);
                textMeshProTitleBox.color = cor;
            }

            // Fade-out da fala do personagem
            if (canvasGroupFala != null)
            {
                canvasGroupFala.alpha -= Time.deltaTime * velocidadeFadeOut;
                canvasGroupFala.alpha = Mathf.Clamp01(canvasGroupFala.alpha);
            }

            yield return null; // Aguarda o próximo frame
        }

        Debug.Log("Fade-out concluído. Carregando próxima cena...");

        // Carrega a próxima cena
        if (!string.IsNullOrEmpty(nomeDaProximaCena))
        {
            SceneManager.LoadScene(nomeDaProximaCena); // Troca para a próxima cena
        }
        else
        {
            Debug.LogError("Nome da próxima cena não está definido no Inspector!");
        }
    }
}