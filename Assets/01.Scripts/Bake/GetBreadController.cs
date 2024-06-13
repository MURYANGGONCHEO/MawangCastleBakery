using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBreadController : MonoBehaviour
{
    [SerializeField] private GameObject _grapicsElementParentObj;
    private IBakingProductionObject[] _productionGraphicsObjArr;

    [SerializeField] private int[] _perc = { 5, 25, 70 };
    [SerializeField] private int _testidx;

    private void Awake()
    {
        _productionGraphicsObjArr =
        _grapicsElementParentObj.GetComponentsInChildren<IBakingProductionObject>();
    }

    public void OnProduction()
    {
        foreach(var production in _productionGraphicsObjArr)
        {
            production.OnProduction();
        }
    }

    public void ExitProduction()
    {
        foreach (var production in _productionGraphicsObjArr)
        {
            production.ExitProduction();
        }
    }

    public void DoughInStove(float randomValue)
    {
        int grade = -1;
        int cakeCount = 0;
        float cumulative = 0f;

        for (int i = 0; i < _perc.Length; i++)
        {
            cumulative += _perc[i];
            if(cumulative >= randomValue)
            {
                grade = i;
                break;
            }
        }

        switch(grade)
        {
            case 0:
                cakeCount = 10;
                break;
            case 1:
                cakeCount = 50;
                break;
            case 2:
                cakeCount = 150;
                break;
            default:
                break;
        }

        UIManager.Instance.GetSceneUI<BakeryUI>().ToGetCakeitemData = (Inventory.Instance.GetRandomCake(),  cakeCount);

        foreach (var production in _productionGraphicsObjArr)
        {
            production.DoughInStove(grade);
        }
    }
}
