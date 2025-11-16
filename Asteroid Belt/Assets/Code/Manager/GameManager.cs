using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

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
    public int totalMoney;   // across rounds
    public int roundMoney;   // earned during a round

    [Header("Spawners (VFX Graph)")]
    [SerializeField] private VisualEffect[] spawnSystems;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        // auto-hook UI events
        //if (startButton != null)
        //{
        //    startButton.onClick.AddListener(StartRound);
        //}
        //if (endButton != null)
        //{
        //    endButton.onClick.AddListener(EndRound);
        //}
    }

    private void Start()
    {
        Debug.Log("GameManager Start");

        CurrentState = GameState.MainMenu;
        Time.timeScale = 1f;

        if (startButton != null)
        {
            startButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("startButton is NOT assigned on GameManager!", this);
        }

        if (endButton != null)
        {
            endButton.interactable = false;
        }
        else
        {
            Debug.LogWarning("endButton is NOT assigned on GameManager!", this);
        }

        Debug.Log($"spawnSystems length = {spawnSystems?.Length ?? -1}");
    }


    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;

    public void StartRound()
    {
        Debug.Log("StartRound called!");

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        // UI: toggle buttons
        //if (startButton != null) startButton.interactable = false;
        startButton.enabled = false;
        //if (endButton != null) endButton.interactable = true;
        endButton.enabled = true;

        // reset round money
        roundMoney = 0;

        // init player stats from base (struct copy)
        var playerStats = FindObjectOfType<PlayerStatsComponent>();
        if (playerStats != null)
        {
            playerStats.InitFromBaseStats(basePlayerStats);
        }

        // turn ON VFX-based spawners
        foreach (var vfx in spawnSystems)
        {
            if (vfx == null) continue;

            // if your graph is controlled just by play/stop:
            vfx.Play();

            // OPTIONAL: if you use an exposed "SpawnRate" or "IsSpawning" param:
            // if (vfx.HasFloat("SpawnRate")) vfx.SetFloat("SpawnRate", 100f);
            // if (vfx.HasBool("IsSpawning")) vfx.SetBool("IsSpawning", true);
        }
    }

    public void EndRound()
    {
        Debug.Log("EndRound called!");
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;

        // UI: toggle buttons
        //if (startButton != null) startButton.interactable = true;
        startButton.enabled = true;
        //if (endButton != null) endButton.interactable = false;
        endButton.enabled = false;

        // 1. Store money
        totalMoney += roundMoney;
        roundMoney = 0;

        // 2. Turn OFF VFX-based spawners
        foreach (var vfx in spawnSystems)
        {
            if (vfx == null) continue;

            // stop all spawn systems in the VFX Graph
            vfx.Stop();

            // OPTIONAL: also zero out spawn params if you use them
            // if (vfx.HasFloat("SpawnRate")) vfx.SetFloat("SpawnRate", 0f);
            // if (vfx.HasBool("IsSpawning")) vfx.SetBool("IsSpawning", false);
        }

        // 3. Disable player input / shooting
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // 4. Freeze gameplay time (optional)
        Time.timeScale = 0f;

        // 5. TODO: show Game Over UI
    }



}
