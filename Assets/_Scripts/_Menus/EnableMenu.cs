using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMenu : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameObject.FindGameObjectWithTag ("UI").GetComponent<UIManager> ().EndRevealUI ();
	}
}