using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;

public class TimeLabelBehaviour : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		Pools.pool.GetGroup(Matcher.Tick).OnEntityAdded += delegate {updateText();};
	}

  	void updateText()
  	{
		var tick = Pools.pool.tick.currentTick;
		var sec = (tick / 60) % 60;
		var min = (tick / 3600);
		var secText = sec > 9 ? "" + sec : "0" + sec;
		var minText = min > 9 ? "" + min : "0" + min;

		GetComponent<Text>().text = minText + ":" + secText;
  	}

	// Update is called once per frame
	void Update () {
	
	}
}
