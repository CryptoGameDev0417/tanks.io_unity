//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Tienkio.Utilities;

namespace Tienkio.Tanks {
    public class TankController : MonoBehaviour {
        public string nick;
        public Text nickLabel;
        [HideInInspector]
        public int kills;

        [Space]
        public Material bodyMaterial;

        public StatsHolder stats;
        public ScoreCounter scoreCounter;
        [HideInInspector]
        public TankHealth healthBar;
        [HideInInspector]
        public Gun[] guns;

        [Space]
        public int damageComputationCycles = 20;
        public float bodyDamageForBulletMultiplier = 1;

        [Space]
        public UnityEvent onGameOver;

        new Rigidbody rigidbody;

        void Awake() {
            healthBar = GetComponent<TankHealth>();
            rigidbody = GetComponent<Rigidbody>();
        }

        void Start() {
            nickLabel.text = nick;
        }

        void FixedUpdate() {
            if (healthBar.health <= 0) onGameOver.Invoke();
        }

        void OnTriggerEnter(Collider collider) {
            if (collider.CompareTag("Bullet")) {
                var bullet = collider.GetComponent<Bullet>();
                if (bullet.tank == this) return;

                Rigidbody bulletRigidbody = collider.attachedRigidbody;

                Vector3 bulletDirection = bulletRigidbody.velocity.normalized;
                rigidbody.AddForce(bulletDirection * bullet.knockback, ForceMode.Impulse);

                float bulletDamagePerCycle = bullet.damage / damageComputationCycles;
                float bodyDamagePerCycle = stats.bodyDamage.Value * bodyDamageForBulletMultiplier / damageComputationCycles;

                for (int cycle = 0; cycle < damageComputationCycles && healthBar.health > 0 && bullet.health > 0; cycle++) {
                    healthBar.health -= bulletDamagePerCycle;
                    bullet.health -= bodyDamagePerCycle;
                }

                if (healthBar.health <= 0 && bullet.tank.healthBar.health > 0) {
                    bullet.tank.kills++;
                    bullet.tank.scoreCounter.score += scoreCounter.score;
                }
            }
        }

        void OnCollisionEnter(Collision collision) {
            GameObject collider = collision.gameObject;
            if (collider.CompareTag("Tank")) {
                var tankHealthBar = collider.GetComponent<Health>();
                var tank = collider.GetComponent<TankController>();

                tankHealthBar.health -= stats.bodyDamage.Value;
                if (tankHealthBar.health <= 0) {
                    kills++;
                    scoreCounter.score += tank.scoreCounter.score;
                }
            }
        }
    }
}
