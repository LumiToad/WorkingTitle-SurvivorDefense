using SurvivorDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public static class AchivementManager 
{
    private static List<AbstractAchivement> achivements;

    public static List<AbstractAchivement> GetAchivements()
    {
        if(achivements == null || achivements == new List<AbstractAchivement>())
        {
            PopulateAchivements(out achivements);
        }

        return achivements;
    }

    public static void ProgressAchivement(int value, object[] args)
    {
        if (achivements == null || achivements == new List<AbstractAchivement>())
        {
            PopulateAchivements(out achivements);
        }

        foreach (var achivement in achivements)
        {
            achivement.TryProgress(value, args);
        }

        SaveAchivements();
    }

    private static void PopulateAchivements(out List<AbstractAchivement> result)
    {
        GameSaveFile gsf = new();
        gsf.LoadGameFileBinary();

        var saveFiles = new List<SaveableAchievement>();
        foreach(var saveFile in gsf.Achievements)
        {
            saveFiles.Add(saveFile);
        }

        result = new List<AbstractAchivement>();

        result.AddRange(new List<AbstractAchivement>()
        {
            new TutorialAchivement(saveFiles),

            #region KillAchivements
            #region Easy
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Easy, 250),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Easy, 100),
            new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Easy, 25),
            new KillAchivement(saveFiles,"GlueEnemyA_V4", AchivementDifficulty.Glue_Easy, 3),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Easy, 250),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Easy, 100),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Easy, 25),
            new KillAchivement(saveFiles,"GlueEnemyB_V4", AchivementDifficulty.Glue_Easy, 3),
        #endregion

            #region Medium
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Medium, 1000),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Medium, 500),
            new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Medium, 100),
            new KillAchivement(saveFiles,"GlueEnemyA_V4", AchivementDifficulty.Glue_Medium, 20),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Medium, 1000),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Medium, 500),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Medium, 100),
            new KillAchivement(saveFiles,"GlueEnemyB_V4", AchivementDifficulty.Glue_Medium, 20),
        #endregion

            #region Hard
            new KillAchivement(saveFiles,"GlueEnemyA_V1", AchivementDifficulty.Glue_Hard, 5000),
            new KillAchivement(saveFiles,"GlueEnemyA_V2", AchivementDifficulty.Glue_Hard, 2500),
            new KillAchivement(saveFiles,"GlueEnemyA_V3", AchivementDifficulty.Glue_Hard, 1000),
            new KillAchivement(saveFiles,"GlueEnemyA_V4", AchivementDifficulty.Glue_Hard, 100),

            new KillAchivement(saveFiles,"GlueEnemyB_V1", AchivementDifficulty.Glue_Hard, 5000),
            new KillAchivement(saveFiles,"GlueEnemyB_V2", AchivementDifficulty.Glue_Hard, 2500),
            new KillAchivement(saveFiles,"GlueEnemyB_V3", AchivementDifficulty.Glue_Hard, 1000),
            new KillAchivement(saveFiles,"GlueEnemyB_V4", AchivementDifficulty.Glue_Hard, 100),
            #endregion
            #endregion

            #region DamageAchivements
            #region easy
            new DamageAchivement(saveFiles, "Ballista", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Easy, 1000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Easy, 1000),
            #endregion

            #region Medium
            new DamageAchivement(saveFiles,"Ballista", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Medium, 10000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Medium, 10000),
            #endregion

            #region Hard
            new DamageAchivement(saveFiles,"Ballista", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Books", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Catapult", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"HeatRadiation", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Knives", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Poison", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"ScatterBomb", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Scythe", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Shotgun", AchivementDifficulty.Glue_Hard, 30000),
            new DamageAchivement(saveFiles,"Tesla", AchivementDifficulty.Glue_Hard, 30000),
            #endregion

            #endregion

        });
}

    //PLEASE DELETE!
    
    private static void SaveAchivements()
    {
        List<SaveableAchievement> saveableAchievements = new(); // create empty list of protobuf serializable achievements

        foreach (var achievement in GetAchivements())
        {
            saveableAchievements.Add(achievement.GetSaveFile());
        }

        SaveFileUtils.SaveGameFileBinary(saveableAchievements); //save game file
        // this game file currently ONLY contains the achievements.
        // in case more stuff is supposed to be saved, the protobuf file needs to change
    }

    /*
    public static void ExampleLoadAchievements()
    {
        List<SaveableAchievement> retVal = new();
        foreach (var achievement in gsf.Achievements)
        {
            retVal.Add(achievement);
            //each ach contains
            //ach.AchievementDifficulty;
            //ach.Name;
            //ach.Value;
            //ach.OptionalInfo;

            // you could also directly construct an achievement from it.
            //KillAchivement killAchivement = new(ach.Name, (AchivementDifficulty)ach.AchievementDifficulty, ach.Value);
        }

        return retVal;
    }
    */

    //PLEASE DELETE!
}
