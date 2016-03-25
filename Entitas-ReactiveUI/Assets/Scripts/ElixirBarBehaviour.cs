using UnityEngine;

public class ElixirBarBehaviour : MonoBehaviour, IElixirListener {

	void Start () {
		Pools.pool.CreateEntity().AddElixirListener(this);
	}

	public void ElixirAmountChanged ()
	{
		var ratio = Pools.pool.elixir.amount / ElixirProduceSystem.ElixirCapacity;
		GetComponent<RectTransform>().localScale = new Vector3(ratio, 1f, 1f);
	}

}
