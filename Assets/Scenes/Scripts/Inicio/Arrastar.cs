using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrastar : MonoBehaviour
{
    private Vector3 offset;
    private float zDistance;

    // Definir o cursor de pegada e o cursor normal
    public Texture2D grabCursor;  // A imagem do cursor de pegada
    public Texture2D defaultCursor; // O cursor normal

    void OnMouseDown()
    {
        // Calcular a distância e o deslocamento para o movimento do objeto
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPosition();

        // Mudar para o cursor de pegada com ponto de ancoragem centralizado
        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.ForceSoftware);
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
}

