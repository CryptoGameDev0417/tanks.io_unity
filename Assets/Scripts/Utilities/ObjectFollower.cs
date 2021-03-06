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

namespace Tienkio.Utilities {
    public class ObjectFollower : MonoBehaviour {
        public Transform follow;
        new Transform camera;

        new Transform transform;
        Vector3 posOffset;
        Quaternion rotOffset;

        void Awake() {
            transform = base.transform;
            camera = UnityEngine.Camera.main.transform;
        }

        void Start() {
            posOffset = transform.position - follow.position;
            rotOffset = transform.rotation * Quaternion.Inverse(follow.rotation);
        }

        void FixedUpdate() {
            if (follow != null) transform.position = camera.rotation * posOffset + follow.position;
            transform.rotation = camera.rotation * rotOffset;
        }
    }
}
