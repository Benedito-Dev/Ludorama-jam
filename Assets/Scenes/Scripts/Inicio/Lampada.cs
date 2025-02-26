using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lampada : MonoBehaviour
{
    public GameObject lampada; // Referência ao GameObject da lâmpada (arraste no Inspector)
    public Dialogo dialogoScript; // Referência ao script de diálogo (arraste no Inspector)

    void OnMouseDown()
    {
        // Oculta a lâmpada ao ser clicada
        if (lampada != null)
        {
            lampada.SetActive(false); // Desativa o GameObject da lâmpada
        }

        // Notifica o script de diálogo para exibir o balão de diálogo
        if (dialogoScript != null)
        {
            dialogoScript.IniciarDialogo(); // Chama o método para iniciar o diálogo
        }
    }
}