using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicCardBase : CardBase
{
    protected int GetNoteCount()
    {
        return Player.BuffStatCompo.GetStack(StackEnum.DEFMusicalNote) + Player.BuffStatCompo.GetStack(StackEnum.DMGMusicaldNote) + Player.BuffStatCompo.GetStack(StackEnum.FAINTMusicalNote);
    }

    protected bool HasDEFMusicalNoteStack() { return Player.BuffStatCompo.GetStack(StackEnum.DEFMusicalNote) > 0; }

    protected bool HasDMGMusicalNoteStack() { return Player.BuffStatCompo.GetStack(StackEnum.DMGMusicaldNote) > 0; }

    protected bool HasFAINTMusicalNoteStack() { return Player.BuffStatCompo.GetStack(StackEnum.FAINTMusicalNote) > 0; }

    protected void ApplyDebuffToAllEnemy()
    {
        foreach(var e in battleController.onFieldMonsterList)
        {
            e?.BuffStatCompo.AddBuff(buffSO, (int)CombineLevel + 2, (int)CombineLevel);
        }
    }
}
