using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resident {
	public class CharacterManager : MonoBehaviour {
		[SerializeField]
		private List<Material> _typeMaterials;
		public List<Material> GetTypeMaterials => _typeMaterials;
		[SerializeField]
		private List<GameObject> _walls;
		[SerializeField]
		private List<Transform> _spawnPoints;
		[SerializeField]
		private List<Character> _spawnResidents;

		private List<Character> _listCharacter = new List<Character>();
		public List<Character> GetListCharacter => _listCharacter;
		private List<Character> _searchCharacter = new List<Character>();
		private List<Vector3> _wallsPosition = new List<Vector3>();
		public List<Vector3> GetWallsPosition => _wallsPosition;
		public UnityAction<Character> OnKillCharacter;

		private bool _breakCycle;

		private void Start() {
			foreach(var wall in _walls) {
				_wallsPosition.Add(wall.transform.position + wall.GetComponent<BoxCollider>().center);
			}
		}

		public void AddCharacter(Character character, bool canSearch) {
			_listCharacter.Add(character);
			if(canSearch) {
				_searchCharacter.Add(character);
			}
		}

		private void FixedUpdate() {
			foreach(var character in _listCharacter) {
				foreach(var searchChar in _searchCharacter) {
					if(character != searchChar) {
						if(Vector3.Distance(character.transform.position, searchChar.transform.position) <
							character.GetSearchRadius + searchChar.GetSearchRadius) {
							character.ChangeCorners(searchChar.GetCorners);
							// Убрал возможность есть других особей у пассивных резидентов
							if(character.GetCharacterType == Type.Passive) {
								RemoveCharacter(character);
							} else {
								RemoveCharacter(searchChar);
							}
							_breakCycle = true;
							break;
						}
					}
				}
				if(_breakCycle) { _breakCycle = false; break; }
			}
		}

		public void RemoveCharacter(Character character) {
			StartCoroutine(nameof(DestroyWithDelay), character);
			OnKillCharacter?.Invoke(character);
		}

		private IEnumerator DestroyWithDelay(Character character) {
			yield return new WaitForFixedUpdate();
			_listCharacter.Remove(character);
			_searchCharacter.Remove(character);
			character.gameObject.SetActive(false);
			yield return new WaitForSeconds(1f);
			Destroy(character.gameObject);
		}

		public void SpawnCharacter(int count, Dictionary<Type, int> chance) {
			List<bool> pointOff = new List<bool>(); bool hunter = false;
			for(int i = 0; i < _spawnPoints.Count; i++) {
				pointOff.Add(true);
			}
			
			int deadCount = 1000;
			while(count > 0 && deadCount > 0) {
				int rand = -1;
				for(int r = chance.Count - 1; r >= 0; r--) {
					float chanceR = Random.value * 100;
					if(chanceR <= chance[(Type)r]) {
						if((Type)r == Type.Hunter && hunter) continue;
						else hunter = true;
						rand = r;
						break;
					}
				}

				for(int i = 0; i < _spawnPoints.Count; i++) {
					if(rand >= 0 && pointOff[i]) {
						var newResident = Instantiate(_spawnResidents[rand],
							_spawnPoints[i].position, Quaternion.identity);
						pointOff[i] = false;
						count--;
						break;
					}
				}
				deadCount--;
			}
		}
	}
}