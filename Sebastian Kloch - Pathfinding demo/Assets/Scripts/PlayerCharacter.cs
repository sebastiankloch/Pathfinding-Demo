using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class PlayerCharacter : Unit
	{
		private static readonly int speedHash = Animator.StringToHash("Speed");

		[SerializeField]
		private int moveRange;
		[SerializeField]
		private int attackRange;
		[SerializeField]
		private float moveSpeed;
		[SerializeField]
		private Animator animator;
		[SerializeField]
		private Grid grid;

		private Transform trans;
		private Coroutine running;

		private void Awake()
		{
			trans = transform;
		}

		public int GetMoveRange()
		{
			return moveRange;
		}

		public void SetMoveRange(int value)
		{
			moveRange = value;
		}

		public int GetAttackRange()
		{
			return attackRange;
		}

		public void SetAttackRange(int value)
		{
			attackRange = value;
		}

		public void TeleportTo(GridElement gridElement)
		{
			StopRunning();
			transform.position = gridElement.GetGlobaPos();
			SetGridPosition(gridElement.GetGridPosition());
		}

		public void RunTo(GridElement gridElement, List<GridPosition> path)
		{
			StopRunning();
			animator.SetFloat(speedHash, 6f);
			running = StartCoroutine(Running(gridElement, path));
		}

		private void StopRunning()
		{
			if (running != null)
				StopCoroutine(running);
			animator.SetFloat(speedHash, 0f);
		}

		IEnumerator Running(GridElement gridElement, List<GridPosition> path)
		{
			for (int id = 1; id < path.Count; id++)
			{
				GridElement nextElement = grid.GetElementAt(path[id]);
				Vector3 nextTarget = nextElement.GetGlobaPos();
				while (Vector3.Distance(trans.position, nextTarget)> 0.01f)
				{
					trans.LookAt(nextTarget);
					trans.position = Vector3.MoveTowards(trans.position, nextTarget, moveSpeed * Time.deltaTime);
					yield return null;
				}

				SetGridPosition(nextElement.GetGridPosition());
			}

			animator.SetFloat(speedHash, 0f);
			transform.position = gridElement.GetGlobaPos();
			SetGridPosition(gridElement.GetGridPosition());
			running = null;
		}

		public bool IsRunning()
		{
			return running != null;
		}
	}
}