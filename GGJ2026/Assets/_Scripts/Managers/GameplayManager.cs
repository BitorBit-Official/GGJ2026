using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    [SerializeField] List<House> houses = new List<House>();
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] Player playerPrefab;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text captivesObjective;
    public event Action onLevelStart;
    (int rescued, int toRescue) captives;
    List<House> captiveHouses = new List<House>();

    GameObject player;

    private void OnDisable()
    {
        onLevelStart -= LevelStart;
    }
    private void Awake()
    {
        //GameStateManager.Instance.CurrentState = GameState.IN_GAME;
        onLevelStart += LevelStart;
        Instance = this;
        player = Instantiate(playerPrefab.gameObject,gameObject.transform); //Auto-create the player instance and set it to the thing
        player.GetComponent<Player>().onLivesChanged += UpdateLives;
        onLevelStart?.Invoke(); //Fire the onLevelStart event - this means any method subscribed to this event, such as LevelStart above.
    }
    private void LevelStart()
    {
        captiveHouses.Clear();
        captives = (0, UnityEngine.Random.Range(3,6));
        UpdateObjective();
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        //This is the place where level setup logic lives. We're doing the houses here.
        for (int i = 0; i < captives.toRescue; i++)
        {
            int randomHouse = 0;
            do
            {
                randomHouse = UnityEngine.Random.Range(0, houses.Count); //Get a random index.
            }
            while (captiveHouses.Contains(houses[randomHouse]));
            houses[randomHouse].HasCaptive = true;
            houses[randomHouse].onCaptiveRescued += CheckLevelWin;
            captiveHouses.Add(houses[randomHouse]);
        Debug.Log($"Set house at index {randomHouse} as captive: {houses[randomHouse].name}");
        }

        //The enemies and players will run their own methods when onLevelStart is fired, again by subscribing to it.
    }

    private void UpdateLives(int lives, int maxLives)
    {
        Debug.Log($"Update life bar: {lives} / {maxLives}");
        healthBar.fillAmount = ((float)lives / maxLives);
        if (lives <= 0)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void CheckLevelWin()
    {
        captives.rescued++;
        UpdateObjective();
        if (captives.rescued >= captives.toRescue)
        {
            winPanel.SetActive(true);
        }
    }

    private void UpdateObjective()
    {
        captivesObjective.text = $"Captives rescued:\n {captives.rescued} out of {captives.toRescue}";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
