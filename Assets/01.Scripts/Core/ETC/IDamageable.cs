using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void ApplyDamage(int damage, Entity dealer, Action action);

    //�����̻� �ɱ�
    public void SetAilment(AilmentEnum ailment, int duration);
}