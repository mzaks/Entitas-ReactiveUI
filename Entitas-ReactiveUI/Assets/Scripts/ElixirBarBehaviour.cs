using UnityEngine;

public class ElixirBarBehaviour : MonoBehaviour, ElixirListener {

	void Start () {
		Pools.pool.CreateEntity().AddElixirListener(this);
	}

	public void ElixirAmountChanged (float amount)
	{
		var ratio = amount / ProduceElixirSystem.ElixirCapacity;
		GetComponent<RectTransform>().localScale = new Vector3(ratio, 1f, 1f);
	}

}
