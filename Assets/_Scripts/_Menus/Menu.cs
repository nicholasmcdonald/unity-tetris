using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Menu : MonoBehaviour {
	protected GameObject ActiveButton {
		get { 
			return allButtons [selection];
		}
	}
	protected UIManager uiManager;
	protected List<GameObject> allButtons;
	protected int selection;

	protected void SetUp() {
		uiManager = GameObject.FindGameObjectWithTag ("UI").GetComponent<UIManager> ();
		allButtons = new List<GameObject>();
		selection = 0;
	}

	public void SelectNext () {
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Normal;
		selection = (selection + 1) % allButtons.Count;
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	public void SelectPrevious () {
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Normal;
		selection = Mathf.Abs ((selection - 1) % allButtons.Count);
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	abstract public void Press ();
}
