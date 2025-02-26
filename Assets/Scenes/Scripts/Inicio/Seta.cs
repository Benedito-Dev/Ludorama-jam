using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seta : MonoBehaviour
{
    public trocarTela controlador; // Arraste o objeto que contém o script "trocarTela" no Inspector
    public enum DirecaoSeta { Esquerda, Direita } // Enum para definir a direção da seta
    public DirecaoSeta direcao; // Define a direção da seta no Inspector

    void OnMouseDown()
    {
        Debug.Log("Seta clicada: " + direcao);

        if (controlador != null)
        {
            if (direcao == DirecaoSeta.Esquerda)
            {
                controlador.MoverCamera(-1); // Move a câmera para a esquerda
            }
            else if (direcao == DirecaoSeta.Direita)
            {
                controlador.MoverCamera(1); // Move a câmera para a direita
            }
        }
    }
}