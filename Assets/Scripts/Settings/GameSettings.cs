using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour, ISettings
{
    [SerializeField, ShowInInspector, Required]
    private Dropdown languageOption;

    [SerializeField, ShowInInspector, Required]
    private DeleteOkay deleteOkay;

    //not pretty, but the best solution I came up with
    private bool skipNextApply = false;

    private const string GAMESETTINGS_SAVEFILE_NAME = "gameSettings";

    public void SetUp()
    {
        LoadDefaultLanguage();
    }

    private void LoadDefaultLanguage()
    {
        GameSettingsDTO dto = new();
        if (!SaveFileUtils.TryLoadClassFromJsonFile<GameSettingsDTO>(ref dto, GAMESETTINGS_SAVEFILE_NAME)) return;

        SelectedLanguage.value = dto.language;

        foreach (var text in FindObjectsOfType<TextByLanguage>(true))
        {
            text.languageType = dto.language;
            text.SetTextsInUI();
        }

        switch (dto.language)
        {
            case LanguageType.None:
                break;
            case LanguageType.DE:
                skipNextApply = true;
                languageOption.value = 1;
                break;
            case LanguageType.EN:
                languageOption.value = 0;
                break;
            case LanguageType.FR:
                break;
        }

    }

    public void ApplySettings()
    {
        GameSettingsDTO dto = new();

        //Apply language Settings
        switch (languageOption.value)
        {
            case 0:
                SelectedLanguage.value = LanguageType.EN;
                break;
            case 1:
                SelectedLanguage.value = LanguageType.DE;
                break;
        }

        SaveFileUtils.SaveClassToJson<GameSettingsDTO>(dto, GAMESETTINGS_SAVEFILE_NAME);

        foreach (var text in FindObjectsOfType<TextByLanguage>(true))
        {
            text.languageType = SelectedLanguage.value;
            text.SetTextsInUI();
        }
    }

    public void OnDeleteSaveClicked()
    {
        deleteOkay.gameObject.SetActive(true);
        deleteOkay.Setup("Save", "save");
    }

    public void OnDeleteSettingsClicked()
    {
        deleteOkay.gameObject.SetActive(true);
        deleteOkay.Setup("Settings", "videoSettings", "audioSettings", "gameSettings");
    }

    public void OnCancelClicked()
    {
        
    }

    public void OnConfirmClicked()
    {
        
    }
}
