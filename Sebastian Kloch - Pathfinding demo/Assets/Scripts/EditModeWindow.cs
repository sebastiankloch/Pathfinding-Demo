using TMPro;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class EditModeWindow : MonoBehaviour
	{
		[SerializeField]
		private TMP_InputField inputFieldWidth;
		[SerializeField]
		private TMP_InputField inputFieldDepth;
		[SerializeField]
		private Grid grid;
		[SerializeField]
		private UnitManager unitManager;
		[SerializeField]
		private PlayModeWindow playModeWindow;
		[SerializeField]
		private GridEditor gridEditor;

		private void Start()
		{
			inputFieldWidth.text = grid.GetStartXSize().ToString();
			inputFieldDepth.text = grid.GetStartXSize().ToString();
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void ApplyGridSize()
		{
			Debug.Log("Apply Grid Size");
			grid.ChangeGridSize(int.Parse(inputFieldWidth.text), int.Parse(inputFieldDepth.text));
			unitManager.ClearUnitsOutsideOfBounds(grid.GetMapXSize(), grid.GetMapZSize());
		}

		public void SwitchToPlayMode()
		{
			GameplayManager.ChangeState(GameplayState.PlayMode);
			playModeWindow.Open();
			gameObject.SetActive(false);
		}

		public void SetEraseUnitBrush()
		{
			gridEditor.SetBrushToUnit(UnitType.None);
		}

		public void SetPlayerBrush()
		{
			gridEditor.SetBrushToUnit(UnitType.Player);
		}

		public void SetEnemyBrush()
		{
			gridEditor.SetBrushToUnit(UnitType.Enemy);
		}

		public void SetTraversableBrush()
		{
			gridEditor.SetBrushToGridElement(GridElementType.Travelsable);
		}

		public void SetObstacleBrush()
		{
			gridEditor.SetBrushToGridElement(GridElementType.Obstacle);
		}

		public void SetCoverBrush()
		{
			gridEditor.SetBrushToGridElement(GridElementType.Cover);
		}
	}
}