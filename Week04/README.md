### Step 3
**Question:** The server receives axis = 50. Which line makes the paddle move at normal speed, and on which machine does that line run?
**Answer:** The line `axis = Mathf.Clamp(axis, -1f, 1f);` makes the paddle move at normal speed, and it runs on the **Server**.



### Step 4
**why 30 ms?**
At a 30 Hz tick rate, one tick takes about 33.3 ms. Setting the limit to 30 ms allows one valid input per tick from a real keyboard but blocks faster inputs from cheat programs.

**What happens with 5 ms?**
A 5 ms limit is too small; the server would accept multiple inputs within a single 33.3 ms tick, allowing players to easily speed hack by sending a flood of inputs.

**What happens with 500 ms?**
A 500 ms limit is too large; the server would ignore any keyboard input that happens faster than half a second. The game would feel terribly unresponsive and unplayable.



### Step 5

**(1) C sends 20 inputs and the server accepts one. Why is the paddle not 20 times faster?**
Because the 30 ms rate limit rejects the 19 extra inputs that arrive too fast within the same tick.

**(2) T moved the paddle on your screen. Why did the ball on the server not notice?**
Because Client Authoritative is off; the server controls the true position and immediately overwrites the client's local visual teleport.

**(3) Which of the three cheats would also fail without the rate limit, and why?**
X (speed hack) would fail because of the `Mathf.Clamp` applied in Step 3, and T (teleport) would fail because the server ignores local client position changes entirely.

![Server Console](console.png)