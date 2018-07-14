using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour 
{
	//Anzahl an Zeigebewegungen in denen die virtuelle Hand kontinuierlich repräsentiert wird
	private int hand = 10;
	//Anzahl an Zeigebewegungen Gewöhnungs- und Basiserhebung
	private int basis = 30;
	//Anzahl an Zeigebewegungen der Readaptationsphase
	private int readapt = 30;
	//Versuchsbedingung mit der maximalen Anzahl an Zeigebewegungegen während der Adaptationsphase
	private int max = 35;
	//Zeitliche Dauer bis Signalton ertönt, nachdem das Zielobjekt oder die Startposition erreicht wurde
	private float expTimer = 1.0f;
	//Virtueller Raum wird rotiert oder nur horizontal verschoben
	private bool rotateRoom = true;
	//Virtuelle Repräsentation der Hand der Versuchsperson
	private bool handAsFeedback = true;
	//Während der Adaptationsphase wird der gesamte Raum oder nur das Feedback verschoben
	private bool onlyFeed = false;

	//Kontrollbedingung, 0 Zeigebewegungen während der Adaptationsphase
	public void setButton0 () 
	{
		GameObject.Find("fingertip").GetComponent<Main>().expCond = 0;
		GameObject.Find("Button0").GetComponent<Image>().color = Color.green;
		GameObject.Find("Button5").GetComponent<Image>().color = Color.white;
		GameObject.Find("Button50").GetComponent<Image>().color = Color.white;
	}

	//Versuchsbedingung 1, 5 Zeigebewegungen während der Adaptationsphase
	public void setButton5 () 
	{
		GameObject.Find("fingertip").GetComponent<Main>().expCond = 5;
		GameObject.Find("Button5").GetComponent<Image>().color = Color.green;
		GameObject.Find("Button0").GetComponent<Image>().color = Color.white;
		GameObject.Find("Button50").GetComponent<Image>().color = Color.white;
	}

	//Versuchsbedingung 2, maximale Anzahl an Zeigebewegungen während der Adaptationsphase
	public void setButton100 () 
	{
		GameObject.Find("fingertip").GetComponent<Main>().expCond = max;
		GameObject.Find("Button50").GetComponent<Image>().color = Color.green;
		GameObject.Find("Button0").GetComponent<Image>().color = Color.white;
		GameObject.Find("Button5").GetComponent<Image>().color = Color.white;
	}

	public void disableCanvas () 
	{
		//Überprüft, ob eine Versuchsbedingung ausgewählt wurde
		if (GameObject.Find ("Button0").GetComponent<Image> ().color == Color.green ||
			GameObject.Find ("Button5").GetComponent<Image> ().color == Color.green ||
			GameObject.Find ("Button50").GetComponent<Image> ().color == Color.green) {
			//Manipulationsparameter werden definiert, siehe oben
			GameObject.Find("fingertip").GetComponent<Main>().hand = hand;
			GameObject.Find("fingertip").GetComponent<Main>().basis = basis;
			GameObject.Find("fingertip").GetComponent<Main>().readapt = readapt;
			GameObject.Find("fingertip").GetComponent<Main>().vbMax = max;
			GameObject.Find("fingertip").GetComponent<Main>().rotations = rotateRoom;
			GameObject.Find("fingertip").GetComponent<Main>().handFeed = handAsFeedback;
			GameObject.Find("fingertip").GetComponent<Main>().onlyFeed = onlyFeed;
			GameObject.Find("fingertip").GetComponent<Main>().expTimer = expTimer;
			GameObject.Find ("Canvas").SetActive (false);
			//Experiment wird gestartet
			GameObject.Find ("fingertip").GetComponent<Main> ().expStart = true;
			GameObject.Find ("Model").SetActive (false);
			GameObject.Find ("fingertip").GetComponent<Main> ().bugCheck = true;
			GameObject.Find ("fingertip").GetComponent<Main> ().wait = true;
			GameObject.Find ("fingertip").GetComponent<Main> ().contactPoint = "start";
		}
	}

	//Versuchspersonencode wird im Modal definiert
	public void vpnSet (string vpnCode) 
	{
		GameObject.Find ("fingertip").GetComponent<Main> ().Vp = vpnCode;
	}
}
