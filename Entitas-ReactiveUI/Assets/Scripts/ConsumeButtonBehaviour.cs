using UnityEngine;
using UnityEngine.UI;

public class ConsumeButtonBehaviour : MonoBehaviour, IPauseListener, IElixirListener {

	public Text label;
	public RectTransform progressBox;
	public int consumptionAmmount;

	float maxHeight;

	void Awake()
	{
		maxHeight = progressBox.rect.height;
		label.text = consumptionAmmount.ToString();

		Pools.pool.CreateEntity().AddPauseListener(this).AddElixirListener(this);
	}

	public void PauseStateChanged ()
	{
		GetComponent<Button>().enabled = !Pools.pool.isPause;
	}

	public void ElixirAmountChanged ()
	{
		var ratio = 1 - Mathf.Min(1f, (Pools.pool.elixir.amount / (float)consumptionAmmount));
		progressBox.sizeDelta = new Vector2(progressBox.rect.width, maxHeight * ratio);
		GetComponent<Button>().enabled = (System.Math.Abs (ratio - 0) < Mathf.Epsilon);
	}

    public void ButtonPressed()
    {
		if(Pools.pool.isPause) return;
        Pools.pool.CreateEntity().AddConsume(consumptionAmmount);
    }
}
