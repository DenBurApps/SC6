using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class BonusProgressSaver
{
    static BonusProgressSaver()
    {
        Load();
    }

    private static string _savePath = Path.Combine(Application.persistentDataPath, "BonusSave");

    public static int SweetJackpot5Progress { get; private set; }
    public static int FruitBlast10Progress { get; private set; }
    public static int DessertParade5Progress { get; private set; }
    public static int SweetJackpot10Progress { get; private set; }
    public static bool IsSweetJackpot5Collected { get; private set; }
    public static bool IsFruitBlast10Collected { get; private set; }
    public static bool IsDessertParade5Collected { get; private set; }
    public static bool IsSweetJackpot10Collected { get; private set; }

    public static void CollectedSweetJackpot5()
    {
        IsSweetJackpot5Collected = true;
        Save();
    }

    public static void CollectedFruitBlast10()
    {
        IsFruitBlast10Collected = true;
        Save();
    }

    public static void CollectedDessertParade5()
    {
        IsDessertParade5Collected = true;
        Save();
    }

    public static void CollectedSweetJackpot10()
    {
        IsSweetJackpot10Collected = true;
        Save();
    }

    public static void IncreaseProgress(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.DessertParade:
                IncreaseDessertParadeProgress();
                break;
            case GameType.FruitBlast:
                IncreaseFruitBlastProgress();
                break;
            case GameType.SweetJackpot:
                IncreaseSweetJackpotProgress();
                break;
        }
    }

    private static void IncreaseSweetJackpotProgress()
    {
        if (SweetJackpot5Progress >= 5 && SweetJackpot10Progress >= 10) return;

        if (!IsSweetJackpot5Collected)
            SweetJackpot5Progress++;
        
        SweetJackpot10Progress++;
        Save();
    }

    private static void IncreaseFruitBlastProgress()
    {
        if (FruitBlast10Progress >= 10)
            return;

        FruitBlast10Progress++;
        Save();
    }

    private static void IncreaseDessertParadeProgress()
    {
        if (DessertParade5Progress >= 5)
            return;

        DessertParade5Progress++;
        Save();
    }

    private static void Load()
    {
        try
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("No save file found.");
                ResetAllValues();
                return;
            }

            var json = File.ReadAllText(_savePath);
            var data = JsonConvert.DeserializeObject<BonusInfo>(json);

            SweetJackpot5Progress = data.SweetJackpot5Progress;
            FruitBlast10Progress = data.FruitBlast10Progress;
            DessertParade5Progress = data.DessertParade5Progress;
            SweetJackpot10Progress = data.SweetJackpot10Progress;
            IsSweetJackpot5Collected = data.IsSweetJackpot5Collected;
            IsFruitBlast10Collected = data.IsFruitBlast10Collected;
            IsDessertParade5Collected = data.IsDessertParade5Collected;
            IsSweetJackpot10Collected = data.IsSweetJackpot10Collected;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading gift data: {ex.Message}");
            ResetAllValues();
        }
    }

    private static void Save()
    {
        try
        {
            var data = new BonusInfo(
                SweetJackpot5Progress,
                FruitBlast10Progress,
                DessertParade5Progress,
                SweetJackpot10Progress,
                IsSweetJackpot5Collected,
                IsFruitBlast10Collected,
                IsDessertParade5Collected,
                IsSweetJackpot10Collected);
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(_savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save balance: " + e);
        }
    }

    private static void ResetAllValues()
    {
        SweetJackpot5Progress = 0;
        FruitBlast10Progress = 0;
        DessertParade5Progress = 0;
        SweetJackpot10Progress = 0;

        IsSweetJackpot5Collected = false;
        IsFruitBlast10Collected = false;
        IsDessertParade5Collected = false;
        IsSweetJackpot10Collected = false;
    }

    [Serializable]
    private class BonusInfo
    {
        public int SweetJackpot5Progress;
        public int FruitBlast10Progress;
        public int DessertParade5Progress;
        public int SweetJackpot10Progress;
        public bool IsSweetJackpot5Collected;
        public bool IsFruitBlast10Collected;
        public bool IsDessertParade5Collected;
        public bool IsSweetJackpot10Collected;

        public BonusInfo(int sweetJackpot5Progress, int fruitBlast10Progress, int dessertParade5Progress,
            int sweetJackpot10Progress,
            bool isSweetJackpot5Collected, bool isFruitBlast10Collected, bool isDessertParade5Collected,
            bool isSweetJackpot10Collected)
        {
            SweetJackpot5Progress = sweetJackpot5Progress;
            FruitBlast10Progress = fruitBlast10Progress;
            DessertParade5Progress = dessertParade5Progress;
            SweetJackpot10Progress = sweetJackpot10Progress;
            IsSweetJackpot5Collected = isSweetJackpot5Collected;
            IsFruitBlast10Collected = isFruitBlast10Collected;
            IsDessertParade5Collected = isDessertParade5Collected;
            IsSweetJackpot10Collected = isSweetJackpot10Collected;
        }
    }
}