using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
   
    [SerializeField] private string nomeCenaJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelLoja;
    [SerializeField] private GameObject painelConquistas;

    public void Jogar()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void AbrirLoja()
    {
        painelMenuInicial.SetActive(false);
        painelLoja.SetActive(true);
    }

    public void FecharLoja()
    {
        painelMenuInicial.SetActive(true);
        painelLoja.SetActive(false);
    }

    public void AbrirConquistas()
    {
        painelMenuInicial.SetActive(false);
        painelConquistas.SetActive(true);
    }

        public void FecharConquistas()
    {
        painelMenuInicial.SetActive(true);
        painelConquistas.SetActive(false);
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