using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHC
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2f; // 공격 후 재사용 대기 시간

        public float maximumAttackAngle = 35f; // 공격 가능한 최대 각도
        public float minimumAttackAngle = -35f; // 공격 가능한 최소 각도

        public float maximumDistanceNeededToAttack = 3f; // 공격을 시도할 수 있는 최대 거리
        public float minimumDistanceNeededToAttack = 0f; // 공격을 시도할 수 있는 최소 거리
    }
}
