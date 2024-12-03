using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private int playerCount = 0;

    private List<GameObject> _players = new List<GameObject>();

    public List<GameObject> Players { get => _players; set => _players = value; }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;

        GameObject playerObject = playerInput.gameObject;

        // Vector3 initialPosition = new Vector3(-3.0f * playerCount - 1, 0.5f, 0.0f);
        // playerObject.transform.position = initialPosition;

        playerObject.name = $"Player ({playerCount})";
        playerObject.GetComponent<PlayerController>().PlayerIndex = playerCount - 1;
        _players.Add(playerObject);

        print($"{playerObject.name}が入室！");

        playerInput.SwitchCurrentControlScheme(playerInput.devices.ToArray());
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        GameObject playerObject = playerInput.gameObject;

        print($"{playerObject.name}が退室！");
    }
}
