using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resident {
	public class CharacterRandomTime: CharacterCanMove {
		[SerializeField]
		private float _transitionDelay = 1;

		private float _delay;

		private void Update() {
			if(_delay > _transitionDelay) {
				RandomDirection();
				_delay = 0;
			} else {
				RandomMove(transform.position);
			}
			_delay += Time.deltaTime;
		}
	}
}