using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KHC
{
    public abstract class State : MonoBehaviour
    {
        // ���� ������Ʈ
        public abstract State StateUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager);
    }
}
