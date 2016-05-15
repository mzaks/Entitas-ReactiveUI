using System;
using System.Collections.Generic;
using Entitas;

public class TickUpdateSystem : IInitializeSystem, IExecuteSystem, ISetPool
{
	Pool _pool;

	public void SetPool(Pool pool){_pool = pool;}

	public void Initialize(){ _pool.ReplaceTick(0); }

	public void Execute()
	{
		if (!_pool.isPause)
		{
		    _pool.ReplaceTick(_pool.tick.currentTick + 1);
		}
	}
}


public class ProduceElixirSystem : IInitializeSystem, IReactiveSystem, ISetPool
{
  	Pool _pool;

  	// This should be inside of a config file
  	public const float ElixirCapacity = 10f;
  	const float ProductionStep = 0.01f;

	public TriggerOnEvent trigger { get { return Matcher.Tick.OnEntityAdded(); }}
	
	public void SetPool(Pool pool){ _pool = pool; }

	public void Initialize ()
	{
		_pool.ReplaceElixir(0);
	}

  	public void Execute(List<Entity> entities)
  	{
		var newAmount = _pool.elixir.amount + ProductionStep;
		newAmount = Math.Min(ElixirCapacity, newAmount);
      	_pool.ReplaceElixir(newAmount);
  	}
}

public class ConsumeElixirSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
  	Pool _pool;

	public TriggerOnEvent trigger { get { return Matcher.ConsumeElixir.OnEntityAdded();}}

	public IMatcher ensureComponents { get { return Matcher.ConsumeElixir; }}

	public void SetPool(Pool pool){ _pool = pool; }

	public void Execute(List<Entity> entities)
	{
	    foreach (var entity in entities)
	    {
			if(entity.consumeElixir.amount > _pool.elixir.amount){
				UnityEngine.Debug.LogError("Consume more than produced. Should not happen");
			}
			var newAmount = Math.Max(0, _pool.elixir.amount - entity.consumeElixir.amount);
	        _pool.ReplaceElixir(newAmount);
	    }
	}
}

public class PersistConsumeElixirSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;

	public TriggerOnEvent trigger { get { return Matcher.ConsumeElixir.OnEntityAdded();}}

	public IMatcher ensureComponents { get { return Matcher.ConsumeElixir; }}

	public void SetPool(Pool pool){ _pool = pool; }
	
	public void Execute(List<Entity> entities)
	{
		if(_pool.isPause){
			return;
		}
		var previousEntries = 
			_pool.hasConsumtionHistory ? _pool.consumtionHistory.entries : 
											new List<ConsumtionEntry>();
		foreach (var entity in entities)
		{
			previousEntries.Add(
				new ConsumtionEntry(_pool.tick.currentTick, entity.consumeElixir.amount
			));
		}
		_pool.ReplaceConsumtionHistory(previousEntries);
	}
}

public class ConsumeElixirCleanupSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;

	public TriggerOnEvent trigger { get { return Matcher.ConsumeElixir.OnEntityAdded();}}

	public IMatcher ensureComponents { get { return Matcher.ConsumeElixir; }}

	public void SetPool(Pool pool){ _pool = pool; }
	
	public void Execute(List<Entity> entities)
	{
		foreach (var entity in entities)
		{
			_pool.DestroyEntity(entity);
		}
	}
}

public class ReplaySystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;

	public TriggerOnEvent trigger { get { return Matcher.JumpInTime.OnEntityAdded(); }}

	public void SetPool(Pool pool){ _pool = pool; }

	public IMatcher ensureComponents { get { return Matcher.JumpInTime; }}
	
	public void Execute(List<Entity> entities)
	{
		var logicSystems = _pool.logicSystems.systems;
		logicSystems.Initialize();
		var actions = _pool.hasConsumtionHistory ? 
						_pool.consumtionHistory.entries : 
						new List<ConsumtionEntry>();
		var actionIndex = 0;
		for (int tick = 0; tick <= _pool.jumpInTime.targetTick; tick++) {
			_pool.ReplaceTick(tick);
			if(actions.Count > actionIndex && actions[actionIndex].tick == tick) {
				_pool.CreateEntity().AddConsumeElixir(actions[actionIndex].amount);
				actionIndex++;
			}
			logicSystems.Execute();
		}
	}
}

public class CleanupConsumtionHistorySystem : IReactiveSystem, ISetPool, IExcludeComponents
{
	Pool _pool;

	public TriggerOnEvent trigger { get { return Matcher.Pause.OnEntityRemoved(); }}

	public void SetPool(Pool pool){ _pool = pool; }

	public IMatcher excludeComponents { get { return Matcher.Pause; }}
	
	public void Execute(List<Entity> entities)
	{
		var actions = _pool.hasConsumtionHistory ? _pool.consumtionHistory.entries : new List<ConsumtionEntry>();
		int count = 0;
		for (int index = actions.Count-1; index >= 0; index--) {
			if(actions[index].tick>_pool.tick.currentTick){
				count++;
			} else {
				break;
			}
		}
		actions.RemoveRange(actions.Count - count, count);
	}
}

public class NotifyTickListenersSystem : IReactiveSystem, ISetPool
{
	Pool _pool;
	Group listeners;

	public TriggerOnEvent trigger { get { return Matcher.Tick.OnEntityAddedOrRemoved(); }}

	public void SetPool(Pool pool){
		_pool = pool;
		listeners = _pool.GetGroup(Matcher.TickListener);
	}

	public void Execute(List<Entity> entities)
	{
		foreach (var entity in listeners.GetEntities()) {
			entity.tickListener.value.TickChanged();
		}
	}
}

public class NotifyPauseListenersSystem : IReactiveSystem, ISetPool
{
	Pool _pool;
	Group listeners;

	public TriggerOnEvent trigger { get { return Matcher.Pause.OnEntityAddedOrRemoved(); }}

	public void SetPool(Pool pool){
		_pool = pool;
		listeners = _pool.GetGroup(Matcher.PauseListener);
	}
	
	public void Execute(List<Entity> entities)
	{
		foreach (var entity in listeners.GetEntities()) {
			entity.pauseListener.value.PauseStateChanged();
		}
	}
}

public class NotifyElixirListenersSystem : IReactiveSystem, ISetPool
{
	Pool _pool;
	Group listeners;

	public TriggerOnEvent trigger { get { return Matcher.Elixir.OnEntityAddedOrRemoved(); }}

	public void SetPool(Pool pool) {
		_pool = pool;
		listeners = _pool.GetGroup(Matcher.ElixirListener);
	}
	
	public void Execute(List<Entity> entities)
	{
		foreach (var entity in listeners.GetEntities()) {
			entity.elixirListener.value.ElixirAmountChanged();
		}
	}
}