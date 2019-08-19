using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argh.Utilities.Pooling.Unity
{
    /// <summary>
    /// Wraper around autopool that is attachable to game object
    /// </summary>
    public class MonoPoolable : MonoBehaviour, IPoolable
    {
        protected AutoPool myPool;

        virtual public void Activate(AutoPool pool)
        {
            myPool = pool;
            gameObject.SetActive(true);
        }

        virtual public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        virtual public void SendToPool()
        {
            if (myPool == null) Debug.Log("");
            myPool.ReturnObject(this);
        }
    }

    /// <summary>
    /// Pool that deals specificaly with Unity Objects
    /// Here we use the composition design to put the concept with UnityObjects
    /// </summary>
    public class GameObjectPool : IPool<MonoPoolable>
    {
        private AutoPool pool = new AutoPool();

        /// <summary>
        /// Prefab used to replenish pool if there is no free object
        /// The monobehaviour attached to the object has to have implement IPoolable
        /// </summary>
        public MonoPoolable prefab;

        /// <summary>
        /// Initial count of objects that will be spawned at start()
        /// </summary>
        public int initialCount;

        /// <summary>
        /// Max amount objects in the pool
        /// </summary>
        public int maxCount = 20;

        public GameObjectPool(MonoPoolable prefab, int initialCount)
        {
            this.prefab = prefab;
            this.prefab.Deactivate();
            pool.ReturnObject(this.prefab);

            this.initialCount = initialCount;
            for (int i = 1; i < initialCount; i++)
            {
                SpawnToPool();
            }
        }

        /// <summary>
        /// Creates new instance on the level and adds to autopool
        /// </summary>
        private void SpawnToPool()
        {

            var spawn = Object.Instantiate(prefab.gameObject);
            var poolable = spawn.GetComponent<MonoPoolable>();
            poolable.Deactivate();
            pool.ReturnObject(poolable);
        }

        /// <summary>
        /// Returns free poolable object
        /// </summary>
        /// <returns>null if maxCount was exceed</returns>
        public MonoPoolable GetPoolable()
        {
            //Spawn and return;
            if (pool.PoolCount == 0 && pool.PoolCount <= maxCount) SpawnToPool();
            return pool.GetPoolable<MonoPoolable>();
        }

        /// <summary>
        /// Push the object back to the pool
        /// </summary>
        public void ReturnObject(MonoPoolable obj)
        {
            pool.ReturnObject(obj);
        }
    }
}
