using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] protected Button _firstSelected;

    protected virtual void OnEnable()
    {
        SetFirstSelected(_firstSelected);
    }

    public void SetFirstSelected(Button firstSelectedObject)
    {
        firstSelectedObject.Select();
    }
}
