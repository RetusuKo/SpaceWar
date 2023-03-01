using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : Menu
{
    [SerializeField] private MainManu _mainMenu;
    [SerializeField] private Button _backButton;

    [SerializeField] private ConfirmationPopupMenu _confirmationPopup;

    private SaveSlot[] saveSlots;

    private bool _isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }
    public void OnSaveSlotClick(SaveSlot saveSlot)
    {
        DiscableMenuButtons();
        if(_isLoadingGame)
        {
            DataManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene();
        }
        else if(saveSlot.HasData)
        {
            _confirmationPopup.ActivateMenu("Starting a New Game with this slot will override the currently saved data. Are you sure?",
                () =>
                {
                    StartNewGame(saveSlot);
                },
                () =>
                {
                    ActivateMenu(_isLoadingGame);
                }
                );
        }
        else
        {
            StartNewGame(saveSlot);
        }
    }
    private void StartNewGame(SaveSlot saveSlot)
    {
        DataManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        DataManager.Instance.NewGame();
        SaveGameAndLoadScene();
    }
    public void OnClearClicked(SaveSlot saveSlot)
    {
        DiscableMenuButtons();
        _confirmationPopup.ActivateMenu("Are you sure you want delte this  saved data?",
            () =>
            {
                DataManager.Instance.DelteProfile(saveSlot.GetProfileId());
                ActivateMenu(_isLoadingGame);
            },
            () =>
            {
                ActivateMenu(_isLoadingGame);
            }
            );
    }
    public void OnBackClicked()
    {
        _mainMenu.ActivateMenu();
        DeActivate();
    }
    public void ActivateMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);
        _isLoadingGame = isLoadingGame;
        Dictionary<string, GameData> profilesGameData = DataManager.Instance.GetAllPrrofilesGameData();
        _backButton.interactable = true;   
        GameObject firstSelected = _backButton.gameObject;
        foreach (var item in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(item.GetProfileId(), out profileData);
            item.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                item.SetInteractable(false);
            }
            else
            {
                item.SetInteractable(true);
                if (firstSelected.Equals(_backButton.gameObject))
                {
                    firstSelected = item.gameObject;
                }
            }
        }
        Button firstSelectButton = firstSelected.GetComponent<Button>();
        SetFirstSelected(firstSelectButton);
    }
    private void SaveGameAndLoadScene()
    {
        DataManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(new GameData().SaveSceneName);
    }
    public void DeActivate()
    {
        gameObject.SetActive(false);
    }
    private void DiscableMenuButtons()
    {
        foreach (var item in saveSlots)
            item.SetInteractable(false);
        _backButton.interactable = false;   
    }
}
