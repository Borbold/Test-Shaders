using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resident {
	public abstract class CharacterCanMove: Character {
		[SerializeField]
		protected float _speed = 10;

		protected bool WallCheck(Vector3 position) {
			if((position.z > _wallsPosition[0].z || position.z < _wallsPosition[1].z) ||
				(position.x < _wallsPosition[2].x || position.x > _wallsPosition[3].x)) {
				RandomDirection();
				return true;
			}

			return false;
		}

		protected void RandomDirection() {
			_directionMovement = _possibleAreas[Random.Range(0, _possibleAreas.Count)];
		}

		protected void RandomMove(Vector3 position) {
			Vector3 locPosition = position + _directionMovement;
			if(WallCheck(locPosition)) return;
			transform.position = Vector3.MoveTowards(position, locPosition, _speed * Time.deltaTime);
		}
	}
}