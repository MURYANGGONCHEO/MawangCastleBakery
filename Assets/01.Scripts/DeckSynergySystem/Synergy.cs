using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SynergyClass
{
    public class Synergy : ScriptableObject
    {
        public string SynergyName;
        public string SynergyDesc;
        public string SynergyImpactName;
        public SynergyImpactType ImpactType;
        public SynergyImpact SynergyImpact;
        public int ConditionCheckValue;
        public List<CardBase> ConditionCards;
        public bool Enable;
    }
}