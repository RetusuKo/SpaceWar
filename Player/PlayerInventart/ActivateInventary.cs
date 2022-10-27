using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInventary : MonoBehaviour
{
    public void ActivateInventory()
    {
        bool activate = gameObject.activeSelf;
        gameObject.SetActive(!activate);
        Time.timeScale = !activate ? 1.0f : 0.0f;
    }
}
