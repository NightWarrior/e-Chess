using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitType {
PAWN,
	ROOK,
	KNIGHT,
	BISHOP,
	QUEEN,
	KING,
	NULL}
;

public class board : MonoBehaviour {


	public GameObject[,] rows = new GameObject[8, 8];
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
				rows [i, j] = transform.GetChild (count++).gameObject;
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 getTilePos (int x, int z) { 
		Vector3 pos;
		pos = rows [x, z].GetComponent<Transform> ().position;

		return pos;
	}

	public Vector3 checkTileHitandGetPos () { // checks if mouse clicked on the tile
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

	public bool checkTileFree (Vector3 pos, Vector3 piecePos) { // check if a highlight tile can be placed at the location for movement
		if (pos.x < 0 || pos.x > 7 || pos.z < 0 || pos.z > 7)
			return false;
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) == UnitType.NULL// checking if the area is free
		    && GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) == UnitType.NULL) {
			BoardStructure bs = new BoardStructure ();
			return bs.checkMovable (piecePos, pos);
		}
			
		return false;
	}

	bool checkTileFree (Vector3 pos) { // check if a highlight tile can be placed at the location for movement
		if (pos.x < 0 || pos.x > 7 || pos.z < 0 || pos.z > 7)
			return false;
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) == UnitType.NULL// checking if the area is free
			&& GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) == UnitType.NULL)
			return true;
		return false;
	}

	bool checkPathFreeHorizontal (Vector3 pos1, Vector3 pos2) { // checks if a horizontal path is clear or not, both indexes inclusive
		if(pos1.z != pos2.z || pos1.y != pos2.y) return false;

		bool free = true;
		for (int i = (int)pos1.x; i <= (int)pos2.x; i++) {
			if (!checkTileFree (new Vector3 (i, pos1.y, pos1.z))) {
				free = false;
				break;
			}
		}
		//Debug.Log ("check path '"+pos1 +"', '"+pos2 +"' returned: " + free);
		return free;
	}


	public bool checkHitableWhiteEnemy (Vector3 pos, Vector3 piecePos) {
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			BoardStructure bs = new BoardStructure ();
			return bs.checkMovable (piecePos, pos);
		}
		return false;
	}


	public bool checkHitableWhiteEnemy (Vector3 pos) {
		if (GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			return true;
		}
		return false;
	}

	public bool checkHitableBlackEnemy (Vector3 pos, Vector3 piecePos) {
		if (GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			BoardStructure bs = new BoardStructure ();
			return bs.checkMovable (piecePos, pos);
		}
		return false;
	}

	public bool checkHitableBlackEnemy (Vector3 pos) {
		if (GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (pos) != UnitType.NULL) {
			return true;
		}
		return false;
	}

	public void highLightMoveLocations (UnitType unitType, Vector3 pos, int face) {
		

		if (unitType == UnitType.PAWN) {
			List <GameObject> highlightList = new List<GameObject> ();
			highlightCount = 0;
			if (pos.z == 1 && face == 1 || pos.z == 6 && face == -1) {
				// Check if pawn is at initial stage and can move two steps
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
					if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 2 * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 2 * face), Quaternion.identity));
						highlightCount++;
					}
				}
			} else {
				// Check rest of the times
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			if (face == 1) {
				// Check if the pawn can hit enemy diagonally right to it
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;

				}
				// Check if the pawn can hit enemy diagonally left to it
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			} else {
				// Check if the pawn can hit enemy diagonally right to it
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;

				}
				// Check if the pawn can hit enemy diagonally left to it
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}

			}


			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}
		} else if (unitType == UnitType.KNIGHT) { // Knight movements availability /////////////////////////////////////////////////////////////////////////////////
			List <GameObject> highlightList = new List<GameObject> ();

			// Check where knight can move
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}

			if (face == 1) {
				// Check what knight can capture
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			} else {
				// Check what knight can capture
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + -2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + -1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -1, pos.y, pos.z + 2 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + -2, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}
		} else if (unitType == UnitType.ROOK) {///////////////////////////////////////////////////////////////////////////////////////////////////////
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;
			highlightCount = 0;
			for (int i = 0; i < 8; i++) {	
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				}
			}
			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}

		} else if (unitType == UnitType.BISHOP) {////////////////////////////////////////////////////////////////////////////////////////////////////////////
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;

			highlightCount = 0;
			for (int i = 0; i < 8; i++) {	
				if (breaker == true) {
					break;
				}	
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} 
			}
			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}

		} else if (unitType == UnitType.QUEEN) {////////////////////////////////////////////////////////////////////////////////////////////////////////////
			List <GameObject> highlightList = new List<GameObject> ();
			bool breaker = false;
			highlightCount = 0;
			for (int i = 0; i < 8; i++) {		
				if (breaker == true) {
					break;
				}
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z), pos)) {
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z - 1 * face - i * face), pos)) {
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
				if (checkTileFree (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1 - i, pos.y, pos.z + 1 * face + i * face), Quaternion.identity));
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
				if (checkTileFree (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
					highlightCount++;
				} else if (face == 1) { 
					breaker = true;
					if (checkHitableBlackEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} else if (face == -1) {
					breaker = true;
					if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), pos)) {
						highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1 + i, pos.y, pos.z - 1 * face - i * face), Quaternion.identity));
						highlightCount++;
						break;
					}
				} 
			}
			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}

		} else if (unitType == UnitType.KING) {////////////////////////////////////////////////////////////////////////////////////////////////////////////
			List <GameObject> highlightList = new List<GameObject> ();
			highlightCount = 0;

			// Check Movement 
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z - 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}
			if (checkTileFree (new Vector3 (pos.x, pos.y, pos.z + 1 * face), pos)) {
				highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
				highlightCount++;
			}

			// Checking Castling  /////////////////////////////////////
			if (checkPathFreeHorizontal(new Vector3(pos.x+1, pos.y, pos.z), new Vector3(pos.x+2, pos.y, pos.z)) && (face==1 ? 
				GameObject.Find ("White").GetComponent<White> ().canCastle() : GameObject.Find ("Black").GetComponent<Black> ().canCastle())) { // if face is 1, check for white, else black
				if((face==1 ? 
					GameObject.Find ("White").GetComponent<White> ().canCastleRook1() : GameObject.Find ("Black").GetComponent<Black> ().canCastleRook2())){
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x+2, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
			}
			if (checkPathFreeHorizontal(new Vector3(pos.x-3, pos.y, pos.z), new Vector3(pos.x-1, pos.y, pos.z)) && (face==1 ? 
				GameObject.Find ("White").GetComponent<White> ().canCastle() : GameObject.Find ("Black").GetComponent<Black> ().canCastle())) {
				if((face==1 ? 
					GameObject.Find ("White").GetComponent<White> ().canCastleRook2() : GameObject.Find ("Black").GetComponent<Black> ().canCastleRook1())){
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x-2, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
			}


			if (face == 1) {
				// Check capture moves
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableBlackEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			} else {
				// Check capture moves
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x + 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z - 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x - 1, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
				if (checkHitableWhiteEnemy (new Vector3 (pos.x, pos.y, pos.z + 1 * face), pos)) {
					highlightList.Add (Instantiate (highlight, new Vector3 (pos.x, pos.y, pos.z + 1 * face), Quaternion.identity));
					highlightCount++;
				}
			}

			highlights = new GameObject[highlightList.Count];
			int c = 0;
			foreach (GameObject g in highlightList) {
				highlights [c++] = g;
			}

		}
	}

	public void removeHighlights () {
		//Destroy (GameObject.FindGameObjectsWithTag ("highlight"));

		if (highlights != null) {
			foreach (GameObject high in highlights) {
				GameObject.Destroy (high);
			}
			highlightCount = 0;
		}
		//highlights = null;
	}

	public bool checkHightlighted (Vector3 pos) {
		for (int i = 0; i < highlightCount; i++) {
			if (pos.x == highlights [i].transform.position.x
			   && pos.y == highlights [i].transform.position.y
			   && pos.z == highlights [i].transform.position.z)
				return true;
		}

		return false;
	}

	public bool isWhiteCheckmate(){

		// make highlights invisible
		Color color = highlight.GetComponent<Renderer> ().sharedMaterial.color;
		color.a = 0;
		highlight.GetComponent<Renderer> ().sharedMaterial.color = color;
		UnitType ut = UnitType.NULL;


		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				ut = GameObject.Find ("White").GetComponent<White> ().getUnitTypeAtPosition (new Vector3 (i, 0, j));
				if (ut != UnitType.NULL) {
					removeHighlights ();
					highLightMoveLocations(ut, new Vector3(i, 0, j), 1); // face is 1 for white, -1 for black

					if (highlightCount != 0) {
						// make highlights visible again
						color.a = (1f/255f*150f);
						highlight.GetComponent<Renderer> ().sharedMaterial.color = color;

						removeHighlights ();
						return false;
					}
				}
			}
		}
		removeHighlights ();



		// make highlights visible again
		color.a = (1f/255f*150f);
		highlight.GetComponent<Renderer> ().sharedMaterial.color = color;

		return true;
	}

	public bool isBlackCheckmate(){

		// make highlights invisible
		Color color = highlight.GetComponent<Renderer> ().sharedMaterial.color;
		color.a = 0;
		highlight.GetComponent<Renderer> ().sharedMaterial.color = color;
		UnitType ut = UnitType.NULL;


		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				ut = GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (new Vector3 (i, 0, j));
				if (ut != UnitType.NULL) {
					removeHighlights ();
					highLightMoveLocations(ut, new Vector3(i, 0, j), -1); // face is 1 for white, -1 for black

					if (highlightCount != 0) {
						// make highlights visible again
						color.a = (1f/255f*150f);
						highlight.GetComponent<Renderer> ().sharedMaterial.color = color;

						removeHighlights ();
						return false;
					}
				}
			}
		}
		removeHighlights ();



		// make highlights visible again
		color.a = (1f/255f*150f);
		highlight.GetComponent<Renderer> ().sharedMaterial.color = color;

		return true;
	}

}
