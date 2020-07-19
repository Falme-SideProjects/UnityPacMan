public enum Direction
{
    up,
    left,
    right,
    down
}

public enum ElementType
{
    empty,
    wall,
    pacdot,
    power,
    door
}

public enum Items
{
    pacdot = 10,
    power = 50,
    cherry = 100,
    strawberry = 300,
    orange = 500,
    apple = 700,
    melon = 1000,
    galaxian = 2000,
    bell = 3000,
    key = 5000
}

public enum GhostState
{
    chase,
    scatter,
    frightened,
    eaten
}

public enum GhostType
{
    blinky,
    pinky,
    inky,
    clyde
}