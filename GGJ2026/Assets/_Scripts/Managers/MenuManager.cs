using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Object assets")]
    [SerializeField] GameObject menuBackground;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject selectBox;
    [SerializeField] GameObject dialBox;
    [SerializeField] GameObject mapProp;
    [SerializeField] GameObject startButtonObject;
    Button startButton;

    [Header("Background positions")]
    [SerializeField] Vector3[] bgPositions;

    private void Awake()
    {
        GameStateManager.Instance.CurrentState = GameState.MAIN_MENU; //this is the main menu
        startButton = startButtonObject.GetComponent<Button>(); //Grab the button component from the start button object
        startButton.enabled = true;

        selectArrow.SetActive(false); //Force hide it
        selectBox.SetActive(false); //Same as above
        dialBox.SetActive(false); //Same as above
        mapProp.SetActive(false); //Yup. Same as above
    }

    private void BeginCutscene()
    {
        //Disable the start button to prevent multiple clicks
        startButton.enabled = false;
        startButtonObject.SetActive(false);
    }

    private IEnumerator StartCutscene()
    {
        if (bgPositions.Length == 0 || bgPositions == null)
        {
            Debug.LogError($"Error! No positions for BG, aborting");
            yield break; //No positions to move to
        }
        int i = 0;
        while (i < bgPositions.Length)
        {
            menuBackground.transform.localPosition = bgPositions[i];
            i++;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
