using UnityEngine;
using UnityEngine.UI;

public class ConsumeButtonBehaviour : MonoBehaviour, PauseListener, ElixirListener {

	public Text label;
	public Image progressBox;
	public int consumtionAmmount;

	float maxHeight;

	void Awake()
	{
		label.text = consumtionAmmount.ToString();

		Pools.pool.CreateEntity().AddPauseListener(this).AddElixirListener(this);
	}

	public void PauseStateChanged ()
	{
		GetComponent<Button>().enabled = !Pools.pool.isPause;
	}

	public void ElixirAmountChanged ()
	{
		var ratio = 1 - Mathf.Min(1f, (Pools.pool.elixir.amount / (float)consumtionAmmount));
		progressBox.fillAmount = ratio;
		GetComponent<Button>().enabled = (System.Math.Abs (ratio - 0) < Mathf.Epsilon);
	}

    public void ButtonPressed()
    {
		if(Pools.pool.isPause) return;
		Pools.pool.CreateEntity().AddConsumeElixir(consumtionAmmount);
    }
}
