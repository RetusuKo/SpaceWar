using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManu : Menu, IDatePersistance
{
    [SerializeField] private SaveSlotMenu _saveSlotMenu;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _loadGameButton;

    //private string _startSceneName = "Lvl 1";
    private string _sceneName;

    private void Start()
    {
        DisableButtonsDependingOnData();
    }
    private void DisableButtonsDependingOnData()
    {
        if (!DataManager.Instance.HasGameDate())
        {
            _continueGameButton.interactable = false;
            _loadGameButton.interactable = false;
        }
    }
    public void OnNewGameClick()
    {
        _saveSlotMenu.ActivateMenu(false);
        DeactivateMenu();
    }
    public void OnLoadGameClick()
    {
        _saveSlotMenu.ActivateMenu(true);
        DeactivateMenu();
    }
    public void OnContinueGameClick()
    {
        DisableMenuButtons();
        DataManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(_sceneName);
    }

    private void DisableMenuButtons()
    {
        _newGameButton.interactable = false;
        _continueGameButton.interactable = false;
    }

    public void LoadDate(GameData data)
    {
        _sceneName = data.SaveSceneName;
    }

    public void SaveData(GameData data)  {}
    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
    public void ActivateMenu()
    {
        gameObject.SetActive(true);

        DisableButtonsDependingOnData();
    }
}
