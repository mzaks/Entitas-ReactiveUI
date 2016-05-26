using UnityEngine;
using UnityEngine.UI;

public class PauseResumeButtonBehaviour : MonoBehaviour, PauseListener {

	public Text label;

	void Awake()
	{
		Pools.pool.CreateEntity().AddPauseListener(this);
	}

	public void ButtonPressed()
	{
		Pools.pool.isPause = !Pools.pool.isPause;
	}

	public void PauseStateChanged (bool isPaused)
	{
		label.text = isPaused ? "Resume" : "Pause";
	}
}
