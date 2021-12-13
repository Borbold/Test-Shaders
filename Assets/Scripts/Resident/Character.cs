using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resident {
	public abstract class Character: MonoBehaviour {
		[SerializeField]
		private Type _type;
		[SerializeField]
		private Graph.GPUGraph _childGPUGraph;
		[SerializeField, Min(2)]
		private int _countCorners;
		public int GetCorners => _countCorners;
		[SerializeField]
		private int _searchRadius = 1;
		public int GetSearchRadius => _searchRadius;

		protected List<Vector3> _possibleAreas = new List<Vector3>();
		protected List<Vector3> _wallsPosition = new List<Vector3>();
		protected Vector3 _directionMovement = new Vector3();

		protected CharacterManager _characterManager;
		private float _rotation;
		public float GetRotation => _rotation;
		private Material _typeMaterial;
		public Material GetTypeMaterial => _typeMaterial;

		private void Awake() {
			_rotation = Random.Range(0, 360) * Mathf.Deg2Rad;

			_characterManager = FindObjectOfType<CharacterManager>();
			_typeMaterial = _characterManager.GetTypeMaterials[ResidentType.TypeToInt(_type)];

			FillPossibleAreas();

			_wallsPosition = _characterManager.GetWallsPosition;
			_directionMovement = _possibleAreas[Random.Range(0, _possibleAreas.Count)];
		}

		private void OnEnable() {
			_characterManager.AddCharacter(this, _type != Type.Hunter);
		}

		protected virtual void OnDrawGizmos() {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _searchRadius);

			Gizmos.color = Color.red;
			for(int i = 0; i < _possibleAreas.Count; i++) {
				Gizmos.DrawLine(transform.position, transform.position + _possibleAreas[i] * 3);
			}
		}

		public void ChangeCorners(int value) {
			_childGPUGraph.SmoothChange();
			if(_countCorners + value <= 10) {
				_countCorners += value;
			} else {
				_countCorners = 10;
			}

			_possibleAreas.Clear();
			FillPossibleAreas();
		}

		private void FillPossibleAreas() {
			for(int i = 1; i < _countCorners + 1; i++) {
				float angle = (360 * i / _countCorners + 90 / _countCorners) * Mathf.Deg2Rad - _rotation / _countCorners;
				_possibleAreas.Add(new Vector3(
					Mathf.Cos(angle),
					0,
					Mathf.Sin(angle))
				);
			}
		}
	}
}