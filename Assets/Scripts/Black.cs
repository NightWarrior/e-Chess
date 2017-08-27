using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Black : MonoBehaviour {


	public GameObject pawn1;
	public GameObject pawn2;
	public GameObject pawn3;
	public GameObject pawn4;
	public GameObject pawn5;
	public GameObject pawn6;
	public GameObject pawn7;
	public GameObject pawn8;
	public GameObject rook1;
	public GameObject knight1;
	public GameObject bishop1;
	public GameObject queen;
	public GameObject king;
	public GameObject bishop2;
	public GameObject knight2;
	public GameObject rook2;

	public float speed;

	public bool castleRook1;
	public bool castleRook2;
	public bool castleKing;


	Vector3 trash = new Vector3(-5, 0, -5);


	// Use this for initialization
	void Start () {
		castleRook1 = true;
		castleRook2 = true;
		castleKing = true;
	}

	public void hasCastled(){
		castleRook1 = false;
		castleRook2 = false;
		castleKing = false;
	}

	public bool canCastle(){
		return castleKing && (castleRook1 || castleRook2);
	}

	public bool canCastleRook1(){
		return castleRook1;
	}

	public bool canCastleRook2(){
		return castleRook2;
	}

	// Update is called once per frame
	void Update () {

	}

	public void moveObjectAtLocationTo (Vector3 pos, Vector3 pos2) {


		if (pawn1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn1));
		else if (pawn2.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn2));
		else if (pawn3.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn3));
		else if (pawn4.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn4));
		else if (pawn5.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn5));
		else if (pawn6.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn6));
		else if (pawn7.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn7));
		else if (pawn8.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, pawn8));
		else if (rook1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, rook1));
		else if (rook1.transform.position.Equals (pos)) {
			if (canCastleRook1 ()) castleRook1 = false;
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, rook1));
		} else if (rook2.transform.position.Equals (pos)) {
			if (canCastleRook2 ()) castleRook2 = false;
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, rook2));
		} else if (knight1.transform.position.Equals (pos)) 
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, knight1));
		else if (knight2.transform.position.Equals (pos)) 
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, knight2));
		else if (bishop1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, bishop1));
		else if (bishop2.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, bishop2));
		else if (queen.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, queen));
		else if (king.transform.position.Equals (pos)) {
			if (canCastle ())
				hasCastled ();
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, king));

			// check if its a castling move
			if(pos.x+2 == pos2.x)
				StartCoroutine (smoothMoveToLocationRoutine (new Vector3(pos.x+3, pos.y, pos.z), new Vector3(pos2.x-1, pos2.y, pos2.z), rook1));

			if(pos.x-2 == pos2.x)
				StartCoroutine (smoothMoveToLocationRoutine (new Vector3(pos.x-4, pos.y, pos.z), new Vector3(pos2.x+1, pos2.y, pos2.z), rook2));
		}

	}

	IEnumerator smoothMoveToLocationRoutine (Vector3 pos1, Vector3 pos2, GameObject obj) {
		float t = 0.0f;

		while (obj.transform.position != pos2) {

			obj.transform.position = new Vector3 (Mathf.Lerp (pos1.x, pos2.x, t),
				Mathf.Lerp (pos1.y, pos2.y, t),
				Mathf.Lerp (pos1.z, pos2.z, t));


			t += speed * Time.deltaTime;

			yield return null;
		}

		BoardStructure bs = new BoardStructure ();
		if (bs.isWhiteCheck ()) {
			GameObject.Find ("GameManager").GetComponent<gameManager> ().check = true;
			GameObject.Find ("CheckText").GetComponent<Text> ().text = "King in check.";
//			Debug.Log("After Black' turn: checkmate: " + GameObject.Find ("Board").GetComponent<board> ().isWhiteCheckmate());
			if (GameObject.Find ("Board").GetComponent<board> ().isWhiteCheckmate ()) {
				GameObject.Find ("GameManager").GetComponent<gameManager> ().resetGame ("Black wins\nPress any key to restart.");
			}
		}
	}

	public UnitType getUnitTypeAtPosition (Vector3 pos) {
		if (pawn1.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn2.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn3.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn4.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn5.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn6.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn7.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (pawn8.transform.position.Equals (pos))
			return UnitType.PAWN;
		else if (rook1.transform.position.Equals (pos))
			return UnitType.ROOK;
		else if (rook2.transform.position.Equals (pos))
			return UnitType.ROOK;
		else if (knight1.transform.position.Equals (pos))
			return UnitType.KNIGHT;
		else if (knight2.transform.position.Equals (pos))
			return UnitType.KNIGHT;
		else if (bishop1.transform.position.Equals (pos))
			return UnitType.BISHOP;
		else if (bishop2.transform.position.Equals (pos))
			return UnitType.BISHOP;
		else if (queen.transform.position.Equals (pos))
			return UnitType.QUEEN;
		else if (king.transform.position.Equals (pos))
			return UnitType.KING;

		return UnitType.NULL;
	}

	public void removeUnit (Vector3 pos) {
		if (pawn1.transform.position.Equals (pos))
			pawn1.transform.position = trash;
		else if (pawn2.transform.position.Equals (pos))
			pawn2.transform.position = trash;
		else if (pawn3.transform.position.Equals (pos))
			pawn3.transform.position = trash;
		else if (pawn4.transform.position.Equals (pos))
			pawn4.transform.position = trash;
		else if (pawn5.transform.position.Equals (pos))
			pawn5.transform.position = trash;
		else if (pawn6.transform.position.Equals (pos))
			pawn6.transform.position = trash;
		else if (pawn7.transform.position.Equals (pos))
			pawn7.transform.position = trash;
		else if (pawn8.transform.position.Equals (pos))
			pawn8.transform.position = trash;
		else if (rook1.transform.position.Equals (pos))
			rook1.transform.position = trash;
		else if (rook2.transform.position.Equals (pos))
			rook2.transform.position = trash;
		else if (knight1.transform.position.Equals (pos))
			knight1.transform.position = trash;
		else if (knight2.transform.position.Equals (pos))
			knight2.transform.position = trash;
		else if (bishop1.transform.position.Equals (pos))
			bishop1.transform.position = trash;
		else if (bishop2.transform.position.Equals (pos))
			bishop2.transform.position = trash;
		else if (queen.transform.position.Equals (pos))
			queen.transform.position = trash;
		else if (king.transform.position.Equals (pos))
			king.transform.position = trash;
	}

	public Vector3 getKingLocation () {
		return king.transform.position;
	}

}
