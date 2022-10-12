using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private string _profileId = "";

    [SerializeField] private GameObject _noDataContent;
    [SerializeField] private GameObject _hasDataContent;
    [SerializeField] private TextMeshProUGUI _percentageCompleteText;

    [SerializeField] private Button _clearButton;

    public bool HasData { get; private set; } = false;

    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = GetComponent<Button>();
    }
    public void SetData(GameData data)
    {
        if(data == null)
        {
            HasData = false;
            _noDataContent.SetActive(true);
            _hasDataContent.SetActive(false);
            _clearButton.gameObject.SetActive(false);
        }
        else
        {
            HasData = true;
            _noDataContent.SetActive(false);
            _hasDataContent.SetActive(true);
            _clearButton.gameObject.SetActive(true);

            _percentageCompleteText.text = data.GetPercentageComplate() + "% COMPLETE";
        }
    }
    public string GetProfileId()
    {
        return _profileId;
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
        _clearButton.interactable = interactable;
    }
}