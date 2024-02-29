using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteOkay : MonoBehaviour, ISettings
{
    [SerializeField, ShowInInspector, Required]
    private OptionsScreenNew options;

    [SerializeField, ShowInInspector, Required]
    private TextMeshProUGUI fileText;

    private string[] filenames;

    public void Setup(string toDelete, params string[] deletionFilename)
    {
        fileText.text = toDelete;
        filenames = deletionFilename;
    }

    public void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnConfirmClicked()
    {
        foreach (string filename in filenames) 
        {
            SaveFileUtils.DeleteSaveFileBinary(filename);
            SaveFileUtils.DeleteSaveFileJSON(filename);
        }
        fileText.text += " has been deleted!";
        EnableButtons(false);

        StartCoroutine(HideInternal());
    }

    private void EnableButtons(bool enabled)
    {
        foreach (Button btn in GetComponentsInChildren<Button>())
        {
            btn.interactable = enabled;
        }
    }

    private IEnumerator HideInternal()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        EnableButtons(true);
        gameObject.SetActive(false);
        options.Hide();
    }
}
