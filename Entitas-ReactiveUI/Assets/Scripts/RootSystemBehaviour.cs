using UnityEngine;
using System.Collections;
using Entitas;

public class RootSystemBehaviour : MonoBehaviour
{
  private Systems _systems;

    void Awake() {
        Application.targetFrameRate = 60;
	}
	
	// Use this for initialization
	void Start ()
	{
		var logicSystems = CreateLogicSystems();

		_systems = new Systems();
		_systems.Add(Pools.pool.CreateSystem<ReplaySystem>());
		_systems.Add(Pools.pool.CreateSystem<CleanupConsumtionHistorySystem>());
		_systems.Add(logicSystems);
		_systems.Initialize();
	}

	Systems CreateLogicSystems()
	{
		var pool = Pools.pool;
		if(!pool.hasLogicSystems){
			pool.SetLogicSystems(new Systems()
				.Add(pool.CreateSystem<TickUpdateSystem>())
				.Add(pool.CreateSystem<ElixirProduceSystem>())
				.Add(pool.CreateSystem<ElixirConsumeSystem>())
				.Add(pool.CreateSystem<ElixirConsumePersistSystem>())
				.Add(pool.CreateSystem<ElixirConsumeCleanupSystem>()));
		}
		return pool.logicSystems.systems;
	}

	// Update is called once per frame
	void Update () {
        _systems.Execute();
	}
}
