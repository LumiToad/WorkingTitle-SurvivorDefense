using SurvivorDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum AchivementDifficulty
{
    Glue_Easy,
    Glue_Medium,
    Glue_Hard
}

public abstract class AbstractAchivement 
{
    protected int progress = 0;
    protected int maxProgress = 0;

    private string saveId = "";

    public abstract string GetName();
    public abstract string GetDescription();

    public AbstractAchivement(string saveId, List<SaveableAchievement> allDatas)
    {
        this.saveId = saveId;

        for(int i = 0;i < allDatas.Count;i++)
        {
            SaveableAchievement achievement = allDatas[i];
            if (TryLoadAchivement(achievement))
            {
                allDatas.Remove(achievement);
            }
        }
    }

    private bool TryLoadAchivement(SaveableAchievement data)
    {
        if (data.OptionalInfo == saveId)
        {
            progress = data.Value;
            return true;
        }

        return false;
    }

    public SaveableAchievement GetSaveFile()
    {
        var saveFile = new SaveableAchievement();

        saveFile.Value = progress;
        saveFile.OptionalInfo = saveId;

        return saveFile;
    }

    public bool TryProgress(int value, object[] args)
    {
        if(CanProgress(value, args))
        {
            Progress(value);
            return true;
        }
        return false;
    }

    protected abstract bool CanProgress(int value, object[] args);

    public string GetProgressString()
    {
        var result = CSVLanguageFileParser.GetLangDictionary("Translation/MenuText/Achivements", SelectedLanguage.value)["Glue%"];

        var displayPercent = progress / (float)maxProgress;
        displayPercent *= 100;
        displayPercent = Mathf.RoundToInt(displayPercent);

        result = result.Replace("<m1>", displayPercent.ToString());

        return result;
    }

    public float GetProgressPercent() => progress / (float)maxProgress;

    protected void Progress(int amount)
    {
        progress = Mathf.RoundToInt(Mathf.Clamp(progress + amount, 0, maxProgress));
    }
}
