using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DateManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    [SerializeField] private bool _useEncryption = true;

    private GameData _gameDate;

    private List<IDatePersistance> _datePersistanceObject;

    private FileDateHandler _dateHandler;
    public static DateManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Morethan 1 DateManager");
        }
        instance = this;
        PlayerUpgrade.UpgradePut();
    }
    private void Start()
    {
        _dateHandler = new FileDateHandler(Application.persistentDataPath, fileName, _useEncryption);
        _datePersistanceObject = FindAllDataPersistanceObject();
        LoadGame();
    }

    public void NewGame()
    {
        _gameDate = new GameData();
    }

    public void LoadGame()
    {
        _gameDate = _dateHandler.Load();
        if (_gameDate == null)
        {
            Debug.Log("Create new Game");
            NewGame();
        }
        foreach (var item in _datePersistanceObject)
        {
            item.LoadDate(_gameDate);
        }
        print("load");
    }
    public void SaveGame()
    {
        foreach (var item in _datePersistanceObject)
        {
            item.SaveData(_gameDate);
        }
        _dateHandler.Save(_gameDate);
        print("save");
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDatePersistance> FindAllDataPersistanceObject()
    {
        IEnumerable<IDatePersistance> datePersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDatePersistance>();
        return new List<IDatePersistance>(datePersistanceObjects);
    }
}
