﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : BaseScreen
{

	public void ReturnTown ()
	{
		CameraController.instance.OutDungeon ();
		Destroy (GameObject.Find ("Board"));
		screenManager.Open ((int)Screens.TownScreen);

	}

	public void ToGameOver ()
	{
		CameraController.instance.OutDungeon ();
		Destroy (GameObject.Find ("Board"));
		screenManager.Open ((int)Screens.GameOverScreen);
	}

	public void OpenCharacter ()
	{
		CharacterScreen.SetLocation (CharacterScreen.Location.Dungeon);

		//CharacterScreen.SetInTown (false);
		screenManager.Open ((int)Screens.CharacterScreen);
	}


}
