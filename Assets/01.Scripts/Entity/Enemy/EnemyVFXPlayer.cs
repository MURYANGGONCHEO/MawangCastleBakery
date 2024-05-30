using Particle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VFXData
{
    public string name;
    public ParticleSystem particle;
}

public class EnemyVFXPlayer : MonoBehaviour
{
    [SerializeField]private List<VFXData> datas = new();
    private Dictionary<string, ParticleSystem> _dataDic = new();

    public event Action OnEndEffect;
    private void Awake()
    {
        foreach (var d in datas)
        {
            _dataDic.Add(d.name, d.particle);
        }
    }

    public void PlayParticle(EnemyAttack enemyAttack)
    {
        enemyAttack.attack.gameObject.SetActive(true);
        enemyAttack.attack.StartParticle(null,OnEndEffect);
    }
    public void PlayParticle(string particleName)
    {
        if(_dataDic.TryGetValue(particleName, out ParticleSystem p))
        {
            p.Play();
        }
    }
    public void EmitParticle(string particleName)
    {
        if (_dataDic.TryGetValue(particleName, out ParticleSystem p))
        {
            p.Emit(1);
        }
    }
}
