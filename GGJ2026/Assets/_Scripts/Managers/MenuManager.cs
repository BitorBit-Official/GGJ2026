using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Object assets")]
    [SerializeField] GameObject menuBackground;
    [SerializeField] GameObject selectArrow;
    [SerializeField] GameObject selectBox;
    [SerializeField] GameObject dialBox;
    [SerializeField] Button dialAdvance;
    [SerializeField] TMP_Text textUI;
    [SerializeField] GameObject mapProp;
    [SerializeField] GameObject maskProp;
    [SerializeField] GameObject startButtonObject;
    [SerializeField] float typeDelay;

    readonly string startingDialogue = "Station 23, December 1st, 2042\n" +
        "Fellow soldier, if you are listening to this, you have a mission.\n" +
        "Collect the remaining human survivors from captivity and bring them back safely.\n" +
        "There are 3 stations built by the Strange, made to capture and enslave humans.\n" +
        "Your mission is to find the survivors, and bring them back into the bunker without being caught by the Strange.\n" +
        "Use this mask to trick them, but be careful, if you aren't quick enough they will figure you out.\n" +
        "Good luck!";
    Button startButton;
    int index;
    int currentLineIndex = 0;

    bool advancePressed;
    bool isTyping;

    private void Awake()
    {
        GameStateManager.Instance.CurrentState = GameState.MAIN_MENU; //this is the main menu
        startButton = startButtonObject.GetComponent<Button>(); //Grab the button component from the start button object
        startButton.enabled = true;
        startButton.onClick.AddListener(BeginCutscene); //Makes the button start the cutscene
        selectBox.GetComponent<Button>().onClick.AddListener(selectBox.GetComponent<BoxButton>().OnButtonPressed);

        maskProp.SetActive(false);
        selectArrow.SetActive(false); //Force hide it
        selectBox.SetActive(false); //Same as above
        dialBox.SetActive(false); //Same as above
        mapProp.SetActive(false); //Yup. Same as above
        dialAdvance.onClick.AddListener(Advance);
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
            float xOffset = UnityEngine.Random.Range(-30 * power, 31 * power);
            float yOffset = UnityEngine.Random.Range(-30 * power, 31 * power);

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
        mapProp.SetActive(true); //And the map prop
        yield return StartCoroutine(TypeDialogue());
    }

    IEnumerator TypeDialogue()
    {
        index = 0;

        while (index < startingDialogue.Length)
        {
            textUI.text = "";
            isTyping = true;
            if (currentLineIndex == 5)
            {
                maskProp.SetActive(true);
            }


            // TYPE UNTIL NEWLINE OR END
            while (index < startingDialogue.Length && startingDialogue[index] != '\n')
            {
                textUI.text += startingDialogue[index];
                index++;

                // Optional: skip typing if button pressed
                if (advancePressed)
                {
                    while (index < startingDialogue.Length && startingDialogue[index] != '\n')
                    {
                        textUI.text += startingDialogue[index];
                        index++;
                    }
                    break;
                }

                yield return new WaitForSeconds(typeDelay);
            }

            isTyping = false;
            advancePressed = false;
            currentLineIndex++;
            // WAIT FOR BUTTON TO CONTINUE
            yield return new WaitUntil(() => advancePressed);

            advancePressed = false;

            // SKIP THE NEWLINE
            if (index < startingDialogue.Length && startingDialogue[index] == '\n')
                index++;
        }

        // Dialogue finished
        OnDialogueFinished();
    }

    private void OnDialogueFinished()
    {
        SceneManager.LoadScene("Game");
    }
    public void Advance()
    {
        advancePressed = true;
    }
}
