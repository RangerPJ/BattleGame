﻿using codeshaper.data;
using UnityEngine;

namespace codeshaper.entity.unit.task.attack {

    public abstract class AttackBase {

        protected UnitBase unit;

        private float lastAttack;

        public AttackBase(UnitBase unit) {
            this.unit = unit;
        }

        public virtual SidedObjectEntity attack(SidedObjectEntity target) {
            if (Time.time >= (this.lastAttack + Constants.AI_MELEE_ATTACK_RATE) && this.inRangeToAttack(target)) {
                this.preformAttack(target);
                this.lastAttack = Time.time;
            }
            return target;
        }

        /// <summary>
        /// Checks if this unit is in range to preform an attack on the passed target.
        /// </summary>
        public abstract bool inRangeToAttack(SidedObjectEntity target);

        /// <summary>
        /// Attempts to preform an attack on the passed target.  The attack will fail if
        /// <see cref="inRangeToAttack(SidedObjectEntity)"/> returns false or the
        /// attack cooldown has not finished.
        /// </summary>
        protected abstract void preformAttack(SidedObjectEntity target);
    }
}