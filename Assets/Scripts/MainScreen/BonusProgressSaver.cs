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

    public static void IncreaseSweetJackpotProgress()
    {
        if(SweetJackpot5Progress >= 5 || SweetJackpot10Progress >= 10)
            return;

        SweetJackpot5Progress++;
        SweetJackpot10Progress++;
        Save();
    }

    public static void IncreaseFruitBlastProgress()
    {
        if(FruitBlast10Progress >= 10)
            return;

        FruitBlast10Progress++;
        Save();
    }

    public static void IncreaseDessertParadeProgress()
    {
        if(DessertParade5Progress >= 5)
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
            var wrapper = JsonConvert.DeserializeObject<BonusInfo>(json);

            SweetJackpot5Progress = wrapper.SweetJackpot5Progress;
            FruitBlast10Progress = wrapper.FruitBlast10Progress;
            DessertParade5Progress = wrapper.DessertParade5Progress;
            SweetJackpot10Progress = wrapper.SweetJackpot10Progress;
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
            BonusInfo playerBalanceWrapper = new BonusInfo(SweetJackpot5Progress, FruitBlast10Progress, DessertParade5Progress,SweetJackpot10Progress);
            string json = JsonUtility.ToJson(playerBalanceWrapper);

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
    }
}

[Serializable]
public class BonusInfo
{
    public int SweetJackpot5Progress;
    public int FruitBlast10Progress;
    public int DessertParade5Progress;
    public int SweetJackpot10Progress;

    public BonusInfo(int sweetJackpot5Progress, int fruitBlast10Progress, int dessertParade5Progress, int sweetJackpot10Progress)
    {
        SweetJackpot5Progress = sweetJackpot5Progress;
        FruitBlast10Progress = fruitBlast10Progress;
        DessertParade5Progress = dessertParade5Progress;
        SweetJackpot10Progress = sweetJackpot10Progress;
    }
}