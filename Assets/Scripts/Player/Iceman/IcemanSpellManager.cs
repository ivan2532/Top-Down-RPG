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

        public static void SpawnFrostbolt(Vector3 spawnPosition, Vector3 targetPoint)
        {
            var frostbolt = frostboltPool.GetNextObjectFromPool();
            var frostboltTransform = frostbolt.transform;

            frostboltTransform.position = spawnPosition;
            frostboltTransform.rotation = Quaternion.LookRotation(targetPoint - spawnPosition);

            frostbolt.SetActive(true);
        }
    }
}
