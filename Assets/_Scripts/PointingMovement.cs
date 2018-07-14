using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for CSV
using System.Text;
using System.IO;
using System;

public class PointingMovement : MonoBehaviour 
{
	//CSV
	private List<string[]> rowData = new List<string[]>();

	//Sound
	public AudioClip MusicClip;
	public AudioSource MusicSource;

	//Start Conditions - get set by Modal
	public String Vp;
	public int expCond;

	//Experiment Conditions
	private int pointNum;
	private float collisionTime = 0.0f;
	private float infoTime = 0.0f;
	private float infoGoneTime = 0.0f;
	private float pauseTime = 0.0f;
	private bool trial = true;
	private bool allowCollision = false;
    public bool tooFast = false;
	private string phase;
	private int pointNumInPhase;
	private float TimeOfFeedback;
	public float trialResetTime = 0.0f;
	public bool checkForReset = false;
	public bool expStart = false;
	private bool infoOn = false;
	private bool infoGone = false;
	private bool pauseOn = false;
	public float nextPointWait = 0.0f;
	public bool wait = false;

	//30 Prismendioptrin
	//private Vector3 displacement = new Vector3(-0.23943348f, 0f, 0f);

	//20 Prismendioptrin
	private Vector3 displacement = new Vector3(-0.15962232f, 0f, 0f);

	//10 Prismendioptrin
	//private Vector3 displacement = new Vector3(-0.07981116f, 0f, 0f);

	//displacement of Sphere
	private float disSph = 0.05f;
	private Vector3 sphPosOne = new Vector3 (0.2399787f, 0.964f, -0.0456f);
	//private Vector3 sphPosOne = new Vector3 (0.2445f, 1.183f, 0.28f);
	private Vector3 sphPosTwo;
	private Vector3 sphPosThr;
	private Vector3 sphPosFou;
	private Vector3 sphPosFiv;
	private Vector3 sphPosSix;
	private Vector3 sphPosSev;
	private Vector3 sphPosEig;
	private Vector3 sphPosNin;
	Vector3 [] sphPosCha;
    
	//Controller Setup
    private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device Controller 
	{
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}
		
	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Start () 
	{
		Save();
        pointNum = 1;
		MusicSource.clip = MusicClip;
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
		sphPosCha = new Vector3[] {
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
		};
		sphPosCha = new Vector3[] {
			sphPosOne, sphPosNin, sphPosFiv, sphPosThr, sphPosEig, sphPosFou, sphPosTwo, sphPosFiv, sphPosNin, sphPosFiv, 
			sphPosNin, sphPosThr, sphPosOne, sphPosSev, sphPosEig, sphPosOne, sphPosFou, sphPosThr, sphPosTwo, sphPosOne,
			sphPosThr, sphPosSev, sphPosOne, sphPosEig, sphPosThr, sphPosOne, sphPosSev, sphPosSix, sphPosSev, sphPosTwo,
			sphPosFou, sphPosEig, sphPosSix, sphPosOne, sphPosNin, sphPosEig, sphPosTwo, sphPosSev, sphPosSix, sphPosTwo,
			sphPosSev, sphPosThr, sphPosFiv, sphPosSix, sphPosSev, sphPosThr, sphPosEig, sphPosThr, sphPosFou, sphPosEig,
			sphPosFou, sphPosSix, sphPosTwo, sphPosSev, sphPosOne, sphPosSev, sphPosNin, sphPosOne, sphPosFou, sphPosSev,
			sphPosFou, sphPosOne, sphPosSev, sphPosFiv, sphPosFou, sphPosEig, sphPosFou, sphPosNin, sphPosSix, sphPosFiv,
			sphPosFou, sphPosThr, sphPosTwo, sphPosFou, sphPosOne, sphPosTwo, sphPosThr, sphPosOne, sphPosEig, sphPosFiv,
			sphPosNin, sphPosOne, sphPosSix, sphPosThr, sphPosFou, sphPosSev, sphPosOne, sphPosEig, sphPosSix, sphPosOne,
			sphPosThr, sphPosOne, sphPosThr, sphPosNin, sphPosSev, sphPosSix, sphPosOne, sphPosFiv, sphPosSix, sphPosThr,
			sphPosOne, sphPosTwo, sphPosNin, sphPosTwo, sphPosEig, sphPosTwo, sphPosSix, sphPosFiv, sphPosNin, sphPosSix
		};*/
    }

