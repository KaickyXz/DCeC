using UnityEngine;
using UnityEngine.UI;

public class MinigameCopacabana : MonoBehaviour
{
    public enum Fase { Cavar, Esconder, Fechar }

    [Header("Sprites do Buraco")]
    [SerializeField] private Sprite[] spritesBuraco; // 3 sprites: pequeno, médio, grande
    [SerializeField] private Image imagemBuraco;

    [Header("Celular")]
    [SerializeField] private GameObject celular;

    [Header("GIF / Animação de Cavar")]
    [SerializeField] private Animator animadorBraco;
    [SerializeField] private string paramVelocidade = "Velocidade"; // parâmetro float no Animator

    [Header("Configurações")]
    [SerializeField] private float sensibilidade = 3f;       // quanto o mouse precisa mover
    [SerializeField] private float progressoPorFase = 100f;  // progresso necessário por fase

    private Fase faseAtual = Fase.Cavar;
    private float progressoAtual = 0f;
    private float mousePosAnterior;
    private float velocidadeMouse = 0f;
    private bool jogoAtivo = true;
    private int estagioburaco = 0; // 0, 1, 2

    void Start()
    {
        imagemBuraco.sprite = spritesBuraco[0];
        celular.SetActive(false);
        mousePosAnterior = Input.mousePosition.x;
    }

    void Update()
    {
        if (!jogoAtivo) return;

        CalcularVelocidadeMouse();
        AtualizarAnimacao();

        if (velocidadeMouse > 0.1f)
            ProgressarFase();
    }

    void CalcularVelocidadeMouse()
    {
        float deltaMouse = Mathf.Abs(Input.mousePosition.x - mousePosAnterior);
        mousePosAnterior = Input.mousePosition.x;

        // Suaviza a velocidade para não ser abrupto
        velocidadeMouse = Mathf.Lerp(velocidadeMouse, deltaMouse, Time.deltaTime * 10f);
    }

    void AtualizarAnimacao()
    {
        if (animadorBraco == null) return;

        // Normaliza entre 0 e 1 para o Animator
        float velocidadeNormalizada = Mathf.Clamp01(velocidadeMouse / 30f);
        animadorBraco.SetFloat(paramVelocidade, velocidadeNormalizada);
    }

    void ProgressarFase()
    {
        progressoAtual += velocidadeMouse * sensibilidade * Time.deltaTime;

        AtualizarSpriteBuraco();

        if (progressoAtual >= progressoPorFase)
        {
            progressoAtual = 0f;
            AvancarFase();
        }
    }

    void AtualizarSpriteBuraco()
    {
        // Atualiza o sprite conforme o progresso dentro da fase
        float proporcao = progressoAtual / progressoPorFase;

        if (faseAtual == Fase.Cavar)
        {
            // Buraco vai abrindo: sprite 0→1→2
            int novoEstagio = Mathf.FloorToInt(proporcao * spritesBuraco.Length);
            novoEstagio = Mathf.Clamp(novoEstagio, 0, spritesBuraco.Length - 1);

            if (novoEstagio != estagioburaco)
            {
                estagioburaco = novoEstagio;
                imagemBuraco.sprite = spritesBuraco[estagioburaco];
            }
        }
        else if (faseAtual == Fase.Fechar)
        {
            // Buraco vai fechando: sprite 2→1→0
            int novoEstagio = spritesBuraco.Length - 1 -
                Mathf.FloorToInt(proporcao * spritesBuraco.Length);
            novoEstagio = Mathf.Clamp(novoEstagio, 0, spritesBuraco.Length - 1);

            if (novoEstagio != estagioburaco)
            {
                estagioburaco = novoEstagio;
                imagemBuraco.sprite = spritesBuraco[estagioburaco];
            }
        }
    }

    void AvancarFase()
    {
        switch (faseAtual)
        {
            case Fase.Cavar:
                // Buraco aberto, mostra celular
                faseAtual = Fase.Esconder;
                estagioburaco = spritesBuraco.Length - 1;
                imagemBuraco.sprite = spritesBuraco[estagioburaco];
                celular.SetActive(true);
                Debug.Log("Fase: Esconder o celular!");
                break;

            case Fase.Esconder:
                // Celular escondido, fecha o buraco
                faseAtual = Fase.Fechar;
                celular.SetActive(false);
                Debug.Log("Fase: Fechar o buraco!");
                break;

            case Fase.Fechar:
                // Buraco fechado, acertou!
                jogoAtivo = false;
                imagemBuraco.sprite = spritesBuraco[0];
                Resultado(true);
                break;
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