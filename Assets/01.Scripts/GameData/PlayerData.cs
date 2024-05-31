using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CanSaveData
{
    public int level = 1;
    public int exp = 0;
    public string nickName = "�÷��̾�";
    public int level = 4;
    public int exp = 0;
    public string nickName = "����ȣ";

    public int attak = 5;
    public int defination = 3;
    public int heartPoint = 20;

    public override void SetInitialValue()
    {
        attak = 5;
        defination = 3;
        heartPoint = 20;
    }
}
