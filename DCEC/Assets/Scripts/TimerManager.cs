using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image barraTimer;

    private float tempoTotal;
    private float tempoRestante;
    private bool timerAtivo = false;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            tempoTotal = 30f;
            Debug.LogWarning("GameManager não encontrado! Usando tempo padrão.");
        }
        else
        {
            tempoTotal = GameManager.Instance.TempoAtual;
        }

        tempoRestante = tempoTotal;
        timerAtivo = true;
    }

    void Update()
    {
        if (!timerAtivo) return;

        tempoRestante -= Time.deltaTime;
        barraTimer.fillAmount = tempoRestante / tempoTotal;

        if (tempoRestante <= 0)
        {
            PararTimer();
            GameManager.Instance.Errou(); // Tempo esgotado = errou
        }
    }

    public void PararTimer()
    {
        timerAtivo = false;
        barraTimer.fillAmount = 0f;
    }
}