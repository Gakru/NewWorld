using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    public class DeathState : State
    {
        public override State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}
