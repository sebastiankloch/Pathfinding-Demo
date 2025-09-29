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

		private void Start()
		{
			inputFieldWidth.text = grid.GetStartXSize().ToString();
			inputFieldDepth.text = grid.GetStartXSize().ToString();
		}

		public void ApplyGridSize()
		{
			Debug.Log("Apply Grid Size");
			grid.ChangeGridSize(int.Parse(inputFieldWidth.text), int.Parse(inputFieldDepth.text));
		}
	}
}