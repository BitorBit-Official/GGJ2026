using UnityEngine;

public class BoxButton : MonoBehaviour
{
    public bool WaitPressed { get; private set; } = false;

    public void OnButtonPressed()
    {
        WaitPressed = true;
    }
}
