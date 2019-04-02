using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Room {

	public int xSize, ySize, xPos, yPos;

	public Room(int xS, int yS, int xP, int yP) {
		xSize = xS;
		ySize = yS;
		xPos = xP;
		yPos = yP;
	}

}
