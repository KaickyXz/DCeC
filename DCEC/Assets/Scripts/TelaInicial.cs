using UnityEngine;

public class TelaInicial : MonoBehaviour
{
    [SerializeField] private GameObject painelCreditos;

    void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
        painelCreditos.SetActive(false);
    }

    public void Jogar()
    {
        GameManager.Instance.IniciarJogo();
    }

    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        painelCreditos.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
    }
}