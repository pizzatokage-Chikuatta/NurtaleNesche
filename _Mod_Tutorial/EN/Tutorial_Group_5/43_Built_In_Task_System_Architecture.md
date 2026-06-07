Built-In Task System Architecture
================================

This tutorial explains how the built-in task system is shaped.

Read this after 41_Task_Provider_Code_Mod.md. This is not the first tutorial for writing a DLL, but it is important if you want to make serious task-related code mods.

Core Idea
--------

Patrollers do not directly "just run AI code" every frame.

The current flow is:

1. ActionScheduler2 asks task providers if they can propose a task.
2. A provider returns `null` if it has nothing to do.
3. A provider returns `PatrollerTaskInfoContainer` if it wants to run behavior.
4. The scheduler compares priority and current task state.
5. If accepted, the scheduler runs the states and state args inside the task container.

The important object is:

~~~text
PatrollerTaskInfoContainer
~~~

It usually contains:

1. provider/owner
2. list of states
3. list of state args
4. task priority
5. target captive info when needed
6. hurriedness
7. task tag

TaskProviderCore
--------

`TaskProviderCore` is the base class for plain C# providers.

Important built-in fields/helpers:

1. `controller`: the owner patroller.
2. `memory`: the owner patroller memory.
3. `priority`: configured provider priority.
4. `ProviderCooldownMedian`: provider cooldown from JSON.
5. `IsConditionMetForPriority(...)`: checks enabled state, cooldown, event blocks, and current scheduler priority.
6. `WithSpeechPrelude(...)`: optionally attaches speech before the task.
7. `GetTaskTag(...)`: reads configured task tag strings.

Simple providers inherit `TaskProviderCore` or `TaskSetCore` and implement `GetNewTask`.

TaskSetCore
--------

`TaskSetCore` is a task provider that often owns child options.

It still emits one task to the scheduler, but it can choose that task from multiple child options.

Built-in examples:

1. `SoloActionTaskSet`
2. `ChaseTaskSet`
3. `AbilityTaskSet`
4. `HandlingCaptivesTaskSet`
5. `ArrowHitEventTaskSet`
6. `PotionHitEventTaskSet`

TaskOption
--------

`TaskOption` is not usually polled directly by ActionScheduler2.

Instead:

1. A TaskSet owns child TaskOptions.
2. The TaskSet asks each option whether it can create a proposal.
3. The TaskSet chooses the highest-priority proposal.
4. If multiple proposals have the same priority, one can be chosen randomly.
5. The TaskSet returns the chosen task to the scheduler.

Task options usually have:

1. `Priority`
2. `ApplyConfig`
3. cooldown
4. optional `Tick`
5. task-building logic

Normal TaskSet Example: SoloActionTaskSet
--------

Built-in ID registration:

~~~csharp
reg.RegisterPlainProvider("task_set.solo_action", () => new Patroller.SoloActionTaskSet());
~~~

Built-in child option registration:

~~~csharp
reg.Register("task_option.solo_action.patrol", typeof(SoloPatrolTaskOption));
reg.Register("task_option.solo_action.grab_captive", typeof(SoloGrabCaptiveTaskOption));
~~~

Patroller Data shape:

~~~json
{
  "id": "task_set.solo_action",
  "priority": "Patrol",
  "providerCooldownMedian": 0.5,
  "options": [
    {
      "id": "task_option.solo_action.patrol",
      "enabled": true,
      "config": {
        "idleBeforeSecondsMin": 1.5,
        "idleBeforeSecondsMax": 2.0
      }
    }
  ]
}
~~~

Meaning:

1. `task_set.solo_action` creates the parent task set.
2. The task set reads `options`.
3. `task_option.solo_action.patrol` creates a child option.
4. The child option reads its own `config`.
5. The child option builds a patrol task when possible.

Built-in option behavior, simplified:

~~~csharp
public override PatrollerTaskForSoloAction GetSoloActionTask(SoloActionTaskSet owner, BaseController controller)
{
    if (owner == null || controller == null)
        return null;

    Checkpoint checkpoint = ResolveCheckpoint(owner, controller);
    if (checkpoint == null)
        return null;

    var task = new PatrollerTaskInfoContainer(
        owner,
        states,
        args,
        owner.SchedulerPriority,
        null,
        null,
        HurriednessEnum.Free,
        owner.BuildTaskTagForOption("patrol"));

    return new PatrollerTaskForSoloAction(Priority, task);
}
~~~

