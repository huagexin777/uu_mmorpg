using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PrefType 
{
    /// <summary>
    /// ’À∫≈
    /// </summary>
    Account,
}

public class PlayerPrefInstance : MonoBehaviour
{
    #region µ•¿˝

    private static PlayerPrefInstance _instance;
    public static PlayerPrefInstance Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (PlayerPrefInstance)FindObjectOfType(typeof(PlayerPrefInstance));
            }
            return _instance;
        }
    }

    #endregion


    #region Õ®”√

    public static int GetInt(string key) 
    {
        return PlayerPrefs.GetInt(key);
    }

    public static void SetInt(string key,int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    #endregion

    #region Account

    public static void SetAccount(AccountPref pref)
    {
        PlayerPrefs.SetInt("accountId", pref.accountId);
        PlayerPrefs.SetString("userName", pref.userName);
        PlayerPrefs.SetString("password", pref.password);
    }

    public static bool HasAccount(string userName) 
    {
        return PlayerPrefs.HasKey(userName);
    }

    public static AccountPref GetAccount()
    {
        AccountPref pref = new AccountPref()
        {
            accountId = PlayerPrefs.GetInt("accountId"),
            userName = PlayerPrefs.GetString("userName"),
            password = PlayerPrefs.GetString("password")
        };
        return pref;
    }

    #endregion

    public static void Clear() 
    {

        PlayerPrefs.DeleteAll();
    }

    public static void Clear(params PrefType[] type)
    {
        for (int i = 0; i < type.Length; i++)
        {
            if (type[i] == PrefType.Account)
            {
                //’À∫≈¿‡. ∞§∏ˆ…æ≥˝key
                List<string> keyNames = new AccountPref().KeyNames;
                for (int j = 0; j < keyNames.Count; j++)
                {
                    PlayerPrefs.DeleteKey(keyNames[j]);
                }
            }
        }
    }

}


/// <summary>
/// ’À∫≈
/// </summary>
public class AccountPref 
{
    public int accountId;
    public string userName;
    public string password;

    public List<string> KeyNames = new List<string>();

    public AccountPref() 
    {
        KeyNames.Add("accountId");
        KeyNames.Add("userName");
        KeyNames.Add("password");
    }
}
