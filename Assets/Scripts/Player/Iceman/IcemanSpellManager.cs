using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Iceman
{
    public class IcemanSpellManager
    {
        private const string frostboltPrefabPath = "Spell Effects/Frostbolt Projectile";

        private const int frostboltPoolSize = 5;
        private static ObjectPool frostboltPool = new ObjectPool();

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            frostboltPool.InitializePool(frostboltPoolSize, Resources.Load<GameObject>(frostboltPrefabPath), "Frostbolt Pool");
        }

        public static void SpawnFrostbolt(Vector3 spawnPosition, Ray cameraToPointerRay)
        {
            var frostbolt = frostboltPool.GetNextObjectFromPool();
            var frostboltTransform = frostbolt.transform;

            var spineXZPlane = new Plane(Vector3.up, spawnPosition);
            if (spineXZPlane.Raycast(cameraToPointerRay, out float distanceToPlane))
            {
                var targetPoint = cameraToPointerRay.GetPoint(distanceToPlane);

                frostboltTransform.rotation = Quaternion.LookRotation(targetPoint - spawnPosition);
                frostboltTransform.position = spawnPosition + frostboltTransform.forward;

                frostbolt.SetActive(true);
            }
        }
    }
}
