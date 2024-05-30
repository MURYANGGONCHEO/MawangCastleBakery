using DG.Tweening;
using ExtensionFunction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform _deckElemetTrm;
    [SerializeField] private CanUseDeckElement _canUseDeckPrefab;
    [SerializeField] private SelectedDeck _selectDeckObj;
    [SerializeField] private int _startPage = 1;
    [SerializeField] private TextMeshProUGUI _pageText;
    private int _currentPage;

    private DeckElement _selectDeck;
    public DeckElement SelectDeck
    {
        get
        {
            return _selectDeck;
        }
        set
        {
            _selectDeck = value;
            SetSelectDeck(value);
        }
    }

    private SaveDeckData _saveDeckData = new SaveDeckData();
    private Tween _scalingTween;

    public List<DeckElement> CurrentDeckList { get; private set; } = new List<DeckElement>();

    protected virtual void Start()
    {
        _currentPage = _startPage;

        GenerateDeckList();
        ResetDeckList(_saveDeckData.SaveDeckList);
    }

    public void GenerateDeckList()
    {
        if (DataManager.Instance.IsHaveData(DataKeyList.saveDeckDataKey))
        {
            _saveDeckData = DataManager.Instance.LoadData<SaveDeckData>(DataKeyList.saveDeckDataKey);
        }

        foreach(DeckElement de in _saveDeckData.SaveDeckList)
        {
            CurrentDeckList.Add(de);
        }
    }

    private void SetPageText()
    {
        _pageText.text = $"{_currentPage} ÆäÀÌÁö";
    }

    public void GoAfterPage()
    {
        if(_currentPage > Mathf.CeilToInt(_saveDeckData.SaveDeckList.Count / 4))
        {
            Debug.Log(_saveDeckData.SaveDeckList.Count);
            return;
        }

        ++_currentPage;
        SetPageText();
        ResetDeckList(_saveDeckData.SaveDeckList);
    }

    public void GoBeforePage()
    {
        if (_currentPage - 1 <= 0)
        {
            return;
        }

        --_currentPage;
        SetPageText();
        ResetDeckList(_saveDeckData.SaveDeckList);
    }

    public void ResetDeckList(List<DeckElement> deList)
    {
        _deckElemetTrm.Clear();

        int startIdx = 4 * (_currentPage - 1);
        int maxIdx;

        if(_currentPage * 4 <= deList.Count)
        {
            maxIdx = 4;
        }
        else
        {
            maxIdx = startIdx + (deList.Count % 4);
        }

        if (startIdx == maxIdx) return;

        for (int i = startIdx; i < maxIdx; i++)
        {
            CanUseDeckElement cude = Instantiate(_canUseDeckPrefab, _deckElemetTrm);
            cude.SetDeckInfo(deList[i], this);
        }

        _scalingTween?.Kill();
        _scalingTween = _deckElemetTrm.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    protected virtual void SetSelectDeck(DeckElement deckElement)
    {
        if(deckElement.deck == null)
        {
            _selectDeckObj.gameObject.SetActive(false);
            return;
        }
        print(deckElement.deck);
        _selectDeckObj.gameObject.SetActive(true);
        _selectDeckObj.SetDeckInfo(deckElement.deckName, 
                                   DeckManager.Instance.GetDeck(deckElement.deck));
    }
}
