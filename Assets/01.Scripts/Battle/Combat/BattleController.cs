using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Playables;

[Serializable]
public class SEList<T>
{
    public List<T> list;
}

public class BattleController : MonoSingleton<BattleController>
{
    private SEList<SEList<bool>> _isStuckCheckList;

    public Enemy[] OnFieldMonsterArr { get; private set; }
    public List<Enemy> DeathEnemyList { get; private set; } = new List<Enemy>();
    public List<Enemy> SpawnEnemyList { get; private set; } = new List<Enemy>();

    private HpBarMaker _hpBarMaker;

    [SerializeField] [Range(0.01f, 0.1f)] private float _spawnTurm;

    private EnemyGroupSO _enemyGroup;

    public List<Vector3> EnemyGroupPosList { get; set; }

    private Vector3 _formationCenterPos = Vector3.zero;
    public Vector3 FormationCenterPos
    {
        get
        {
            if(_formationCenterPos != Vector3.zero) return _formationCenterPos;

            Vector3 centerPos = Vector3.zero;

            foreach(Vector3 pos in EnemyGroupPosList)
            {
                centerPos += pos;
            }

            return centerPos / EnemyGroupPosList.Count;
        }
    }
    private Queue<PoolingType> _enemyQue = new Queue<PoolingType>();

    private Player _player;
    public Player Player
    {
        get
        {
            if (_player != null) return _player;
            _player = FindObjectOfType<Player>();
            return _player;
        }
    }
    private bool _isGameEnd;
    public bool IsGameEnd
    {
        get => _isGameEnd;
        set
        {
            _isGameEnd = value;
            if (_isGameEnd)
            {

                for (int i = 0; i < OnFieldMonsterArr.Length; i++)
                {
                    Enemy e = OnFieldMonsterArr[i];
                    if (e == null) continue;

                    OnFieldMonsterArr[i] = null;
                    e.turnStatus = TurnStatus.End;
                    e.GotoPool();
                    //PoolManager.Instance.Push(e);
                }

                CostCalculator.Init();
                //SelectPlayerTarget(null, null);

                //UIManager.Instance.GetSceneUI<BattleUI>().SystemActive?.Invoke(true);
                _hpBarMaker.DeleteAllHPBar();
                StopAllCoroutines();
            }
        }
    }

    public Action OnChangeTurnEnemy;

    [SerializeField] private UnityEvent<Enemy, Vector3> _maskCreateEvent;
    public UnityEvent<Enemy> maskEnableEvent;
    public UnityEvent<Enemy> maskDisableEvent;
    public CameraController CameraController { get; private set; }


    private Action _spawnCallBack;

    private void Start()
    {
        EnemyGroupPosList = StageManager.Instanace.SelectStageData.enemyFormation.positionList;

        _hpBarMaker = FindObjectOfType<HpBarMaker>();

        CameraController = FindObjectOfType<CameraController>();
        CameraController.BattleController = this;


        OnFieldMonsterArr = new Enemy[EnemyGroupPosList.Count];
        CardReader.SkillCardManagement.useCardEndEvnet.AddListener(CalculateDeathEntity);
        TurnCounter.PlayerTurnStartEvent += HandleCardDraw;
        TurnCounter.EnemyTurnStartEvent += OnEnemyTurnStart;
        TurnCounter.EnemyTurnEndEvent += OnEnemyTurnEnd;


        Player.BattleController = this;
        _hpBarMaker.SetupHpBar(Player);
        Player.HealthCompo.OnDeathEvent.AddListener(() => IsGameEnd = true);
    }


    private void HandleCardDraw(bool obj)
    {
        BattleReader.CardDrawer.DrawCard(3, false);
    }


        private void OnDestroy()
    {
        CardReader.SkillCardManagement.useCardEndEvnet.RemoveListener(CalculateDeathEntity);

        TurnCounter.EnemyTurnStartEvent -= OnEnemyTurnStart;
        TurnCounter.EnemyTurnEndEvent -= OnEnemyTurnEnd;
        TurnCounter.PlayerTurnStartEvent -= HandleCardDraw;
    }
    private void OnEnemyTurnStart(bool value)
    {
        foreach (var e in OnFieldMonsterArr)
        {
            if (e is null) continue;

            e.TurnStart();
        }
        StartCoroutine(EnemySquence());
    }



