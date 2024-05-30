using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightningCardBase : CardBase
{
    [SerializeField] private ParticleSystem _shockedEffect;
    [SerializeField] private ParticleSystem _staticEffect;
    private ParticleSystem.MainModule _mainModule;

    protected void ExtraAttack(Entity me)
    {
        foreach (var e in battleController.onFieldMonsterList)
        {
            try
            {
                if (e != null && e.HealthCompo.AilmentStat.HasAilment(AilmentEnum.Shocked) && e != me)
                {
                    e?.HealthCompo.AilmentStat.UsedToAilment(AilmentEnum.Shocked);
                    ParticleSystem shockedFX = Instantiate(_staticEffect, Vector3.Lerp(me.transform.position, e.transform.position, 0.5f), Quaternion.identity);
                    Vector2 dir = e.transform.position - me.transform.position;
                    float zRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    shockedFX.transform.rotation = Quaternion.Euler(0, 0, zRot);
                    //Destroy(shockedFX, 2f);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(e);
            }
        }
    }

    protected void ApplyShockedAilment(Entity enemy)
    {
        enemy.HealthCompo.AilmentStat.ApplyAilments(AilmentEnum.Shocked);
    }

    protected void RandomApplyShockedAilment(Entity enemy, float percentage)
    {
        if (UnityEngine.Random.value * 100 >= percentage)
            enemy.HealthCompo.AilmentStat.ApplyAilments(AilmentEnum.Shocked);
    }
}
