using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    [SerializeField] List<House> houses = new List<House>();
    [SerializeField] Player playerPrefab;
    List<House> visitedHouses = new List<House>();
    House winningHouse;
    public event Action onLevelStart;
    GameObject player;

    private void OnEnable()
    {
        onLevelStart += LevelStart;
    }
    private void OnDisable()
    {
        onLevelStart -= LevelStart;
    }
    private void Awake()
    {
        //GameStateManager.Instance.CurrentState = GameState.IN_GAME;
        Instance = this;
        onLevelStart?.Invoke(); //Fire the onLevelStart event - this means any method subscribed to this event, such as LevelStart above.
        player = Instantiate(playerPrefab.gameObject,gameObject.transform); //Auto-create the player instance and set it to the thing
    }
    private void LevelStart()
    {
        //This is the place where level setup logic lives. We're doing the houses here.
        visitedHouses.Clear(); //Clear all the houses that may have been visited previously - generally make sure the list is empty first.
        int randomHouse = UnityEngine.Random.Range(0, houses.Count); //Get a random index.
        winningHouse = houses[randomHouse]; //Then, set the house at that index as the winning one.

        //The enemies and players will run their own methods when onLevelStart is fired, again by subscribing to it.
    }

    private bool IsWinningHouse(House checkHouse)
    {
        return checkHouse == winningHouse;
    }

    public void TryCheckHouse(House house)
    {
        IsWinningHouse(house);
    }
}
