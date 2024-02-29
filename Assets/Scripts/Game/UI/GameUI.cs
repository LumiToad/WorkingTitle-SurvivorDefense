using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;

    private GameOverScreen gameOverScreen;
    private PauseScreen pauseScreen;
    private VictoryScreen victoryScreen;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;

        gameOverScreen = GetComponentInChildren<GameOverScreen>(true);
        pauseScreen = GetComponentInChildren<PauseScreen>(true);
        victoryScreen = GetComponentInChildren<VictoryScreen>(true);

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Navigation.Enable();
        playerInputActions.Navigation.Pause.Enable();
    }

    private void Start()
    {
        pauseScreen.SetUp();
    }

    private void Update()
    {
        if (playerInputActions.Navigation.Pause.WasPerformedThisFrame())
        {
            PauseShowHide();
        }
    }

    public void PauseShowHide()
    {
        if (!pauseScreen.gameObject.activeInHierarchy)
        {
            pauseScreen.Show();
        }
        else
        {
            pauseScreen.Hide();
        }
    }

    public static void ShowGameOverScreen(string message) => instance.gameOverScreen.Show(message);
    public static void ShowVictoryScreen() => instance.victoryScreen.Show();
}
