using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cenas")]
    public string menuScene = "Menu";
    public string gameOverScene = "GameOver";

    private List<string> minigames = new List<string>
    {
        "CopaArvore",
        "CopaBaralho",
        "CopaTaca",
        "Copacabana"
    };

    [Header("Configurações")]
    public int pontosAcerto = 50;
    public int pontosErro = 35;

    // Estado do jogo
    public int Vidas { get; private set; } = 3;
    public int Pontos { get; private set; } = 0;
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
        AudioManager.Instance.ResetMinigameMusic();
        CarregarMinigameAleatorio();
    }

    public void Acertou()
    {
        Pontos += pontosAcerto;
        CarregarMinigameAleatorio();
    }

    public void Errou()
    {
        Pontos = Mathf.Max(0, Pontos - pontosErro); // Pontos não vão abaixo de 0
        Vidas--;

        if (Vidas <= 0)
            GameOver();
        else
            CarregarMinigameAleatorio();
    }

    void CarregarMinigameAleatorio()
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
        SceneManager.LoadScene(menuScene);
    }
}