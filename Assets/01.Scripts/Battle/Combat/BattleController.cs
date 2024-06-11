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
    [SerializeField] private SEList<SEList<bool>> isStuck;

    public Enemy[] onFieldMonsterList;
    public List<Enemy> DeathEnemyList { get; private set; } = new List<Enemy>();
    public List<Enemy> SpawnEnemyList { get; private set; } = new List<Enemy>();

    [HideInInspector] private HpBarMaker _hpBarMaker;

    [Header("���� ��")]
    [SerializeField] [Range(0.01f, 0.1f)] private float _spawnTurm;

    [SerializeField] private EnemyGroupSO _enemyGroup;

    [SerializeField] private List<Transform> enemySpawnTrm = new();
    [HideInInspector] public List<Vector3> enemySpawnPos = new();
    [SerializeField] private Transform enemyGroupCenter;
    [HideInInspector] public Vector3 enemyGroupPos;
    private Queue<PoolingType> _enemyQue = new Queue<PoolingType>();

    [SerializeField] private Player _player;
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
                for (int i = 0; i < onFieldMonsterList.Length; i++)
                {
                    Enemy e = onFieldMonsterList[i];
                    if (e == null) continue;

                    onFieldMonsterList[i] = null;
                    e.turnStatus = TurnStatus.End;
                    e.GotoPool();
                    //PoolManager.Instance.Push(e);
                }

                OnGameEndEvent?.Invoke();
                CostCalculator.Init();
                //SelectPlayerTarget(null, null);

                //UIManager.Instance.GetSceneUI<BattleUI>().SystemActive?.Invoke(true);
                _hpBarMaker.DeleteAllHPBar();
                StopAllCoroutines();
            }
        }
    }

    public Action OnChangeTurnEnemy;
    [SerializeField] private UnityEvent OnGameEndEvent;
    [SerializeField] private UnityEvent<Enemy, Vector2> _maskCreateEvent;
    public UnityEvent<Enemy> maskEnableEvent;
    public UnityEvent<Enemy> maskDisableEvent;
    public CameraController CameraController { get; private set; }

    private void Start()
    {
        enemySpawnPos.Clear();

        foreach (var p in enemySpawnTrm)
        {
            enemySpawnPos.Add(p.position);
        }
        enemyGroupPos = enemyGroupCenter.position;

        _hpBarMaker = FindObjectOfType<HpBarMaker>();

        CameraController = FindObjectOfType<CameraController>();
        CameraController.BattleController = this;

        onFieldMonsterList = new Enemy[enemySpawnPos.Count];

        CardReader.SkillCardManagement.useCardEndEvnet.AddListener(CalculateDeathEntity);
        TurnCounter.PlayerTurnStartEvent += HandleCardDraw;
        TurnCounter.EnemyTurnStartEvent += OnEnemyTurnStart;
        TurnCounter.EnemyTurnEndEvent += OnEnemyTurnEnd;

        _enemyGroup = MapManager.Instanace.SelectStageData.enemyGroup;
        foreach (var e in _enemyGroup.enemies)
        {
            _enemyQue.Enqueue(e.poolingType);
        }
        Player.BattleController = this;
        _hpBarMaker.SetupHpBar(Player);
        Player.HealthCompo.OnDeathEvent.AddListener(() => IsGameEnd = true);
    }
    private void HandleCardDraw(bool obj)
    {
        CardReader.CardDrawer.DrawCard(3, false);
    }
    private void OnDestroy()
    {
        CardReader.SkillCardManagement.useCardEndEvnet.RemoveListener(CalculateDeathEntity);

        TurnCounter.EnemyTurnStartEvent -= OnEnemyTurnStart;
        TurnCounter.EnemyTurnEndEvent -= OnEnemyTurnEnd;
        TurnCounter.PlayerTurnStartEvent -= HandleCardDraw;
    }
    #region 적 공격
    private void OnEnemyTurnStart(bool value)
    {
        foreach (var e in onFieldMonsterList)
        {
            if (e is null) continue;

            e.TurnStart();
            maskEnableEvent?.Invoke(e);
        }
        StartCoroutine(EnemySquence());
    }
    private void OnEnemyTurnEnd()
    {
        foreach (var e in onFieldMonsterList)
        {
            if (e is null) continue;

            e.TurnEnd();
            maskDisableEvent?.Invoke(e);
        }
    }
    private IEnumerator EnemySquence()
    {
        foreach (var e in onFieldMonsterList)
        {
            if (e is null) continue;
            Player.VFXManager.SetBackgroundColor(Color.gray);

            e.TurnAction();

            yield return new WaitUntil(() => e.turnStatus == TurnStatus.End);
            DamageTextManager.Instance.PushAllText();

            OnChangeTurnEnemy?.Invoke();

            CalculateDeathEntity();
            Player.VFXManager.SetBackgroundColor(Color.white);
            if (_isGameEnd)
                break;
            yield return new WaitForSeconds(1.5f);
        }

        if (!_isGameEnd)
        {
            TurnCounter.ChangeTurn();
        }
    }
    #endregion
    private void CalculateDeathEntity()
    {
        foreach (var e in onFieldMonsterList)
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
        Debug.Log(MapManager.Instanace.SelectStageData.enemyGroup);


        if (MapManager.Instanace.SelectStageData.stageCutScene != null) return;
        InitField();
    }
    public void InitField()
    {
        foreach (var e in MapManager.Instanace.SelectStageData.enemyGroup.firstSpawns)
        {
            if (!SpawnMonster(e.enemy.poolingType, e.mapIdx))
            {
                _enemyQue.Enqueue(e.enemy.poolingType);
            }
        }
        for (int i = 0; i < enemySpawnPos.Count; i++)
        {
            if (_enemyQue.Count > 0)
                SpawnMonster(_enemyQue.Dequeue(), i);
        }
    }
    private bool SpawnMonster(PoolingType enemyType, int idx)
    {
        if (onFieldMonsterList[idx] != null)
            return false;
        Vector3 pos = enemySpawnPos[idx];
        print(pos);
        Enemy selectEnemy = PoolManager.Instance.Pop(enemyType) as Enemy;
        selectEnemy.transform.position = pos;
        selectEnemy.BattleController = this;
        int posChecker = ((idx + 3) % 2) * 2;
        selectEnemy.Spawn(pos);
        _maskCreateEvent?.Invoke(selectEnemy, pos);
        selectEnemy.SpriteRendererCompo.sortingOrder = posChecker;

        selectEnemy.HealthCompo.OnDeathEvent.AddListener(() => DeadMonster(selectEnemy));

        onFieldMonsterList[idx] = selectEnemy;
        selectEnemy.target = Player;

        SpawnEnemyList.Add(selectEnemy);
        _hpBarMaker.SetupHpBar(selectEnemy);
        return true;
    }

    public void DeadMonster(Enemy enemy)
    {
        onFieldMonsterList[Array.IndexOf(onFieldMonsterList, enemy)] = null;

        DeathEnemyList.Add(enemy);
        maskDisableEvent?.Invoke(enemy);
    }
    public bool IsStuck(int to, int who)
    {
        return isStuck.list[to].list[who];
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
    public void BackgroundColor(Color color)
    {
        Player.VFXManager.SetBackgroundColor(color);
    }
}
