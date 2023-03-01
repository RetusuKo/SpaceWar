using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateMenu : Menu
{
    [SerializeField] private GameObject _hideObject;
    public void ActivateGameMenu()
    {
        bool activate = _hideObject.activeSelf;
        _hideObject.SetActive(!activate);
        Pause.PauseGame(!activate);
        SetFirstSelected(_firstSelected);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
            ActivateGameMenu();
    }
    public void GoToMainMenu()
    {
        DataManager.Instance.SaveGame();
        Pause.PauseGame(false);
        SceneManager.LoadScene(0);
    }
}
