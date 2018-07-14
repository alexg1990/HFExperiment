using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for CSV
using System.Text;
using System.IO;
using System;

public class Main : MonoBehaviour
{
	//CSV
	private List<string[]> rowData = new List<string[]> ();

	//AudioSource - Signal für Beginn der Zeigebewegung
	public AudioClip MusicClip;
	public AudioSource MusicSource;

	//AudioSource - Signal für Ende der Zeigebewegung
	public AudioClip MusicClipTwo;
	public AudioSource MusicSourceTwo;

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

	//Wenn Versuchsperson nicht korrekt auf den Signalton wartet, erscheint eine Fehlermeldung. Die Häufigkeit, mit welcher diese Fehlermeldung erscheint wird hier getracked.
	//Fehlermeldungen ab der Basiserhebung
	private int targetTooFastCounter = 0;
	private int tableTooFastCounter = 0;
	//Fehlermeldungen Insgesamt
	private int totalTargetTooFastCounter = 0;
	private int totalTableTooFastCounter = 0;

	//Manipulationsparameter - siehe ButtonInteraction.cs
	public bool rotations;
	public bool handFeed;
	public int vbMax;
	public bool onlyFeed;
	public float expTimer;
	public int hand;
	public int basis;
	public int readapt;

	//Nummer der aktuellen Zeigebewegung insgesamt (pointNum) und innerhalb der Basiserhebung, Adaptations- sowie Readaptationsphase (pointNumInPhase)
	private int pointNum;
	private int pointNumInPhase;

	//Timer für die Dauer der Pausen, eingeblendeten Informationen sowie Fehlermeldungen
	private float waitTimer = 0.0f;
	private bool tooFastInfo = false;
	private float tooFastTimer = 0.0f;
	private bool pauseInfo = false;
	private float pauseTimer = 0.0f;
	private bool handInfo = false;
	private float handTimer = 0.0f;
	private bool pertInfo = false;
	private float pertTimer = 0.0f;
	private bool noPertInfo = false;
	private float noPertTimer = 0.0f;
	private bool info = true;

	//siehe unten
	public bool bugCheck = false;
	private float bugCheckTimer = 0.0f;

	//Variablen zur temporären Zuweisung von Koordinaten
	private float temp;
	private float tempSize;
	private float tempSizeTwo;

	//Befindet sich die Hand der Versuchsperson auf dem Tisch oder zeigt sie auf das Zielobjekt
	public string contactPoint;

	//Versuchsabschnitt (Basiserhebung, Adaptationsphase, Readaptationsphase), Zeitpunkt zu dem Vp das Ziel erreicht, Zeitpunkt zu dem Vp die Bewegung intiierte
	private string phase;
	private float TimeOfFeedback;
	private float TimeOfStart;

	private bool onceOne = true;
	private bool onceTwo = true;




	//Stärke der Verschiebung/Rotation wird definiert
	//Anmerkung: Versuchstisch ist um 45° gedreht, weshalb Verschiebung sowohl in x- als auch z-Richtung erfolgen, um eine horizontale Verschiebung zu erreichen

	//20 Prismendioptrin
	//horizontale Verschiebung
	//private Vector3 displacement = new Vector3 (-0.0707f, 0f, 0.0707f);
	//Rotation
	private float prismRotate = 11.31f;

	//10 Pirsmendioptrin
	//horizontale Verschiebung
	private Vector3 displacement = new Vector3 (-0.0354f, 0f, -0.0354f);
	//Rotation
	//private float prismRotate = -5.711f;

	//Position der Zielscheibe wird definiert
	//Zielscheibe wird um "disSph" sowohl in x- als auch z-Richtung verschoben, um die gewünschte horizontale Verschiebung zu erreichen
	private float disSph = 0.02121312f;
	//Initiale Position der Zielscheibe
	private Vector3 sphPosOne = new Vector3 (-0.1380306f, 1.15f, -0.1035237f);
	//restlichen 8 Positionen
	private Vector3 sphPosTwo;
	private Vector3 sphPosThr;
	private Vector3 sphPosFou;
	private Vector3 sphPosFiv;
	private Vector3 sphPosSix;
	private Vector3 sphPosSev;
	private Vector3 sphPosEig;
	private Vector3 sphPosNin;
	Vector3[] sphPosCha;

