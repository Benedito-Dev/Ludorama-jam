using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFixo : MonoBehaviour
{
    private Rigidbody2D rb;
    private FixedJoint2D joint;
    private bool isSnapped = false; // Indica se o objeto est� encaixado

    public char letter;      // A letra que ser� atribu�da ao card fixo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obt�m o Rigidbody2D do objeto fixo
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o card que entra na zona de snap � um card m�vel e se ele pode ser encaixado
        if (other.CompareTag("Card") && !isSnapped)
        {
            Card card = other.GetComponent<Card>(); // Acessa o script do card m�vel

            if (card != null)
            {
                // Verifica se as letras coincidem, considerando mai�sculas/min�sculas
                Debug.Log($"Letras comparadas: {card.letter} vs {letter}");
                if (char.ToLower(card.letter) == char.ToLower(letter))
                {
                    Debug.Log("Correto");
                   // SnapCard(other.gameObject); // Conecta o card m�vel ao card fixo
                }
                else
                {
                    Debug.Log("As letras n�o coincidem. N�o � poss�vel encaixar.");
                }
            }
        }
    }

    void SnapCard(GameObject card)
    {
        joint = card.AddComponent<FixedJoint2D>(); // Adiciona o FixedJoint2D no card
        joint.connectedBody = rb; // Conecta o card fixo ao card m�vel
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = Vector2.zero;

        isSnapped = true; // Marca o card fixo como encaixado
        Debug.Log("Card encaixado: " + card.name);
    }

    // Fun��o para liberar o card somente se as letras n�o coincidirem
    void ReleaseCard(GameObject card)
    {
        // Caso o card j� tenha sido encaixado, verifique as letras antes de soltar
        Card cardScript = card.GetComponent<Card>();
        if (cardScript != null)
        {
            if (char.ToLower(cardScript.letter) != char.ToLower(letter))
            {
                Debug.Log("Letras n�o coincidem, n�o h� necessidade de soltar.");
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
