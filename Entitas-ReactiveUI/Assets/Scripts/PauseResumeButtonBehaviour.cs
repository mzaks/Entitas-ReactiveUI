using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseResumeButtonBehaviour : MonoBehaviour {

	public Text label;

	public void ButtonPressed()
	{
		Pools.pool.isPause = !Pools.pool.isPause;
		label.text = Pools.pool.isPause ? "Resume" : "Pause";
	}
}
