using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SynergyClass
{
    public class SynergyChecker : MonoBehaviour
    {
        private List<Synergy> _synergyList;
        public List<Synergy> SynergyList
        {
            get
            {
                return _synergyList;
            }
        }

        public void SetSynergyEnable(Synergy synergy)
        {
            int checkCnt = 0;

            foreach (CardBase conditionCard in synergy.ConditionCards)
            {
                checkCnt += (BattleReader.InDeckCardList.Find(card => card.CardID == conditionCard.CardID) != null) ? 1 : 0;
            }

            synergy.Enable = (checkCnt >= synergy.ConditionCheckValue) ? true : false;
        }

        public void SetAllSynergyEnable()
        {
            foreach (Synergy synergy in _synergyList)
            {
                SetSynergyEnable(synergy);
            }
        }

        public void SetAllSynergyDisable()
        {
            foreach (Synergy synergy in _synergyList)
            {
                synergy.Enable = false;
            }
        }
    }
}