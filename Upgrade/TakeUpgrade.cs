using UnityEngine;
using UnityEngine.Audio;

public class TakeUpgrade : MonoBehaviour, IDatePersistance
{
    [SerializeField] private string _upgradeName;
    [SerializeField] private AudioClip _collectClip;

    private bool _taken = false;
    private void Start()
    {
        if(_taken)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var _audio = collision.GetComponent<AudioSource>();
            _audio.PlayOneShot(_collectClip);
            PlayerUpgrade.UpgradeChangeBool(_upgradeName);
            _taken = true;
            Destroy(gameObject);
        }
    }

    public void LoadDate(GameData data)
    {
        data.PlayerUpgrade.TryGetValue(_upgradeName, out _taken);
    }

    public void SaveData(GameData data)
    {
        
    }
}
