using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerBalanceController
{
    static PlayerBalanceController()
    {
        IsMultiplied = false;
        FreeSpinsCount = 0;
        LoadBalanceData();
    }

    private static string _saveFilePath => Path.Combine(Application.persistentDataPath, "PlayerBalance");
    private static int _initBalance = 1000;
    private static bool IsMultiplied;
    private static int FreeSpinsCount;
    public static int CurrentBalance { get; private set; }
    

    public static event Action<int> BalanceChanged;

    public static void IncreaseBalance(int count)
    {
        if (IsMultiplied)
        {
            count *= 3;
            CurrentBalance += count;
            IsMultiplied = false;
            SaveBalanceData();
            return;
        }
        
        CurrentBalance += count;
        SaveBalanceData();
        BalanceChanged?.Invoke(CurrentBalance);
    }

    public static void DecreaseBalance(int count)
    {
        if (FreeSpinsCount > 0)
        {
            FreeSpinsCount--;
            SaveBalanceData();
            return;
        }
        
        if (CurrentBalance - count >= 0)
        {
            CurrentBalance -= count;
            SaveBalanceData();
        }
        else
        {
            Debug.Log("Not enough balance");
        }

        BalanceChanged?.Invoke(CurrentBalance);
        SaveBalanceData();
    }

    public static void AddFreeSpins(int spins)
    {
        FreeSpinsCount += spins;
        SaveBalanceData();
    }

    public static void SetMultiplier()
    {
        IsMultiplied = true;
        SaveBalanceData();
    }
    
    private static void LoadBalanceData()
    {
        if (File.Exists(_saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(_saveFilePath);

                PlayerBalanceWrapper playerBalanceWrapper = JsonUtility.FromJson<PlayerBalanceWrapper>(json);

                CurrentBalance = playerBalanceWrapper.Balance;
                IsMultiplied = playerBalanceWrapper.IsMultiplyAcive;
                FreeSpinsCount = playerBalanceWrapper.FreeSpinsCount;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load balance: " + e);
                CurrentBalance = _initBalance;
                FreeSpinsCount = 0;
                IsMultiplied = false;
            }
        }
        else
        {
            CurrentBalance = _initBalance;
            FreeSpinsCount = 0;
            IsMultiplied = false;
        }
    }

    private static void SaveBalanceData()
    {
        try
        {
            PlayerBalanceWrapper playerBalanceWrapper = new PlayerBalanceWrapper(CurrentBalance, IsMultiplied, FreeSpinsCount);
            string json = JsonUtility.ToJson(playerBalanceWrapper);

            File.WriteAllText(_saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save balance: " + e);
        }
    }
}

[Serializable]
public class PlayerBalanceWrapper
{
    public int Balance;
    public bool IsMultiplyAcive;
    public int FreeSpinsCount;

    public PlayerBalanceWrapper(int balance, bool multiplier, int freeSpinsCount)
    {
        Balance = balance;
        IsMultiplyAcive = multiplier;
        FreeSpinsCount = freeSpinsCount;
    }
}