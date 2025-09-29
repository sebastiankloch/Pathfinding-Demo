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
		private TMP_InputField inputFieldMoveRange;
		[SerializeField]
		private TMP_InputField inputFieldAttackRange;
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
			inputFieldMoveRange.text = unitManager.GetPlayer().GetMoveRange().ToString();
			inputFieldAttackRange.text = unitManager.GetPlayer().GetAttackRange().ToString();
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void ApplyGridSize()
		{
			Debug.Log("Apply Grid Size");
			if (!string.IsNullOrEmpty(inputFieldWidth.text) && !string.IsNullOrEmpty(inputFieldDepth.text))
			{
				grid.ChangeGridSize(int.Parse(inputFieldWidth.text), int.Parse(inputFieldDepth.text));
				unitManager.ClearUnitsOutsideOfBounds(grid.GetMapXSize(), grid.GetMapZSize());
				inputFieldWidth.text = grid.GetMapXSize().ToString();
				inputFieldDepth.text = grid.GetMapZSize().ToString();
			}
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

		public void SetPlayerMoveRange(string rangeAsText)
		{
			if (!string.IsNullOrEmpty(rangeAsText))
				unitManager.GetPlayer().SetMoveRange(int.Parse(rangeAsText));
		}

		public void SetPlayerAttackRange(string rangeAsText)
		{
			if (!string.IsNullOrEmpty(rangeAsText))
				unitManager.GetPlayer().SetAttackRange(int.Parse(rangeAsText));
		}
	}
}