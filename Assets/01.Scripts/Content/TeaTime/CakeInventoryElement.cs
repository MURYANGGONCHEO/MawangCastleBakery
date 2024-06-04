using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CakeInventoryElement : MonoBehaviour, IPointerClickHandler
{
    [Header("케이크 참조")]
    [SerializeField] private Image _backgroundImg;
    [SerializeField] private Image _cakeImg;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;
    private CakeCollocation _cakeCollocation;

    [Header("마스크")]
    [SerializeField] private GameObject _usingMask;

    private bool _isSelectThisCake;
    private ItemDataBreadSO _myBreadData;
    private CakeInventoryPanel _cakeInvenPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isSelectThisCake) return;

        _isSelectThisCake = true;
        _cakeCollocation.CollocateCake(_myBreadData);
        _usingMask.SetActive(true);
        _cakeInvenPanel.FadePanel(false, ()=> _cakeInvenPanel.gameObject.SetActive(false));
    }

    public void SetInfo(ItemDataSO info, 
                        int count,
                        CakeCollocation cakeCollocation,
                        CakeInventoryPanel cakeInvenPanel)
    {
        _myBreadData = info as ItemDataBreadSO;
        _cakeCollocation = cakeCollocation;
        _cakeInvenPanel = cakeInvenPanel;

        _cakeImg.sprite = info.itemIcon;
        _nameText.text = info.itemName;
        _countText.text = $"<size=\"35\">X{count}</size>";
    }
}
