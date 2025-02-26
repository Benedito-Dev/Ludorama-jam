using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFixo : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isSnapped = false; // Indica se o objeto está encaixado
    public char letter; // A letra que será atribuída ao card fixo

    public GameObject checkSymbolPrefab; // Prefab do símbolo de check (✔️)
    public GameObject xSymbolPrefab;     // Prefab do símbolo de x (❌)

    private GameObject checkSymbol; // Instância do símbolo de check
    private GameObject xSymbol;     // Instância do símbolo de x

    public float yOffset = -1f; // Distância abaixo do CardFixo no eixo Y

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do objeto fixo

        // Configura os símbolos
        ConfigureSymbols();
    }

    void ConfigureSymbols()
    {
        // Instancia os símbolos como filhos do CardFixo
        checkSymbol = Instantiate(checkSymbolPrefab, transform);
        xSymbol = Instantiate(xSymbolPrefab, transform);

        // Ajusta a posição dos símbolos
        AdjustSymbolPosition(checkSymbol);
        AdjustSymbolPosition(xSymbol);

        // Oculta ambos os símbolos no início
        checkSymbol.SetActive(false);
        xSymbol.SetActive(false);
    }

    void AdjustSymbolPosition(GameObject symbol)
    {
        if (symbol != null)
        {
            // Mantém o mesmo eixo X e ajusta o eixo Y
            Vector3 newPosition = transform.position;
            newPosition.y += yOffset; // Aplica o deslocamento no eixo Y
            symbol.transform.position = newPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o card que entra na zona de snap é um card móvel e se ele pode ser encaixado
        if (other.CompareTag("Card") && !isSnapped)
        {
            Card card = other.GetComponent<Card>(); // Acessa o script do card móvel

            if (card != null && !card.isSnapped)
            {
                // Verifica se as letras coincidem, considerando maiúsculas/minúsculas
                if (char.ToLower(card.letter) == char.ToLower(letter))
                {
                    SnapCard(card); // Passa o componente Card do card móvel como argumento
                }
                else
                {
                    SnapCard(card); // Encaixa mesmo se a letra estiver incorreta
                }
            }
        }
    }

    void SnapCard(Card card)
    {
        card.SnapTo(rb); // Usa o método do card móvel para encaixar
        isSnapped = true; // Marca o card fixo como encaixado

        // Mostra o símbolo correspondente
        if (char.ToLower(card.letter) == char.ToLower(letter))
        {
            ShowSymbol(checkSymbol); // Mostra o símbolo de check (✔️)
        }
        else
        {
            ShowSymbol(xSymbol); // Mostra o símbolo de x (❌)
        }

        Debug.Log($"Card {card.letter} encaixado no CardFixo {letter}. isSnapped = {isSnapped}");
    }

    void ShowSymbol(GameObject symbol)
    {
        // Oculta ambos os símbolos antes de mostrar o correto
        checkSymbol.SetActive(false);
        xSymbol.SetActive(false);

        // Mostra o símbolo passado como parâmetro
        symbol.SetActive(true);
    }

    public void ReleaseCard()
    {
        isSnapped = false; // Marca o card fixo como liberado

        // Oculta ambos os símbolos
        checkSymbol.SetActive(false);
        xSymbol.SetActive(false);
    }
}