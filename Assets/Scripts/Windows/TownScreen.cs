using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScreen : BaseScreen
{

	public void OpenCharacter ()
	{
		CharacterScreen.SetInTown (true);
		screenManager.Open ((int)Screens.CharacterScreen);
	}

	public void OpenDungeon ()
	{ 
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

}
