using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para facilitar o acesso ao GameManager

    public List<CardFixo> cardsFixos; // Lista de todos os cards fixos na cena
    public string palavraCorreta; // A palavra correta que deve ser formada

    public GameObject Winner; // Prefab do símbolo de check (✔️)
    public GameObject Incorret;     // Prefab do símbolo de x (❌)

    private void Awake()
    {
        Winner.SetActive(false);
        Incorret.SetActive(false);

        // Configura o Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Assina os eventos do Card
        Card.OnCardSnapped += HandleCardSnapped;
        Card.OnCardReleased += HandleCardReleased;
    }

    private void OnDisable()
    {
        // Desassina os eventos do Card
        Card.OnCardSnapped -= HandleCardSnapped;
        Card.OnCardReleased -= HandleCardReleased;
    }

    private void Start()
    {
        // Inicializa a lista de cards fixos
        cardsFixos = new List<CardFixo>(FindObjectsOfType<CardFixo>());

        // Ordena os cards fixos pela posição x (assumindo que estão alinhados horizontalmente)
        cardsFixos.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        Debug.Log($"Número de cards fixos encontrados: {cardsFixos.Count}");

        // Log da ordem dos cards fixos para depuração
        foreach (CardFixo cardFixo in cardsFixos)
        {
            Debug.Log($"CardFixo {cardFixo.letter} na posição x: {cardFixo.transform.position.x}");
        }
    }

    private void HandleCardSnapped(Card card)
    {
        // Atualiza o estado do CardFixo correspondente
        CardFixo cardFixo = cardsFixos.Find(cf => cf.letter == card.letter);
        if (cardFixo != null)
        {
            cardFixo.isSnapped = true;
        }

        VerificarPreenchimento();
    }

    private void HandleCardReleased(Card card)
    {
        // Atualiza o estado do CardFixo correspondente
        CardFixo cardFixo = cardsFixos.Find(cf => cf.letter == card.letter);
        if (cardFixo != null)
        {
            cardFixo.isSnapped = false;
        }

        VerificarPreenchimento();
    }

    // Método para verificar se todos os cards fixos foram preenchidos
    public void VerificarPreenchimento()
    {
        bool todosPreenchidos = true;
        foreach (CardFixo cardFixo in cardsFixos)
        {
            if (!cardFixo.isSnapped)
            {
                todosPreenchidos = false;
                Debug.Log($"CardFixo {cardFixo.letter} não está preenchido.");
                break;
            }
        }

        if (todosPreenchidos)
        {
            Debug.Log("Todos os cards estão preenchidos.");
            FormarPalavra();
        }
        else
        {
            Debug.Log("Nem todos os cards estão preenchidos.");
        }
    }

    // Método para formar a palavra a partir das letras dos cards móveis encaixados
    private void FormarPalavra()
    {
        string palavraFormada = "";

        foreach (CardFixo cardFixo in cardsFixos)
        {
            if (cardFixo.isSnapped)
            {
                Card cardMovel = cardFixo.GetComponentInChildren<Card>();
                if (cardMovel != null)
                {
                    palavraFormada += cardMovel.letter;
                    Debug.Log($"Letra adicionada: {cardMovel.letter}");
                }
                else
                {
                    Debug.Log($"Card móvel não encontrado no CardFixo {cardFixo.letter}");
                }
            }
        }

        Debug.Log("Palavra formada: " + palavraFormada);

        // Verifica se a palavra formada está correta
        if (palavraFormada.ToLower() == palavraCorreta.ToLower())
        {
            Debug.Log("Parabéns! A palavra está correta.");
            Winner.SetActive(true);
        }
        else
        {
            Debug.Log("A palavra está incorreta. Tente novamente.");
            Incorret.SetActive(true) ;
        }
    }
}