using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class ConfirmationPopupMenu : Menu
{
    [SerializeField] private TextMeshProUGUI _displayText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cacelAction)
    {
        gameObject.SetActive(true);
        _displayText.text = displayText;

        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });
        _cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cacelAction();
        });
    }
    private void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
