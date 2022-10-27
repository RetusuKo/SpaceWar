using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [SerializeField] private bool _disableDataPersistence = false;
    [SerializeField] private bool _initializeDataIfNull = false;

    [SerializeField] private bool _overrideSelectedProfileId = false;
    private string testSeleectedProfileId = "test";

    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption = true;

    [SerializeField] private float _autoSaveTimeSeconds = 30f;

    private GameData _gameDate;

    private List<IDatePersistance> _datePersistanceObject;

    private FileDateHandler _dateHandler;

    private string _selectedProfileId  = "";

    private Coroutine _autoSaveCoroutine;
    public static DataManager Instance { get; private set; }
    private static bool _firstActivate = true;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(_firstActivate)
        {
            PlayerUpgrade.UpgradePut();
            print("Upgrade Put");
            _firstActivate = false;
        }
        _dateHandler = new FileDateHandler(Application.persistentDataPath, _fileName, _useEncryption);

        InitializeSelectedProfileId();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _datePersistanceObject = FindAllDataPersistanceObject();
        LoadGame();
        //if (_autoSaveCoroutine != null)
        //    StopCoroutine(_autoSaveCoroutine);
        _autoSaveCoroutine = StartCoroutine(AutoSave());
    }
    public void ChangeSelectedProfileId(string newProfileeId)
    {
        _selectedProfileId = newProfileeId;
        LoadGame();
    }
    public void DelteProfile(string profileId)
    {
        _dateHandler.Delte(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        _selectedProfileId = _dateHandler.GetMustRecentlyUpdatedProfileId();
        if (_overrideSelectedProfileId)
            _selectedProfileId = testSeleectedProfileId;
    }

    public void NewGame()
    {
        _gameDate = new GameData();
    }

    public void LoadGame()
    {
        if (_disableDataPersistence)
            return;
        if (SceneManager.GetActiveScene().rootCount == 0)
            return;
        _gameDate = _dateHandler.Load(_selectedProfileId);
        if (_initializeDataIfNull && _gameDate == null)
            NewGame();
        if (_gameDate == null)
            return;
        foreach (var item in _datePersistanceObject)
            item.LoadDate(_gameDate);
    }
    public void SaveGame()
    {
        if (_disableDataPersistence)
            return;
        if (gameObject == null /*|| SceneManager.GetActiveScene().rootCount == 0*/)
            return;
        foreach (var item in _datePersistanceObject)
            if(item !=  null)
                item.SaveData(_gameDate);
        _gameDate.LastUpdated = DateTime.Now.ToBinary();
        _dateHandler.Save(_gameDate,  _selectedProfileId);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDatePersistance> FindAllDataPersistanceObject()
    {
        IEnumerable<IDatePersistance> datePersistanceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDatePersistance>();
        return new List<IDatePersistance>(datePersistanceObjects);
    }
    public bool HasGameDate()
    {
        return _gameDate != null;
    }
    public Dictionary<string, GameData> GetAllPrrofilesGameData()
    {
        return _dateHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveTimeSeconds);
            SaveGame();
            print("autoSave");
        }
    }
}
