using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScreen : BaseScreen
{

	public void OpenCharacter ()
	{
		CharacterScreen.SetLocation (CharacterScreen.Location.Town);

		//CharacterScreen.SetInTown (true);
		screenManager.Open ((int)Screens.CharacterScreen);
	}

	public void OpenDungeon ()
	{ 
		//TODO: SAVE 

		screenManager.Open ((int)Screens.DungeonScreen);
		GameObject.FindObjectOfType<GameManager> ().InitGame ();
		CameraController.instance.InDungeon ();

	}

	public void OpenShop ()
	{
		screenManager.Open ((int)Screens.ShopScreen);
	}

	public void OpenArena ()
	{
		screenManager.Open ((int)Screens.ArenaScreen);
	}

	public void OpenGuild ()
	{
		screenManager.Open ((int)Screens.GuildScreen);
	}

	public void Exit ()
	{
		//TODO: SAVE 
		screenManager.Open ((int)Screens.StartScreen);
	}

	public void OpenPCGDungeon ()
	{
		//TODO: SAVE 

		screenManager.Open ((int)Screens.PCGDungeonScreen);
		GameObject.FindObjectOfType<PCGGameManager> ().InitGame ();
		CameraController.instance.InDungeon ();
	}

}
