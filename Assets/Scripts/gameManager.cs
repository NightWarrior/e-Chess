using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {
	private Vector3 unitUnderHighlight;
	private int turnColor = 0; // 0 for white, 1 for black


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos1;

		if(Input.GetMouseButtonDown(0)){
			pos1 = GameObject.Find ("Board").GetComponent<board> ().checkTileHitandGetPos ();
			if (pos1.x != -1) {
				if (turnColor == 0) {
					whiteClick (pos1);
				} else {
					blackClick (pos1);
				}
			}

		}

		if(Input.GetMouseButtonDown(1)){
			GameObject.Find ("Board").GetComponent<board> ().removeHighlights ();
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			turnColor = (++turnColor % 2);
		}



		// routine



		/*
		white
			check unit select
			check move location
			check any captures
			check check or checkmates
				playerTurn = 0;

		black
			check unit select
			check move location
			check any captures
			check check or checkmates
				playerTurn = 1;


		*/


	}

	private void changeColor(){
		//Debug.Log("check: " + GameObject.Find ("Board").GetComponent<board> ().isCheck(turnColor));
		turnColor = (++turnColor % 2);
	}

	public void whiteClick(Vector3 pos1){

		UnitType unitType = GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(pos1);
		if (unitType != UnitType.NULL) {
			GameObject.Find ("Board").GetComponent<board> ().removeHighlights ();
			GameObject.Find ("Board").GetComponent<board> ().highLightMoveLocations (unitType, pos1, 1); //face is 1 for up (or white), -1 for down
			unitUnderHighlight = new Vector3(pos1.x, pos1.y, pos1.z);
		}
		else {
			//Debug.Log ("non unit");
			if (GameObject.Find ("Board").GetComponent<board> ().checkHightlighted (pos1)) {
				GameObject.Find ("Board").GetComponent<board> ().removeHighlights ();
				//Debug.Log ("highlighted");
				if (GameObject.Find ("Board").GetComponent<board> ().checkHitableBlackEnemy (pos1)) {
					GameObject.Find ("Black").GetComponent<Black> ().removeUnit (pos1);
				}
				GameObject.Find ("White").GetComponent<White> ().moveObjectAtLocationTo(unitUnderHighlight, pos1);
				changeColor ();
			}
		}

	}

	public void blackClick(Vector3 pos1){
		UnitType unitType = GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(pos1);
		if (unitType != UnitType.NULL) {
			GameObject.Find ("Board").GetComponent<board> ().removeHighlights ();
			GameObject.Find ("Board").GetComponent<board> ().highLightMoveLocations (unitType, pos1, -1);
			unitUnderHighlight = new Vector3(pos1.x, pos1.y, pos1.z);
//			Debug.Log (unitUnderHighlight);
		}
		else {
//			Debug.Log ("non unit");
			if (GameObject.Find ("Board").GetComponent<board> ().checkHightlighted (pos1)) {
				GameObject.Find ("Board").GetComponent<board> ().removeHighlights ();
//				Debug.Log ("highlighted");
				if (GameObject.Find ("Board").GetComponent<board> ().checkHitableWhiteEnemy (pos1)) {
					GameObject.Find ("White").GetComponent<White> ().removeUnit (pos1);
				}
				GameObject.Find ("Black").GetComponent<Black> ().moveObjectAtLocationTo(unitUnderHighlight, pos1);
				changeColor ();

			}
		}

	}
}
