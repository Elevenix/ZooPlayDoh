using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScreen : MonoBehaviour
{
    [SerializeField] private GameObject screenshotWindow;
    [SerializeField] private GameObject timelapseWindow;

    private void Awake()
    {
        OpenTimelapseWindow(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            OpenTimelapseWindow(!timelapseWindow.activeSelf);
    }

    public void OpenTimelapseWindow(bool value)
    {
        timelapseWindow.SetActive(value);
        screenshotWindow.SetActive(!value);
    }
}
