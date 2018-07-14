using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for CSV
using System.Text;
using System.IO;
using System;

public class MainBackup : MonoBehaviour
{
	//CSV
	private List<string[]> rowData = new List<string[]> ();

	//Sound
	public AudioClip MusicClip;
	public AudioSource MusicSource;

	//Start Conditions - get set by Modal
	public String Vp;
	public int expCond;
	private float fps;
	private float timeNeeded;
	public bool wait = false;
	public bool trial = false;
	public bool expStart = false;
	public bool targetExit = true;
	public bool startExit = false;

	//Experiment Conditions
	private int pointNum;
	private int pointNumInPhase;
	private float waitTimer = 0.0f;

	private bool tooFastInfo = false;
	private float tooFastTimer = 0.0f;
	private bool pauseInfo = false;
	private float pauseTimer = 0.0f;
	private bool pertInfo = false;
	private float pertTimer = 0.0f;
	private bool noPertInfo = false;
	private float noPertTimer = 0.0f;
	private bool info = true;

	private string contactPoint;
	private string phase;
	private float TimeOfFeedback;
	private float TimeOfStart;

	//30 Prismendioptrin
	//private Vector3 displacement = new Vector3(-0.23943348f, 0f, 0f);

	//20 Prismendioptrin
	private Vector3 displacement = new Vector3 (-0.15962232f, 0f, 0f);

	//10 Prismendioptrin
	//private Vector3 displacement = new Vector3(-0.07981116f, 0f, 0f);

	//displacement of Sphere
	private float disSph = 0.05f;
	private Vector3 sphPosOne = new Vector3 (0.2399787f, 0.964f, -0.0456f);
	private Vector3 sphPosTwo;
	private Vector3 sphPosThr;
	private Vector3 sphPosFou;
	private Vector3 sphPosFiv;
	private Vector3 sphPosSix;
	private Vector3 sphPosSev;
	private Vector3 sphPosEig;
	private Vector3 sphPosNin;
	Vector3[] sphPosCha;


	// Use this for initialization
	void Start ()
	{
		Save ();
		pointNum = 1;
		MusicSource.clip = MusicClip;

		//ACHTUNG HIER

		GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("Sphere").transform.position = sphPosOne;
		sphPosTwo = new Vector3 (sphPosOne.x - disSph, sphPosOne.y, sphPosOne.z);
		sphPosThr = new Vector3 (sphPosOne.x - disSph, sphPosOne.y + disSph, sphPosOne.z);
		sphPosFou = new Vector3 (sphPosOne.x, sphPosOne.y + disSph, sphPosOne.z);
		sphPosFiv = new Vector3 (sphPosOne.x + disSph, sphPosOne.y + disSph, sphPosOne.z);
		sphPosSix = new Vector3 (sphPosOne.x + disSph, sphPosOne.y, sphPosOne.z);
		sphPosSev = new Vector3 (sphPosOne.x + disSph, sphPosOne.y - disSph, sphPosOne.z);
		sphPosEig = new Vector3 (sphPosOne.x, sphPosOne.y - disSph, sphPosOne.z);
		sphPosNin = new Vector3 (sphPosOne.x - disSph, sphPosOne.y - disSph, sphPosOne.z);
		/*sphPosCha = new Vector3[] {
			//Basiserhebung
			sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosSix, sphPosSix, 
			sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosSix, sphPosSix,
			sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix,
			//Adaptationsphase
			sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix,
			sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, 
			sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo,
			sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix,
			sphPosTwo, sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix,
			//Readaptationsphase
			sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo,
			sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosSix, sphPosSix,
			sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix
		};
		/* sphPosCha = new Vector3[] {
			//Basiserhebung
			sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosSix, 
			sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo,
			sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix,
			//Adaptationsphase
			sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo,
			sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosSix, 
			sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix,
			sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix,
			sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo, sphPosTwo, sphPosSix,
			//Readaptationsphase
			sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo,
			sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosTwo, sphPosTwo,
			sphPosSix, sphPosSix, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosSix, sphPosTwo, sphPosTwo, sphPosSix
		};*/
		sphPosCha = new Vector3[] {
			//Basiserhebung
			sphPosOne, sphPosTwo, sphPosFiv, sphPosFiv, sphPosNin, sphPosOne, sphPosTwo, sphPosNin, sphPosTwo, sphPosSix, 
			sphPosFiv, sphPosTwo, sphPosNin, sphPosFiv, sphPosTwo, sphPosThr, sphPosSix, sphPosThr, sphPosTwo, sphPosFiv,
			sphPosTwo, sphPosEig, sphPosFiv, sphPosFou, sphPosEig, sphPosEig, sphPosFou, sphPosEig, sphPosNin, sphPosOne,

			//Adaptationsphase
			sphPosOne, sphPosThr, sphPosOne, sphPosSix, sphPosEig, sphPosSix, sphPosTwo, sphPosNin, sphPosOne, sphPosSix,
			sphPosThr, sphPosEig, sphPosFou, sphPosFiv, sphPosTwo, sphPosOne, sphPosFiv, sphPosNin, sphPosFou, sphPosThr,
			sphPosTwo, sphPosThr, sphPosSix, sphPosEig, sphPosThr, sphPosFiv, sphPosEig, sphPosNin, sphPosEig, sphPosFiv,
			sphPosFiv, sphPosNin, sphPosOne, sphPosFiv, sphPosFou, 

			//Readaptationsphase
			sphPosSev, sphPosFiv, sphPosEig, sphPosOne, sphPosFiv, sphPosSix, sphPosSev, sphPosEig, sphPosOne, sphPosEig, 
			sphPosEig, sphPosSix, sphPosOne, sphPosEig, sphPosNin, sphPosFiv, sphPosSev, sphPosTwo, sphPosNin, sphPosThr, 
			sphPosSev, sphPosFou, sphPosFou, sphPosEig, sphPosTwo, sphPosSev, sphPosSix, sphPosSix, sphPosFou, sphPosTwo
		};
	}