    private void OnEnemyTurnEnd()
    {
        foreach (var e in OnFieldMonsterArr)
        {
            if (e is null) continue;

            e.TurnEnd();
            maskDisableEvent?.Invoke(e);
        }
    }


    private IEnumerator EnemySquence()
    {
        foreach (var e in OnFieldMonsterArr)
        {
            if (e is null) continue;
            BackGroundFadeOut();

            e.TurnAction();
            yield return new WaitUntil(() => e.turnStatus == TurnStatus.End);
             DamageTextManager.Instance.PushAllText();

            OnChangeTurnEnemy?.Invoke();

            CalculateDeathEntity();
            BackGroundFadeIn();

            if (_isGameEnd) break;
            yield return new WaitForSeconds(1.5f);
        }

        if (!_isGameEnd)
        {
            TurnCounter.ChangeTurn();
        }
    }
    private void CalculateDeathEntity()
    {
        foreach (var e in OnFieldMonsterArr)
        {
            if (e is null) continue;

            if (e.HealthCompo.IsDead)
                e.HealthCompo.InvokeDeadEvent();
        }
        if (Player.HealthCompo.IsDead)
            Player.HealthCompo.InvokeDeadEvent();
    }
    public void SetStage()
    {
        if (StageManager.Instanace.SelectStageData.stageCutScene != null) return;
        InitField();
    }
    public void InitField()
    {
        foreach (var e in StageManager.Instanace.SelectStageData.enemyGroup.firstSpawns)
        {
            if (!SpawnMonster(e.enemy.poolingType, e.mapIdx))
            {
                _enemyQue.Enqueue(e.enemy.poolingType);
            }
        }
        for (int i = 0; i < EnemyGroupPosList.Count; i++)
        {
            if (_enemyQue.Count > 0)
                SpawnMonster(_enemyQue.Dequeue(), i);
        }
    }
    private bool SpawnMonster(PoolingType enemyType, int idx)
    {
        if (OnFieldMonsterArr[idx] != null)
            return false;
        Vector3 selectPos = EnemyGroupPosList[idx];
        Enemy selectEnemy = PoolManager.Instance.Pop(enemyType) as Enemy;

        selectEnemy.transform.position = selectPos;
        selectEnemy.BattleController = this;

        int posChecker = ((idx + 3) % 2) * 2;

         _spawnCallBack = null;
            _spawnCallBack += ()=> _maskCreateEvent?.Invoke(selectEnemy, selectPos);
            _spawnCallBack += ()=> _hpBarMaker.SetupHpBar(selectEnemy);

            selectEnemy.Spawn(selectPos, _spawnCallBack);

        selectEnemy.SpriteRendererCompo.sortingOrder = posChecker;

        selectEnemy.HealthCompo.OnDeathEvent.AddListener(() => DeadMonster(selectEnemy));

        OnFieldMonsterArr[idx] = selectEnemy;
        selectEnemy.target = Player;

        SpawnEnemyList.Add(selectEnemy);
        return true;

    }

    public void DeadMonster(Enemy enemy)
    {
        OnFieldMonsterArr[Array.IndexOf(OnFieldMonsterArr, enemy)] = null;

        DeathEnemyList.Add(enemy);
        maskDisableEvent?.Invoke(enemy);
    }

    public bool IsStuck(int to, int who)
    {
        return _isStuckCheckList.list[to].list[who];
    }

    public void ChangePosition(Transform e1, Transform e2, Action callback = null)
    {
        e1.DOMove(e2.position, 0.5f);
        e2.DOMove(e1.position, 0.5f).OnComplete(() => callback?.Invoke());
    }
    public void ChangeXPosition(Transform e1, Transform e2, Action callback = null)
    {
        e1.DOMoveX(e2.position.x, 0.5f);
        e2.DOMoveX(e1.position.x, 0.5f).OnComplete(() => callback?.Invoke());
    }

    public void SelectPlayerTarget(CardBase cardBase, Entity entity)
    {
        Player.SaveSkillToEnemy(cardBase, entity);
    }


    public void BackGroundFadeIn()
    {
        Player.VFXManager.SetBackgroundFadeIn(0.5f);
    }

    public void BackGroundFadeOut()
    {
        Player.VFXManager.SetBackgroundFadeOut(0.5f);
    }
}
