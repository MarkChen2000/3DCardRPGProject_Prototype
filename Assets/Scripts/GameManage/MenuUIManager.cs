using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    GameObject Panel_MainMenu, Panel_Options, Panel_RestartMenu, Panel_GameoverMenu, Panel_PauseMenu;
    GameStateController GameStateCon;

    public float FadeInOut_Time = 2f;

    void Awake()
    {
        GameStateCon = FindObjectOfType<GameStateController>();

        Panel_MainMenu = transform.GetChild(0).GetChild(0).gameObject;
        Panel_Options = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        Panel_RestartMenu = transform.GetChild(0).GetChild(1).gameObject;
        Panel_GameoverMenu = transform.GetChild(0).GetChild(2).gameObject;
        Panel_PauseMenu = transform.GetChild(0).GetChild(3).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        Panel_GameoverMenu.SetActive(false);
        Panel_RestartMenu.GetComponent<CanvasGroup>().alpha = 0;
        Panel_RestartMenu.SetActive(false);
        SwitchPauseMenuOnOff(false);

        SwitchOptionMenuOnOff(false);
    }

    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape ))
        {
            SwitchPauseMenu();
        }
    }

    public void SwitchMainMenuOnOff(bool OnOff)
    {
        Panel_MainMenu.SetActive(OnOff);
    }

    public void SwitchOptionMenuOnOff(bool OnOff)
    {
        Panel_Options.SetActive(OnOff);
    }
    
    public IEnumerator StartFadeInRestartMenu()
    {
        SwitchRestartMenuOnOff(true);
        CanvasGroup cg = Panel_RestartMenu.GetComponent<CanvasGroup>();
        while ( cg.alpha < 1f )
        {
            cg.alpha += 1 / FadeInOut_Time * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator StartFadeInGameOverMenu()
    {
        SwitchGameOverMenuOnOff(true);
        CanvasGroup cg = Panel_RestartMenu.GetComponent<CanvasGroup>();
        while (cg.alpha < 1f)
        {
            cg.alpha += 1 / FadeInOut_Time * Time.deltaTime;
            yield return null;
        }
    }

    public void SwitchRestartMenuOnOff(bool OnOff)
    {
        Panel_RestartMenu.SetActive(OnOff);
    }

    public void SwitchGameOverMenuOnOff(bool OnOff)
    {
        Panel_GameoverMenu.SetActive(OnOff);
    }

    void SwitchPauseMenuOnOff(bool OnOff)
    {
        Panel_PauseMenu.SetActive(OnOff);
        if (OnOff) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    void SwitchPauseMenu()
    {
        if (Panel_PauseMenu.activeSelf == true)
            SwitchPauseMenuOnOff(false);
        else
            SwitchPauseMenuOnOff(true);
    }

    public void StartGame() //Button
    {
        SwitchMainMenuOnOff(false);
        SwitchPauseMenuOnOff(false);
        GameStateCon.FirstEnterVillage();
    }

    public void RestartGame() //Button
    {
        SwitchRestartMenuOnOff(false);
        SwitchPauseMenuOnOff(false);
        GameStateCon.EnterState(GameState.Prepare);
    }

    public void BacktoMainMenu() //Button
    {
        SwitchRestartMenuOnOff(false);
        SwitchPauseMenuOnOff(false);
        GameStateCon.EnterState(GameState.MainMenu);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit the Game!");
        Application.Quit();
    }

}
