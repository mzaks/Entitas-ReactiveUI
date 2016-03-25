using UnityEngine;

public class ElixirBarBehaviour : MonoBehaviour, IElixirListener {

	float maxWidth;

	void Start () {
		maxWidth = GetComponent<RectTransform>().rect.width;
		Pools.pool.CreateEntity().AddElixirListener(this);
	}

	public void ElixirAmountChanged ()
	{
		var ratio = Pools.pool.elixir.amount / ElixirProduceSystem.ElixirCapacity;
		GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth * ratio, GetComponent<RectTransform>().rect.height);
	}

}
