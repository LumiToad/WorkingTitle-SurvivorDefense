using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI deathMessage;

    public void Show(string message)
    {
        if (gameObject.activeInHierarchy) return;

        gameObject.SetActive(true);
        deathMessage.text = message;
    }

    //called by UI button
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
