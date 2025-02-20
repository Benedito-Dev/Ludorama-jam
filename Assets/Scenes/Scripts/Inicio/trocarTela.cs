using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trocarTela : MonoBehaviour
{
    public Transform cameraTarget; // Este campo deve aparecer no Inspector
    public float speed = 5f; // Velocidade de movimento da câmera

    private bool moverCamera = false; // Inicialize como false

    void Update() // Corrigido para "Update" (com U maiúsculo)
    {
        if (moverCamera)
        {
            // Move a câmera suavemente para o alvo
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                cameraTarget.position,
                speed * Time.deltaTime // Corrigido para usar "speed"
            );

            // Se a câmera estiver próxima o suficiente do alvo, pare de mover
            if (Vector3.Distance(Camera.main.transform.position, cameraTarget.position) < 0.1f)
            {
                moverCamera = false;
            }
        }
    }

    void OnMouseDown()
    {
        // Quando o jogador clicar na seta, ative o movimento da câmera
        moverCamera = true;
    }
}