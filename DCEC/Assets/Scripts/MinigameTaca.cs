using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameTaca : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image barraTaca;
    [SerializeField] private Image zonaAlvo;
    [SerializeField] private Slider sliderApoio;
    [SerializeField] private TextMeshProUGUI textInstrucao;

    [Header("Configurações")]
    [SerializeField] private float sensibilidade = 0.00f;
    [SerializeField] private float tamanhoZonaMin = 0.1f;
    [SerializeField] private float tamanhoZonaMax = 0.25f;

    private float nivelAtual = 0f;
    private float zonaInicio;
    private float zonaFim;
    private float mousePosAnterior;
    private bool jogoAtivo = false;
    private bool segurando = false;

    void Start()
    {
        // Configura o slider
        sliderApoio.minValue = 0f;
        sliderApoio.maxValue = 1f;
        sliderApoio.value = 0f;
        sliderApoio.interactable = false; // só visual, jogador não arrasta

        GerarDesafio();

        if (textInstrucao != null)
            textInstrucao.text = "Segure e arraste para cima!";
    }

    void GerarDesafio()
    {
        float tamanhoZona = Random.Range(tamanhoZonaMin, tamanhoZonaMax);
        zonaInicio = Random.Range(0.15f, 0.85f - tamanhoZona);
        zonaFim = zonaInicio + tamanhoZona;

        AtualizarZonaAlvo(tamanhoZona);

        nivelAtual = 0f;
        barraTaca.fillAmount = 0f;
        sliderApoio.value = 0f;
        jogoAtivo = true;
    }

    void AtualizarZonaAlvo(float tamanhoZona)
    {
        RectTransform rtZona = zonaAlvo.rectTransform;
        RectTransform rtBarra = barraTaca.rectTransform;

        float alturaTotal = rtBarra.rect.height;

        rtZona.anchoredPosition = new Vector2(
            rtZona.anchoredPosition.x,
            zonaInicio * alturaTotal
        );
        rtZona.sizeDelta = new Vector2(
            rtZona.sizeDelta.x,
            tamanhoZona * alturaTotal
        );
    }

    void Update()
    {
        if (!jogoAtivo) return;

        if (Input.GetMouseButtonDown(0))
        {
            segurando = true;
            mousePosAnterior = Input.mousePosition.y;

            if (textInstrucao != null)
                textInstrucao.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0) && segurando)
        {
            segurando = false;
            jogoAtivo = false;

            bool acertou = nivelAtual >= zonaInicio && nivelAtual <= zonaFim;
            Resultado(acertou);
        }

        if (segurando)
        {
            float deltaMouse = Input.mousePosition.y - mousePosAnterior;
            mousePosAnterior = Input.mousePosition.y;

            if (deltaMouse > 0)
            {
                nivelAtual += deltaMouse * sensibilidade;
                nivelAtual = Mathf.Clamp01(nivelAtual);
                barraTaca.fillAmount = nivelAtual;
                sliderApoio.value = nivelAtual; // atualiza slider junto
            }

            if (nivelAtual >= 1f)
            {
                jogoAtivo = false;
                Resultado(false);
            }
        }
    }

    void Resultado(bool acertou)
    {
        TimerManager timer = FindAnyObjectByType<TimerManager>();
        if (timer != null) timer.PararTimer();

        if (acertou)
            GameManager.Instance.Acertou();
        else
            GameManager.Instance.Errou();
    }
}