	void Save ()
	{
		//Create first row for CSV
		string[] rowDataTemp;
		rowDataTemp = new string[] {
			"Versuchsperson",
			"Versuchsbedingung",
			"Phase",
			"ZeigebewegungTotal",
			"ZeigebewegungPhase",
			//WURDE HINZUGEFÜGT, ACHTUNG R
			"ReaktionszeitHTCVive",
			"ZeitStart",
			"ZeitFeedback",
			"HandX",
			"HandY",
			"HandZ",
			"FedX",
			"FedY",
			"FedZ",
			"SphereX",
			"SphereY",
			"SphereZ"
		};
		rowData.Add (rowDataTemp);
	}

	private string getPath ()
	{
		#if UNITY_EDITOR
		return Application.dataPath + "/CSV/" + expCond + "_" + Vp + ".csv";
		#elif UNITY_ANDROID
		return Application.persistentDataPath+"Saved_data.csv";
		#elif UNITY_IPHONE
		return Application.persistentDataPath+"/"+"Saved_data.csv";
		#else
		return Application.dataPath +"/"+"Saved_data.csv";
		#endif
	}

	void Update ()
	{
		fps = Time.deltaTime;
		if (wait == true) {
			if (pertInfo == true) {
				if (pertTimer == 0.0f) {
					pertTimer = Time.time;
					expStart = false;
					GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
				} else if (Time.time - pertTimer >= 10.0f) {
					pertInfo = false;
					expStart = true;
					GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;


		//GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position + new Vector3 (5f, 0f, 0f);


				}
			}
			if (noPertInfo == true) {
				if (noPertTimer == 0.0f) {
					noPertTimer = Time.time;
					expStart = false;
					GameObject.Find ("InfoNormal").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("InfoNormalBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
				} else if (Time.time - noPertTimer >= 10.0f) {
					noPertInfo = false;
					expStart = true;
					GameObject.Find ("InfoNormal").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("InfoNormalBackground").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;


		//GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position - new Vector3 (5f, 0f, 0f);


				}
			}
			if (pauseInfo == true) {
				if (pauseTimer == 0.0f) {
					pauseTimer = Time.time;
					expStart = false;
					GameObject.Find ("Pause").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("PauseBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
				} else if (Time.time - pauseTimer >= 10.0f) {
					pauseInfo = false;
					info = false;
					expStart = true;
					pauseTimer = 0.0f;
					GameObject.Find ("Pause").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("PauseBackground").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;
				}
			}
			if (tooFastInfo == true) {
				if (tooFastTimer == 0.0f) {
					Debug.Log ("info");
					tooFastTimer = Time.time;
					expStart = false;
					GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("toofastBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
				} else if (Time.time - tooFastTimer >= 5.0f) {
					tooFastInfo = false;
					expStart = true;
					tooFastTimer = 0.0f;
					GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("toofastBackground").GetComponent<Renderer> ().enabled = false;
					wait = false;
					if (contactPoint == "target") {
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
					}
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;
				}
			}
			if (expStart == true) {
				if (waitTimer == 0.0f) {
					waitTimer = Time.time;
				} else if (Time.time - waitTimer >= 1.0f) {
					MusicSource.Play ();
					waitTimer = 0.0f;
					wait = false;
					if (contactPoint == "target") {
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
						targetExit = true;
						startExit = false;
						trial = false;
					} else if (contactPoint == "start") {
						if (pointNum <= 30) {
							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1];
						} else if (pointNum > 30 && pointNum <= 30 + expCond) {
							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1] + displacement;
						} else if (pointNum > expCond + 30 && pointNum <= expCond + 60) {


							//ACHTUNG HIER: 50 statt 35


							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - expCond - 1 + 35];
							//GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - expCond];
						} else if (pointNum == 60 + expCond + 1) {
							GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
							GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
							GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
		GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
							Debug.Log ("Experiment finished");

							//write to CSV
							string[][] output = new string[rowData.Count][];

							for (int i = 0; i < output.Length; i++) {
								output [i] = rowData [i];
							}

							int length = output.GetLength (0);
							string delimiter = ",";

							StringBuilder sb = new StringBuilder ();

							for (int index = 0; index < length; index++)
								sb.AppendLine (string.Join (delimiter, output [index]));


							string filePath = getPath ();

							StreamWriter outStream = System.IO.File.CreateText (filePath);
							outStream.WriteLine (sb);
							outStream.Close ();
							GameObject.Find ("Ende").GetComponent<Renderer> ().enabled = true;
							pointNum++;
						}
						targetExit = false;
						trial = true;
					}
				}
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (expStart == true) {
			if (col.gameObject.tag == "proxWall") {
				if (trial == true) {
					if (pointNum <= 30) {
						GameObject.Find ("Feedback").transform.position = col.contacts [0].point + new Vector3 (0f, 0f, -0.003f);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "baseline";
						pointNumInPhase = pointNum;
						TimeOfFeedback = Time.time;
						Debug.Log (phase + " " + pointNum);
					} else if (pointNum > 30 && pointNum <= 30 + expCond) {
		GameObject.Find ("Feedback").transform.position = col.contacts [0].point + new Vector3 (0f, 0f, -0.003f) + displacement;
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "adaptation";
						pointNumInPhase = pointNum - 30;
						TimeOfFeedback = Time.time;
						Debug.Log (phase + " " + pointNum);
					} else if (pointNum > expCond + 30 && pointNum <= expCond + 60) {
		GameObject.Find ("Feedback").transform.position = col.contacts [0].point + new Vector3 (0f, 0f, -0.003f);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						Debug.Log (phase + " " + pointNum);
						pointNumInPhase = pointNum - 30 - expCond;
						TimeOfFeedback = Time.time;
						phase = "readaptation";
					}

					//Add to CSV-Array


					//FRAMES HINZUFÜGEN


					Vector3 feedCoor = GameObject.Find ("Feedback").transform.position;
					Vector3 sphereCoor = GameObject.Find ("Sphere").transform.position;

					if (fps > Time.fixedDeltaTime) {
						timeNeeded = fps;
					} else if (fps <= Time.fixedDeltaTime) {
						timeNeeded = Time.fixedDeltaTime;
					}

					string[] rowDataTem;
					rowDataTem = new string[] {
						Vp,
						expCond.ToString (),
						phase,
						pointNum.ToString (),
						pointNumInPhase.ToString (),
						timeNeeded.ToString (),
						TimeOfStart.ToString (),
						TimeOfFeedback.ToString (),
						col.contacts [0].point.x.ToString (),
						col.contacts [0].point.y.ToString (),
						col.contacts [0].point.z.ToString (),
						feedCoor.x.ToString (),
						feedCoor.y.ToString (),
						feedCoor.z.ToString (),
						sphereCoor.x.ToString (),
						sphereCoor.y.ToString (),
						sphereCoor.z.ToString ()
					};
					rowData.Add (rowDataTem);
					pointNum++;
					info = true;
					wait = true;
					contactPoint = "target";
					trial = false;
				} else if (trial == false && startExit == true) {
					wait = true;
				}
			} else if (col.gameObject.tag == "table") {
				if (trial == false && targetExit == true) {
					wait = true;
					contactPoint = "start";
					if (expCond > 0 && pointNum == 31) {
						pertInfo = true;
					}
					if (expCond > 0 && pointNum == (31 + expCond)) {
						noPertInfo = true;
					}
					if (expCond == 0 && info == true) {
						if (pointNum % 15 == 1) {
							pauseInfo = true;
							Debug.Log ("pause");
						}
					} else if (expCond > 0 && info == true) {
						if (pointNum <= (31 + expCond) && pointNum % 15 == 1 && pointNum != 31 && pointNum != (31 + expCond - 5) && pointNum != 1) {
							pauseInfo = true;
							Debug.Log ("pause");
						} else if (pointNum > (31 + expCond) && (pointNum - 30 - expCond) % 15 == 1) {
							pauseInfo = true;
							Debug.Log ("pause");
						}
					}
				}
			}
		}
	}

	void OnCollisionExit (Collision col)
	{
		if (expStart == true) {
			if (col.gameObject.tag == "proxWall") {
				if (wait == true) {
					//wait = false;
					waitTimer = 0.0f;
					tooFastInfo = true;
					Debug.Log ("should Info");
				}
			} else if (col.gameObject.tag == "table") {
				if (wait == true) {
					//wait = false;
					waitTimer = 0.0f;
					tooFastInfo = true;
					Debug.Log ("should Info");
				} else {
					TimeOfStart = Time.time;
					startExit = true;
				}
			}
		}
	}
}
