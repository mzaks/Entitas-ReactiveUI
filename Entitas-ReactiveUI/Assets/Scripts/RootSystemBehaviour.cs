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
		var pool = Pools.pool;
		_systems = new Systems()
			.Add(pool.CreateSystem<TickUpdateSystem>())
				.Add(pool.CreateSystem<ElixirProduceSystem>())
				.Add(pool.CreateSystem<ElixirConsumeSystem>());

		_systems.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        _systems.Execute();
	}
}
