using System;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateCost : MonoBehaviour
{
    [SerializeField] private Vector3 _generateData;
    [SerializeField] private GameObject _menuObjectPrefab;

    [SerializeField] private List<AccumulateObject> _menuObjects = new List<AccumulateObject>();
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private int _healCost = 3;
    [SerializeField] private int _suffleCost = 5;

    [SerializeField] private ParticleSystem _healFX;
    [SerializeField] private ParticleSystem _suffleFX;

    private List<Action> _actionList = new List<Action>();

    private void Awake()
    {
        ActionGenerate();
    }
    private void Start()
    {
        ObjectGenerate();
    }

    private void ObjectGenerate()
    {
        int index = 0;
        for (int i = (int)_generateData.x; i < _generateData.y + _generateData.x; i++)
        {
            Vector3 pos = new Vector3(
                Mathf.Cos(((_generateData.z / _generateData.y) * i) * Mathf.Deg2Rad),
                Mathf.Sin(((_generateData.z / _generateData.y) * i) * Mathf.Deg2Rad),
                0.0f
            ) * 200.0f;

            GameObject obj = Instantiate(_menuObjectPrefab);
            AccumulateObject ao = obj.GetComponent<AccumulateObject>();

            obj.transform.SetParent(transform);
            obj.GetComponent<RectTransform>().localScale = Vector3.zero;
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            ao.movePos = pos;

            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                AccumulateMenuClose();
            });

            switch (index)
            {
                case 0:
                    ao.skillImage.sprite = _sprites[0];
                    ao.skillImage.color = new Color(1, 1, 1, 1);
                    obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        Heal();
                    });
                    break;
                case 1:
                    ao.skillImage.sprite = _sprites[1];
                    ao.skillImage.color = new Color(1, 1, 1, 1);
                    obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        Suffle();
                    });
                    break;
                default:
                    break;
            }

            _menuObjects.Add(ao);
            index++;
        }

        AccumulateMenuClose();
    }

    public void AccumulateMenuOpen()
    {
        foreach (var item in _menuObjects)
        {
            if (item != null)
            {
                item.OpenMenu();
            }
            else
            {
                Debug.LogError("item is null : at AccumulateCost Open");
            }
        }
    }

    public void AccumulateMenuClose()
    {
        foreach (var item in _menuObjects)
        {
            if (item != null)
            {
                item.CloseMenu();
            }
            else
            {
                Debug.LogError("item is null : at AccumulateCost Close");
            }
        }
    }

    private void ActionGenerate()
    {
        _actionList.Add(() =>
        {
            Heal();
        });

        _actionList.Add(() =>
        {
            Suffle();
        });
    }

    private void Heal()
    {
        if (CostCalculator.CurrentAccumulateMoney < _healCost) return;
        CostCalculator.AccumulateChangeEvent?.Invoke(-_healCost);

        _healFX.Play();
        BattleController.Instance.Player.HealthCompo.ApplyHeal(10);
    }

    private void Suffle()
    {
        if (CostCalculator.CurrentAccumulateMoney < _suffleCost) return;
        CostCalculator.AccumulateChangeEvent?.Invoke(-_suffleCost);
        int count = BattleReader.CountOfCardInHand();
        foreach (CardBase card in BattleReader.GetHandCards())
        {
            BattleReader.CardDrawer.DestroyCard(card);
        }

        _suffleFX.Play();
        BattleReader.GetHandCards().Clear();
        BattleReader.CardDrawer.DrawCard(count, true);
    }
}
