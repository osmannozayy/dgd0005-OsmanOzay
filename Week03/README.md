Week 03 Homework: Pong Connects and Replicates
Osman Özay - 2400004965



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

# \## Design sheet

# 

# | THING | STATE OR EVENT? | FISHNET PIECE | ON WHICH OBJECT? | WHO WRITES / CALLS? |

# | :--- | :--- | :--- | :--- | :--- |

# | Ball position | State | NetworkTransform | Ball | Server |

# | Ball in play? | State | SyncVar | Ball | Server |

# | Two scores | State | SyncVar | GameState | Server |

# | Whose serve | State | SyncVar | GameState | Server |

# | "Let me serve" | Event | ServerRpc | Paddle | Client |

# | "LEFT scored!" to all| Event | ObserversRpc | Paddle | Server |

# | "YOU SERVE" to one | Event | TargetRpc | Paddle | Server |

# | Event feed | State | SyncList | GameState | Server |

# 

# The `BallSpawner` script creates the one ball at the center coordinates `(0, Court.BallY, 0)` within the `OnStartServer` callback. The first line of the ball's Update is `if (!IsServerInitialized) return;`.


### Step 10

| TICK RATE | SERVER OUT B/S (RALLY) | BALL IN A CLIENT WINDOW | OTHER PADDLE |
| :--- | :--- | :--- | :--- |
| 10 | ~ 1200 B/s | Choppy and teleporting | Stuttering and delayed |
| 30 | ~ 3500 B/s | Normal | Normal |
| 60 | ~ 7000 B/s | Very smooth | Very smooth |

* What did 60 cost?
It cost significantly more bandwidth (higher Server Out B/s) to send updates more frequently.

* What did it buy?
It bought much smoother visual movement for the ball and the other player's paddle.

* Why did the ball step even for the player moving a paddle?
Because the ball's movement is completely controlled by the server (Server Authoritative), so the client must wait for the server's position updates to render the ball.
