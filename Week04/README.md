### Step 3
**Question:** The server receives axis = 50. Which line makes the paddle move at normal speed, and on which machine does that line run?
**Answer:** The line `axis = Mathf.Clamp(axis, -1f, 1f);` makes the paddle move at normal speed, and it runs on the **Server**.