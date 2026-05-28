using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
   
    [SerializeField] private string nomeCenaJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;


    public void Jogar()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }
    
    public void fecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);

    }

    public void Sair()
    {
        Debug.Log("você saiu");
        Application.Quit();        
    }
}
