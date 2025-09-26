using UnityEngine;

namespace SK.PathfindingDemo
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		private float moveSpeed;
		[SerializeField]
		private float sprintMultiplayer;
		[SerializeField]
		private Transform cameraPivot;
		[SerializeField]
		private Transform cameraTrans;

		public void Move(Vector2 input, bool sprint)
		{
			Debug.Log("Move: " + input);
			Vector3 forward = Vector3.ProjectOnPlane(cameraTrans.forward, Vector3.up).normalized;
			Vector3 right = Vector3.ProjectOnPlane(cameraTrans.right, Vector3.up).normalized;
			Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
			float moveSpeed;
			if (sprint)
				moveSpeed = this.moveSpeed * sprintMultiplayer;
			else
				moveSpeed = this.moveSpeed;
			cameraPivot.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
		}
	}
}