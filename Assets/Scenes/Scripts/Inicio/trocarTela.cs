using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class trocarTela : MonoBehaviour
{
    public float targetXPositionRight; // Posição X do alvo para a direita (aparece no Inspector)
    public float targetXPositionLeft;  // Posição X do alvo para a esquerda (aparece no Inspector)
    public float speed = 5f; // Velocidade de movimento da câmera
    public float speedSeta = 1.2f;
    public float speedLampada = 0.2f;
    public GameObject setaDireita; // Objeto público para a seta da direita (aparece no Inspector)
    public GameObject setaEsquerda; // Objeto público para a seta da esquerda (aparece no Inspector)
    public GameObject Lampada;

    private bool moverCamera = false; // Inicialize como false
    private bool voltarCamera = false; // Novo estado para voltar a câmera
    private float posicaoInicialX; // Armazena a posição X inicial da câmera
    private float setaDireitaPosicaoInicialX; // Armazena a posição X inicial da seta da direita
    private float setaEsquerdaPosicaoInicialX; // Armazena a posição X inicial da seta da esquerda
    private bool moverSetaFrente = true; // Controla a direção do movimento da seta
    private float targetXPositionAtual; // Armazena a posição X alvo atual

    private float lampadaPosicaoInicialY; // Armazena a posição Y inicial da lâmpada

    void Start()
    {
        // Armazena a posição X inicial da câmera no início do jogo
        posicaoInicialX = Camera.main.transform.position.x;

        if (Lampada != null)
        {
            lampadaPosicaoInicialY = Lampada.transform.position.y;
        }

        // Armazena a posição X inicial das setas
        if (setaDireita != null)
        {
            setaDireitaPosicaoInicialX = setaDireita.transform.position.x;
        }
        if (setaEsquerda != null)
        {
            setaEsquerdaPosicaoInicialX = setaEsquerda.transform.position.x;
        }
    }

    void Update()
    {
        // Verifica se a seta para a esquerda foi pressionada
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverCamera(-1); // Move a câmera para a esquerda
        }

        // Verifica se a seta para a direita foi pressionada
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverCamera(1); // Move a câmera para a direita
        }

        if (moverCamera)
        {
            // Move a câmera suavemente para a posição X do alvo
            Vector3 novaPosicao = Camera.main.transform.position;
            novaPosicao.x = Mathf.Lerp(novaPosicao.x, targetXPositionAtual, speed * Time.deltaTime);
            Camera.main.transform.position = novaPosicao;

            // Se a câmera estiver próxima o suficiente do alvo, pare de mover
            if (Mathf.Abs(Camera.main.transform.position.x - targetXPositionAtual) < 0.1f)
            {
                moverCamera = false;
            }
        }

        if (voltarCamera)
        {
            // Move a câmera suavemente de volta para a posição X inicial
            Vector3 novaPosicao = Camera.main.transform.position;
            novaPosicao.x = Mathf.Lerp(novaPosicao.x, posicaoInicialX, speed * Time.deltaTime);
            Camera.main.transform.position = novaPosicao;

            // Se a câmera estiver próxima o suficiente da posição X inicial, pare de mover
            if (Mathf.Abs(Camera.main.transform.position.x - posicaoInicialX) < 0.1f)
            {
                voltarCamera = false;
            }
        }

        // Movimentação das setas
        if (setaDireita != null)
        {
            MoveSeta(setaDireita, setaDireitaPosicaoInicialX);
        }
        if (setaEsquerda != null)
        {
            MoveSeta(setaEsquerda, setaEsquerdaPosicaoInicialX);
        }

        MoverLampada();
    }

    void MoveSeta(GameObject seta, float posicaoInicialX)
    {
        // Define a direção do movimento da seta
        float direcao = moverSetaFrente ? 1f : -1f;

        // Move a seta no eixo X
        Vector3 novaPosicaoSeta = seta.transform.position;
        novaPosicaoSeta.x += direcao * speedSeta * Time.deltaTime;
        seta.transform.position = novaPosicaoSeta;

        // Verifica se a seta atingiu o limite de movimento
        if (moverSetaFrente && novaPosicaoSeta.x >= posicaoInicialX + 0.2f) // Limite de movimento para frente
        {
            moverSetaFrente = false;
        }
        else if (!moverSetaFrente && novaPosicaoSeta.x <= posicaoInicialX - 0.2f) // Limite de movimento para trás
        {
            moverSetaFrente = true;
        }
    }

    void MoverLampada()
    {
        // Define a direção do movimento da lâmpada
        float direcao = moverSetaFrente ? 1f : -1f;

        // Move a lâmpada no eixo Y
        Vector3 novaPosicaoLampada = Lampada.transform.position;
        novaPosicaoLampada.y += direcao * speedLampada * Time.deltaTime;
        Lampada.transform.position = novaPosicaoLampada;

        // Verifica se a lâmpada atingiu o limite de movimento
        if (moverSetaFrente && novaPosicaoLampada.y >= lampadaPosicaoInicialY + 0.1f) // Limite de movimento para frente
        {
            moverSetaFrente = false;
        }
        else if (!moverSetaFrente && novaPosicaoLampada.y <= lampadaPosicaoInicialY - 0.1f) // Limite de movimento para trás
        {
            moverSetaFrente = true;
        }
    }

    public void MoverCamera(int direcao)
    {
        if (direcao == 1) // Direita
        {
            targetXPositionAtual = targetXPositionRight;
        }
        else if (direcao == -1) // Esquerda
        {
            targetXPositionAtual = targetXPositionLeft;
        }

        moverCamera = true;
        voltarCamera = false;
    }

    public void VoltarCamera()
    {
        moverCamera = false;
        voltarCamera = true;
    }
}