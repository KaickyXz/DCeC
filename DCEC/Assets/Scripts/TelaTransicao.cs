using UnityEngine;
using TMPro;
using System.Collections;

public class TelaTransicao : MonoBehaviour
{
    [Header("Pontuação")]
    [SerializeField] private TextMeshProUGUI textPontos;
    [SerializeField] private float velocidadeContagem = 50f;

    [Header("Vidas")]
    [SerializeField] private GameObject[] iconesVidas;

    [Header("Tempo da tela")]
    [SerializeField] private float tempoDuracao = 5f;

    void Start()
    {
        AtualizarVidas();
        StartCoroutine(ContarPontos());
        StartCoroutine(EsperarEAvancar());
    }

    void AtualizarVidas()
    {
        for (int i = 0; i < iconesVidas.Length; i++)
        {
            if (iconesVidas[i] != null) // Boa prática para evitar erros bobos na Jam
                iconesVidas[i].SetActive(i < GameManager.Instance.Vidas);
        }
    }

    IEnumerator ContarPontos()
    {
        float pontosExibidos = 0f;
        int pontosAlvo = GameManager.Instance.Pontos;

        // Garante que o texto exiba pelo menos "0" se a pontuação for zero
        textPontos.text = "0";

        // Se o alvo for maior que 0, faz a contagem progressiva
        while (pontosExibidos < pontosAlvo)
        {
            pontosExibidos = Mathf.MoveTowards(
                pontosExibidos,
                pontosAlvo,
                velocidadeContagem * Time.deltaTime
            );

            // Arredonda para o inteiro mais próximo apenas para exibir visualmente
            textPontos.text = Mathf.RoundToInt(pontosExibidos).ToString();
            yield return null;
        }

        // Garante que o valor final exato seja exibido no término do loop
        textPontos.text = pontosAlvo.ToString();
    }

    IEnumerator EsperarEAvancar()
    {
        yield return new WaitForSeconds(tempoDuracao);
        GameManager.Instance.CarregarProximoMinigame();
    }
}