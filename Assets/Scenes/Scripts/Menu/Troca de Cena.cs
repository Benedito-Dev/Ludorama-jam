using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocadeCena : MonoBehaviour
{
    [SerializeField] private string cena; // Nome da cena definido no Inspector

    void Update()
    {
        // Verifica se o usuário pressionou a tecla Enter (Return)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MudarParaCenaPredefinida();
        }
    }

    public void MudarParaCena(string nomeCena)
    {
        if (!string.IsNullOrEmpty(nomeCena))
        {
            SceneManager.LoadScene(nomeCena);
        }
        else
        {
            Debug.LogWarning("O nome da cena está vazio ou nulo!");
        }
    }

    public void MudarParaCenaPredefinida()
    {
        MudarParaCena(cena);
    }
}

