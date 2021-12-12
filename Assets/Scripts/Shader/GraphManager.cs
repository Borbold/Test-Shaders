using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph {
	public class GraphManager: MonoBehaviour {
		private const int _maxResolution = 128;

		[SerializeField]
		private ComputeShader _computeShader;
		public ComputeShader GetComputeShader => _computeShader;
		[SerializeField]
		private Mesh _mesh;
		public Mesh GetMesh => _mesh;
		[SerializeField, Range(10, _maxResolution)]
		private int _resolution = 10;
		public int GetResolution => _resolution;
		[SerializeField, Min(0f)]
		private float _transitionDuration = 1f;
		public float GetTransitionDuration => _transitionDuration;
	}
}