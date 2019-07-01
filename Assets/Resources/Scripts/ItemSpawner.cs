using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

	[System.Serializable]
	public class InferenceResult{

		public float Minimum;
		public string AddHPPlayerStatus;
		public string AddHPSeaStatus;
		public string DoubleScoreStatus;
		public string SlowdownStatus;

	}
		
	public GameObject[] spawnItem;
	public Transform[] spawnItemLocation;
	private int itemlocationIndex;
	public float itemspawnWait = 2f;
	public float itemcountDown = 2f;

	private float HPPlayerLow;
	private float HPPLayerMid;
	private float HPPlayerHigh;

	private float HPSeaLow;
	private float HPSeaMid;
	private float HPSeaHigh;

	private float FailLow;
	private float FailMid;
	private float FailHigh;

	public InferenceResult[] rules;

	public static float Z_addHPPlayer;
	public static float Z_addHPSea;
	public static float Z_slowerSpeed;
	public static float Z_doubleScore;

	// Use this for initialization
	void Start () {

		rules = new InferenceResult[27];
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		itemlocationIndex = Random.Range (0, spawnItemLocation.Length);

		if (GameController.startCountingPoint > 30) {

			GameController.startCountingPoint = 0;

			PlayerHPFuzzification (GameController.boatHealthValue);
			SeaHPFuzzification (GameController.seaHealthValue);
			FailFuzzification (GameController.failValue);

			FuzzyRulesInference ();

			AddPlayerHPItemDefuzzification ();
			AddSeaHPItemDefuzzification ();
			SlowdownItemDefuzzification ();
			DoubleScoreItemDefuzzification ();

			StartCoroutine ("SpawnObject");
		}
	}

	public IEnumerator SpawnObject(){

		for (int i = 0; i < spawnItem.Length; i++) {

			Instantiate (spawnItem [i], spawnItemLocation [itemlocationIndex].position, spawnItemLocation [itemlocationIndex].rotation);

			yield return new WaitForSeconds (2.0f);
		}


	}

	public void PlayerHPFuzzification (float playerHP){

		if (playerHP <= 30) {
			HPPlayerLow = 1;
			HPPLayerMid = 0;
			HPPlayerHigh = 0;
		} 
		else if ((playerHP > 30) && (playerHP < 45)) {
			HPPlayerLow = (45-playerHP)/(45-30);
			HPPLayerMid = (playerHP-30)/(45-30);
			HPPlayerHigh = 0;
		}
		else if ((playerHP >= 45) && (playerHP <= 60)){
			HPPlayerLow = 0;
			HPPLayerMid = 1;
			HPPlayerHigh = 0;
		}
		else if((playerHP > 60) && (playerHP < 75)){
			HPPlayerLow = 0;
			HPPLayerMid = (75-playerHP)/(75-60);
			HPPlayerHigh = (playerHP-60)/(75-60);
		}
		else if (playerHP >= 75){
			HPPlayerLow = 0;
			HPPLayerMid = 0;
			HPPlayerHigh = 1;
		}

		Debug.Log ("HPPlayerLow = " + HPPlayerLow);
		Debug.Log ("HPPLayerMid = " + HPPLayerMid);
		Debug.Log ("HPPlayerHigh = " + HPPlayerHigh);
	}

	public void SeaHPFuzzification(float seaHP){

		if (seaHP <= 30) {
			HPSeaLow = 1;
			HPSeaMid = 0;
			HPSeaHigh = 0;
		} 
		else if ((seaHP > 30) && (seaHP < 50)) {
			HPSeaLow = (50 - seaHP) / (50 - 30);
			HPSeaMid = (seaHP - 30) / (50 - 30);
			HPSeaHigh = 0;
		} 
		else if ((seaHP >= 50) && (seaHP <= 65)) {
			HPSeaLow = 0;
			HPSeaMid = 1;
			HPSeaHigh = 0;
		} 
		else if ((seaHP > 65) && (seaHP < 85)) {
			HPSeaLow = 0;
			HPSeaMid = (seaHP - 65) / (85 - 65);
			HPSeaHigh = (85 - seaHP) / (85 - 65);
		} 
		else if (seaHP >= 85) {
			HPSeaLow = 0;
			HPPLayerMid = 0;
			HPSeaHigh = 1;
		}

		Debug.Log ("HPSeaLow = " + HPSeaLow);
		Debug.Log ("HPSeaMid = " + HPSeaMid);
		Debug.Log ("HPSeaHigh = " + HPSeaHigh);
	}

	public void FailFuzzification (float fail){

		if (fail <= 3) {
			FailLow = 1;
			FailMid = 0;
			FailHigh = 0;
		} else if ((fail > 3) && (fail < 7)) {
			FailLow = (7 - fail) / (7 - 3);
			FailMid = (fail - 3) / (7 - 3);
			FailHigh = 0;
		} else if (fail == 7) {
			FailLow = 0;
			FailMid = 1;
			FailHigh = 0;
		} else if ((fail > 7) && (fail < 10)) {
			FailLow = 0;
			FailMid = (10 - fail) / (10 - 7);
			FailHigh = (fail - 7) / (10 - 7);
		} else if (fail >= 10) {
			FailLow = 0;
			FailMid = 0;
			FailHigh = 1;
		}

		Debug.Log ("FailLow = " + FailLow);
		Debug.Log ("FailMid = " + FailMid);
		Debug.Log ("FailHigh = " + FailHigh);
	}

	void FuzzyRulesInference(){

		rules[0].Minimum = Mathf.Min(HPPlayerLow, HPSeaLow, FailLow);
		rules[0].AddHPPlayerStatus = "Sering"; rules[0].AddHPSeaStatus = "Sering"; 
		rules[0].DoubleScoreStatus = "Jarang"; rules[0].SlowdownStatus = "Jarang";

		rules[1].Minimum = Mathf.Min (HPPlayerLow, HPSeaLow, FailMid); 
		rules[1].AddHPPlayerStatus = "Sering"; rules[1].AddHPSeaStatus = "Sering"; 
		rules[1].DoubleScoreStatus = "Jarang"; rules[1].SlowdownStatus = "Jarang";

		rules[2].Minimum = Mathf.Min (HPPlayerLow, HPSeaLow, FailHigh); 
		rules[2].AddHPPlayerStatus = "Sering"; rules[2].AddHPSeaStatus = "Sering"; 
		rules[2].DoubleScoreStatus = "Jarang"; rules[2].SlowdownStatus = "Sering";

		rules[3].Minimum = Mathf.Min (HPPlayerLow, HPSeaMid, FailLow); 
		rules[3].AddHPPlayerStatus = "Sering"; rules[3].AddHPSeaStatus = "Jarang"; 
		rules[3].DoubleScoreStatus = "Jarang"; rules[3].SlowdownStatus = "Jarang";

		rules[4].Minimum = Mathf.Min (HPPlayerLow, HPSeaMid, FailMid); 
		rules[4].AddHPPlayerStatus = "Sering"; rules[4].AddHPSeaStatus = "Jarang"; 
		rules[4].DoubleScoreStatus = "Jarang"; rules[4].SlowdownStatus = "Jarang";

		rules[5].Minimum = Mathf.Min (HPPlayerLow, HPSeaMid, FailHigh); 
		rules[5].AddHPPlayerStatus = "Sering"; rules[5].AddHPSeaStatus = "Jarang"; 
		rules[5].DoubleScoreStatus = "Jarang"; rules[5].SlowdownStatus = "Sering";

		rules[6].Minimum = Mathf.Min (HPPlayerLow, HPSeaHigh, FailLow); 
		rules[6].AddHPPlayerStatus = "Sering"; rules[6].AddHPSeaStatus = "Jarang"; 
		rules[6].DoubleScoreStatus = "Jarang"; rules[6].SlowdownStatus = "Jarang";

		rules [7].Minimum = Mathf.Min (HPPlayerLow, HPSeaHigh, FailMid); 
		rules[7].AddHPPlayerStatus = "Sering"; rules[7].AddHPSeaStatus = "Jarang"; 
		rules[7].DoubleScoreStatus = "Jarang"; rules[7].SlowdownStatus = "Jarang";

		rules[8].Minimum = Mathf.Min (HPPlayerLow, HPSeaHigh, FailHigh); 
		rules[8].AddHPPlayerStatus = "Sering"; rules[8].AddHPSeaStatus = "Jarang"; 
		rules[8].DoubleScoreStatus = "Jarang"; rules[8].SlowdownStatus = "Sering";

		rules[9].Minimum = Mathf.Min (HPPLayerMid, HPSeaLow, FailLow); 
		rules[9].AddHPPlayerStatus = "Jarang"; rules[9].AddHPSeaStatus = "Sering"; 
		rules[9].DoubleScoreStatus = "Jarang"; rules[9].SlowdownStatus = "Jarang";

		rules[10].Minimum = Mathf.Min (HPPLayerMid, HPSeaLow, FailMid); 
		rules[10].AddHPPlayerStatus = "Jarang"; rules[10].AddHPSeaStatus = "Sering"; 
		rules[10].DoubleScoreStatus = "Jarang"; rules[10].SlowdownStatus = "Jarang";

		rules[11].Minimum = Mathf.Min (HPPLayerMid, HPSeaLow, FailHigh); 
		rules[11].AddHPPlayerStatus = "Jarang"; rules[11].AddHPSeaStatus = "Sering"; 
		rules[11].DoubleScoreStatus = "Jarang"; rules[11].SlowdownStatus = "Sering";

		rules[12].Minimum = Mathf.Min (HPPLayerMid, HPSeaMid, FailLow); 
		rules[12].AddHPPlayerStatus = "Jarang"; rules[12].AddHPSeaStatus = "Jarang"; 
		rules[12].DoubleScoreStatus = "Sering"; rules[12].SlowdownStatus = "Jarang";

		rules[13].Minimum = Mathf.Min (HPPLayerMid, HPSeaMid, FailMid); 
		rules[13].AddHPPlayerStatus = "Jarang"; rules[13].AddHPSeaStatus = "Jarang"; 
		rules[13].DoubleScoreStatus = "Sering"; rules[13].SlowdownStatus = "Jarang";

		rules[14].Minimum = Mathf.Min (HPPLayerMid, HPSeaMid, FailHigh); 
		rules[14].AddHPPlayerStatus = "Jarang"; rules[14].AddHPSeaStatus = "Jarang"; 
		rules[14].DoubleScoreStatus = "Jarang"; rules[14].SlowdownStatus = "Sering";

		rules[15].Minimum = Mathf.Min (HPPLayerMid, HPSeaHigh, FailLow); 
		rules[15].AddHPPlayerStatus = "Jarang"; rules[15].AddHPSeaStatus = "Jarang"; 
		rules[15].DoubleScoreStatus = "Sering"; rules[15].SlowdownStatus = "Jarang";

		rules[16].Minimum = Mathf.Min (HPPLayerMid, HPSeaHigh, FailMid); 
		rules[16].AddHPPlayerStatus = "Jarang"; rules[16].AddHPSeaStatus = "Jarang"; 
		rules[16].DoubleScoreStatus = "Sering"; rules[16].SlowdownStatus = "Jarang";

		rules[17].Minimum = Mathf.Min (HPPLayerMid, HPSeaHigh, FailHigh); 
		rules[17].AddHPPlayerStatus = "Jarang"; rules[17].AddHPSeaStatus = "Jarang"; 
		rules[17].DoubleScoreStatus = "Jarang"; rules[17].SlowdownStatus = "Sering";

		rules[18].Minimum = Mathf.Min (HPPlayerHigh, HPSeaLow, FailLow); 
		rules[18].AddHPPlayerStatus = "Jarang"; rules[18].AddHPSeaStatus = "Sering"; 
		rules[18].DoubleScoreStatus = "Jarang"; rules[18].SlowdownStatus = "Jarang";

		rules[19].Minimum = Mathf.Min (HPPlayerHigh, HPSeaLow, FailMid); 
		rules[19].AddHPPlayerStatus = "Jarang"; rules[19].AddHPSeaStatus = "Sering"; 
		rules[19].DoubleScoreStatus = "Jarang"; rules[19].SlowdownStatus = "Jarang";

		rules[20].Minimum = Mathf.Min (HPPlayerHigh, HPSeaLow, FailHigh); 
		rules[20].AddHPPlayerStatus = "Jarang"; rules[20].AddHPSeaStatus = "Sering"; 
		rules[20].DoubleScoreStatus = "Jarang"; rules[20].SlowdownStatus = "Sering";

		rules[21].Minimum = Mathf.Min (HPPlayerHigh, HPSeaMid, FailLow); 
		rules[21].AddHPPlayerStatus = "Jarang"; rules[21].AddHPSeaStatus = "Jarang"; 
		rules[21].DoubleScoreStatus = "Jarang"; rules[21].SlowdownStatus = "Jarang";

		rules[22].Minimum = Mathf.Min (HPPlayerHigh, HPSeaMid, FailMid); 
		rules[22].AddHPPlayerStatus = "Jarang"; rules[22].AddHPSeaStatus = "Jarang"; 
		rules[22].DoubleScoreStatus = "Sering"; rules[22].SlowdownStatus = "Jarang";

		rules[23].Minimum = Mathf.Min (HPPlayerHigh, HPSeaMid, FailHigh); 
		rules[23].AddHPPlayerStatus = "Jarang"; rules[23].AddHPSeaStatus = "Jarang"; 
		rules[23].DoubleScoreStatus = "Sering"; rules[23].SlowdownStatus = "Sering";

		rules[24].Minimum = Mathf.Min (HPPlayerHigh, HPSeaHigh, FailLow); 
		rules[24].AddHPPlayerStatus = "Jarang"; rules[24].AddHPSeaStatus = "Jarang"; 
		rules[24].DoubleScoreStatus = "Sering"; rules[24].SlowdownStatus = "Jarang";

		rules[25].Minimum = Mathf.Min (HPPlayerHigh, HPSeaHigh, FailMid); 
		rules[25].AddHPPlayerStatus = "Jarang"; rules[25].AddHPSeaStatus = "Jarang"; 
		rules[25].DoubleScoreStatus = "Jarang"; rules[25].SlowdownStatus = "Jarang";

		rules[26].Minimum = Mathf.Min (HPPlayerHigh, HPSeaHigh, FailHigh); 
		rules[26].AddHPPlayerStatus = "Jarang"; rules[26].AddHPSeaStatus = "Jarang"; 
		rules[26].DoubleScoreStatus = "Jarang"; rules[26].SlowdownStatus = "Sering";

		GameController.failValue = 0;
 	}

	public void AddPlayerHPItemDefuzzification(){

		float maxJarangAddHPPlayer = rules[0].Minimum;
		float maxSeringAddHPPlayer = rules[0].Minimum;

		for (int i = 0; i < rules.Length; i++) {

			if ((rules[i].Minimum > maxJarangAddHPPlayer) && rules[i].AddHPPlayerStatus == "Jarang"){
				maxJarangAddHPPlayer = rules[i].Minimum;
			}
			if ((rules [i].Minimum > maxSeringAddHPPlayer) && rules [i].AddHPPlayerStatus == "Sering") {
				maxSeringAddHPPlayer = rules [i].Minimum;
			}
		}

		Z_addHPPlayer = ((maxJarangAddHPPlayer * (1 + 2 + 3)) + (maxSeringAddHPPlayer * (6 + 7 + 8))) 
						/ ((maxJarangAddHPPlayer * 3) + (maxSeringAddHPPlayer * 3));

		Debug.Log ("Nilai Z ADD_HPK : " + Z_addHPPlayer);
		Z_addHPPlayer = Mathf.Round (Z_addHPPlayer);

		//Debug.Log (maxJarangAddHPPlayer);
		//Debug.Log (maxSeringAddHPPlayer);
		Debug.Log ("Nilai Z ADD_HPK (Round) : " + Z_addHPPlayer);
	}

	public void AddSeaHPItemDefuzzification(){

		float maxJarangAddHPSea= rules [0].Minimum;
		float maxSeringAddHPSea = rules [0].Minimum; 

		for (int i = 0; i < rules.Length; i++) {

			if ((rules[i].Minimum > maxJarangAddHPSea) && rules[i].AddHPSeaStatus == "Jarang"){
				maxJarangAddHPSea = rules[i].Minimum;
			}
			if ((rules [i].Minimum > maxSeringAddHPSea) && rules [i].AddHPSeaStatus == "Sering") {
				maxSeringAddHPSea = rules [i].Minimum;
			}
		}

		Z_addHPSea = ((maxJarangAddHPSea * (1 + 2 + 3)) + (maxSeringAddHPSea * (6 + 7 + 8))) 
					 / ((maxJarangAddHPSea * 3) + (maxSeringAddHPSea * 3));

		Debug.Log ("Nilai Z ADD_HPL : " + Z_addHPSea);
		Z_addHPSea = Mathf.Round (Z_addHPSea);

		//Debug.Log (maxJarangAddHPSea);
		//Debug.Log (maxSeringAddHPSea);
		Debug.Log ("Nilai Z ADD_HPL (Round) : " + Z_addHPSea);

	}

	public void SlowdownItemDefuzzification(){

		float maxJarangSlowdown = rules [0].Minimum;
		float maxSeringSlowdown = rules [0].Minimum;

		for (int i = 0; i < rules.Length; i++) {

			if ((rules[i].Minimum > maxJarangSlowdown) && rules[i].SlowdownStatus == "Jarang"){
				maxJarangSlowdown = rules[i].Minimum;
			}
			if ((rules [i].Minimum > maxSeringSlowdown) && rules [i].SlowdownStatus == "Sering") {
				maxSeringSlowdown = rules [i].Minimum;
			}
		}

		Z_slowerSpeed = ((maxJarangSlowdown * (1 + 2 + 3)) + (maxSeringSlowdown * (6 + 7 + 8)))
						/ ((maxJarangSlowdown * 3) + (maxSeringSlowdown* 3));
		
		Debug.Log ("Nilai Z SLOW: " + Z_slowerSpeed);
		Z_slowerSpeed = Mathf.Round (Z_slowerSpeed);

		//Debug.Log (maxJarangSlowdown);
		//Debug.Log (maxSeringSlowdown);
		Debug.Log ("Nilai Z SLOW (Round) : " + Z_slowerSpeed);
	}

	public void DoubleScoreItemDefuzzification(){

		float maxJarangDoubleScore = rules [0].Minimum;
		float maxSeringDoubleScore = rules [0].Minimum;

		for (int i = 0; i < rules.Length; i++) {

			if ((rules[i].Minimum > maxJarangDoubleScore) && rules[i].DoubleScoreStatus == "Jarang"){
				maxJarangDoubleScore = rules[i].Minimum;
			}
			if ((rules [i].Minimum > maxSeringDoubleScore) && rules [i].DoubleScoreStatus == "Sering") {
				maxSeringDoubleScore = rules [i].Minimum;
			}
		}

		Z_doubleScore = ((maxJarangDoubleScore * (1 + 2 + 3)) + (maxSeringDoubleScore * (6 + 7 + 8)))
						/ ((maxJarangDoubleScore * 3) + (maxSeringDoubleScore * 3));

		Debug.Log ("Nilai Z DOUBLE : " + Z_doubleScore);
		Z_doubleScore = Mathf.Round (Z_doubleScore);

		//Debug.Log (maxJarangDoubleScore);
		//Debug.Log (maxSeringDoubleScore);
		Debug.Log ("Nilai Z DOUBLE (Round) : " + Z_doubleScore);
	}
}


/*m1 = (Min / 2) * (Mathf.Pow (a_predikat1, 2));
		m2 = ((((1/(1-2))/2)*(Mathf.Pow(a_predikat2, 3))) - (((2/(1-2))/2)*Mathf.Pow(a_predikat2, 2))-(((1/(1-2))/3)*(Mathf.Pow(a_predikat1, 3)))-(((2/(1-2))/2*Mathf.Pow(a_predikat1,2))));
		m3 = ((Max * Mathf.Pow (1, 2)) / 2) - (Max * (1 - 2) / 2) * (1 - 2);

		A1 = a_predikat1 * Min;
		A2 = (Min + Max) * (a_predikat2 - a_predikat1) / 2;
		A3 = (1 - (1 - 2)) * Max;

		z = (m1 + m2 + m3) / (A1 + A2 + A3);*/