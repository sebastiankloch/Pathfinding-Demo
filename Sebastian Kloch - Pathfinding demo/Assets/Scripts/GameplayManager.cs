using UnityEngine;

namespace SK.PathfindingDemo
{
	public class GameplayManager : MonoBehaviour
	{
		private static GameplayManager inst;
		private GameplayState state = GameplayState.EditMode;

		private void Awake()
		{
			if (inst)
			{
				Destroy(gameObject);
				return;
			}

			inst = this;
		}

		public static GameplayState GetState()
		{
			return inst.state;
		}
	}
}