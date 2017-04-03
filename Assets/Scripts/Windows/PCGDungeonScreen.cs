using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGDungeonScreen : BaseScreen
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
		CameraController.instance.OutDungeon ();
		CharacterScreen.SetLocation (CharacterScreen.Location.PCGDungeon);

		//CharacterScreen.SetInTown (false);
		screenManager.Open ((int)Screens.CharacterScreen);
	}
}
