using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : BaseScreen
{
	GameManager gameManager;

	public void ReturnTown ()
	{
		Destroy (GameObject.Find ("Board"));

		screenManager.Open ((int)Screens.TownScreen);

	}

	public void ToGameOver ()
	{
		Destroy (GameObject.Find ("Board"));
		screenManager.Open ((int)Screens.GameOverScreen);
	}

	public void OpenCharacter ()
	{
		CharacterScreen.SetInTown (false);
		screenManager.Open ((int)Screens.CharacterScreen);
	}


}
