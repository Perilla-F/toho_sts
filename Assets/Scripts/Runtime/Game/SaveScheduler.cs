using System;
using System.Collections;
using UnityEngine;

public class SaveScheduler : MonoBehaviour, ISaveScheduler
{
    [SerializeField] private float saveDelay = 2f; // 2秒後にセーブ
    private Action _pendingSave;
    private Coroutine _saveCoroutine;

    public void ScheduleSave(Action saveAction)
    {
        _pendingSave = saveAction;
        if (_saveCoroutine != null)
        {
            StopCoroutine(_saveCoroutine);
        }
        _saveCoroutine = StartCoroutine(SaveRoutine());
    }

    private IEnumerator SaveRoutine()
    {
        yield return new WaitForSeconds(saveDelay);
        _pendingSave?.Invoke();
        _pendingSave = null;
        _saveCoroutine = null;
        Debug.Log("Auto-saved successfully!");
    }
}