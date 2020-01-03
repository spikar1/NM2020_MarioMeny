using System;
using UnityEngine;

public interface IBumpable
{
    void Bumped(Player bumpee, Vector2 collisionVector);
}

