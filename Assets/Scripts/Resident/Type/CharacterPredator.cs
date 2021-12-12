using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resident {
	public class CharacterPredator: CharacterCanMove {
		[SerializeField]
		private float _huntSearchRadius = 3;

		private Character _victim;

		private void Start() {
			_characterManager.OnKillCharacter += KillCharacter;
		}

		private void Update() {
			if(_victim != null) {
				Manhunt(transform.position);
			} else {
				RandomMove(transform.position);
			}
		}

		private void Manhunt(Vector3 position) {
			transform.position = Vector3.MoveTowards(position, _victim.transform.position, _speed * Time.deltaTime);
		}

		private void FixedUpdate() {
			foreach(var searchChar in _characterManager.GetListCharacter) {
				if(this != searchChar) {
					if(Vector3.Distance(transform.position, searchChar.transform.position) <
						_huntSearchRadius + searchChar.GetSearchRadius) {
						_victim = searchChar;
						break;
					}
				}
			}
		}

		private void KillCharacter(Character character) {
			if(_victim == character) {
				_victim = null;
			}
		}

		protected override void OnDrawGizmos() {
			base.OnDrawGizmos();
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, _huntSearchRadius);
		}
	}
}