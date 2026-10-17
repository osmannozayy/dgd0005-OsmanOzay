\### Checkpoint D



1\. \*\*Which component gives the ObjectId?\*\*

&#x20;  The `NetworkObject` component.



2\. \*\*Which machine chose LEFT, how did the client learn it?\*\*

&#x20;  The server chose LEFT in `OnStartServer` by picking the first free side, and the client learned it via synchronization of the `SyncVar<int> Side`.



3\. \*\*Which line stops other clients moving your paddle?\*\*

&#x20;  The ownership check in `Update()`: `if (!base.IsOwner) return;`.



\---



\### Predictions (Before Step 6) \& Grades



1\. \*\*You press W, nothing moves elsewhere. Who must tell whom, in what order?\*\*

&#x20;  The client sends its transform update to the server, then the server replicates it to the other clients.

&#x20;  \*\*Grade:\*\* Right



2\. \*\*Three balls. Which machine should own the one real ball? What must a client receive to draw it?\*\*

&#x20;  The server must own the one real ball; clients only need to receive position updates via `NetworkTransform` to render it.

&#x20;  \*\*Grade:\*\* Right



3\. \*\*If the server believes "my paddle is at z = 3.2" from a client, what is the first cheat?\*\*

&#x20;  Teleportation / speed hacking (the client sending arbitrary coordinate jumps without server-side validation).

&#x20;  \*\*Grade:\*\* Right



\---



\### Connection Card (Step 11)



\* \*\*Partner:\*\* Demir Özpak

\* \*\*Transport:\*\* Tugboat (UDP 7770)

\* \*\*Tick Rate:\*\* 30 Hz

\* \*\*Network:\*\* Mobile Hotspot



| Direction | Server IP | Client ID / Side | Partner Client ID / Side | RTT | Worked First Try? |

| :--- | :--- | :--- | :--- | :--- | :--- |

| \*\*I host, partner joins\*\* | 192.168.43.112 | 0 / LEFT | 1 / RIGHT | \~24 ms | Yes |

| \*\*Partner hosts, I join\*\* | 192.168.43.85 | 1 / RIGHT | 0 / LEFT | \~28 ms | Yes |



\*Both paddles moved smoothly on both screens without input cross-talk.\*

