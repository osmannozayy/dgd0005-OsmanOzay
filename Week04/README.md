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