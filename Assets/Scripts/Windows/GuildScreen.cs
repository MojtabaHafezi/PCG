using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildScreen : BaseScreen {

	public void BackToTown() {
		screenManager.Open ((int)Screens.TownScreen);
	}
}
