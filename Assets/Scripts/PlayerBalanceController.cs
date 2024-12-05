using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerBalanceController
{
    static PlayerBalanceController()
    {
        LoadBalanceData();
    }

    private static string _saveFilePath => Path.Combine(Application.persistentDataPath, "PlayerBalance");
    private static int _initBalance = 1000;
    public static int CurrentBalance { get; private set; }

    public static event Action<int> BalanceChanged;

    public static void IncreaseBalance(int count)
    {
        CurrentBalance += count;
        SaveBalanceData();
        BalanceChanged?.Invoke(CurrentBalance);
    }

    public static void DecreaseBalance(int count)
    {
        if (CurrentBalance - count >= 0)
        {
            CurrentBalance -= count;
            SaveBalanceData();
        }
        else
        {
            Debug.Log("Not enough balance");
        }

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
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load balance: " + e);
                CurrentBalance = _initBalance;
            }
        }
        else
        {
            CurrentBalance = _initBalance;
        }
    }

    private static void SaveBalanceData()
    {
        try
        {
            PlayerBalanceWrapper playerBalanceWrapper = new PlayerBalanceWrapper(CurrentBalance);
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

    public PlayerBalanceWrapper(int balance)
    {
        Balance = balance;
    }
}