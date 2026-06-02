using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textPontuacao;

    void Start()
    {
        textPontuacao.text = "Pontuação: " + GameManager.Instance.Pontos;
    }

    public void Reiniciar()
    {
        GameManager.Instance.IniciarJogo();
    }

    public void VoltarMenu()
    {
        GameManager.Instance.VoltarMenu();
    }
}