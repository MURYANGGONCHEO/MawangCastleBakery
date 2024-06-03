using CardDefine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRecord
{
    public int CardID { get; }
    public string CardName { get; }
    public CombineLevel CombineLevel { get; }

    public CardRecord(int handIdx, int cardIdx, string info, CombineLevel combineLv)
    => (CardID, CardName, CombineLevel) = (cardIdx, info, combineLv);
}

public class HandRecover : MonoBehaviour
{
    [SerializeField] private SkillCardManagement _skillCardManagement;
    private List<CardBase> _inWaitZoneCardList => _skillCardManagement.InCardZoneList;

    [SerializeField] private Transform _cardHandZone;
    [SerializeField] private Transform _resotreCardZone;

    public void RevertHand(CardBase card)
    {
        if (card.CardID != _inWaitZoneCardList[_inWaitZoneCardList.Count - 1].CardID) return;

        CostCalculator.GetCost(card.AbilityCost);

        CardRecord myRec = card.CardRecordList.FirstOrDefault(x => x.CardID == card.CardID);
        CardReader.InHandCardList.Insert(card.SaveHandIDX, card);
        card.transform.SetParent(_cardHandZone);

        _inWaitZoneCardList.Remove(card);
        RestoreNotExistCard(card.CardRecordList);

        card.IsOnActivationZone = false;
        _skillCardManagement.SetSkillCardInHandZone();
    }

    public void RevertAllHand()
    {
        if (_inWaitZoneCardList.Count == 0) return;

        CardBase oldCard = _inWaitZoneCardList[0];

        foreach(var card in _inWaitZoneCardList)
        {
            CardRecord myRec = oldCard.CardRecordList.FirstOrDefault(x => x.CardID == card.CardID);
            CardReader.InHandCardList.Insert(card.SaveHandIDX, card);
            card.transform.SetParent(_cardHandZone);
        }

        _inWaitZoneCardList.Clear();

        oldCard.transform.SetParent(_cardHandZone);

    }

    private void RestoreNotExistCard(List<CardRecord> recordList)
    {
        foreach(var rc in recordList)
        {
            CardBase recordCard = CardReader.InHandCardList.
            FirstOrDefault(c => c.CardID == rc.CardID);

            if (recordCard == null)
            {
                CardBase cb = Instantiate(DeckManager.Instance.GetCard(rc.CardName), _cardHandZone);
                cb.SetInfo(rc.CardID, 0, rc.CombineLevel);
                cb.transform.position = _resotreCardZone.position;

                CardReader.InHandCardList.Insert(recordCard.SaveHandIDX, cb);
            }
            else
            {
                if(recordCard.CombineLevel != rc.CombineLevel)
                {
                    recordCard.CombineLevel = rc.CombineLevel;
                }
            }
        }
    }
}
