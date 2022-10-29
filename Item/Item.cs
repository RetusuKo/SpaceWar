using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected AudioClip _collectClip;

    [SerializeField] protected bool _taken = false;
    protected abstract void TakeItem(Collider2D collision);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TakeItem(collision);
        }
    }
    protected void AfterTakeObject(Collider2D collision)
    {
        var _audio = collision.GetComponent<AudioSource>();
        _audio.PlayOneShot(_collectClip);
        _taken = true;
        Destroy(gameObject);
    }
}
