using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necessário para usar TextMeshPro

public class Dialogo : MonoBehaviour
{
    public GameObject balaoDialogo; // Referência ao GameObject do balão de diálogo (Dialogue)
    public TextMeshProUGUI nomePersonagem; // Referência ao componente TextMeshPro do nome do personagem (Name)
    public TextMeshProUGUI falaPersonagem; // Referência ao componente TextMeshPro da fala do personagem (Speach)
    public string[] frasesIniciais; // Frases iniciais obrigatórias (configure no Inspector)
    public string palavraSecreta; // Palavra secreta do jogo da forca (configure no Inspector)
    public float velocidadeDigitacao = 0.05f; // Velocidade da digitação (ajuste no Inspector)
    private int indiceFrase = 0; // Índice da frase atual
    private bool dialogoAtivo = false; // Controla se o diálogo está ativo
    private bool digitando = false; // Controla se o texto está sendo digitado
    private int indiceLetra = 0; // Índice da letra atual da palavra secreta

    void Start()
    {
        // Garante que o balão de diálogo está desativado no início
        if (balaoDialogo != null)
        {
            balaoDialogo.SetActive(false);
        }
    }

    // Método público para iniciar o diálogo
    public void IniciarDialogo()
    {
        // Ativa o balão de diálogo e exibe a primeira frase inicial
        balaoDialogo.SetActive(true);
        dialogoAtivo = true;
        indiceFrase = 0; // Reseta o índice para a primeira frase
        StartCoroutine(DigitarTexto(frasesIniciais[indiceFrase])); // Inicia o efeito de digitação
    }

    void OnMouseDown()
    {
        // Avança para a próxima frase ou exibe dicas sobre a palavra secreta
        if (dialogoAtivo)
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
                    // Exibe dicas sobre a palavra secreta
                    if (indiceLetra < palavraSecreta.Length)
                    {
                        string dica = $"A próxima letra da palavra é: {palavraSecreta[indiceLetra]}";
                        StartCoroutine(DigitarTexto(dica)); // Inicia o efeito de digitação
                        indiceLetra++; // Avança para a próxima letra
                    }
                    else
                    {
                        // Desativa o balão de diálogo quando todas as letras forem reveladas
                        balaoDialogo.SetActive(false);
                        dialogoAtivo = false;
                    }
                }
            }
        }
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
}