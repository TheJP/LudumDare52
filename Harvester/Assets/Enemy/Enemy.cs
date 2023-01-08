using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected bool AttackedByPlayer { get; private set; } = false;

    /// <summary>
    /// ResourceField this enemy should protect.
    /// Can be null if this enemy is part of a wave.
    /// </summary>
    public ResourceField Protectee { get; set; } = null;

    public void GotAttacked() => AttackedByPlayer = true;
}
