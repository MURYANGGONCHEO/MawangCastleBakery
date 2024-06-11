using DG.Tweening;
using Particle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Sequence = DG.Tweening.Sequence;

[Serializable]
public struct EnemyAttack
{
    public ParticleInfo attack;
}

public abstract class Enemy : Entity
{
    [SerializeField] protected EnemyAttack attackParticle;
    [SerializeField] protected CameraMoveTypeSO _cameraMoveInfo;

    protected int attackAnimationHash = Animator.StringToHash("attack");
    protected int attackTriggerAnimationHash = Animator.StringToHash("attackTrigger");
    protected int spawnAnimationHash = Animator.StringToHash("spawn");

    public EnemyVFXPlayer VFXPlayer { get; private set; }
    protected Collider2D Collider;

    protected override void Awake()
    {
        base.Awake();
        VFXPlayer = GetComponent<EnemyVFXPlayer>();
        Collider = GetComponent<Collider2D>();

        if(attackParticle.attack != null)
            SetParticleInfo();

    }
    private void SetParticleInfo()
    {
        attackParticle.attack.owner = this;
        attackParticle.attack.damages = SetDamage((EnemyStat)CharStat);

    }
    private int[] SetDamage(EnemyStat stat)
    {
        int[] list = new int[stat.attackCnt];
        for (int i = 0; i < stat.attackCnt; i++)
        {
            list[i] = stat.damage.GetValue();
        }
        return list;
    }
    protected virtual void HandleAttackStart()
    {
        BattleController.Player.VFXManager.SetBackgroundColor(Color.gray);
        AnimatorCompo.SetBool(attackAnimationHash, true);
    }
    protected virtual void HandleAttackEnd()
    {
        BattleController.Player.VFXManager.SetBackgroundColor(Color.white);
        AnimatorCompo.SetBool(attackAnimationHash, false);
    }
    public virtual void HandleCameraAction()
    {
        BattleController.CameraController.StartCameraSequnce(_cameraMoveInfo);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HealthCompo.OnDeathEvent.AddListener(GotoPool);

        OnAttackStart += HandleAttackStart;
        OnAttackStart += HandleCameraAction;
        OnAttackEnd += HandleAttackEnd;
        target = BattleController?.Player;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        HealthCompo.OnDeathEvent.RemoveListener(GotoPool);
        OnAttackStart -= HandleAttackStart;
        OnAttackStart -= HandleCameraAction;
        OnAttackEnd -= HandleAttackEnd;
    }
    public abstract void Attack();
    public virtual void TurnStart()
    {
        turnStatus = TurnStatus.Ready;
        Collider.enabled = false;
    }
    public abstract void TurnAction();
    public virtual void TurnEnd()
    {
        turnStatus = TurnStatus.End;
        Collider.enabled = true;
        ChainningCardList.Clear();
    }
    public virtual void Spawn(Vector3 spawnPos)
    {
        SpriteRendererCompo.material.SetFloat("_dissolve_amount", 0);

        AnimatorCompo.SetBool(spawnAnimationHash, true);

        transform.position = spawnPos + new Vector3(-4f, 6f);
        transform.DOMoveX(spawnPos.x, 1f);
        transform.DOMoveY(spawnPos.y, 1f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            AnimatorCompo.SetBool(spawnAnimationHash, false);
            turnStatus = TurnStatus.Ready;
        });
    }
    public void MoveFormation(Vector3 pos)
    {
        transform.DOMove(pos, 1f);
    }

    public void SelectedOnAttack(CardBase selectCard)
    {
        BattleController.SelectPlayerTarget(selectCard, this);
    }

    protected override void HandleDie()
    {
        base.HandleDie();
        EnemyStat es = CharStat as EnemyStat;
        Inventory.Instance.GetIngredientInThisBattle.Add(es.DropItem);
        Inventory.Instance.AddItem(es.DropItem);
    }
}