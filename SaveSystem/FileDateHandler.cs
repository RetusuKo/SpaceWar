using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileDateHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    private bool _useEncryption = true;

    private readonly string _encryptionCodeWord = "qLOcY8PJdrhk1%jtpsI";

    private readonly string _backupExtension = ".bak";

    public FileDateHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _useEncryption = useEncryption;     
    }

    public GameData Load(string profileId, bool allowRestoreFile = true)
    {
        if (profileId == null)
        {
            return null;
        }
        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestoreFile)
                {
                    Debug.LogWarning(fullPath + "\n" + e);
                    bool rollbackSeccess = AttemptRoolback(fullPath);
                    if (rollbackSeccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }
            }
        }
        return loadedData;
    }
    public void Save(GameData data, string profileId)
    {
        if (profileId == null)
        {
            return;
        }
        string fullPath  = Path.Combine(_dataDirPath, profileId, _dataFileName);
        string backupFilePath = fullPath + _backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            if(_useEncryption)
            {
                dataToStore  = EncryptDecrypt(dataToStore);
            }
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
                GameData verifiedGameData = Load(profileId);
                if (verifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError( fullPath + "\n" + e);
        }
    }
    public void Delte(string profileId)
    {
        if (profileId == null)
            return;
        string fullPath  = Path.Combine(_dataDirPath, profileId, _dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Delte dont exist data");
            }
        }
        catch
        {
            throw;
        }
    }
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profilesDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
        foreach (var item in dirInfos)
        {
            string profileId = item.Name;

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Skiping dir wheen loading all profiles{profileId}");
                continue;
            }

            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profilesDictionary.Add(profileId, profileData);
            }
        }
        return profilesDictionary;
    }
    public string GetMustRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;
        Dictionary<string , GameData> profilesGameData = LoadAllProfiles();
        foreach (var item in profilesGameData)
        {
            string profileId = item.Key;
            GameData gameData = item.Value;
            if (gameData ==  null)
            {
                continue;
            }
            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].LastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.LastUpdated);
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedDate = "";
        for (int i = 0; i < data.Length; i++)
            modifiedDate += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        return modifiedDate;
    }
    private bool AttemptRoolback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + _backupExtension;
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Try rool backup ex" + backupFilePath + "\n" +e);
        }
        return success;
    }
}
