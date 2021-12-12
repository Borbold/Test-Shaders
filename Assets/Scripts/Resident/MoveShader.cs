using UnityEngine;

namespace Resident {
	public class MoveShader: MonoBehaviour {
		[SerializeField]
		private Graph.WrapGPUGraph _wrapGPUGraph;

		private void Update() {
			var newPosition = new Vector4(transform.position.x, 0, transform.position.z);
			_wrapGPUGraph?.ChangePosition(newPosition);
		}
	}
}