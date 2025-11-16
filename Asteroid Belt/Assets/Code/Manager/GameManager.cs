using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Countdown,
        Playing,
        Paused,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    [Header("Base Player Stats")]
    public PlayerStats basePlayerStats;

    [Header("Economy")]
    public int totalMoney;
    public int roundMoney;
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI roundMoneyText;

    [Header("Spawners (VFX Graph)")]
    [SerializeField] private VisualEffect[] spawnSystems;

    public GameState CurrentState { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Debug.Log("GameManager Start");

        CurrentState = GameState.MainMenu;
        Time.timeScale = 1f;

        if (startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("startButton is NOT assigned on GameManager!", this);
        }

        if (endButton != null)
        {
            endButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("endButton is NOT assigned on GameManager!", this);
        }

        Debug.Log($"spawnSystems length = {spawnSystems?.Length ?? -1}");
    }

    public void StartRound()
    {
        Debug.Log("StartRound called!");

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        // Re-enable player input / shooting
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // init player stats from base (struct copy)
        var playerStats = FindObjectOfType<PlayerStatsComponent>();
        if (playerStats != null)
        {
            playerStats.InitFromBaseStats(basePlayerStats);
        }

        // UI: toggle buttons
        if (startButton != null) startButton.gameObject.SetActive(false);
        if (endButton != null) endButton.gameObject.SetActive(true);

        // reset round money
        roundMoney = 0;



        // turn ON VFX-based spawners
        foreach (var vfx in spawnSystems)
        {
            if (vfx == null) continue;
            vfx.Play();
        }
    }

    public void EndRound()
    {
        Debug.Log("EndRound called!");
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;

        // UI: toggle buttons
        if (startButton != null) startButton.gameObject.SetActive(true);
        if (endButton != null) endButton.gameObject.SetActive(false);

        // 1. Store money
        totalMoney += roundMoney;
        roundMoney = 0;

        // 2. Turn OFF VFX-based spawners
        foreach (var vfx in spawnSystems)
        {
            if (vfx == null) continue;
            vfx.Stop();
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

    public void AddMoney(int amount)
    {
        roundMoney += amount;
        Debug.Log($"Added {amount} money. Round Money: {roundMoney}, Total Money: {totalMoney}");
        roundMoneyText.text = $"Round Money: {roundMoney}";
        if (roundMoney > totalMoney)
        {
            totalMoney = roundMoney;
            totalMoneyText.text = $"Total Money: {totalMoney}";
        }
    }
}
