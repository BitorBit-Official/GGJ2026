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


    private void Awake()
    {
        GameStateManager.Instance.CurrentState = GameState.MAIN_MENU; //this is the main menu
        startButton = startButtonObject.GetComponent<Button>(); //Grab the button component from the start button object
        startButton.enabled = true;
        startButton.onClick.AddListener(BeginCutscene); //Makes the button start the cutscene
        selectBox.GetComponent<Button>().onClick.AddListener(selectBox.GetComponent<BoxButton>().OnButtonPressed);

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
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1f); //Wait a bit before starting

        int maxIterations = 60;
        int i = 1;
        while (i < maxIterations)
        {
            float t = (i / (float)maxIterations) * 2 - 1f;
            float power = 1f - Mathf.Pow(Mathf.Sin(t * Mathf.PI * 0.5f),2);
            float xOffset = Random.Range(-30 * power, 31 * power);
            float yOffset = Random.Range(-30 * power, 31 * power);

            Vector3 newPosition = new Vector3(xOffset , yOffset, 0);
            menuBackground.transform.localPosition = newPosition;
            i++;
            yield return new WaitForSeconds(0.03f);
        }
        menuBackground.transform.localPosition = Vector3.zero;

        selectArrow.SetActive(true); //Show it
        yield return new WaitForSeconds(0.7f);
        selectBox.SetActive(true); //Show this too
        yield return new WaitUntil(() => selectBox.GetComponent<BoxButton>().WaitPressed); //Wait for the box button to be pressed

        selectArrow.SetActive(false); //Hide it
        selectBox.SetActive(false); //Hide this too
        dialBox.SetActive(true); //Show the dialogue box
    }
}
