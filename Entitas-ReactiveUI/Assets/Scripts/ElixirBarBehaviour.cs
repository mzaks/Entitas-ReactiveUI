using UnityEngine;
using UnityEngine.UI;
using Entitas;

public class ElixirBarBehaviour : MonoBehaviour {

	float maxWidth;

	void Start () {
		maxWidth = GetComponent<RectTransform>().rect.width;
		Pools.pool.GetGroup(Matcher.Elixir).OnEntityAdded += (Group group, Entity entity, int index, IComponent component) => {updateBar();};
	}
	
	void updateBar(){
		var ratio = Pools.pool.elixir.amount / ElixirProduceSystem.ElixirCapacity;
		GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth * ratio, GetComponent<RectTransform>().rect.height);
	}
	
//	void Update () {
//	
//	}
}
