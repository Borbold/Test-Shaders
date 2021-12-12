using UnityEngine;

namespace Graph {
	public class GPUGraph: MonoBehaviour {
		[SerializeField]
		private Resident.Character _character;
		[SerializeField]
		private WrapGPUGraph _wrapGPUGraph;

		private GraphManager _graphManager;
		private Material _material;
		private ComputeShader _computeShader;
		private Mesh _mesh;
		private int _resolution = 10;
		private float _transitionDuration = 1f;
		private float _changedDuration;
		private ComputeBuffer _positionsBuffer;
		private Vector4 _offsetPosition = new Vector4();
		private void SetOffsetPosition(Vector4 pos) {
			_offsetPosition = pos;
		}

		private void Awake() {
			_wrapGPUGraph.ChangePosition += SetOffsetPosition;

			_graphManager = FindObjectOfType<GraphManager>();
			_computeShader = _graphManager.GetComputeShader;
			_mesh = _graphManager.GetMesh;
			_resolution = _graphManager.GetResolution;
			_transitionDuration = _graphManager.GetTransitionDuration;
		}

		private void Start() {
			_material = new Material(_character.GetTypeMaterial);
		}

		static readonly int
			positionsId = Shader.PropertyToID("_Positions"),
			countCornersId = Shader.PropertyToID("_CountCorners"),
			prevCountCornerId = Shader.PropertyToID("_PrevCountCorner"),
			offsetPositionId = Shader.PropertyToID("_OffsetPosition"),
			rotationId = Shader.PropertyToID("_Rotation"),
			resolutionId = Shader.PropertyToID("_Resolution"),
			stepId = Shader.PropertyToID("_Step"),
			transitionProgressId = Shader.PropertyToID("_TransitionProgress");

		bool _transitioning;
		public void SmoothChange() {
			_computeShader.SetInt(prevCountCornerId, _character.GetCorners);
			_transitioning = true;
		}

		void Update() {
			if(_transitioning) {
				if(_changedDuration >= _transitionDuration) {
					_changedDuration -= _transitionDuration;
					_transitioning = false;
				} else {
					_changedDuration += Time.deltaTime;
				}
			}

			UpdateFunctionOnGPU();
		}

		void UpdateFunctionOnGPU() {
			float step = 2f / _resolution;
			_computeShader.SetFloat(rotationId, _character.GetRotation);
			_computeShader.SetInt(resolutionId, _resolution);
			_computeShader.SetInt(countCornersId, _character.GetCorners);
			_computeShader.SetFloat(stepId, step);
			_computeShader.SetVector(offsetPositionId, _offsetPosition);

			int kernelIndex = _computeShader.FindKernel("HexagonKernel");
			if(_transitioning) {
				_computeShader.SetFloat(
					transitionProgressId,
					Mathf.SmoothStep(0f, 1f, _changedDuration / _transitionDuration)
				);
				kernelIndex = _computeShader.FindKernel("HexagonToHexagonKernel");
			}
			_computeShader.SetBuffer(kernelIndex, positionsId, _positionsBuffer);

			int groups = Mathf.CeilToInt(_resolution / 8f);
			_computeShader.Dispatch(kernelIndex, groups, groups, 1);

			_material.SetBuffer(positionsId, _positionsBuffer);
			_material.SetFloat(stepId, step * 4f);
			var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / _resolution));
			Graphics.DrawMeshInstancedProcedural(
				_mesh, 0, _material, bounds, _resolution * _resolution / 4
			);
		}

		void OnEnable() {
			_positionsBuffer = new ComputeBuffer(_resolution * _resolution, 12);
		}

		void OnDisable() {
			_positionsBuffer.Release();
			_positionsBuffer = null;
		}
	}
}