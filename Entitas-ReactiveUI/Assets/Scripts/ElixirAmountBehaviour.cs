using UnityEngine;
using UnityEngine.UI;
using Entitas;

public class ElixirAmountBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Pools.pool.GetGroup(Matcher.Elixir).OnEntityAdded += (Group group, Entity entity, int index, IComponent component) => {updateText();};
	}

	void updateText(){
		var label = GetComponent<Text>();
		label.text = ((int)Pools.pool.elixir.amount).ToString();
		label.color = System.Math.Abs (Pools.pool.elixir.amount - ElixirProduceSystem.ElixirCapacity) < Mathf.Epsilon ? Color.red : Color.black;
	}
	
//	void Update () {
//	
//	}
}
