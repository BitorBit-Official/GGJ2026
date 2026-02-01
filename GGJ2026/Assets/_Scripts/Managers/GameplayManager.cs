using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    [SerializeField] List<House> houses = new List<House>();
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] Player playerPrefab;
    [SerializeField] Image healthBar;
    public event Action onLevelStart;

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
        //This is the place where level setup logic lives. We're doing the houses here.
        int randomHouse = UnityEngine.Random.Range(0, houses.Count); //Get a random index.
        houses[randomHouse].HasCaptive = true; //Then, set the house at that index as the winning one.
        Debug.Log($"Set house at index {randomHouse} as winning: {houses[randomHouse].name}");

        //The enemies and players will run their own methods when onLevelStart is fired, again by subscribing to it.
    }

    private void UpdateLives(int lives, int maxLives)
    {
        Debug.Log($"Update life bar: {lives} / {maxLives}");
        healthBar.fillAmount = ((float)lives / maxLives);
    }
}
