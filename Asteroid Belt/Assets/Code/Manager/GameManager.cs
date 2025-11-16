using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Countdown,  // like starting a race, 3, 2, 1 GO!
        Playing,    // enable PlayerController, cursor locked outside of UI to game
        Paused,     // unlock Cursor, view UI, freeze time &|| input
        GameOver    // show results, add money, disable shooting and spawning
    }

    public static GameManager Instance { get; private set; }

    [Header("Base Player Stats")]
    public PlayerStats basePlayerStats;

    [Header("Economy")]
    public int totalMoney;      // across rounds
    public int roundMoney;     // earned during a round

    [Header("Spawners (ParticleSystems)")]
    [SerializeField] private ParticleSystem[] spawnSystems;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrentState = GameState.MainMenu;
        Time.timeScale = 1f;
    }

    public void StartRound()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        // init stats, money, etc.
        roundMoney = 0;

        var playerStats = FindObjectOfType<PlayerStatsComponent>();
        if (playerStats != null)
        {
            // PlayerStats is a struct!!
            playerStats.InitFromBaseStats(basePlayerStats);
        }

        // turn on particle-based spawners
        foreach (var ps in spawnSystems)
        {
            if (ps == null) continue;

            // Make sure emission is enabled and play the system
            var emission = ps.emission;
            emission.enabled = true;
            ps.Play(true);
        }
    }

    public void EndRound()
    {
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;

        // store money
        totalMoney += roundMoney;
        roundMoney = 0;

        // turn off particle-based spawners
        foreach (var ps in spawnSystems)
        {
            if (ps == null) continue;

            // disable emission so new particles don't spawn!!!
            var emission = ps.emission;
            emission.enabled = false;

            // stop the system:
            // - StopEmitting: keeps current particles until they die
            // - StopEmittingAndClear: kills everything immediately
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // disable player input / shooting
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // optional! freeze gameplay *after* stopping spawners.
        Time.timeScale = 0f;

        // show and hide UI screen TODO.
    }
}
