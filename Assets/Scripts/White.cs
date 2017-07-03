using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public float speed;


	Vector3 trash = new Vector3(-5, 0, -5);


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



		// get tile location and move
		/*Vector3 pos = GameObject.Find ("Board").GetComponent<board> ().getTilePos (0, 2);

		if(Input.GetMouseButtonDown(0)){ // TODO: Add coroutine for movement...
			float timePassed = 0;
			while (timePassed < 1.0f) {
				// Increment the time to prevent an infinite loop
				timePassed += Time.deltaTime; 
				// Slowly adjust the position and rotation so it approaches the spawn position and rotation
				//transform.position += deltaPos * Time.deltaTime;
				pawn1.GetComponent<Transform>().position = Vector3.MoveTowards(pawn1.GetComponent<Transform>().position, pos, Time.deltaTime);
			}

			Debug.Log (pos);


			//float step = speed * Time.deltaTime;
			//pawn1.GetComponent<Transform>().Translate(new Vector3(pos.x, pawn1.GetComponent<Transform>().position.y, pos.y));
			Debug.Log (pawn1.GetComponent<Transform>().position.x + " " + pawn1.GetComponent<Transform>().position.z);
		
		}
		*/
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
		else if (rook1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, rook1));
		else if (rook2.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, rook2));
		else if (knight1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, knight1));
		else if (knight2.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, knight2));
		else if (bishop1.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, bishop1));
		else if (bishop2.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, bishop2));
		else if (queen.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, queen));
		else if (king.transform.position.Equals (pos))
			StartCoroutine (smoothMoveToLocationRoutine (pos, pos2, king));

	}

	IEnumerator smoothMoveToLocationRoutine(Vector3 pos1, Vector3 pos2, GameObject obj){
		float t = 0.0f;

		while(obj.transform.position != pos2){
			
			obj.transform.position = new Vector3 (	Mathf.Lerp(pos1.x, pos2.x, t) ,
													Mathf.Lerp(pos1.y, pos2.y, t) ,
													Mathf.Lerp(pos1.z, pos2.z, t) );


			t += speed * Time.deltaTime;
				
			yield return null;
		}
		Debug.Log("check: " + GameObject.Find ("Board").GetComponent<board> ().isCheck(0+0));

	}

	public UnitType getUnitTypeAtPosition(Vector3 pos){
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
