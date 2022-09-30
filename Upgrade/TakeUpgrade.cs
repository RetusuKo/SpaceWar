using UnityEngine;
using UnityEngine.Audio;

public class TakeUpgrade : MonoBehaviour
{
    [SerializeField] private int _upgradeNumber;
    [SerializeField] private AudioClip _collectClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var _audio = collision.GetComponent<AudioSource>();
            _audio.PlayOneShot(_collectClip);
            PlayerInfo.Upgrades[_upgradeNumber] = true;
            Destroy(gameObject);
        }
    }
}
