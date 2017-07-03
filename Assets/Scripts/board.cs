using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitType {PAWN, ROOK, KNIGHT, BISHOP, QUEEN, KING, NULL};

public class board : MonoBehaviour {


	public GameObject[,] rows=new GameObject[8,8];
	private GameObject[] highlights;
	private int highlightCount;
	public GameObject highlight;


	// turnColor is used to keep a hold on which player's turn is it, 0 for black and 1 for white


	// Use this for initialization
	void Start () {
		int count = 0;

		// Grabbing White unit pieces from White class
		for (int j = 0; j < 8; j++) {
			for (int i = 0; i < 8; i++) {
				rows[i,j] = transform.GetChild( count++ ).gameObject;
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public void whiteClick(Vector3 pos1){

		UnitType unitType = GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(pos1);
		if (unitType != UnitType.NULL) {
			removeHighlights ();
			highLightMoveLocations (unitType, pos1);
			unitUnderHighlight = new Vector3(pos1.x, pos1.y, pos1.z);
		}
		else {
			Debug.Log ("non unit");
			if (checkHightlighted (pos1)) {
				removeHighlights ();
				Debug.Log ("highlighted");
				if (checkHitableEnemy (pos1)) {
					GameObject.Find ("Black").GetComponent<Black> ().removeUnit (pos1);
				}
				GameObject.Find ("White").GetComponent<White> ().moveObjectAtLocationTo(unitUnderHighlight, pos1);

			}
		}

	}

	public void blackClick(Vector3 pos1){
		UnitType unitType = GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(pos1);
		if (unitType != UnitType.NULL) {
			removeHighlights ();
			highLightMoveLocations (unitType, pos1);
			unitUnderHighlight = new Vector3(pos1.x, pos1.y, pos1.z);
			Debug.Log (unitUnderHighlight);
		}
		else {
			Debug.Log ("non unit");
			if (checkHightlighted (pos1)) {
				removeHighlights ();
				Debug.Log ("highlighted");
				if (checkHitableEnemy (pos1)) {
					GameObject.Find ("White").GetComponent<White> ().removeUnit (pos1);
				}
				GameObject.Find ("Black").GetComponent<Black> ().moveObjectAtLocationTo(unitUnderHighlight, pos1);

			}
		}

	}
	*/


	public Vector3 getTilePos(int x, int z){ 
		Vector3 pos;
		pos = rows [x,z].GetComponent<Transform> ().position;

		return pos;
	}

	public Vector3 checkTileHitandGetPos(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (rows [i, j].GetComponent<Collider> ().Raycast (ray, out hitInfo, Mathf.Infinity)) {
					return rows [i, j].GetComponent<Transform> ().position;
				}
			}
		}
		return new Vector3 (-1, -1, -1);

	}

	public bool checkTileFree(Vector3 pos){
		if (pos.x < 0 || pos.x > 7 || pos.z < 0 || pos.z > 7)
			return false;
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) == UnitType.NULL 
			&& GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) == UnitType.NULL)
			return true;
		return false;
	}

	public bool checkHitableWhiteEnemy(Vector3 pos){
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			return true;
		}
		return false;
	}

	public bool checkHitableBlackEnemy(Vector3 pos){
		if (GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			return true;
		}
		return false;
	}

	public void highLightMoveLocations(UnitType unitType, Vector3 pos, int face){
		

		if (unitType == UnitType.PAWN) {
			List <GameObject> highlightList = new List<GameObject> ();
			highlightCount = 0;
			if (pos.z == 1 && face == 1 || pos.z == 6 && face == -1) {
				// Check if pawn is at initial stage and can move two steps
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
					if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 2 * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 2 * face), Quaternion.identity));
						highlightCount++;
					}
				}
			} else {
				// Check rest of the times
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			if (face == 1) {
				// Check if the pawn can hit enemy diagonally right to it
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;

				}
				// Check if the pawn can hit enemy diagonally left to it
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			} else {
				// Check if the pawn can hit enemy diagonally right to it
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;

				}
				// Check if the pawn can hit enemy diagonally left to it
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}

			}


			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}
		} else if (unitType == UnitType.KNIGHT) { // Knight movements availability
			List <GameObject> highlightList = new List<GameObject> ();

			// Check where knight can move
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face))){
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}

			if (face == 1) {
				// Check what knight can capture
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}
			else{
				// Check what knight can capture
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}
		} else if (unitType == UnitType.ROOK) {
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;
			highlightCount = 0;
			for (int i = 0; i < 8; i++) {	
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
				}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}

		}else if (unitType == UnitType.BISHOP) {
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;

			highlightCount = 0;
			for (int i = 0; i < 8; i++) {	
				if (breaker == true) {
					break;
				}	
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					} 
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}

		}else if (unitType == UnitType.QUEEN) {
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;
			highlightCount = 0;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x , pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z +1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i , pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face))) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else 
					break;
			}
			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}

		} else if(unitType == UnitType.KING){
			List <GameObject> highlightList = new List<GameObject> ();
			highlightCount = 0;

			// Check Movement 
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x , pos.y, pos.z - 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face))) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}

			if (face == 1) {
				// Check capture moves
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}else {
				// Check capture moves
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face))) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			highlights = new GameObject[highlightList.Count];
			int c =0;
			foreach (GameObject g in highlightList) {
				highlights[c++] = g;
			}

		}
	}

	public void removeHighlights(){
		//Destroy (GameObject.FindGameObjectsWithTag ("highlight"));

		if (highlights != null) {
			foreach (GameObject high in highlights) {
				GameObject.Destroy (high);
			}
			highlightCount = 0;
		}
		//highlights = null;
	}

	public bool checkHightlighted(Vector3 pos){
		for (int i = 0; i < highlightCount; i++) {
			if(pos.x == highlights[i].transform.position.x
				&& pos.y == highlights[i].transform.position.y
				&& pos.z == highlights[i].transform.position.z)
				return true;
		}

		return false;
	}

	public bool isCheck(int playerTurn){ // white played turn, check isCheck on black's king
		Vector3 kingPos;
		if (playerTurn == 0) { // After White's turn
			Debug.Log("Black's turn");
			kingPos = GameObject.Find ("Black").GetComponent<Black> ().getKingLocation();
			// check line of seight for each type of unit
			// knight
			if (!checkTileFree (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + -2 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z - 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + -2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z -1 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z - 1 ))) {
					if (UnitType.KNIGHT.Equals(GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z -1)))) {
						return true;
					}
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + -2 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1, kingPos.y, kingPos.z - 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + -2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + -1 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 2, kingPos.y, kingPos.z - 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + -1 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + 2 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1, kingPos.y, kingPos.z + 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + 2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + 1 ))){
				if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 2, kingPos.y, kingPos.z + 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + 1 )))
						return true;
				}
			}

			// bishop (includes queen's some paths and the pawn's attack)

			bool breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z + 1 + i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z + 1 + i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z + 1 + i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z + 1 + i )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z - 1 - i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z - 1 - i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(i==0 && UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(i==0 && UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
					}
				}
			}



			// rook (includes queen's some paths)


			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z))) {
						if(UnitType.ROOK==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1 + i, kingPos.y, kingPos.z )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x, kingPos.y, kingPos.z - 1 - i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x, kingPos.y, kingPos.z - 1 - i ))) {
						if(UnitType.ROOK==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x, kingPos.y, kingPos.z - 1 - i )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z - 1 - i )))
							return true;
					}
				}
			}
			breaker = false;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				} 
				if (checkTileFree (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i ))) {
					continue;
				} else { 
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i ))) {
						if(UnitType.BISHOP==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
						if(UnitType.QUEEN==GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition(new Vector3 (kingPos.x - 1 - i, kingPos.y, kingPos.z + 1 + i )))
							return true;
					}
				}
			}


		} else if (playerTurn == 1) { // After Black's turn /////////////////////////////////////////////////////////////////////////////
			kingPos = GameObject.Find ("White").GetComponent<White> ().getKingLocation();

			// knight
			if (!checkTileFree (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + 2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z + 1 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + -2 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z - 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + 1, kingPos.y, kingPos.z + -2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z -1 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z - 1 ))) {
					if (UnitType.KNIGHT.Equals(GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (new Vector3 (kingPos.x + 2, kingPos.y, kingPos.z -1)))) {
						return true;
					}
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + -2 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x - 1, kingPos.y, kingPos.z - 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + -2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + -1 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x - 2, kingPos.y, kingPos.z - 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + -1 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + 2 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x - 1, kingPos.y, kingPos.z + 2 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -1, kingPos.y, kingPos.z + 2 )))
						return true;
				}
			}
			if (!checkTileFree (new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + 1 ))){
				if (checkHitableBlackEnemy (new Vector3 (kingPos.x - 2, kingPos.y, kingPos.z + 1 ))) {
					if(UnitType.KNIGHT==GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition(new Vector3 (kingPos.x + -2, kingPos.y, kingPos.z + 1 )))
						return true;
				}
			}

		} else
			return	 false;
		return false;
	}


}