Ability TaskSet Example: Goblin Hunting Net
--------

Built-in provider ID:

~~~text
task_set.ability.goblin
~~~

Built-in option ID:

~~~text
task_option.ability.goblin.hunting_net
~~~

The parent `AbilityTaskSet` chooses ability options. `GoblinHuntingNetTaskOption` is a child option that decides whether the goblin can throw a hunting net.

Important pattern:

1. The option reads JSON in `ApplyConfig`.
2. The option resolves runtime references in `Initialize`.
3. The option can subscribe/unsubscribe in `SetEnable` and `SetDisable`.
4. The option builds a `PatrollerTaskForAbility`.
5. The option uses a commit callback to spend the net only when the task proposal is accepted.

This is more advanced than the beginner task-provider tutorial because it has state, cooldown, event subscriptions, and save/load.

Event TaskSet Example: ArrowHitEventTaskSet
--------

Event task sets are different from normal scheduler-polled task sets.

`ArrowHitEventTaskSet.GetNewTask()` returns `null` because arrow hits are not proposed every scheduler tick.

Instead, the flow is:

1. Arrow collision reaches the patroller hit manager.
2. The hit manager calls the arrow-hit event task set.
3. The event task set creates an `ArrowHitEventContext`.
4. It asks child `IArrowHitEventTaskOption` handlers if they can handle it.
5. It chooses the highest-priority handler.
6. The chosen handler returns `ArrowHitEventResult`.
7. If the result contains a task, the event task set can force-switch the scheduler to that task.

Built-in provider ID:

~~~text
event_task_set.arrow_hit
~~~

Built-in option IDs:

~~~text
event_task_option.arrow_hit.general
event_task_option.arrow_hit.ignore_self_heal.masked_orc
event_task_option.arrow_hit.ignore_underground.golem
event_task_option.arrow_hit.pass_through.tentacle
event_task_option.arrow_hit.ignore.slave
event_task_option.arrow_hit.shadow_creature_special
event_task_option.arrow_hit.shield_block.fat_goblin
event_task_option.arrow_hit.arrow_slash.bodyguard
~~~

Important:

Event task options can block or replace normal hit behavior. Be conservative.

Built-In ID Registries
--------

Built-in task providers include:

~~~text
task_set.solo_action
task_set.chase
task_set.collect_captives
task_set.ability.goblin
task_set.ability.golem
task.suspicion
task_set.handling_captives
task.fallback_idle
event_task_set.arrow_hit
event_task_set.potion_hit
event_task_set.down
~~~

Built-in child option categories include:

1. Solo action options.
2. Chase options.
3. Ability options.
4. Collect-captive options.
5. Captive-treating options.
6. Arrow-hit event options.
7. Potion-hit event options.

Code-Mod Support Boundary
--------

Not every built-in option category is equally supported as an external code-mod surface.

Currently documented/supported manifest surfaces:

1. Task providers: `*.task_providers.json`
2. Arrow/potion event task options: `*.event_task_options.json`
3. Captive treating task options: `*.captive_treating_task_options.json`

Useful to understand but not beginner-promised as manifest surfaces yet:

1. Solo action task options.
2. Chase task options.
3. Ability task options.
4. Collect-captive task options.

Reason:

Those registries exist internally, but the public external loader/manifest path is not as direct as task providers, event options, and captive-treating options. Do not advertise them as beginner-safe until a dedicated loader and tutorial are added.

When To Make A Task Provider
--------

Make a new task provider when:

1. You want to add a new top-level behavior source.
2. Patroller Data can reference it directly.
3. The behavior can decide by itself whether to emit a task.

Example:

~~~text
task_provider.example.wait
~~~

When To Make A Task Option
--------

Make a task option when:

1. There is already a built-in task set that owns the category.
2. You want to add one choice inside that category.
3. The task set already knows how to select among options.

Example:

~~~text
event_task_option.arrow_hit.example
~~~

For v1 tutorials, prefer task providers first. Task options are more coupled to each task-set category.

Reading Built-In Code Safely
--------

When reading built-in code, look for these questions:

1. What ID registers this type?
2. Is this a provider or a child option?
3. Which JSON list references the ID?
4. Does it need `ApplyConfig`?
5. Does it need save/load state?
6. Does it subscribe to events?
7. Does it use a commit callback to avoid spending resources before acceptance?
8. What priority does it use?

If you cannot answer those, copy less and start with a simpler task provider.