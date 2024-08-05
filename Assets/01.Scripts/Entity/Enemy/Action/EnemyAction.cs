using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum EnemyActionEnum
{
	EnemyHeal,
	EnemySeedGunAttack,
	EnemyCandyLaser,
	EnemyHew,
	EnemyJumpAttack,
	EnemyRollingAttack,
	EnemySlideAttack,
	EnemyThorwMelon,
	EnemyThrowKiwi,
	EnemyUltraSound
}
public abstract class EnemyAction : ITurnAction
{
	public Image stateIcon;
	public CameraMoveTypeSO camInfo;
	public AudioClip actionSound;
	protected Enemy _owner;
	protected bool isRunning;
	public EnemyAction(Enemy owner, Image actionIcon, CameraMoveTypeSO cameraInfo, AudioClip skillSound)
	{ _owner = owner; stateIcon = actionIcon; camInfo = cameraInfo; actionSound = skillSound; }
	public abstract void Init();
	public abstract IEnumerator Execute();

	public bool CanUse()
	{
		return !_owner.HealthCompo.IsDead;
	}
}
