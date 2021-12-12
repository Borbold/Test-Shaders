using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resident {
	public class CharacterRandomClash: CharacterCanMove {
		private void Update() {
			RandomMove(transform.position);
		}
	}
}