using UnityEngine;

public class PoteClique : MonoBehaviour
{
    [SerializeField] private int meuIndex;
    private MinigameBaralho minigame;

    void Start()
    {
        minigame = FindObjectOfType<MinigameBaralho>();
    }

    void OnMouseDown()
    {
        minigame.ClicarPote(meuIndex);
    }
}