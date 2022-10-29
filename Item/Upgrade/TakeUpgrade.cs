using UnityEngine;
using UnityEngine.Audio;

public class TakeUpgrade : Item, IDatePersistance
{
    [SerializeField] private string _upgradeName;
    public void LoadDate(GameData data)
    {
        data.PlayerUpgrade.TryGetValue(_upgradeName, out _taken);
        if (_taken)
            Destroy(gameObject);
    }
    public void SaveData(GameData data){}
    protected override void TakeItem(Collider2D collision)
    {
        PlayerUpgrade.UpgradeChangeBool(_upgradeName);
        AfterTakeObject(collision);
    }
}