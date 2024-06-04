using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBreadController : MonoBehaviour
{
    [SerializeField] private GameObject _grapicsElementParentObj;
    [SerializeField] private DoughHandler _dough;
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
            Debug.Log(production);
        }
    }

    public void ExitProduction()
    {
        foreach (var production in _productionGraphicsObjArr)
        {
            production.ExitProduction();
        }
    }

    public void DoughInStove()
    {
        int grade = -1;
        float r = Random.value * 100;
        Debug.Log(r);
        float cumulative = 0f;
        for (int i = 0; i < _perc.Length; i++)
        {
            cumulative += _perc[i];
            if(cumulative >= r)
            {
                grade = i;
                break;
            }
        }

        foreach (var production in _productionGraphicsObjArr)
        {
            production.DoughInStove(/*grade*/_testidx);
        }
    }

    public void ReturnOriginalStove(float waitSecond)
    {
        StartCoroutine(WaitExitProduction(waitSecond));
    }

    private IEnumerator WaitExitProduction(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        ExitProduction();
        _dough.ResetDoughTrm();
    }
}
