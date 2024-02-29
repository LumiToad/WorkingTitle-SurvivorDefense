using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementUI : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private AchivementCard cardTemplate;
    [SerializeField, FoldoutGroup("References")]
    private Transform cardHolder;

    private PlayerInputActions actions;

    private void Awake()
    {
        actions = new PlayerInputActions();
        actions.Navigation.Enable();
        actions.Navigation.Pause.Enable();
    }

    public void SetUp()
    {
       
    }

    private void Update()
    {
        if (actions.Navigation.Pause.WasPerformedThisFrame())
        {
            Hide();
        }
    }

    public void Show()
    {
        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject, 0.01f);
        }

        foreach (var achivement in AchivementManager.GetAchivements())
        {
            var card = Instantiate(cardTemplate);
            card.SetUp(achivement);

            card.transform.SetParent(cardHolder, true);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
