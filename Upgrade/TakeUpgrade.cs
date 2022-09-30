using UnityEngine;
using UnityEngine.Audio;

public class TakeUpgrade : MonoBehaviour
{
    [SerializeField] private string _upgradeName;
    [SerializeField] private AudioClip _collectClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var _audio = collision.GetComponent<AudioSource>();
            _audio.PlayOneShot(_collectClip);
            PlayerUpgrade.UpgradeChangeBool(_upgradeName);
            Destroy(gameObject);
        }
    }
}
