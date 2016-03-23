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


public class ElixirProduceSystem : IReactiveSystem, ISetPool, IInitializeSystem
{
  	Pool _pool;
  	int count = 0;

  	// This should be inside of a config file
  	public const float ElixirCapacity = 10f;
  	const int ProductionFrequency = 3;
  	const float ProductionStep = 0.01f;

	public TriggerOnEvent trigger { get { return Matcher.Tick.OnEntityAdded();}}
	
	public void SetPool(Pool pool){_pool = pool;}

	public void Initialize ()
	{
		_pool.ReplaceElixir(0);
	}

  	public void Execute(List<Entity> entities)
  	{
    	if (count == 0)
    	{
      		var newAmount = Math.Min(ElixirCapacity, _pool.elixir.amount + ProductionStep);
      		_pool.ReplaceElixir(newAmount);
    	}
    	count = ((count + 1) % ProductionFrequency);
  	}
}

public class ElixirConsumeSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
  	Pool _pool;

	public void Execute(List<Entity> entities)
	{
	    foreach (var entity in entities)
	    {
			if(entity.consume.amount > _pool.elixir.amount){
				UnityEngine.Debug.LogError("Consume more than produced. Should not happen");
			}
	        var newAmount = Math.Max(0, _pool.elixir.amount - entity.consume.amount);
	        _pool.ReplaceElixir(newAmount);
	    }
	}

	public TriggerOnEvent trigger { get { return Matcher.Consume.OnEntityAdded();}}

  	public void SetPool(Pool pool){_pool = pool;}

  	public IMatcher ensureComponents { get { return Matcher.Consume; } }
}

public class ElixirConsumePersistSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;
	
	public void Execute(List<Entity> entities)
	{
		if(_pool.isPause){
			return;
		}
		var previousEntries = _pool.hasConsumtionHistory ? _pool.consumtionHistory.entires : new List<ConsumptionEntry>();
		foreach (var entity in entities)
		{
			previousEntries.Add(new ConsumptionEntry(_pool.tick.currentTick, entity.consume.amount));

		}
		_pool.ReplaceConsumtionHistory(previousEntries);
	}
	
	public TriggerOnEvent trigger { get { return Matcher.Consume.OnEntityAdded();}}
	
	public void SetPool(Pool pool){_pool = pool;}
	
	public IMatcher ensureComponents { get { return Matcher.Consume; } }
}

public class ElixirConsumeCleanupSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;
	
	public void Execute(List<Entity> entities)
	{
		foreach (var entity in entities)
		{
			_pool.DestroyEntity(entity);
		}
	}
	
	public TriggerOnEvent trigger { get { return Matcher.Consume.OnEntityAdded();}}
	
	public void SetPool(Pool pool){_pool = pool;}
	
	public IMatcher ensureComponents { get { return Matcher.Consume; } }
}

public class ReplaySystem : IReactiveSystem, ISetPool, IEnsureComponents
{
	Pool _pool;
	
	public void Execute(List<Entity> entities)
	{
		var logicSystems = _pool.logicSystems.systems;
		logicSystems.Initialize();
		var actions = _pool.hasConsumtionHistory ? _pool.consumtionHistory.entires : new List<ConsumptionEntry>();
		var actionIndex = 0;
		for (int tick = 0; tick <= _pool.jumpInTime.targetTick; tick++) {
			_pool.ReplaceTick(tick);
			if(actions.Count > actionIndex && actions[actionIndex].tick == tick){
				_pool.CreateEntity().AddConsume(actions[actionIndex].amount);
				actionIndex++;
			}
			logicSystems.Execute();
		}
	}
	
	public TriggerOnEvent trigger { get { return Matcher.JumpInTime.OnEntityAdded();}}
	
	public void SetPool(Pool pool){_pool = pool;}
	
	public IMatcher ensureComponents { get { return Matcher.JumpInTime; } }
}

public class CleanupConsumtionHistorySystem : IReactiveSystem, ISetPool, IExcludeComponents
{
	Pool _pool;
	
	public void Execute(List<Entity> entities)
	{
		var actions = _pool.hasConsumtionHistory ? _pool.consumtionHistory.entires : new List<ConsumptionEntry>();
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
	
	public TriggerOnEvent trigger { get { return Matcher.Pause.OnEntityRemoved();}}
	
	public void SetPool(Pool pool){_pool = pool;}

	public IMatcher excludeComponents { get { return Matcher.Pause; } }
}