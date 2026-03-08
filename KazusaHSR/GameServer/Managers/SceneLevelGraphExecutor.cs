using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KazusaHSR.GameServer.Resource;
using KazusaHSR.Protocol;
using KazusaHSR.Resource;
using Newtonsoft.Json;

namespace KazusaHSR.GameServer;

public sealed class SceneLevelGraphExecutor
{
    private readonly Scene _scene;
    private readonly Session _session;
    private readonly Logger _log;

    private readonly List<PendingWaitPredicate> _pending = new();

    private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        Converters = new List<JsonConverter> { new TaskConfigJsonConverter() }
    };

    private sealed class PendingWaitPredicate
    {
        public WaitPredicateSucc WaitConfig { get; init; } = null!;
        public List<TaskConfig> NextTasks { get; init; } = new();
        public uint GroupId { get; init; }
    }

    public SceneLevelGraphExecutor(Scene scene)
    {
        _scene = scene ?? throw new ArgumentNullException(nameof(scene));
        _session = scene.session ?? throw new ArgumentNullException(nameof(scene.session));
        _log = _session.c;
    }

    public void StartForGroup(uint groupId, LevelGroupInfo groupInfo)
    {
        if (groupInfo == null)
            return;

        if (string.IsNullOrEmpty(groupInfo.LevelGraph))
            return;

        try
        {
            var config = LoadLevelGraph(groupInfo.LevelGraph);
            if (config == null)
            {
                _log.LogWarning($"[SceneLevelGraphExecutor] Failed to load LevelGraph '{groupInfo.LevelGraph}' for group {groupInfo.GroupGUID}");
                return;
            }

            _log.LogInfo($"[SceneLevelGraphExecutor] Starting LevelGraph '{groupInfo.LevelGraph}' for group {groupInfo.GroupGUID} (GroupId={groupId})");

            ExecuteSequences(config.OnInitSequece ?? Array.Empty<LevelTaskSequence>(), groupId);
            ExecuteSequences(config.OnStartSequece ?? Array.Empty<LevelTaskSequence>(), groupId);
        }
        catch (Exception ex)
        {
            _log.LogWarning($"[SceneLevelGraphExecutor] Exception while loading LevelGraph '{groupInfo.LevelGraph}' for group {groupInfo.GroupGUID}: {ex.Message}");
        }
    }

    private LevelGraphConfig? LoadLevelGraph(string levelGraphPath)
    {
        try
        {
            // levelGraphPath is like "Config/Level/Maze/.../Maze_ChallengeTower_Check2.json"
            string basePath = MainApp.resourceManager.loader._baseResourcePath;
            string relative = levelGraphPath.Replace('/', Path.DirectorySeparatorChar);
            string fullPath = Path.Combine(basePath, relative);

            if (!File.Exists(fullPath))
            {
                _log.LogWarning($"[SceneLevelGraphExecutor] LevelGraph file not found: {fullPath}");
                return null;
            }

            string json = File.ReadAllText(fullPath);
            var config = JsonConvert.DeserializeObject<LevelGraphConfig>(json, _jsonSettings);
            return config;
        }
        catch (Exception ex)
        {
            _log.LogWarning($"[SceneLevelGraphExecutor] Failed to load LevelGraph '{levelGraphPath}': {ex.Message}");
            return null;
        }
    }

    private void ExecuteSequences(IEnumerable<LevelTaskSequence> sequences, uint groupId)
    {
        if (sequences == null)
            return;

        foreach (var seq in sequences)
        {
            if (seq?.TaskList == null || seq.TaskList.Length == 0)
                continue;

            ExecuteTaskList(seq.TaskList, groupId);
        }
    }

    private void ExecuteTaskList(IReadOnlyList<TaskConfig> tasks, uint groupId)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            if (task == null)
                continue;

            if (task is WaitPredicateSucc wait)
            {
                var nextTasks = tasks.Skip(i + 1).Where(t => t != null).ToList();
                RegisterWaitPredicate(wait, nextTasks, groupId);
                break;
            }
            else
            {
                ExecuteImmediateTask(task, groupId);
            }
        }
    }

    private void RegisterWaitPredicate(WaitPredicateSucc wait, List<TaskConfig> nextTasks, uint groupId)
    {
        if (wait.Condition is ByCompareGroupMonsterNum monsterNum)
        {
            uint condGroupId = monsterNum.GroupID != 0 ? monsterNum.GroupID : groupId;
            var pending = new PendingWaitPredicate
            {
                WaitConfig = wait,
                NextTasks = nextTasks,
                GroupId = condGroupId,
            };

            _pending.Add(pending);
            _log.LogInfo($"[SceneLevelGraphExecutor] Registered WaitPredicateSucc(ByCompareGroupMonsterNum) for GroupId={condGroupId}, TargetNum={monsterNum.Number}");
        }
        else
        {
            _log.LogInfo($"[SceneLevelGraphExecutor] WaitPredicateSucc with unsupported condition type {wait.Condition?.GetType().FullName}, ignoring");
        }
    }

    private void ExecuteImmediateTask(TaskConfig task, uint groupId)
    {
        switch (task)
        {
            case ChangePropState change:
                ExecuteChangePropState(change);
                break;

            default:
                _log.LogInfo($"[SceneLevelGraphExecutor] Unhandled task type in level graph: {task.GetType().FullName}");
                break;
        }
    }

    private void ExecuteChangePropState(ChangePropState config)
    {
        if (config.DynamicGroupID == null || config.DynamicGroupPropID == null)
        {
            _log.LogWarning("[SceneLevelGraphExecutor] ChangePropState missing DynamicGroupID or DynamicGroupPropID");
            return;
        }

        uint groupId = (uint)config.DynamicGroupID.Evaluate();
        uint instId = (uint)config.DynamicGroupPropID.Evaluate();

        var propEntity = _scene.EntityManager.Entities.Values
            .OfType<PropEntity>()
            .FirstOrDefault(p => p.GroupId == groupId && p.DbInfo.ID == instId);

        if (propEntity == null)
        {
            _log.LogWarning($"[SceneLevelGraphExecutor] ChangePropState: Prop not found for GroupId={groupId}, InstId={instId}");
            return;
        }

        propEntity.State = config.State;

        var notify = new SceneEntityUpdateScNotify();
        notify.EntityLists.Add(propEntity.ToSceneEntityInfo());
        _session.SendPacket(notify);

        _log.LogInfo($"[SceneLevelGraphExecutor] ChangePropState applied: GroupId={groupId}, InstId={instId}, NewState={config.State}");
    }

    public void OnMonstersChanged()
    {
        if (_pending.Count == 0)
            return;

        var snapshot = _pending.ToList();
        foreach (var pending in snapshot)
        {
            if (pending.WaitConfig.Condition is not ByCompareGroupMonsterNum cond)
                continue;

            uint groupId = pending.GroupId;
            uint targetNum = cond.Number;

            int currentCount = _scene.EntityManager.Entities.Values
                .OfType<MonsterEntity>()
                .Count(m => m.GroupId == groupId);

            if (currentCount == targetNum)
            {
                _log.LogInfo($"[SceneLevelGraphExecutor] WaitPredicateSucc satisfied for GroupId={groupId}: {currentCount} monsters");
                _pending.Remove(pending);

                if (pending.NextTasks != null && pending.NextTasks.Count > 0)
                {
                    ExecuteTaskList(pending.NextTasks, groupId);
                }
            }
        }
    }
}
