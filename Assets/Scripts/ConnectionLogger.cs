using UnityEngine;
using FishNet;
using FishNet.Transporting;
using FishNet.Connection;

public class ConnectionLogger : MonoBehaviour
{
    private void Start()
    {
        if (InstanceFinder.ServerManager != null)
        {
            InstanceFinder.ServerManager.OnServerConnectionState += ServerConnectionState;
            InstanceFinder.ServerManager.OnRemoteConnectionState += RemoteConnectionState;
        }
        if (InstanceFinder.ClientManager != null)
        {
            InstanceFinder.ClientManager.OnClientConnectionState += ClientConnectionState;
        }
    }

    private void OnDestroy()
    {
        if (InstanceFinder.ServerManager != null)
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= ServerConnectionState;
            InstanceFinder.ServerManager.OnRemoteConnectionState -= RemoteConnectionState;
        }
        if (InstanceFinder.ClientManager != null)
        {
            InstanceFinder.ClientManager.OnClientConnectionState -= ClientConnectionState;
        }
    }

    private void ServerConnectionState(ServerConnectionStateArgs args)
    {
        Debug.Log("[Server] State: " + args.ConnectionState.ToString());
    }

    private void RemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        Debug.Log("[Server] Remote Client: " + conn.ClientId + " | Total Clients: " + InstanceFinder.ServerManager.Clients.Count);
    }

    private void ClientConnectionState(ClientConnectionStateArgs args)
    {
        Debug.Log("[Client] State: " + args.ConnectionState.ToString());
    }
}