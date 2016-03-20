using UnityEngine;
using UnityEngine.UI;
using Entitas;

public class ConsumeButtonBehaviour : MonoBehaviour {

	public Text label;
	public RectTransform progressBox;
	public int consumptionAmmount;

	float maxHeight;

	void Awake()
	{
		maxHeight = progressBox.rect.height;
		label.text = consumptionAmmount.ToString();
		Pools.pool.GetGroup(Matcher.Elixir).OnEntityAdded += (Group group, Entity entity, int index, IComponent component) => {updateBar();};
		Pools.pool.GetGroup(Matcher.Pause).OnEntityAdded += (Group group, Entity entity, int index, IComponent component) => {toggleButtonState();};
		Pools.pool.GetGroup(Matcher.Pause).OnEntityRemoved += (Group group, Entity entity, int index, IComponent component) => {toggleButtonState();};
	}

	void updateBar()
	{
		var ratio = 1 - Mathf.Min(1f, (Pools.pool.elixir.amount / (float)consumptionAmmount));
		progressBox.sizeDelta = new Vector2(progressBox.rect.width, maxHeight * ratio);
		GetComponent<Button>().enabled = (System.Math.Abs (ratio - 0) < Mathf.Epsilon);
	}

	void toggleButtonState()
	{
		GetComponent<Button>().enabled = !Pools.pool.isPause;
	}

    public void ButtonPressed()
    {
		if(Pools.pool.isPause) return;
        Pools.pool.CreateEntity().AddConsume(consumptionAmmount);
    }
}
