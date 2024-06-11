using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Particle;

[Serializable]
public struct PlayerVFXData
{
    public CardInfo info;
    public ParticleSystem[] particle;
    public ParticlePoolObject poolObject;
}

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField] private List<PlayerVFXData> cardAndEffects = new();
    private Dictionary<CardInfo, ParticleSystem[]> _cardByEffects = new();
    private Dictionary<CardInfo, ParticlePoolObject> _cardByEffects2 = new();

    public Action OnEffectEvent;
    public Action OnEndEffectEvent;

    [SerializeField] private Player _player;
    [SerializeField] private SpriteRenderer _fadePanel;

    private void Awake()
    {
        foreach (var c in cardAndEffects)
        {
            if (!_cardByEffects.ContainsKey(c.info))
            {
                _cardByEffects.Add(c.info, c.particle);
                _cardByEffects2.Add(c.info, c.poolObject);
            }
            else
            {
                Debug.LogError("?!");
            }
        }

    }

    internal void EndParticle(CardInfo cardInfo, int combineLevel)
    {
        if (!_cardByEffects.ContainsKey(cardInfo))
        {
            Debug.LogError("����Ʈ�� �����");
            return;
        }
        _cardByEffects[cardInfo][combineLevel].Stop();
    }

    public void PlayParticle(CardInfo card, Vector3 pos, int combineLevel)
    {
        if (!_cardByEffects.ContainsKey(card))
        {
            Debug.LogError("����Ʈ�� �����");
            return;
        }
        _cardByEffects[card][combineLevel].transform.position = pos;
        _cardByEffects[card][combineLevel].gameObject.SetActive(true);
        SetBackgroundFadeOut(1);
        ParticleSystem.MainModule mainModule = _cardByEffects[card][combineLevel].main;
        StartCoroutine(EndEffectCo(mainModule.startLifetime.constantMax / mainModule.simulationSpeed));
        _cardByEffects[card][combineLevel].Play();
    }
    public void PlayParticle(CardBase card, Vector3 pos)
    {
        int level = (int)card.CombineLevel;
        ParticlePoolObject obj = PoolManager.Instance.Pop(_cardByEffects2[card.CardInfo].poolingType) as ParticlePoolObject;
        obj.transform.position = pos;
        obj[level].owner = _player;
        obj[level].damages = card.GetDamage(card.CombineLevel);
        foreach (var t in _player.GetSkillTargetEnemyList[card])
        {
            obj[level].AddTriggerTarget(t);
        }
        obj.Active(level, OnEffectEvent, OnEndEffectEvent);
    }
    public void PlayParticle(CardBase card, Vector3 pos, out ParticlePoolObject particle)
    {
        int level = (int)card.CombineLevel;
        ParticlePoolObject obj = PoolManager.Instance.Pop(_cardByEffects2[card.CardInfo].poolingType) as ParticlePoolObject;
        obj.transform.position = pos;
        obj[level].owner = _player;
        obj[level].damages = card.GetDamage(card.CombineLevel);
        foreach (var t in _player.GetSkillTargetEnemyList[card])
        {
            obj[level].AddTriggerTarget(t);
        }
        obj.Active(level, null, OnEndEffectEvent);
        particle = obj;
    }

    public void PlayParticle(CardInfo card, int combineLevel)
    {
        if (!_cardByEffects.ContainsKey(card))
        {
            Debug.LogError("����Ʈ�� �����");
            return;
        }

        _cardByEffects[card][combineLevel].gameObject.SetActive(true);
        SetBackgroundFadeOut(1);
        ParticleSystem.MainModule mainModule = _cardByEffects[card][combineLevel].main;
        StartCoroutine(EndEffectCo(mainModule.startLifetime.constantMax / mainModule.simulationSpeed));
        _cardByEffects[card][combineLevel].Play();
    }

    private IEnumerator EndEffectCo(float f)
    {
        yield return new WaitForSeconds(f);
        SetBackgroundFadeIn(1);
        OnEndEffectEvent?.Invoke();
    }

    public void SetBackgroundFadeOut(float time)
    {
        _fadePanel.DOFade(0.7f, time);
    }

    public void SetBackgroundFadeIn(float time)
    {
        _fadePanel.DOFade(0f, time);
    }
}
