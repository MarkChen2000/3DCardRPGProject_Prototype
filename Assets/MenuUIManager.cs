using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    GameObject Panel_MainMenu, Panel_RestartMenu, Panel_GameoverMenu;
    GameStateController GameStateCon;

    void Awake()
    {
        GameStateCon = FindObjectOfType<GameStateController>();

        Panel_MainMenu = transform.GetChild(0).GetChild(0).gameObject;
        Panel_RestartMenu = transform.GetChild(0).GetChild(1).gameObject;
        Panel_GameoverMenu = transform.GetChild(0).GetChild(2).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        Panel_GameoverMenu.SetActive(false);
        Panel_RestartMenu.SetActive(false);
    }

    public void SwitchMainMenuOnOff(bool OnOff)
    {
        Panel_MainMenu.SetActive(OnOff);
    }
    
    public void StartGame()
    {
        SwitchMainMenuOnOff(false);
        GameStateCon.FirstEnterVillage();
    }

    public void QuitGame()
    {
        Debug.Log("Quit the Game!");
        Application.Quit();
    }

}
