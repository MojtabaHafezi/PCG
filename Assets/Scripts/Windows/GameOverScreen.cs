using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreen : BaseScreen
{
	public Text playerGP;
	public Text playerAtt;
	public Text playerDef;

	public void MainMenuPressed ()
	{
		screenManager.Open ((int)Screens.StartScreen);
	}

	public void GameOverGUI ()
	{
		playerGP.text = "" + PlayerManager.instance.Gold;
		playerAtt.text = "" + (PlayerManager.instance.attackMod + PlayerManager.instance.Attack);
		playerDef.text = "" + (PlayerManager.instance.Defense + PlayerManager.instance.defenseMod);
	}

	public override void Open ()
	{
		base.Open ();
		GameOverGUI ();
	}
}
