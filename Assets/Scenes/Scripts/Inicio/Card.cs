using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{
    private Vector3 offset;
    private float zDistance;

    public Texture2D grabCursor;  // Imagem do cursor de pegada
    public Texture2D defaultCursor; // Cursor normal

    private Rigidbody2D rb;
    private FixedJoint2D joint;
    public bool isSnapped = false; // Indica se o objeto está encaixado
    private bool canSnap = true; // Controle para evitar reencaixe imediato

    public char letter; // A letra que será atribuída ao card

    private CardFixo currentCardFixo; // Referência ao CardFixo atual
    private Collider2D cardCollider; // Collider do card móvel

    // Eventos para notificar encaixe e soltura
    public static event Action<Card> OnCardSnapped;
    public static event Action<Card> OnCardReleased;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do objeto arrastável
        cardCollider = GetComponent<Collider2D>(); // Obtém o Collider do card móvel
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snapzone") && !isSnapped && canSnap)
        {
            // Verifica se o CardFixo é o mais próximo
            CardFixo cardFixo = other.GetComponent<CardFixo>();
            if (cardFixo != null && IsClosestSnapZone(cardFixo))
            {
                currentCardFixo = cardFixo; // Armazena a referência ao CardFixo
                SnapTo(cardFixo.GetComponent<Rigidbody2D>());
            }
        }
    }

    bool IsClosestSnapZone(CardFixo targetCardFixo)
    {
        float closestDistance = float.MaxValue;
        CardFixo closestCardFixo = null;

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1f)) // Verifica colisões próximas
        {
            CardFixo cardFixo = collider.GetComponent<CardFixo>();
            if (cardFixo != null)
            {
                float distance = Vector2.Distance(transform.position, cardFixo.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCardFixo = cardFixo;
                }
            }
        }

        return closestCardFixo == targetCardFixo;
    }

    void OnMouseDrag()
    {
        // Mover o objeto conforme o mouse
        transform.position = GetMouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        // Voltar ao cursor normal quando o usuário soltar o mouse
        Cursor.SetCursor(defaultCursor, new Vector2(defaultCursor.width / 2, defaultCursor.height / 2), CursorMode.ForceSoftware);

        if (isSnapped)
        {
            ReleaseObject(); // Solta o objeto se ele estiver encaixado
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void OnMouseDown()
    {
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition();

        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.ForceSoftware);

        if (isSnapped)
        {
            // Libera o card apenas se o jogador arrastá-lo para fora da zona de encaixe
            StartCoroutine(CheckForRelease());
        }
    }

    IEnumerator CheckForRelease()
    {
        Vector3 initialPosition = transform.position;
        yield return new WaitForSeconds(0.1f); // Espera um pouco para verificar se o card foi arrastado

        if (Vector3.Distance(initialPosition, transform.position) > 0.1f) // Se o card foi movido
        {
            ReleaseObject();
        }
    }

    public void SnapTo(Rigidbody2D targetRb)
    {
        joint = gameObject.AddComponent<FixedJoint2D>(); // Adiciona um FixedJoint2D
        joint.connectedBody = targetRb; // Conecta ao Rigidbody2D do alvo
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = Vector2.zero;

        isSnapped = true; // Marca como encaixado

        // Define o card móvel como filho do card fixo
        transform.SetParent(targetRb.transform);

        Debug.Log($"Card {letter} encaixado no CardFixo. isSnapped = {isSnapped}");

        // Notifica o GameManager que o card foi encaixado
        OnCardSnapped?.Invoke(this);
    }

    void ReleaseObject()
    {
        if (joint != null)
        {
            Destroy(joint); // Remove o FixedJoint2D
            joint = null;
        }

        isSnapped = false; // Permite que o objeto seja encaixado novamente
        canSnap = false; // Bloqueia encaixe imediato

        // Desabilita a colisão temporariamente
        if (currentCardFixo != null)
        {
            Collider2D cardFixoCollider = currentCardFixo.GetComponent<Collider2D>();
            if (cardFixoCollider != null)
            {
                Physics2D.IgnoreCollision(cardCollider, cardFixoCollider, true); // Ignora colisão
            }

            currentCardFixo.ReleaseCard();
            currentCardFixo = null; // Limpa a referência
        }

        // Notifica o GameManager que o card foi solto
        OnCardReleased?.Invoke(this);

        // Reativa a colisão após um breve período
        StartCoroutine(ReenableCollision());
    }

    IEnumerator ReenableCollision()
    {
        yield return new WaitForSeconds(0.5f); // Delay de 0.5 segundos antes de reativar a colisão

        if (currentCardFixo != null)
        {
            Collider2D cardFixoCollider = currentCardFixo.GetComponent<Collider2D>();
            if (cardFixoCollider != null)
            {
                Physics2D.IgnoreCollision(cardCollider, cardFixoCollider, false); // Reativa colisão
            }
        }

        canSnap = true; // Permite encaixe novamente
    }
}