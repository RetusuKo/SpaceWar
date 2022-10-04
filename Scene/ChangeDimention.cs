using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDimention : MonoBehaviour
{
    private Scene _curentScene;
    private void Awake()
    {
        _curentScene = SceneManager.GetActiveScene();
    }
    private void Update()
    {
        ChangeDime();
    }
    private void ChangeDime()
    {
        if (Input.GetButtonDown("ChangeDimention") && PlayerUpgrade.UpgradeCheck(PlayerUpgrade.UpgradesName[0]))
        {
            if (7 >= _curentScene.name.Length)
            {
                SceneManager.LoadScene(_curentScene.name + "Dime2");
            }
            else if (9 <= _curentScene.name.Length)
            {
                SceneManager.LoadScene(_curentScene.name.Remove(_curentScene.name.Length - 5));
            }
        }
    }
}
