using SK.PathfindingDemo;
using UnityEditor;
using UnityEngine;

public class PlayModeWindow : MonoBehaviour
{
	[SerializeField]
	private EditModeWindow editModeWindow;

	public void Open()
    {
        gameObject.SetActive(true);
    }

	public void SwitchToEditMode()
	{
		GameplayManager.ChangeState(GameplayState.EditMode);
		editModeWindow.Open();
		gameObject.SetActive(false);
	}
}
