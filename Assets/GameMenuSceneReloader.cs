using GameCore.GameUI;
using Level;
using UnityEngine;
using Utils.Constants;

public sealed class GameMenuSceneReloader : MonoBehaviour
{
	private RespawnPlayerBehaviour _playerRespawn;
	private GameMenuButton _restartButton;
	
	private void Awake()
	{
		_restartButton = GetComponent<GameMenuButton>();
		
		GameObject obj = GameObject.FindWithTag(Tags.RESPAWN);
		_playerRespawn = obj.GetComponent<RespawnPlayerBehaviour>();
		
		_restartButton.OnClick.AddListener(_playerRespawn.KillPlayer);
	}
}