﻿using codeshaper.data;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskAttackNearby : TaskBase<UnitBase> {

        protected SidedObjectEntity target;

        public TaskAttackNearby(UnitBase unit) : base(unit) {
            this.target = this.findTarget();
        }

        public override bool preform() {
            this.preformAttack();

            return true;
        }

        public override void drawDebug() {
            if (Util.isAlive(this.target)) {
                Color c = this.unit.attack.inRangeToAttack(this.target) ? Color.green : Color.red;
                GLDebug.DrawLine(this.unit.getPos(), this.target.getPos(), c);
            }
        }

        protected virtual void preformAttack() {
            if (Util.isAlive(this.target) && Vector3.Distance(this.unit.getPos(), this.target.getPos()) <= Constants.AI_FIGHTING_FIND_RANGE) {
                // There is a target and it is close enough to "see".
                this.func();
            } else {
                // Target is dead or out of range, find a new one.
                this.target = this.findTarget();
            }
        }

        /// <summary>
        /// Finds a new target and returns it, or null is returned if no target can be found.
        /// </summary>
        /// <returns></returns>
        protected virtual SidedObjectEntity findTarget() {
            return this.findEntityOfType<SidedObjectEntity>(Constants.AI_FIGHTING_FIND_RANGE);
        }

        /// <summary>
        /// Attacks the target if they are in range, or continues to move if they are not in range.
        /// </summary>
        protected void func() {
            if (this.unit.attack.inRangeToAttack(this.target)) {
                this.target = this.unit.attack.attack(this.target);
                this.moveHelper.stop();
            }
            else {
                this.moveHelper.setDestination(this.target);
            }
        }
    }
}
