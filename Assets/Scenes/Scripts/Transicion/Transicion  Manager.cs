using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicionManager : MonoBehaviour
{
    public GameObject Guia;
    public Vector3 posicaoFinal;
    public float velocidade = 2.0f;

    private Vector3 posicaoInicial;
    private float tempo;
    private bool iniciarMovimento = false; // Controla se o movimento da Guia deve começar

    void Start()
    {
        posicaoInicial = Guia.transform.position;
        tempo = 0f;
        Guia.SetActive(false); // Garante que a Guia comece invisível
    }

    void Update()
    {
        if (iniciarMovimento)
        {
            tempo += Time.deltaTime * velocidade;
            Guia.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, tempo);

            // Se a Guia chegou à posição final, desativa o movimento
            if (tempo >= 1.0f)
            {
                iniciarMovimento = false;
            }
        }
    }

    // Método público para iniciar o movimento da Guia
    public void IniciarMovimentoGuia()
    {
        Guia.SetActive(true); // Torna a Guia visível
        iniciarMovimento = true; // Inicia o movimento
    }
}