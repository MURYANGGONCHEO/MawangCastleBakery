using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynergyClass;
using System;

public enum SynergyImpactType
{
    AtkUpContinous,         // Increase ATK during battle
    DEFUpContinous,         // Increase DEF during battle
    MaxHpUpContinous,       // Increase Max Hp during battle
    AtkUpBuff,              // Increase ATK during n turn
    DefUpBuff,              // Increase DEF during n turn
    ForgingStack,           // Increase Forging stack gain
    Music1Statck,           // Increase Music 1 stack gain
    Music2Stack,            // Increase Mucic 2 stack gain
    Music3Stack,            // Increase Music 3 stack gain
    ReleasedDMGUp,          // 'Released' skill extra damage coefficient increase
    LightningDMGUp,         // 'Lightning Theme' skill extra damage coefficient increase
    MusicValueUp,           // 'Finale' skill debuff coefficient increase
    AtkDownDebuff,          // Decrease ATK during n turn
    DefDownDebuff,          // Decrease DEF during n turn
}

public class SynergyController : MonoBehaviour
{
    [SerializeField]
    private SynergyChecker _synergyChecker;

    private Dictionary<SynergyImpactType, SynergyImpact> _synergyImpactDic;

    public void SynergyInit()
    {
        foreach (SynergyImpact impact in Enum.GetValues(typeof(SynergyImpact)))
        {
            string typeName = impact.ToString();
            Type t = Type.GetType($"SynergyClass.{typeName}Impact");

            SynergyImpact synergyImpact = Activator.CreateInstance(t) as SynergyImpact;
            _synergyImpactDic.Add((SynergyImpactType)Enum.Parse(typeof(SynergyImpactType), typeName), synergyImpact);
        }
    }

    public void SynergyActiveEnable()
    {
        // 이제 여기서 시너지 효과를 적용시켜야함
    }

    public void SynergyActiveDisable()
    {
        // 여기서는 시너지 효과를 제거해야함
    }
}
