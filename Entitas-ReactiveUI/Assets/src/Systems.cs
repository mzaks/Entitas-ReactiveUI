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