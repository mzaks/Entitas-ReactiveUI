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
		_systems.Add(logicSystems);
		_systems.Initialize();
	}

	Systems CreateLogicSystems()
	{
		var pool = Pools.pool;
		return new Systems()
			.Add(pool.CreateSystem<TickUpdateSystem>())
				.Add(pool.CreateSystem<ElixirProduceSystem>())
				.Add(pool.CreateSystem<ElixirConsumeSystem>());
	}

	// Update is called once per frame
	void Update () {
        _systems.Execute();
	}
}
