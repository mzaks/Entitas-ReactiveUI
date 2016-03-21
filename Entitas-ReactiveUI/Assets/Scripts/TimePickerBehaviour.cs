using UnityEngine;
using UnityEngine.UI;
using Entitas;

public class TimePickerBehaviour : MonoBehaviour {

	void Start () 
	{
//		Pools.pool.GetGroup(Matcher.Pause).OnEntityAdded += delegate {toggleState();};
//		Pools.pool.GetGroup(Matcher.Pause).OnEntityRemoved += delegate {toggleState();};
//		toggleState();
		gameObject.SetActive(false);
	}

//	void toggleState ()
//	{
//		gameObject.SetActive(Pools.pool.isPause);
//		if(Pools.pool.hasTick){
//			var slider = GetComponent<Slider>();
//			slider.maxValue = Pools.pool.tick.currentTick;
//			slider.value = Pools.pool.tick.currentTick;
//		}
//	}

}
