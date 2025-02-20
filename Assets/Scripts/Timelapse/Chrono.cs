using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chrono : MonoBehaviour
{
    [SerializeField] private Text chronoText;

    private int _valueChrono = 0;
    private Coroutine _currentCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayChrono();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetChrono();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopChrono();
        }
    }

    public void PlayChrono()
    {
        if (_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(LoopChrono());
        else
            StopChrono();
    }

    public void StopChrono()
    {
        StopAllCoroutines();
        _currentCoroutine = null;
    }

    public void ResetChrono()
    {
        _valueChrono = 0;
        chronoText.text = "0";
    }

    private IEnumerator LoopChrono()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _valueChrono++;
            chronoText.text = _valueChrono.ToString();
        }
    }
}
