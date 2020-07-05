using System.Collections;
using System.Collections.Generic;
using Morpeh.Globals;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public class PlayerNumberingTanks : PlayerNumbering
{
    [SerializeField] 
    private GlobalEvent DisconnectEvent;

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        DisconnectEvent.Publish();
    }
}
