using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    MAIN_MENU,
    IN_GAME,
    PAUSED,
    GAME_OVER
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; set; }
    InputAction exit;
    public GameState CurrentState { get; set; } = GameState.MAIN_MENU; //default state

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log($"[{gameObject.name}] Game state manager already exists! Removing duplicate...");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);

        Debug.Log($"Set up game state manager with a state of {CurrentState}");

        exit = InputSystem.actions.FindAction("Exit");
    }

    private void Update()
    {
        if (exit.WasPressedThisFrame())
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}
