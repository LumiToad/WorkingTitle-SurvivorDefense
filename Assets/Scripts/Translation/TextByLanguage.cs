using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextByLanguage : SerializedMonoBehaviour
{
    [Title("Texts")]
    [SerializeField, HideInPlayMode]
    private TextMeshProUGUI[] uiTextMeshes;

    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    [HideInInspector]
    public LanguageType languageType = LanguageType.None;

    [SerializeField, HideInInspector]
    public Dictionary<string, string> TextReplaceKeyValueByCSV => CSVLanguageFileParser.GetLangDictionary(filePath, languageType);

    [SerializeField, HideInInspector]
    public Dictionary<TextMeshProUGUI, string> startValues = new();

    private void Start()
    {
        languageType = SelectedLanguage.value;
        SetTextsInUI();
    }

    private void OnApplicationQuit()
    {
        ResetStartValuesInTextMeshes();
    }

    private void OnDestroy()
    {
        ResetStartValuesInTextMeshes();
    }

    private void SaveStartValues()
    {
        startValues = new();

        if (TextReplaceKeyValueByCSV == null)
        {
            Debug.LogWarning("LANGUAGE FILE NOT FOUND OR FAULTY!");
            Debug.LogWarning("Path to language file set in Editor?");
            Debug.LogWarning("Is file in Resource folder?");

            return;
        }

        foreach (var textMesh in uiTextMeshes)
        {
            if (TextReplaceKeyValueByCSV.ContainsKey(textMesh.text))
            {
                startValues.Add(textMesh, textMesh.text);
            }
        }
    }

    public void SetTextsInUI()
    {
        if (startValues.Count == 0)
        {
            SaveStartValues();
        }

        if(languageType == LanguageType.None)
        {
            foreach(var text in uiTextMeshes)
            {
                if (!startValues.ContainsKey(text)) continue;
                text.text = startValues[text];
            }
            return;
        }

        foreach (var textMesh in startValues.Keys) 
        {
            foreach (var startWord in TextReplaceKeyValueByCSV.Keys)
            {
                if (startValues[textMesh] == startWord)
                {
                    textMesh.text = TextReplaceKeyValueByCSV[startWord];
                }
            }
        }
    }

    private void ResetStartValuesInTextMeshes()
    {
        foreach (var key in startValues.Keys)
        {
            key.text = startValues[key];
        }
    }
}