    void Save()
	{
		//Create first row for CSV
		string[] rowDataTemp;
		rowDataTemp = new string[] {"Versuchsperson", "Versuchsbedingung", "Phase", "ZeigebewegungTotal", "ZeigebewegungPhase", "Zeit", "HandX", "HandY", "HandZ", "FedX", "FedY", "FedZ", "SphereX", "SphereY", "SphereZ"};
		rowData.Add (rowDataTemp);
	}

	private string getPath()
	{
		#if UNITY_EDITOR
		return Application.dataPath +"/CSV/"+expCond+"_"+Vp+".csv";
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
		if (infoOn == true && infoTime == 0.0f) 
		{
			infoTime = Time.time;
			GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = true;
			GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = true;
		} 
		else if (infoOn == true && Time.time - infoTime >= 7.0f) 
		{
			GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = false;
			GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = false;
			infoOn = false;
		} 
		if (infoGone == true && infoGoneTime == 0.0f) 
		{
			infoGoneTime = Time.time;
			GameObject.Find ("InfoNormal").GetComponent<Renderer> ().enabled = true;
			GameObject.Find ("InfoNormalBackground").GetComponent<Renderer> ().enabled = true;
		} 
		else if (infoGone == true && Time.time - infoGoneTime >= 7.0f) 
		{
			GameObject.Find ("InfoNormal").GetComponent<Renderer> ().enabled = false;
			GameObject.Find ("InfoNormalBackground").GetComponent<Renderer> ().enabled = false;
			infoGone = false;
		}
		if (pauseOn == true && pauseTime == 0.0f) 
		{
			pauseTime = Time.time;
			GameObject.Find ("Pause").GetComponent<Renderer> ().enabled = true;
			GameObject.Find ("PauseBackground").GetComponent<Renderer> ().enabled = true;
		} 
		else if (pauseOn == true && Time.time - pauseTime >= 7.0f) 
		{
			GameObject.Find ("Pause").GetComponent<Renderer> ().enabled = false;
			GameObject.Find ("PauseBackground").GetComponent<Renderer> ().enabled = false;
			pauseOn = false;
			pauseTime = 0.0f;
		} 
		if (wait == true && nextPointWait == 0.0f && infoOn == false && infoGone == false && pauseOn == false) 
		{
			nextPointWait = Time.time;
		} 
		else if (wait == true && (Time.time - nextPointWait) > 3.0f && infoOn == false && infoGone == false && pauseOn == false) 
		{
			expStart = true;
			nextPointWait = 0.0f;
			wait = false;
			GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
			if (pointNum <= 30) {
				GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1];
			} else if (pointNum > 30 && pointNum <= 30 + expCond) {
				GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1];
			} else if (pointNum > expCond + 30 && pointNum <= expCond + 60) {
				GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - expCond - 1 + 50];
				//GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - expCond];
			} 
			MusicSource.Play ();
		}

		if (pointNum <= 60 + expCond && expStart == true) 
		{
        	if(GameObject.Find("Sphere").GetComponent<Renderer>().enabled == false) 
			{
            	GameObject.Find("Sphere").GetComponent<Renderer>().enabled = true;
				GameObject.Find("Zielpunkt").GetComponent<Renderer>().enabled = true;
            }
            RaycastHit hit;
		Quaternion rotation = Quaternion.AngleAxis (-7.0f, transform.right);
		if (Physics.Raycast (trackedObj.transform.position, rotation * -transform.forward, out hit, 100)) 
			{
				Debug.DrawRay (trackedObj.transform.position, rotation * -transform.forward);
				if (hit.collider.tag == "target" && collisionTime == 0.0f) 
				{
					collisionTime = Time.time;
					allowCollision = true;
					GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = false;
					checkForReset = true;
					trialResetTime = 0.0f;
				} 
				else if (hit.collider.tag == "target" && (Time.time - collisionTime) >= 2.0f && trial == true && allowCollision == true) 
				{
					if (pointNum <= 30) 
					{
						GameObject.Find ("Feedback").transform.position = hit.point + new Vector3(0f,0f,0f);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "baseline";
						Debug.Log (Camera.main.transform.position);
						Debug.Log (phase + " " + pointNum);
						pointNumInPhase = pointNum;
						TimeOfFeedback = Time.time;
					} 
					else if (pointNum > 30 && pointNum <= 30 + expCond) 
					{
						GameObject.Find ("Feedback").transform.position = hit.point + displacement;
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "adaptation";
						Debug.Log (phase + " " + pointNum);
						pointNumInPhase = pointNum - 30;
						TimeOfFeedback = Time.time;
					} 
					else if (pointNum > expCond + 30 && pointNum <= expCond + 60) 
					{
						GameObject.Find ("Feedback").transform.position = hit.point + new Vector3(0f,0f,0f);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "readaptation";
						Debug.Log (phase + " " + pointNum);
						pointNumInPhase = pointNum - 30 - expCond;
						TimeOfFeedback = Time.time;
					} 

					//Add to CSV-Array

					Vector3 feedCoor = GameObject.Find ("Feedback").transform.position;
					Vector3 sphereCoor = GameObject.Find ("Sphere").transform.position;

					string[] rowDataTem;
					rowDataTem = new string[] 
					{
						Vp,
						expCond.ToString (),
						phase,
						pointNum.ToString (),
						pointNumInPhase.ToString (),
						TimeOfFeedback.ToString (),
						hit.point.x.ToString (),
						hit.point.y.ToString (),
						hit.point.z.ToString (),
						feedCoor.x.ToString (),
						feedCoor.y.ToString (),
						feedCoor.z.ToString (),
						sphereCoor.x.ToString (),
						sphereCoor.y.ToString (),
						sphereCoor.z.ToString ()
					};
					rowData.Add (rowDataTem);

					pointNum++;
					trial = false;
					checkForReset = false;
				} 
				else if (hit.collider.tag == "wall" && (Time.time - collisionTime) >= 2.0f && trial == true && checkForReset == true) 
				{
					if (trialResetTime == 0.0f) 
					{
						Debug.Log ("too fast notification");
						GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = true;
						GameObject.Find ("toofastBackground").GetComponent<Renderer> ().enabled = true;			
						trialResetTime = Time.time;
						allowCollision = false;
					} 
					else if (Time.time - trialResetTime >= 3.0f) 
					{
						Debug.Log ("too fast reset");
						checkForReset = false;
						GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = false;
						GameObject.Find ("toofastBackground").GetComponent<Renderer> ().enabled = false;
						collisionTime = 0.0f;
					}
				} 
				else if (hit.collider.tag == "wall" && trial == false) 
				{
					trial = true;
					allowCollision = false;
					collisionTime = 0.0f;
					Debug.Log ("trial finished - next trial initiated");

					if (expCond > 0 && pointNum == 31) 
					{
						infoOn = true;
						Debug.Log ("info");
					}
					if (expCond > 0 && pointNum == (31 + expCond)) 
					{
						infoGone = true;
						Debug.Log ("2ndInfo");
					}
					if (expCond == 0) {
						if (pointNum % 15 == 1) {
							pauseOn = true;
							Debug.Log ("pause");
						}
					} else if (expCond > 0) {
						if (pointNum <= (31 + expCond) && pointNum % 15 == 1 && pointNum != 31 && pointNum != (31 + expCond - 5)) 
						{
							pauseOn = true;
							Debug.Log ("pause");
						} 
						else if (pointNum > (31 + expCond) && (pointNum - 30 - expCond) % 15 == 1) {
							pauseOn = true;
							Debug.Log ("pause");
						}
					}
					/*if (pointNum%15 == 1) 
					{
						if (expCond > 0 && pointNum != 31 && pointNum != (31 + expCond - 5)) 
						{
							pauseOn = true;
							Debug.Log ("pause");
						}
						else if (expCond == 0)
						{
							pauseOn = true;
							Debug.Log ("pause");
						}
					}*/
					wait = true;
					expStart = false;
				}
			}
		} 
		else if (pointNum == 60 + expCond + 1 && expStart == true) 
		{
            GameObject.Find("Feedback").GetComponent<Renderer>().enabled = false;
            GameObject.Find("Sphere").GetComponent<Renderer>().enabled = false;
			GameObject.Find("Zielpunkt").GetComponent<Renderer>().enabled = false;
            Debug.Log ("Experiment finished");

			//write to CSV
			string[][] output = new string[rowData.Count][];

			for(int i = 0; i < output.Length; i++)
			{
				output[i] = rowData[i];
			}

			int length = output.GetLength(0);
			string delimiter = ",";

			StringBuilder sb = new StringBuilder();

			for (int index = 0; index < length; index++) 
				sb.AppendLine (string.Join (delimiter, output [index]));
			
			
			string filePath = getPath();

			StreamWriter outStream = System.IO.File.CreateText(filePath);
			outStream.WriteLine(sb);
			outStream.Close();
			GameObject.Find ("Ende").GetComponent<Renderer> ().enabled = true;
			pointNum++;
		}
	}
}