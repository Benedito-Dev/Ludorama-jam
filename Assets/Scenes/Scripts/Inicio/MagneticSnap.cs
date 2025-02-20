using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagneticSnap : MonoBehaviour
{
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
            Debug.Log(letter);
            joint = gameObject.AddComponent<FixedJoint2D>(); // Adiciona um FixedJoint2D
            joint.connectedBody = other.GetComponent<Rigidbody2D>(); // Conecta ao Rigidbody2D do SnapZone
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = Vector2.zero;

            isSnapped = true; // Marca como encaixado
        }
    }

    void OnMouseDown()
    {
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