	void Start ()
	{
		//Funktion zum Schreiben der CSV am Ende des Experiments
		Save ();
		//Initiale Nummer der Zeigebewegung
		pointNum = 1;
		//AudioSource für Signaltöne
		MusicSource.clip = MusicClip;
		MusicSourceTwo.clip = MusicClipTwo;
		//Sollte das Feedback ausversehen gerendered worden sein, wird dieses nun deaktiviert
		GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
		//Position der Zielscheibe wird auf initiale Position gesetzt
		GameObject.Find ("Sphere").transform.position = sphPosOne;
		//Ausgehend von der initialen Position der Zielscheibe, wird die Zielscheibe horizontal und vertikal um 3 cm verschoben. Hier werden die einzelnen Positionen definiert: 
		//sphOne = mittig, sphTwo = links, sphThr = links-oben, sphFou = oben, sphFiv = rechts-oben, sphSix = rechts, sphSev = rechts-unten, sphEig = unten, sphNin = links-unten
		sphPosTwo = new Vector3 (sphPosOne.x - disSph, sphPosOne.y, sphPosOne.z - disSph);
		sphPosThr = new Vector3 (sphPosOne.x - disSph, sphPosOne.y + disSph, sphPosOne.z - disSph);
		sphPosFou = new Vector3 (sphPosOne.x, sphPosOne.y + disSph, sphPosOne.z);
		sphPosFiv = new Vector3 (sphPosOne.x + disSph, sphPosOne.y + disSph, sphPosOne.z + disSph);
		sphPosSix = new Vector3 (sphPosOne.x + disSph, sphPosOne.y, sphPosOne.z + disSph);
		sphPosSev = new Vector3 (sphPosOne.x + disSph, sphPosOne.y - disSph, sphPosOne.z + disSph);
		sphPosEig = new Vector3 (sphPosOne.x, sphPosOne.y - disSph, sphPosOne.z);
		sphPosNin = new Vector3 (sphPosOne.x - disSph, sphPosOne.y - disSph, sphPosOne.z - disSph);

		//Reihenfolge, in welcher die einzelnen Positionen dargestellt werden
		sphPosCha = new Vector3[] {
			//With Hand Rendered
			sphPosOne, sphPosOne, sphPosFou, sphPosSev, sphPosFiv, sphPosTwo, sphPosOne, sphPosSev, sphPosNin, sphPosSev,

			//Basiserhebung
			sphPosOne, sphPosTwo, sphPosFiv, sphPosSev, sphPosNin, sphPosOne, sphPosTwo, sphPosNin, sphPosTwo, sphPosSix, 
			sphPosSev, sphPosTwo, sphPosNin, sphPosFiv, sphPosTwo, sphPosSev, sphPosSix, sphPosThr, sphPosTwo, sphPosFiv,
			sphPosTwo, sphPosEig, sphPosFiv, sphPosFou, sphPosEig, sphPosEig, sphPosFou, sphPosSev, sphPosNin, sphPosOne,

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
		//Erste Zeile für die CSV wird erstellt
		string[] rowDataTemp;
		rowDataTemp = new string[] {
			//Versuchspersonencode
			"Versuchsperson",
			//Kontrollbedingung (0), VB1 (5) oder VB2 (35)
			"Versuchsbedingung",
			//Basiserhebung, Adaptations- oder Readaptationsphase
			"Phase",
			//Nummer der Zeigebewegung im ganzen Versuch
			"ZeigebewegungTotal",
			//Nummer der Zeigebewegung innerhalb der einzelnen Phase
			"ZeigebewegungPhase",
			//FPS
			"ReaktionszeitHTCVive",
			//Zeitpunkt zu dem die Zeigebewegung intiiiert wurde
			"ZeitStart",
			//Zeitpunkt zu dem die Zielscheibe erreicht wurde
			"ZeitFeedback",
			//Koordinate der Hand zum Zeitpunkt der Kollision mit dem Zielobjekt
			"HandX",
			"HandY",
			"HandZ",
			//Koordinate der visuellen Rückmeldung, welche der Versuchsperson angezeigt wird
			"FedX",
			"FedY",
			"FedZ",
			//Position der Zielscheibe
			"SphereX",
			"SphereY",
			"SphereZ"
		};
		rowData.Add (rowDataTemp);
	}

	//Zum Schreiben der CSV
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
		//Updaterate innerhalb von Unity wird getracked
		fps = Time.deltaTime;
		if (wait == true) {
			//Da die Kollider kontinuierlich tracken, ob eine Kollision vorhanden ist und die Objekte fixiert wurden, kam es teilweise zum Bug, das wiederholt "Collision", "CollisionExit", "Collision"... für ca. 0.1Sekunden registriert wurden
			//Nach Kollision wird daher 0.2 Sekunden gewartet, bevor weitere Funktionen ausgeführt werden
			if (bugCheck == true) {
				if (bugCheckTimer == 0.0f) {
					bugCheckTimer = Time.time;
				} else if (Time.time - bugCheckTimer >= 0.2f) {
					bugCheck = false;
					bugCheckTimer = 0.0f;
				}
			}
			//Information wird eingeblendet, dass die visuelle Wahrnehmung verschoben wird (perturbationInformation), gleichzeitig wird die Zielscheibe ausgeblendet
			if (pertInfo == true) {
				if (pertTimer == 0.0f) {
					pertTimer = Time.time;
					expStart = false;
					GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
					//Nach 10 Sekunden wird diese Information wieder ausgeblendet
				} else if (Time.time - pertTimer >= 10.0f) {
					pertInfo = false;
					expStart = true;
					GameObject.Find ("InfoVerschoben").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;
					//Raum wird rotiert oder verschoben (siehe ButtonInteraction.cs)
					if (rotations == true && onlyFeed == false) {
						GameObject.Find ("Room").transform.Rotate (0, prismRotate, 0);
					} else if (rotations == false && onlyFeed == false) {
						GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position + displacement;
					}
				}
			}
			//Information wird eingeblendet, dass visuelle Wahrnehmung normalisiert wurde
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
					//Ausrichtung des Raumes wird normalisiert
					if (rotations == true && onlyFeed == false) {
						GameObject.Find ("Room").transform.Rotate (0, -prismRotate, 0);
					} else if (rotations == false && onlyFeed == false) {
						GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position - displacement;
					}
				}
			}
			//Information wird eingeblendet, dass die virtuelle Hand nun nicht mehr kontinuierlich repräsentiert wird
			if (handInfo == true) {
				if (handTimer == 0.0f) {
					handTimer = Time.time;
					expStart = false;
					GameObject.Find ("InfoHand").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
				} else if (Time.time - handTimer >= 10.0f) {
					handInfo = false;
					expStart = true;
					GameObject.Find ("InfoHand").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("InfoVerschobenBackground").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("LoPoly_Hand_Mesh_Right").GetComponent<Renderer> ().enabled = false;
				}
			}
			//Pause wird eingeblendet
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
			//Warnmeldung wird eingeblendet, dass Versuchsperson bitte auf das Signalton warten soll, bevor sie ihre Bewegung ausführt (Dauer:5 Sekunden)
			if (tooFastInfo == true) {
				if (tooFastTimer == 0.0f) {
					tooFastTimer = Time.time;
					expStart = false;
					GameObject.Find ("toofast").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("toofastBackground").GetComponent<Renderer> ().enabled = true;
					GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
					GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
					//Befindet sich die Hand auf Höhe der Zielscheibe, wird diese während der Warnmeldung ausgeblendet
					if (contactPoint == "target") {
						GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = false;
					}
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
					if (contactPoint == "target") {
						GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = true;
					}
				}
			}
			if (expStart == true) {
				//Wartezeit, bis Signalton ertönt
				if (waitTimer == 0.0f) {
					waitTimer = Time.time;
					//Wurde die in ButtonInteraction.cs definierte Wartezeit erfüllt, ertönt das Signal
				} else if (Time.time - waitTimer >= expTimer) {
					//befindet sich die Versuchsperson beim Ziel, ertönt Signalton der MusicSource
					if (contactPoint == "target") {			
						MusicSource.Play ();
						//befindet sich die Versuchsperson beim Ziel, ertönt Signalton der MusicSourceTwo
					} else if (contactPoint == "start") {
						MusicSourceTwo.Play ();
					}
					waitTimer = 0.0f;
					wait = false;
					//Befindet sich die Versuchsperson beim Ertönen des Signaltons beim Zielobjekt
					if (contactPoint == "target") {
						//Feedback-Hilfe (grüner Ring) wird ausgeblendet
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
						//Wenn noch kontinuierliches Feedback der Hand vorhanden ist
						if (handFeed = true) {
							//virtuelle Hand wird ausgeblendet und deren Position entspricht wieder der Hand der Versuchsperson
							GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = false;
							GameObject.Find ("handFeedback").transform.parent = GameObject.Find ("fingertipParent").transform;
							GameObject.Find ("handFeedback").transform.localRotation = Quaternion.Euler (0, 0, 0);
							GameObject.Find ("handFeedback").transform.localPosition = new Vector3 (0, 0, 0);
						}
						targetExit = true;
						startExit = false;
						trial = false;
						//Wird die Distanz des Zielobjekts erreicht, wird der Collider in Z-Richtung erhöht, da Versuchspersonen beim Aufrechthalten ihres Armes sich leicht bewegen und somit nicht konstant exakt mit dem Kollider in Kontakt sind.
						//Hier wird diese Distanz wieder normalisiert
						GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size = new Vector3 (GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.x, GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.y, GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.z - 0.03f);
						Debug.Log (GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.ToString ("F4"));
						//Befindet sich die Versuchsperson beim Ertönen des Signaltons beim Tisch
					} else if (contactPoint == "start") {
						//Gewöhnungsphase + Basiserhebung
						if (pointNum <= (hand + basis)) {
							//Zielposition wird gesetzt
							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1];
							//Adaptationsphase
						} else if (pointNum > (hand + basis) && pointNum <= (hand + basis + expCond)) {
							//Wurde der Raum rotiert, wird zur leichteren Positionierung der Sphere der Raum zurückrotiert, die Position der Sphere gesetzt und der Raum wieder in die Ursprungsposition gebracht
							if (rotations == true) {
								GameObject.Find ("Room").transform.Rotate (0, -prismRotate, 0);
							} else if (rotations == false) {
								GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position - displacement;
							}
							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - 1];
							if (rotations == true) {
								GameObject.Find ("Room").transform.Rotate (0, prismRotate, 0);
							} else if (rotations == false) {
								GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position + displacement;
							}
							//Readaptationsphase
						} else if (pointNum > (expCond + hand + basis) && pointNum <= (expCond + hand + basis + readapt)) {
							GameObject.Find ("Sphere").transform.position = sphPosCha [pointNum - expCond - 1 + vbMax];
							//Experiment ist beendet
						} else if (pointNum == (hand + basis + readapt + expCond + 1)) {
							GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = false;
							GameObject.Find ("Sphere").GetComponent<Renderer> ().enabled = false;
							GameObject.Find ("Zielpunkt").GetComponent<Renderer> ().enabled = false;
							GameObject.Find ("outRing").GetComponent<Renderer> ().enabled = false;
							Debug.Log ("Experiment finished");

							//Anzahl an Fehlern, da nicht auf Signalton gewartet wurde, wird in der Konsole ausgegeben
							Debug.Log ("Fehler beim Ziel Total: " + totalTargetTooFastCounter);
							Debug.Log ("Fehler beim Ziel ab Basiserhebung: " + targetTooFastCounter);
							Debug.Log ("Fehler beim Start Total: " + totalTableTooFastCounter);
							Debug.Log ("Fehler beim Start ab Basiserhebung: " + tableTooFastCounter);



							//CSV wird geschrieben
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
	
	//Wenn eine Kollision mit dem Start- oder Zielobjekt erfolgte
	void OnCollisionEnter (Collision col)
	{
		//Wenn das Experiment durch den Versuchsleiter gestartet wurde
		if (expStart == true) {
			//Entspricht der Kollider dem Zielobjekt
			if (col.gameObject.tag == "proxWall") {
				//Ist die Kollision Teil des Trials und nicht darauf zurückuführen, dass die Bewegung zu früh beendet wurde
				if (trial == true) {
					//Ist die aktuelle Zeigebewegung teil der Gewöhnungs- oder Basiserhebung
					if (pointNum <= (hand + basis)) {
						//Die ersten 10 Bewegungen (siehe ButtonInteraction.cs) werden ausgeführt, während die virtuelle Hand kontinierulich dargestellt wird. Anschließend wird die Hand nur noch nach Erreichen des Zielobjekts repräsentiert
						if (handFeed = true && pointNum >= (hand + 1)) {
							GameObject.Find ("handFeedback").transform.parent = GameObject.Find ("Room").transform;
							GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = true;
						}
						//Position der Feedback-Hilfe (grüner Ring) entspricht Kollisionspunkt
						GameObject.Find ("Feedback").transform.position = col.contacts [0].point;
						//Unity besitzt eine Updatefrequenz von maximal 90Hz bzw. 11ms, weshalb bei sehr schnellen Bewegungen physische Berechnungen leicht versetzt erfolgen können, wenn die Kollision zwischen den 11ms erfolgte
						//Um die Feedback-Hilfe (grüner Ring) und die virtuelle Hand dennoch immer auf derselben Distanz zur Versuchsperson zu repräsentieren, wird überprüft, ob die Kollision zu spät erfolgte und
						//die Differenz ggf. ausgeglichen.
						//Da der virtuelle Raum und der Versuchstisch gedreht sind, wird zur einfacheren Berechnung der frontalen Abweichung, der virtuelle Raum gerade ausgerichtet, die Korrektur vorgenommen und anschließend wieder
						//in die Ausgangsposition gebracht.
						GameObject.Find ("Room").transform.Rotate (0, 45, 0);
						temp = GameObject.Find ("Sphere").transform.position.z - GameObject.Find ("Feedback").transform.position.z - 0.0001f;
						GameObject.Find ("Feedback").transform.position = GameObject.Find ("Feedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("handFeedback").transform.position = GameObject.Find ("handFeedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("Room").transform.Rotate (0, -45, 0);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						//Für die CSV: In welcher Versuchsphase wir uns befinden, Welche Zeigebewegung erfolgte und die Zeit, zu welcher das Zielobjekt erreicht wurde
						phase = "baseline";
						pointNumInPhase = pointNum;
						TimeOfFeedback = Time.time;
						Debug.Log (phase + " " + pointNum);
						//Adaptationsphase
					} else if (pointNum > (hand + basis) && pointNum <= (hand + basis + expCond)) {
						//Im Experiment unterscheiden sich während der Adaptationsphase die virtuelle visuelle wahrgenommene Welt und die physische Welt. Während die Versuchsperson die Zielscheibe bspw. rechts wahrnimmt, befindet sie
						//sich weiterhin vor ihr. Um diesen Effekt zu simulieren, wird bei Kontakt mit dem Collider, der Raum in die Ursprungsposition gebracht, anschließend die Position des Feedbacks bestimmt und getracked
						//sowie daraufhin wieder rotiert, sodass die visuelle Wahrnehmung erneut verschoben ist

						//Wenn der gesamte Raum rotiert wurde
						if (rotations == true && onlyFeed == false) {
							GameObject.Find ("Room").transform.Rotate (0, -prismRotate, 0);
							//Wenn der gesamte Raum lateral verschoben wurde
						} else if (rotations == false && onlyFeed == false) {
							GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position - displacement;
							//Wenn nur die visuelle Rückmeldung rotiert wurde
						} else if (rotations == true && onlyFeed == true) {
							GameObject.Find ("Room").transform.Rotate (0, prismRotate, 0);
							//Wenn nur die visuelle Rückmeldung lateral verschoben wurde
						} else if (rotations == false && onlyFeed == true) {
							GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position + displacement;
						}
						//Wenn zusätzlich zur Feedback-Hilfe (grüner Kreis) eine virtuelle Hand repräsentiert wird
						if (handFeed = true) {
							GameObject.Find ("handFeedback").transform.parent = GameObject.Find ("Room").transform;
							GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = true;
						}
						//Feedback wird wie in Basiserhebung und Readaptationsphase positioniert
						GameObject.Find ("Feedback").transform.position = col.contacts [0].point;
						GameObject.Find ("Room").transform.Rotate (0, 45, 0);
						temp = GameObject.Find ("Sphere").transform.position.z - GameObject.Find ("Feedback").transform.position.z - 0.0001f;
						GameObject.Find ("Feedback").transform.position = GameObject.Find ("Feedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("handFeedback").transform.position = GameObject.Find ("handFeedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("Room").transform.Rotate (0, -45, 0);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						phase = "adaptation";
						pointNumInPhase = pointNum - (hand + basis);
						TimeOfFeedback = Time.time;
						Debug.Log (phase + " " + pointNum);
						//Readaptationsphase (siehe Basiserhebung)
					} else if (pointNum > expCond + (hand + basis) && pointNum <= expCond + (hand + basis + readapt)) {
						GameObject.Find ("Feedback").transform.position = col.contacts [0].point;
						GameObject.Find ("Room").transform.Rotate (0, 45, 0);
						//temp = GameObject.Find ("Sphere").transform.position.z - GameObject.Find ("Feedback").transform.position.z - 0.0017f;
						temp = GameObject.Find ("Sphere").transform.position.z - GameObject.Find ("Feedback").transform.position.z - 0.0001f;
						GameObject.Find ("Feedback").transform.position = GameObject.Find ("Feedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("handFeedback").transform.position = GameObject.Find ("handFeedback").transform.position + new Vector3 (0f, 0f, temp);
						GameObject.Find ("Room").transform.Rotate (0, -45, 0);
						GameObject.Find ("Feedback").GetComponent<Renderer> ().enabled = true;
						Debug.Log (phase + " " + pointNum);
						pointNumInPhase = pointNum - (hand + basis) - expCond;
						TimeOfFeedback = Time.time;
						phase = "readaptation";
						if (handFeed = true) {
							GameObject.Find ("handFeedback").transform.parent = GameObject.Find ("Room").transform.parent;
							GameObject.Find ("handFeedModel").GetComponent<SkinnedMeshRenderer> ().enabled = true;
						}
					}
					//Daten werden dem CSV-Array hinzugefügt
					//Zur leichteren Auswertung mit R wird der Raum um 45° gedreht, bevor die Koordinaten aufgezeichnet werden. Somit muss nurnoch der Abstand in X, anstatt des euklidischen Abstands betrachtet werden
					GameObject.Find ("Room").transform.Rotate (0, 45, 0);
					Vector3 feedCoor = GameObject.Find ("Feedback").transform.position;
					Vector3 sphereCoor = GameObject.Find ("Sphere").transform.position;
					GameObject.Find ("Room").transform.Rotate (0, -45, 0);
					//Kollisionen laufen nicht über die Update-Funktion von Unity sondern FixedUpdate. Diese wurde in den Physics-Settings auf ein Minimum gesetzt. Ist jedoch die UpdateFunktion von Unity langsamer als
					//FixedUpdate, entspricht die FPS zum Zeitpunkt der Kollision Update und nicht FixedUpdate.
					if (fps > Time.fixedDeltaTime) {
						timeNeeded = fps;
					} else if (fps <= Time.fixedDeltaTime) {
						timeNeeded = Time.fixedDeltaTime;
					}
					string[] rowDataTem;
					rowDataTem = new string[] {
						//Versuchspersonencode
						Vp,
						//Experimentalbedingung
						expCond.ToString (),
						//Phase, in welcher sich die Vp befindet
						phase,
						//Nummer der Zeigebewegung Total
						pointNum.ToString (),
						//Nummer der Zeigebewegung innerhalb der Phase
						pointNumInPhase.ToString (),
						//FPS
						timeNeeded.ToString (),
						//Zeitpunkt zu dem die Zeigebewegung initiiert wurde
						TimeOfStart.ToString (),
						//Zeitpunkt zu dem das Zielobjekt erreicht wurde
						TimeOfFeedback.ToString (),
						//Position der Hand zum Zeitpunkt der Kollision
						col.contacts [0].point.x.ToString (),
						col.contacts [0].point.y.ToString (),
						col.contacts [0].point.z.ToString (),
						//Position an welcher das Feedback repräsentiert wird
						feedCoor.x.ToString (),
						feedCoor.y.ToString (),
						feedCoor.z.ToString (),
						//Position der Zielscheibe
						sphereCoor.x.ToString (),
						sphereCoor.y.ToString (),
						sphereCoor.z.ToString ()
					};
					rowData.Add (rowDataTem);
					//Raum wird erneut visuell Verschoben
					if (pointNum > (hand + basis) && pointNum <= (hand + basis) + expCond) {
						if (rotations == true && onlyFeed == false) {
							GameObject.Find ("Room").transform.Rotate (0, prismRotate, 0);
						} else if (rotations == false && onlyFeed == false) {
							GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position + displacement;
						} else if (rotations == true && onlyFeed == true) {
							GameObject.Find ("Room").transform.Rotate (0, -prismRotate, 0);
						} else if (rotations == false && onlyFeed == true) {
							GameObject.Find ("Room").transform.position = GameObject.Find ("Room").transform.position - displacement;
						}
					}
					//Nächste Zeigebewegung
					pointNum++;
					info = true;
					bugCheck = true;
					wait = true;
					contactPoint = "target";
					trial = false;
					//Wird die Distanz des Zielobjekts erreicht, wird der Collider in Z-Richtung erhöht, da Versuchspersonen beim Aufrechthalten ihres Armes sich leicht bewegen und somit nicht konstant exakt mit dem Kollider in Kontakt sind.
					//Hier wird die Distanz erhöht.
					GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size = new Vector3 (GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.x, GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.y, GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.z + 0.03f);
					Debug.Log (GameObject.Find ("HitChecker").GetComponent<BoxCollider> ().size.ToString ("F4"));
				} else if (trial == false && startExit == true) {
					wait = true;
				}
				//Wurd mit der Startposition kollidiert
			} else if (col.gameObject.tag == "table") {
				if (trial == false && targetExit == true) {
					bugCheck = true;
					wait = true;
					contactPoint = "start";
					//Information wird eingeblendet, dass die kontinuierliche Repräsentation nun aufhört
					if (pointNum == (hand + 1)) {
						handInfo = true;
					}
					//Information wird eingeblendet, dass die visuelle Wahrnehmung verschoben wird
					if (expCond > 0 && pointNum == (hand + basis + 1) && onceOne == true) {
						pertInfo = true;
						onceOne = false;
					}
					//Information wird eingeblendet, dass die visuelle Wahrnehmung normalisiert wird
					if (expCond > 0 && pointNum == ((hand + basis + 1) + expCond) && onceTwo == true) {
						noPertInfo = true;
						onceTwo = false;
					}
					//Pause wird eingeblendet alle 15 Zeigebewegungen (nachdem die kontinuierliche Rückmeldung beendet wurde)
					//In der Kontrollbedingung (da diese sich nur aus Basiserhebung + Readaptationsphase zusammensetzt
					if (expCond == 0 && info == true) {
						if ((pointNum - hand) % 15 == 1 && pointNum != (hand + 1) && pointNum != (hand + basis + expCond + readapt + 1)) {
							pauseInfo = true;
							Debug.Log ("Pause");
						}
						//In den Experimentalbedingungen (da diese sich aus Basiserhebung, Adaptationsphase und Readaptationsphase zusammensetzt
					} else if (expCond > 0 && info == true) {
						if (pointNum <= ((hand + basis + 1) + expCond) && (pointNum - hand) % 15 == 1 && pointNum != (hand + basis + 1) && pointNum != 1 && pointNum != (hand + 1)) {
							pauseInfo = true;
							Debug.Log ("Pause");
						} else if (pointNum > ((hand + basis + 1) + expCond) && (pointNum - readapt) % 15 == 1 && pointNum != (hand + basis + expCond + readapt + 1)) {
							pauseInfo = true;
							Debug.Log ("Pause");
						}
					}
				}
			}
		}
	}
	
	//Wenn der Kontakt mit dem Kollider aufhört
	void OnCollisionExit (Collision col)
	{
		if (expStart == true) {
			//Wenn das KolliderObjekt auf Höhe der Zielscheibe war
			if (col.gameObject.tag == "proxWall") {
				//Wenn die Hand zurückgezogen wurde, bevor ein Signalton ertönte
				if (wait == true && bugCheck == false) {
					//Wartezeit wird resettet
					waitTimer = 0.0f;
					//Warnmeldung wird angezeigt
					tooFastInfo = true;
					//Sollte die Basiserhebung bereits angefangen haben, werden die Fehler getracked
					if (pointNum > 31) {
						targetTooFastCounter += 1;
						Debug.Log ("Fehler Nummer " + targetTooFastCounter + " beim Ziel");
					}
					//Fehler insgesamt werden getracked
					totalTargetTooFastCounter += 1;
				}
				//Wenn KolliderObjekt der Startposition entspricht
			} else if (col.gameObject.tag == "table") {
				//Wenn die Zeigebewegung initiiert wurde, bevor ein Signalton ertönte
				if (wait == true && bugCheck == false) {
					//Wartezeit wird resettet
					waitTimer = 0.0f;
					//Warnmeldung wird angezeigt
					tooFastInfo = true;
					//Sollte die Basiserhebung bereits angefangen haben, werden die Fehler getracked
					if (pointNum > 31) {
						tableTooFastCounter += 1;
						Debug.Log ("Fehler Nummer " + tableTooFastCounter + " beim Start");
					}
					//Fehler insgesamt werden getracked
					totalTableTooFastCounter += 1;
					//Ansonsten wird der Zeitpunkt zu dem die Zeigebewegung intiiert wird getracked
				} else {
					TimeOfStart = Time.time;
					startExit = true;
				}
			}
		}
	}
}
