using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevLvl : MonoBehaviour
{
    private Scene _curentScene;
    private void Awake()
    {
        _curentScene = SceneManager.GetActiveScene();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(_curentScene.buildIndex - 1);
        }
    }
}
