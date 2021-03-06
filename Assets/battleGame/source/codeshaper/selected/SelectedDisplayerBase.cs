﻿using codeshaper.button;
using UnityEngine;

namespace codeshaper.selected {

    public abstract class SelectedDisplayerBase : MonoBehaviour {

        private Canvas canvas;

        protected virtual void Awake() {
            this.canvas = this.GetComponent<Canvas>();

            this.setUIVisible(false);
        }

        /// <summary>
        /// Sets if the UI is visible.
        /// </summary>
        public void setUIVisible(bool visible) {
            this.canvas.enabled = visible;
        }

        public abstract int getMask();

        public abstract void clearSelected();

        public abstract void callFunctionOn(ActionButton actionButton);
    }
}
