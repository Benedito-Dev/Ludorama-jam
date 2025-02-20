using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFixo : MonoBehaviour
{
    private Rigidbody2D rb;
    private FixedJoint2D joint;
    private bool isSnapped = false; // Indica se o objeto está encaixado

    public char letter;      // A letra que será atribuída ao card fixo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do objeto fixo
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o card que entra na zona de snap é um card móvel e se ele pode ser encaixado
        if (other.CompareTag("Card") && !isSnapped)
        {
            Card card = other.GetComponent<Card>(); // Acessa o script do card móvel

            if (card != null)
            {
                // Verifica se as letras coincidem, considerando maiúsculas/minúsculas
                Debug.Log($"Letras comparadas: {card.letter} vs {letter}");
                if (char.ToLower(card.letter) == char.ToLower(letter))
                {
                    Debug.Log("Correto");
                   // SnapCard(other.gameObject); // Conecta o card móvel ao card fixo
                }
                else
                {
                    Debug.Log("As letras não coincidem. Não é possível encaixar.");
                }
            }
        }
    }

    void SnapCard(GameObject card)
    {
        joint = card.AddComponent<FixedJoint2D>(); // Adiciona o FixedJoint2D no card
        joint.connectedBody = rb; // Conecta o card fixo ao card móvel
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = Vector2.zero;

        isSnapped = true; // Marca o card fixo como encaixado
        Debug.Log("Card encaixado: " + card.name);
    }

    // Função para liberar o card somente se as letras não coincidirem
    void ReleaseCard(GameObject card)
    {
        // Caso o card já tenha sido encaixado, verifique as letras antes de soltar
        Card cardScript = card.GetComponent<Card>();
        if (cardScript != null)
        {
            if (char.ToLower(cardScript.letter) != char.ToLower(letter))
            {
                Debug.Log("Letras não coincidem, não há necessidade de soltar.");
            }
            else
            {
                FixedJoint2D jointToRemove = card.GetComponent<FixedJoint2D>();
                if (jointToRemove != null)
                {
                    Destroy(jointToRemove); // Remove o FixedJoint2D do card
                    isSnapped = false; // Permite que o card seja encaixado novamente
                    Debug.Log("Card solto: " + card.name);
                }
            }
        }
    }
}
