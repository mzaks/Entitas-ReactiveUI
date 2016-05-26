using UnityEngine;
using UnityEngine.UI;

public class ElixirAmountBehaviour : MonoBehaviour, ElixirListener {

	void Start () {
		Pools.pool.CreateEntity().AddElixirListener(this);
	}

	public void ElixirAmountChanged (float amount)
	{
		var label = GetComponent<Text>();
		label.text = ((int)amount).ToString();
		label.color = System.Math.Abs (amount - ProduceElixirSystem.ElixirCapacity) < Mathf.Epsilon ? Color.blue : Color.black;
	}

}
