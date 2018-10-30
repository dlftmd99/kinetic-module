using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UidCreater 
{
    public static UidCreater _instance = null;
    private List<string> uidList = new List<string>();
    private string[] uidStrings_alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                                          "K", "L", "N", "M", "O", "P", "Q", "R", "S", "T",
                                                          "U", "V", "W", "X", "Y","Z" };
    private string[] uidStrings_number = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


    public static UidCreater Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UidCreater();
            return _instance;
        }
    }

    public string createUid()
    {
        bool isOverlap = true;
        string uid = string.Empty;

        while (isOverlap)
        {
            isOverlap = false;
            uid += uidStrings_alphabet[(int)Random.RandomRange(0, uidStrings_alphabet.Length)];
            uid += uidStrings_number[(int)Random.RandomRange(0, uidStrings_number.Length)];
            uid += uidStrings_alphabet[(int)Random.RandomRange(0, uidStrings_alphabet.Length)];
            uid += uidStrings_number[(int)Random.RandomRange(0, uidStrings_number.Length)];
            uid += uidStrings_alphabet[(int)Random.RandomRange(0, uidStrings_alphabet.Length)];
            uid += uidStrings_number[(int)Random.RandomRange(0, uidStrings_number.Length)];
            for(int a=0; a< uidList.Count; a++)
            {
                if (uidList[a] == uid)
                {
                    uid = string.Empty;
                    isOverlap = true;
                }
            }
        }

        uidList.Add(uid);
        return uid;
    }

    public string getUidNumber(string uid)
    {
        for(int a=0; a< uidList.Count; a++)
        {
            if (uid == uidList[a])
                return a.ToString();
        }
        return null;
    }

    public void removeUid(string uid)
    {
        for(int a=0; a< uidList.Count; a++)
        {
            if (uid == uidList[a])
                uidList.RemoveAt(a);
        }
    }
	
}
