using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinigameBaralho : MonoBehaviour
{
    [Header("Potes")]
    [SerializeField] private GameObject[] potes; // 3 potes no total
    [SerializeField] private float duracaoEmbaralhar = 1f; // duração de cada troca
    [SerializeField] private int quantidadeTrocas = 5; // quantas vezes embaralha

    [Header("Cartas (filhos dos potes)")]
    [SerializeField] private GameObject cartaCopa; // a carta correta (copa)

    private int indexCartaCorreta;
    private bool podeClicar = false;

    void Start()
    {
        // Começa com a carta correta no pote 0
        indexCartaCorreta = 0;
        StartCoroutine(RotinaMiniGame());
    }

    IEnumerator RotinaMiniGame()
    {
        // Mostra as cartas por 2 segundos antes de esconder
        yield return new WaitForSeconds(2f);

        // Esconde as cartas (potes tampam)
        EsconderCartas();

        yield return new WaitForSeconds(0.5f);

        // Embaralha
        yield return StartCoroutine(Embaralhar());

        // Libera o clique
        podeClicar = true;
    }

    void EsconderCartas()
    {
        cartaCopa.SetActive(false);
        // Esconde as outras cartas também (filhos dos outros potes)
        foreach (var pote in potes)
        {
            var carta = pote.transform.GetChild(0).gameObject;
            carta.SetActive(false);
        }
    }

    IEnumerator Embaralhar()
    {
        for (int i = 0; i < quantidadeTrocas; i++)
        {
            // Escolhe dois potes aleatórios diferentes para trocar
            int a = Random.Range(0, potes.Length);
            int b;
            do { b = Random.Range(0, potes.Length); } while (b == a);

            yield return StartCoroutine(TrocarPotes(a, b));

            // Atualiza qual pote tem a carta correta
            if (indexCartaCorreta == a) indexCartaCorreta = b;
            else if (indexCartaCorreta == b) indexCartaCorreta = a;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TrocarPotes(int a, int b)
    {
        Vector3 posA = potes[a].transform.position;
        Vector3 posB = potes[b].transform.position;
        Vector3 meio = (posA + posB) / 2f + Vector3.up * 1.5f;

        float tempo = 0f;
        while (tempo < duracaoEmbaralhar)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracaoEmbaralhar;

            // Pote A faz um arco por cima
            potes[a].transform.position = QuadraticBezier(posA, meio, posB, t);
            // Pote B faz o caminho inverso
            potes[b].transform.position = QuadraticBezier(posB, meio, posA, t);

            yield return null;
        }

        // Garante posição final exata
        potes[a].transform.position = posB;
        potes[b].transform.position = posA;

        // Troca as referências no array
        GameObject temp = potes[a];
        potes[a] = potes[b];
        potes[b] = temp;
    }

    Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

    public void ClicarPote(int indexPote)
    {
        if (!podeClicar) return;
        podeClicar = false;

        if (indexPote == indexCartaCorreta)
        {
            Debug.Log("Acertou!");
            FindObjectOfType<TimerManager>().PararTimer();
            GameManager.Instance.Acertou();
        }
        else
        {
            Debug.Log("Errou!");
            FindObjectOfType<TimerManager>().PararTimer();
            GameManager.Instance.Errou();
        }
    }
}