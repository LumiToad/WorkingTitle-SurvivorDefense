using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickHints : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI hintText;
    [SerializeField]
    [FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;

    private List<string> shownHints = new List<string>();

    private Animator anim;

    private static QuickHints instance;

    private QuickHintsDTO savedShownHints = new();

    private const string QUICKHINTS_SAVEFILE_NAME = "savedShownHints";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        instance = this;
        //LoadShownPromptsFile();
    }

    public static void ShowOnce(string key, string magicNumber1 = "", string magicNumber2 = "")
    {
        return;
        string hint = CSVLanguageFileParser.GetLangDictionary(instance.filePath, SelectedLanguage.value)[key];

        hint = hint.Replace("<m1>", magicNumber1);
        hint = hint.Replace("<m2>", magicNumber2);

        if (instance.shownHints.Contains(hint)) return;

        instance.shownHints.Add(hint);
        //instance.SaveShownPromptsFile();

        instance.hintText.text = hint;
        instance.anim.Play("Show");

        Time.timeScale = 0;
    }

    public void CloseQuickHint()
    {
        Time.timeScale = 1;
        anim.Play("Hide");
    }

    private void LoadShownPromptsFile()
    {
        SaveFileUtils.TryLoadClassFromJsonFile(ref instance.savedShownHints, QUICKHINTS_SAVEFILE_NAME);
        shownHints = savedShownHints.keys;
    }

    private void SaveShownPromptsFile()
    {
        instance.savedShownHints.keys = shownHints;
        SaveFileUtils.SaveClassToJson(instance.savedShownHints, QUICKHINTS_SAVEFILE_NAME);   
    }
}
