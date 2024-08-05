using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct EnemyActionData
{
	public EnemyActionEnum actionType;
	public Image actionIcon;
	public CameraMoveTypeSO cameraInfo;
	public AudioClip skillSound;
}
public abstract class Enemy : Entity
{
	[SerializeField] private List<EnemyActionData> statesData = new();
	protected List<EnemyAction> states = new();
	[SerializeField] private SpawnDataSO spawnData;
	[SerializeField] private SpriteDessolve dessolve;

	protected override void Awake()
	{
		base.Awake();

	}
	protected override void Start()
	{
		foreach (var data in statesData)
		{
			Type t = Type.GetType(data.actionType.ToString());
			var cState = Activator.CreateInstance(t, this, data.actionIcon, data.cameraInfo, data.skillSound) as EnemyAction;
			cState.Init();
			states.Add(cState);
		}
	}
	public ITurnAction GetState()
	{
		return states[UnityEngine.Random.Range(0, states.Count)];
	}

	public void Spawn(Vector3 selectPos, Action spawnCallBack)
	{
		spawnData.SpawnSeq(transform, selectPos, spawnCallBack);
	}
	public void SelectedOnAttack(CardBase selectCard)
	{
		BattleController.SelectPlayerTarget(selectCard, this);
	}
	public bool CanAction()
	{
		if (HealthCompo.IsDead) return false;
		return true;
	}
}
