using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Vector3 offset;
    private float zDistance;

    // Definir o cursor de pegada e o cursor normal
    public Texture2D grabCursor;  // A imagem do cursor de pegada
    public Texture2D defaultCursor; // O cursor normal

    private Rigidbody2D rb;
    private FixedJoint2D joint;
    private bool isSnapped = false; // Indica se o objeto está encaixado
    private bool canSnap = true; // Controle para evitar reencaixe imediato

    public char letter;      // A letra que será atribuída ao card
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do objeto arrastável
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snapzone") && !isSnapped && canSnap) // Só encaixa se puder
        {
            joint = gameObject.AddComponent<FixedJoint2D>(); // Adiciona um FixedJoint2D
            joint.connectedBody = other.GetComponent<Rigidbody2D>(); // Conecta ao Rigidbody2D do SnapZone
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = Vector2.zero;

            isSnapped = true; // Marca como encaixado
        }
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
    }

    // Função para converter a posição do mouse na tela para as coordenadas do mundo
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    void OnMouseDown()
    {
        // Calcular a distância e o deslocamento para o movimento do objeto
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition();

        // Mudar para o cursor de pegada com ponto de ancoragem centralizado
        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.ForceSoftware);
        if (isSnapped)
        {
            ReleaseObject(); // Solta o objeto se ele estiver encaixado
        }
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
        StartCoroutine(EnableSnap()); // Espera um tempo antes de permitir encaixe novamente
    }

    IEnumerator EnableSnap()
    {
        yield return new WaitForSeconds(0.5f); // Delay de 0.5 segundos antes de permitir reencaixe
        canSnap = true;
    }
}
