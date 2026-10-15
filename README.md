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
