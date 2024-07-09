using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateCost : MonoBehaviour
{
    [SerializeField] private Vector3 _generateData;
    [SerializeField] private GameObject _menuObjectPrefab;

    [SerializeField] private List<AccumulateObject> _menuObjects;

    private void Awake()
    {
        ObjectGenerator();
    }

    [ContextMenu("Object Generator")]
    public void ObjectGenerator()
    {
        for(int i = (int)_generateData.x; i < _generateData.y + _generateData.x; i++)
        {
            Vector3 pos = new Vector3(
                Mathf.Cos(((_generateData.z/_generateData.y) * i) * Mathf.Deg2Rad), 
                Mathf.Sin(((_generateData.z/_generateData.y) * i) * Mathf.Deg2Rad), 
                0.0f
            ) * 200.0f;

            GameObject obj = Instantiate(_menuObjectPrefab);
            AccumulateObject ao = obj.GetComponent<AccumulateObject>();

            obj.transform.SetParent(transform);
            obj.GetComponent<RectTransform>().localScale = Vector3.zero;
            ao.movePos = pos;

            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                AccumulateMenuClose();
            });

            _menuObjects.Add(ao);
        }

        AccumulateMenuClose();
    }

    public void AccumulateMenuOpen()
    {
        foreach(var item in _menuObjects)
        {
            if(item != null)
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
        foreach(var item in _menuObjects)
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
}
