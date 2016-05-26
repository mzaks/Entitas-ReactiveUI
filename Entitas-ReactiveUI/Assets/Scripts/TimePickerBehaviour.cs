using UnityEngine;
using UnityEngine.UI;

public class TimePickerBehaviour : MonoBehaviour, PauseListener {

	void Start () 
	{
		Pools.pool.CreateEntity().AddPauseListener(this);
		PauseStateChanged(Pools.pool.isPause);
	}

	public void PauseStateChanged (bool isPaused)
	{
		gameObject.SetActive(isPaused);
		if(Pools.pool.hasTick){
			var slider = GetComponent<Slider>();
			slider.maxValue = Pools.pool.tick.currentTick;
			slider.value = Pools.pool.tick.currentTick;
		}
	}

	public void ChangedValue()
	{
		Pools.pool.ReplaceJumpInTime((long)GetComponent<Slider>().value);
	}
}