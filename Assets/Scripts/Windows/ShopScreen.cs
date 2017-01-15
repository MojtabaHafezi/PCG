using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : BaseScreen {

	public void BackToTown() {
		screenManager.Open ((int)Screens.TownScreen);
	}
}
