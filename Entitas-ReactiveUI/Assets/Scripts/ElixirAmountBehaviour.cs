using UnityEngine;
using UnityEngine.UI;

public class ElixirAmountBehaviour : MonoBehaviour, IElixirListener {

	void Start () {
		Pools.pool.CreateEntity().AddElixirListener(this);
	}

	public void ElixirAmountChanged ()
	{
		var label = GetComponent<Text>();
		label.text = ((int)Pools.pool.elixir.amount).ToString();
		label.color = System.Math.Abs (Pools.pool.elixir.amount - ElixirProduceSystem.ElixirCapacity) < Mathf.Epsilon ? Color.red : Color.black;
	}

}
