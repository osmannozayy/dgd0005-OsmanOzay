using FishNet;
using UnityEngine;

/// <summary>
/// Starts the standalone game as a FishNet client. Set the server address in the Inspector
/// before building; 127.0.0.1 is useful only when the server runs on the same computer.
/// </summary>
public sealed class NetworkClientBootstrap : MonoBehaviour
{
    [SerializeField] private string serverAddress = "127.0.0.1";
    [SerializeField] private ushort port = 7770;

    private void Start()
    {
#if !UNITY_EDITOR
        if (InstanceFinder.ClientManager == null)
        {
            Debug.LogError("Cannot start as a client: FishNet ClientManager was not found.");
            return;
        }

        string address = serverAddress;
        ushort connectionPort = port;
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--server-address" && i + 1 < args.Length)
                address = args[++i];
            else if (args[i] == "--server-port" && i + 1 < args.Length &&
                     ushort.TryParse(args[i + 1], out ushort parsedPort))
            {
                connectionPort = parsedPort;
                i++;
            }
        }

        Debug.Log($"Connecting to FishNet server at {address}:{connectionPort}.");
        if (!InstanceFinder.ClientManager.StartConnection(address, connectionPort))
            Debug.LogError($"FishNet could not start a client connection to {address}:{connectionPort}.");
#endif
    }
}
