using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMainMenu()
    {
        mainMenu.interactable = !mainMenu.interactable;
        mainMenu.blocksRaycasts = !mainMenu.blocksRaycasts;
        mainMenu.alpha = mainMenu.interactable ? 1 : 0;
    }

    public void ToggleOptionsMenu()
    {
        optionsMenu.interactable = !optionsMenu.interactable;
        optionsMenu.blocksRaycasts = !optionsMenu.blocksRaycasts;
        optionsMenu.alpha = optionsMenu.interactable ? 1 : 0;
    }

    public void SwapMenus()
    {
        ToggleMainMenu();
        ToggleOptionsMenu();
    }

    public void MenuButtonPressed()
    {
        if (optionsMenu.interactable)
        {
            SwapMenus();
        }
        else
        {
            ToggleMainMenu();
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
    
#endif
    }
}
