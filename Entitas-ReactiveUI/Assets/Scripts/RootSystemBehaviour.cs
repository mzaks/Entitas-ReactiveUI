using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;

public class RootSystemBehaviour : MonoBehaviour
{
  Systems _systems;

    void Awake() {
        Application.targetFrameRate = 60;
	}
	
	void Start ()
	{
		var logicSystems = CreateLogicSystems();

		_systems = new Feature("Root Systems");
		_systems.Add(Pools.pool.CreateSystem<ReplaySystem>());
		_systems.Add(Pools.pool.CreateSystem<CleanupConsumtionHistorySystem>());
		_systems.Add(Pools.pool.CreateSystem<NotifyTickListenersSystem>());
		_systems.Add(Pools.pool.CreateSystem<NotifyPauseListenersSystem>());
		_systems.Add(Pools.pool.CreateSystem<NotifyElixirListenersSystem>());
		_systems.Add(logicSystems);

		_systems.Initialize();
	}

	Systems CreateLogicSystems()
	{
		var pool = Pools.pool;
		if(!pool.hasLogicSystems){
			pool.SetLogicSystems(new Feature("Logic Systems")
				.Add(pool.CreateSystem<TickUpdateSystem>())
				.Add(pool.CreateSystem<ProduceElixirSystem>())
				.Add(pool.CreateSystem<ConsumeElixirSystem>())
				.Add(pool.CreateSystem<PersistConsumeElixirSystem>())
				.Add(pool.CreateSystem<ConsumeElixirCleanupSystem>()));
		}
		return pool.logicSystems.systems;
	}

	void Update () {
        _systems.Execute();
	}
}
