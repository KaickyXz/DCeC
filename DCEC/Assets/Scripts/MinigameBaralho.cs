using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinigameBaralho : MonoBehaviour
{
    [Header("Potes")]
    [SerializeField] private GameObject[] potes;
    [SerializeField] private float duracaoEmbaralhar = 0.4f;
    [SerializeField] private int quantidadeTrocas = 5;

    [Header("Cartas")]
    [SerializeField] private GameObject cartaCopa;
    [SerializeField] private GameObject[] cartasErradas; // arrasta as 2 cartas erradas aqui

    private int indexCartaCorreta;
    private bool podeClicar = false;

    void Start()
    {
        indexCartaCorreta = 0;
        StartCoroutine(RotinaMiniGame());
    }

    IEnumerator RotinaMiniGame()
    {
        // Mostra todas as cartas por 3 segundos
        yield return new WaitForSeconds(1f);

        // Pisca a carta correta para chamar atenção
        yield return StartCoroutine(PiscarCarta());

        // Esconde tudo e embaralha
        EsconderCartas();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Embaralhar());
        podeClicar = true;
    }

    IEnumerator PiscarCarta()
    {
        SpriteRenderer sr = cartaCopa.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void EsconderCartas()
    {
        cartaCopa.SetActive(false);
        foreach (var carta in cartasErradas)
        {
            carta.SetActive(false);
        }
    }

    IEnumerator Embaralhar()
    {
        for (int i = 0; i < quantidadeTrocas; i++)
        {
            int a = Random.Range(0, potes.Length);
            int b;
            do { b = Random.Range(0, potes.Length); } while (b == a);

            yield return StartCoroutine(TrocarPotes(a, b));

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

            potes[a].transform.position = QuadraticBezier(posA, meio, posB, t);
            potes[b].transform.position = QuadraticBezier(posB, meio, posA, t);

            yield return null;
        }

        potes[a].transform.position = posB;
        potes[b].transform.position = posA;

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

        TimerManager timer = FindAnyObjectByType<TimerManager>();
        if (timer != null) timer.PararTimer();

        if (indexPote == indexCartaCorreta)
        {
            GameManager.Instance.Acertou();
        }
        else
        {
            GameManager.Instance.Errou();
        }
    }
}