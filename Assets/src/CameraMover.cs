﻿using src.buildings;
using src.troop;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace src {

    public class CameraMover : SidedObjectBase {

        public static CameraMover singleton;

        // Object references;
        public Text resourceText;
        public Text troopCountText;
        public BuildOutline buildOutline;

        public SelectedParty party;
        public ActionButtons actionButtons;

        // Options
        public float sensitivity = 0.4f;

        private int resources;

        public BuildingBase selectedBuilding;

        protected override void onAwake() {
            CameraMover.singleton = this;

            base.onAwake();
        }

        protected override void onStart() {
            base.onStart();

            this.party.setCameraMover(this);

            this.setResources(Constants.STARTING_RESOURCES);
        }

        protected override void onUpdate() {
            base.onUpdate();

            float forwardSpeed = Input.GetAxis("Horizontal") * this.sensitivity;
            float sideSpeed = Input.GetAxis("Vertical") * this.sensitivity;

            this.transform.transform.position += new Vector3(forwardSpeed, 0, sideSpeed);

            // Clicking.
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000)) {
                    SidedObjectEntity soe = hit.transform.gameObject.GetComponent<SidedObjectEntity>();
                    if (soe != null && soe.team == this.team) {

                        if(soe is UnitBase) {
                            bool flag = this.party.tryAdd((UnitBase)soe);
                        } else if(soe is BuildingBase) {
                            this.party.disband();
                            this.selectedBuilding = (BuildingBase)soe;
                        } else {
                            print("Error?  " + soe.gameObject.name);
                        }

                        //soe.onClick(this);

                        this.actionButtons.updateButtons();
                    } else if(hit.transform.name == "Ground") {
                        this.party.moveAllTo(hit.point);
                    }
                }
            }

            this.updateTroopCount();
        }

        /// <summary>
        /// Centers the camera on the passed position.
        /// </summary>
        public void centerOn(Vector3 pos) {
            Vector3 v = pos - (this.transform.forward * 10);
            this.transform.position = new Vector3(v.x, this.transform.position.y, v.z);
        }

        public void updateTroopCount() {
            int i = 0;
            foreach (SidedObjectBase o in this.team.members) {
                if (o is UnitBase) {
                    i++;
                }
            }

            int max = this.getMaxTroopCount();
            this.troopCountText.text = "Troop " + i + "/" + max;
            this.troopCountText.fontStyle = (i == max ? FontStyle.Bold : FontStyle.Normal);
        }

        public int getResources() {
            return this.resources;
        }

        public void setResources(int amount) {
            //TODO cache value.
            int maxResources = Constants.DEFAUT_RESOURCE_CAP;
            foreach (SidedObjectBase o in this.team.members) {
                if (o is BuildingWorkshop) {
                    maxResources += Constants.WORKSHOP_RESOURCE_BOOST;
                }
            }

            this.resources = Mathf.Clamp(this.resources + amount, 0, maxResources);
            this.resourceText.text = "Resources: " + this.resources;
            this.resourceText.fontStyle = (this.resources == maxResources ? FontStyle.Bold : FontStyle.Normal);
        }

        public int getMaxTroopCount() {
            int i = Constants.DEFAULT_TROOP_CAP;
            foreach(SidedObjectBase o in this.team.members) {
                if(o is BuildingFarm) {
                    i += Constants.FARM_TROOP_BOOST;
                }
            }
            return i;
        }
    }
}