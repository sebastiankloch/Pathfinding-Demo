using TMPro;
using UnityEngine;

namespace SK.PathfindingDemo
{
	public class GridElement : MonoBehaviour
	{
		[SerializeField]
		private GridElementType type;
		[SerializeField]
		private GridElementData data;
	
		[SerializeField]
		private TMP_Text text;

		private GridPosition gridPosition;

		public void SetGridPosition(GridPosition value)
		{
			gridPosition = value;
		}

		public GridPosition GetGridPosition()
		{
			return gridPosition;
		}

		public Vector3 GetGlobaPos()
		{
			return transform.position;
		}

		public GridElementType GetElementType()
		{
			return type;
		}

		public void HighlightMove()
		{
			text.color = data.moveColor;
		}

		public void HighlightMoveOutOfRange()
		{
			text.color = data.moveOutOfRangeColor;
		}

		public void HighlightAttack()
		{
			text.color = data.attackColor;
		}

		public void HighlightAttackOutOfRange()
		{
			text.color = data.attackOutOfRangeColor;
		}

		public void ClearHighlight()
		{
			text.color = data.normalColor;
		}
	}
}