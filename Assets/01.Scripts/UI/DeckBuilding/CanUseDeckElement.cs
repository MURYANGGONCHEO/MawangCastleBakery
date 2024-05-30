using DG.Tweening;
using ExtensionFunction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanUseDeckElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DeckElement DeckInfo { get; private set; }

    [SerializeField] private Image[] _cardGroupArr = new Image[5];
    [SerializeField] private TextMeshProUGUI _deckNameText;
    [SerializeField] private float _onPointerEnterMoveY;
    [SerializeField] private float _onPointerExitMoveY;
    private DeckElement _deckInfo;

    private DeckGenerator _deckGenerator;
    private string _deckName;

    public void OnPointerClick(PointerEventData eventData)
    {
        _deckGenerator.SelectDeck = _deckInfo;
    }

    public void SetDeckInfo(DeckElement deckInfo, DeckGenerator deckGenerator)
    {
        DeckInfo = deckInfo;
        _deckGenerator = deckGenerator;

        for (int i = 0; i < _cardGroupArr.Length; i++)
        {
            CardBase card = DeckManager.Instance.GetCard(deckInfo.deck[i]);
            _cardGroupArr[i].sprite = card.CardInfo.CardVisual;
            TextMeshProUGUI costText = _cardGroupArr[i].GetComponentInChildren<TextMeshProUGUI>();
            costText.text = card.AbilityCost.ToString();
        }

        _deckName = deckInfo.deckName;

        if (_deckName.Length > 6)
        {
            _deckName = $"{_deckName.Substring(0, 6)}..";
        }

        _deckNameText.text = _deckName;
        _deckInfo = deckInfo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1.05f, 0.3f);

        for (int i = -2; i <= 2; i++)
        {
            Transform trm = _cardGroupArr[i + 2].transform;

            trm.DOKill();
            trm.DOLocalMove(new Vector2(_cardGroupArr[i + 2].transform.localPosition.x + (i * 5),
                                        _onPointerEnterMoveY + (2 - Mathf.Abs(i)) * 5), 0.3f).SetEase(Ease.OutBack);
            trm.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, -i * 5), 0.3f).SetEase(Ease.OutBack);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, 0.3f);

        for (int i = 0; i < _cardGroupArr.Length; i++)
        {
            float value = (i - 2) * 5;

            Transform trm = _cardGroupArr[i].transform;

            trm.DOKill();
            trm.DOLocalMove(new Vector2(_cardGroupArr[i].transform.localPosition.x - value,
                                        _onPointerExitMoveY), 0.3f).SetEase(Ease.OutBack);
            trm.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f).SetEase(Ease.OutBack);
        }
    }
}