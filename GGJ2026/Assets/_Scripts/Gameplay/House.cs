using UnityEngine;
using System;

public class House : MonoBehaviour
{
    [SerializeField] GameObject questionMark;
    bool _hasCaptive;
    public bool wasChecked;
    public event Action onCaptiveRescued;
    public bool HasCaptive { get { return _hasCaptive; } set { _hasCaptive = value; } }

    public void ExamineHouse()
    {
        wasChecked = true;
        questionMark.SetActive(false);
        if (_hasCaptive)
        {
            Debug.Log("This has a captive.");
            onCaptiveRescued?.Invoke();
        }
        else
        {
            Debug.Log("This has no captives.");
        }
    }
}
