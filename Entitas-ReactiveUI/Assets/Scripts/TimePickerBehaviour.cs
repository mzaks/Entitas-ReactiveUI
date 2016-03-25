using UnityEngine;
using UnityEngine.UI;

public class TimePickerBehaviour : MonoBehaviour, IPauseListener {

	void Start () 
	{
		Pools.pool.CreateEntity().AddPauseListener(this);
		PauseStateChanged();
	}

	public void PauseStateChanged ()
	{
		gameObject.SetActive(Pools.pool.isPause);
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