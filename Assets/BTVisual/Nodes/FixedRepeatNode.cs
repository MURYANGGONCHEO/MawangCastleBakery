using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class FixedRepeatNode : DecoratorNode
    {
        [SerializeField] private int _repeatCnt;
        private int _remainCnt;
        protected override void OnStart()
        {
            _remainCnt = _repeatCnt;
        }

        protected override void OnStop()
        {
            _remainCnt = 0;
        }

        protected override State OnUpdate()
        {
            if(child.Update() == State.SUCCESS)
            {
                _remainCnt--;
            }


            if (_remainCnt <= 0)
                return State.SUCCESS;
            return State.RUNNING;
        }
    }
}
