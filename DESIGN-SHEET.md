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

