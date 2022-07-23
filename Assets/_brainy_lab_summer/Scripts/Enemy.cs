using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController
{
    public void TakeDamage()
    {
        Kill();
    }
}
