using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text textDisplayName = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook=nameof(HandleNameUpdated))] 
    [SerializeField] private string displayName = "Name";

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))]
    [SerializeField] private Color playerColor;

    #region ServerMethods
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetPlayerColor(Color newColor)
    {
        playerColor = newColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if(newDisplayName.Length < 3 || newDisplayName.Length > 15) return;

        RpcLogNewName(displayName);
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region  ClientMethods

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleNameUpdated(string oldName, string newName)
    {
        textDisplayName.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }

    [ClientRpc]
    private void RpcLogNewName(string newName)
    {
        Debug.Log(newName);
    }

    #endregion

    
}
