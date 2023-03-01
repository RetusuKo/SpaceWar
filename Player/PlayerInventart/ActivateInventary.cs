using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInventary : MonoBehaviour
{
    public void ActivateInventory()
    {
        bool activate = gameObject.activeSelf;
        gameObject.SetActive(!activate);
        //Pause.PauseGame(activate);
    }
}
