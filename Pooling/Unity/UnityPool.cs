using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argh.Utilities.Pooling.Unity
{
    /// <summary>
    /// Finally wrap to the attachable monobehavior
    /// </summary>
    public class UnityPool : MonoBehaviour
    {
        GameObjectPool pool;
        public MonoPoolable prefab;
        public int initialCount;
        private void Start()
        {
            if (!(prefab is MonoPoolable)) Debug.LogError("Prefab is not monopoolable");
            pool = new GameObjectPool(prefab, initialCount);
        }

        public MonoPoolable GetMonoPoolable()
        {
            return pool.GetPoolable();
        }
    }
}
