using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUITask : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI progressText;
    [SerializeField,FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    public void UpdateProgress(int left)
    {
        string empty = CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)["GlueLeft"];

        string full = empty.Replace("<m1>", left.ToString());
        progressText.text = full;
    }
}
