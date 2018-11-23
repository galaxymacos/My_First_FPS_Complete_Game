using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessBreak : MonoBehaviour {

	[SerializeField] private BossInformation _bossInformation;

	public void BreakWeakness() {
		_bossInformation.BreakWeakness();
	}
}
