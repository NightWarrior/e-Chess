using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class White : MonoBehaviour {

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

	public GameObject knightModel;
	public GameObject bishopModel;
	public GameObject rookModel;
	public GameObject queenModel;

	private UnitType[] pawnPromotions;

	public float speed;

	public bool castleRook1;
	public bool castleRook2;
	public bool castleKing;
	public bool inPassing;

	public Vector3 inPassingPawn;


	Vector3 trash = new Vector3(-5, 0, -5);


	// Use this for initialization
	void Start () {
		pawnPromotions = new UnitType[8];
		for (int i = 0; i<8; i++) {
			pawnPromotions[i] = UnitType.PAWN;
		}

		castleRook1 = true;
		castleRook2 = true;
		castleKing = true;

		inPassing = false;
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

	public void moveObjectAtLocationTo(Vector3 pos, Vector3 pos2){
		

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
				StartCoroutine (smoothMoveToLocationRoutine (new Vector3(pos.x+3, pos.y, pos.z), new Vector3(pos2.x-1, pos2.y, pos2.z), rook2));

			if(pos.x-2 == pos2.x)
				StartCoroutine (smoothMoveToLocationRoutine (new Vector3(pos.x-4, pos.y, pos.z), new Vector3(pos2.x+1, pos2.y, pos2.z), rook1));
		}


	}

	IEnumerator smoothMoveToLocationRoutine(Vector3 pos1, Vector3 pos2, GameObject obj){
		float t = 0.0f;

		// adjusting for inPassing move
		if (GameObject.Find ("Black").GetComponent<Black> ().isInPassing (pos2, pos1)) {
			GameObject.Find ("Black").GetComponent<Black> ().removeUnit (GameObject.Find ("Black").GetComponent<Black> ().inPassingPawn);
		}

		while(obj.transform.position != pos2){
			
			obj.transform.position = new Vector3 (	Mathf.Lerp(pos1.x, pos2.x, t) ,
													Mathf.Lerp(pos1.y, pos2.y, t) ,
													Mathf.Lerp(pos1.z, pos2.z, t) );


			t += speed * Time.deltaTime;
				
			yield return null;
		}

		if (pos1.z == 1 && pos2.z == 3 && pos1.x == pos2.x && getUnitTypeAtPosition (pos2) == UnitType.PAWN) {
			inPassing = true;
			inPassingPawn = new Vector3 (pos2.x, pos2.y, pos2.z);
		}

		BoardStructure bs = new BoardStructure ();
		if (bs.isBlackCheck ()) {
			GameObject.Find ("GameManager").GetComponent<gameManager> ().check = true;
			GameObject.Find ("CheckText").GetComponent<Text> ().text = "Black King in check.";
//			Debug.Log("After White' turn: checkmate: " + GameObject.Find ("Board").GetComponent<board> ().isBlackCheckmate());
			if(GameObject.Find ("Board").GetComponent<board> ().isBlackCheckmate()){
				GameObject.Find ("GameManager").GetComponent<gameManager> ().resetGame("White wins\nPress any key to restart.");
			}
		}

		GameObject.Find ("Black").GetComponent<Black> ().disableInPassing ();


		// Promotion
		if(getUnitTypeAtPosition (pos2) == UnitType.PAWN && pos2.z==7){
			StartCoroutine(promotionRoutine(pos2));
		}
	}

	IEnumerator promotionRoutine(Vector3 pos2){
		GameObject.Find ("Board").GetComponent<board> ().setPromotionType ("Null");
		GameObject.Find ("Board").GetComponent<board> ().PromotionMenu.SetActive (true);
		while(GameObject.Find ("Board").GetComponent<board> ().getPromotionType() == UnitType.NULL){
			yield return null;
		}
		promotePawn (GameObject.Find ("Board").GetComponent<board> ().getPromotionType (), pos2);
		GameObject.Find ("Board").GetComponent<board> ().PromotionMenu.SetActive (false);
		GameObject.Find ("Board").GetComponent<board> ().setPromotionType ("Null");

	}

	public void disableInPassing(){
		if (inPassing)
			inPassing = false;
	}

	public bool isInPassing(){
		return inPassing;
	}

	public bool isInPassing(Vector3 pos, Vector3 killerPiecePos){
		if (!isInPassing () || GameObject.Find ("Black").GetComponent<Black> ().getUnitTypeAtPosition (killerPiecePos) != UnitType.PAWN)
			return false;
		if (pos.z+1 == inPassingPawn.z && pos.x == inPassingPawn.x) {
			return true;
		}
		return false;
	}

	private GameObject getModelforType(UnitType u){
		switch (u) {
		case UnitType.BISHOP:
			return bishopModel;
		case UnitType.KNIGHT:
			return knightModel;
		case UnitType.ROOK:
			return rookModel;
		case UnitType.QUEEN:
			return queenModel;
		}
		return null;
	}

	private float getModelHeightforType(UnitType u){
		switch (u) {
		case UnitType.BISHOP:
			return 0.936F;
		case UnitType.KNIGHT:
			return 0.69F;
		case UnitType.ROOK:
			return 0.67F;
		case UnitType.QUEEN:
			return 0.96F;
		}
		return 0F;
	}

	private void promotePawn(UnitType u, Vector3 pos){
		float xOffset = (u == UnitType.BISHOP ? -0.18F : 0F);
		if (pawn1.transform.position.Equals (pos)) {
			pawnPromotions [0] = u;
			foreach(Transform child in pawn1.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn1.transform;
			model.transform.position = new Vector3(pawn1.transform.position.x+xOffset, pawn1.transform.position.y+getModelHeightforType(u), pawn1.transform.position.z);
		} else if (pawn2.transform.position.Equals (pos)){
			pawnPromotions[1] = u;
			foreach(Transform child in pawn2.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn2.transform;
			model.transform.position = new Vector3(pawn2.transform.position.x+xOffset, pawn2.transform.position.y+getModelHeightforType(u), pawn2.transform.position.z);
		} else if (pawn3.transform.position.Equals (pos)){
			pawnPromotions[2] = u;
			foreach(Transform child in pawn3.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn3.transform;
			model.transform.position = new Vector3(pawn3.transform.position.x+xOffset, pawn3.transform.position.y+getModelHeightforType(u), pawn3.transform.position.z);
		} else if (pawn4.transform.position.Equals (pos)){
			pawnPromotions[3] = u;
			foreach(Transform child in pawn4.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn4.transform;
			model.transform.position = new Vector3(pawn4.transform.position.x+xOffset, pawn4.transform.position.y+getModelHeightforType(u), pawn4.transform.position.z);
		} else if (pawn5.transform.position.Equals (pos)){
			pawnPromotions[4] = u;
			foreach(Transform child in pawn5.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn5.transform;
			model.transform.position = new Vector3(pawn5.transform.position.x+xOffset, pawn5.transform.position.y+getModelHeightforType(u), pawn5.transform.position.z);
		} else if (pawn6.transform.position.Equals (pos)){
			pawnPromotions[5] = u;
			foreach(Transform child in pawn6.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn6.transform;
			model.transform.position = new Vector3(pawn6.transform.position.x+xOffset, pawn6.transform.position.y+getModelHeightforType(u), pawn6.transform.position.z);
		} else if (pawn7.transform.position.Equals (pos)){
			pawnPromotions[6] = u;
			foreach(Transform child in pawn7.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn7.transform;
			model.transform.position = new Vector3(pawn7.transform.position.x+xOffset, pawn7.transform.position.y+getModelHeightforType(u), pawn7.transform.position.z);
		} else if (pawn8.transform.position.Equals (pos)){
			pawnPromotions[7] = u;
			foreach(Transform child in pawn8.GetComponentInChildren<Transform>()){
				GameObject.Destroy (child.gameObject);
			}
			GameObject model = Instantiate (getModelforType(u));
			model.transform.parent = pawn8.transform;
			model.transform.position = new Vector3(pawn8.transform.position.x+xOffset, pawn8.transform.position.y+getModelHeightforType(u), pawn8.transform.position.z);
		}
	}

	public UnitType getUnitTypeAtPosition(Vector3 pos){
		if (pawn1.transform.position.Equals (pos))
			return pawnPromotions[0];
		else if (pawn2.transform.position.Equals (pos))
			return pawnPromotions[1];
		else if (pawn3.transform.position.Equals (pos))
			return pawnPromotions[2];
		else if (pawn4.transform.position.Equals (pos))
			return pawnPromotions[3];
		else if (pawn5.transform.position.Equals (pos))
			return pawnPromotions[4];
		else if (pawn6.transform.position.Equals (pos))
			return pawnPromotions[5];
		else if (pawn7.transform.position.Equals (pos))
			return pawnPromotions[6];
		else if (pawn8.transform.position.Equals (pos))
			return pawnPromotions[7];
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

	public void removeUnit(Vector3 pos){
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

	public Vector3 getKingLocation(){
		return king.transform.position;
	}

}
