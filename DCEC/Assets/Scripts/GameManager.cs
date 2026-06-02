using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cenas")]
    public string menuScene = "Menu";
    public string gameOverScene = "GameOver";
    public string transicaoScene = "Transicao";

    private List<string> minigames = new List<string>
    {
        "CopaArvore",
        "CopaBaralho",
        "CopaTaca",
        "Copacabana"
    };

    [Header("Configurações de Pontos")]
    public int pontosAcerto = 50;
    public int pontosErro = 35;

    [Header("Configurações de Tempo")]
    public float tempoInicial = 30f;      // Tempo do primeiro minigame
    public float reducaoTempo = 2f;       // Quanto reduz a cada minigame
    public float tempoMinimo = 5f;        // Nunca vai abaixo disso

    // Estado do jogo
    public int Vidas { get; private set; } = 3;
    public int Pontos { get; private set; } = 0;
    public float TempoAtual { get; private set; }
    private string lastScene = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IniciarJogo()
    {
        Vidas = 3;
        Pontos = 0;
        TempoAtual = tempoInicial;
        AudioManager.Instance.ResetMinigameMusic();
        CarregarProximoMinigame();
    }

    public void Acertou()
    {
        Pontos += pontosAcerto;
        ReduzirTempo();
        CarregarTransicao();
    }

    public void Errou()
    {
        Pontos = Mathf.Max(0, Pontos - pontosErro);
        Vidas--;
        ReduzirTempo();

        if (Vidas <= 0)
            GameOver();
        else
            CarregarTransicao();
    }

    void ReduzirTempo()
    {
        TempoAtual = Mathf.Max(tempoMinimo, TempoAtual - reducaoTempo);
    }

    void CarregarTransicao()
    {
        SceneManager.LoadScene(transicaoScene);
    }

    public void CarregarProximoMinigame()
    {
        List<string> available = new List<string>(minigames);

        if (lastScene != "")
            available.Remove(lastScene);

        int index = Random.Range(0, available.Count);
        lastScene = available[index];

        AudioManager.Instance.PlayMinigameMusic();
        SceneManager.LoadScene(lastScene);
    }

    void GameOver()
    {
        AudioManager.Instance.PlayMenuMusic();
        SceneManager.LoadScene(gameOverScene);
    }

    public void VoltarMenu()
    {
        AudioManager.Instance.ResetMinigameMusic();
        AudioManager.Instance.PlayMenuMusic();
        Vidas = 3;
        Pontos = 0;
        TempoAtual = tempoInicial;
        SceneManager.LoadScene(menuScene);
    }
}