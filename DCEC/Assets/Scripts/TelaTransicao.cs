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
            iconesVidas[i].SetActive(i < GameManager.Instance.Vidas);
        }
    }

    IEnumerator ContarPontos()
    {
        int pontosExibidos = 0;
        int pontosAlvo = GameManager.Instance.Pontos;

        while (pontosExibidos != pontosAlvo)
        {
            pontosExibidos = (int)Mathf.MoveTowards(
                pontosExibidos,
                pontosAlvo,
                velocidadeContagem * Time.deltaTime
            );
            textPontos.text = pontosExibidos.ToString();
            yield return null;
        }
    }

    IEnumerator EsperarEAvancar()
    {
        yield return new WaitForSeconds(tempoDuracao);
        GameManager.Instance.CarregarProximoMinigame();
    }
}