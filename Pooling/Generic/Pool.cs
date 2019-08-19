using System.Collections.Generic;

namespace Argh.Utilities.Pooling
{
    /// <summary>
    /// Interface of reusable object
    /// </summary>
    public interface IPoolable
    {
        void Activate(AutoPool pool);
        void Deactivate();
        void SendToPool();
    }

    /// <summary>
    /// Bare-bone interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T>
    {
        T GetPoolable();
        void ReturnObject(T obj);
    }

    /// <summary>
    /// Simple base pool class which holds the queue of objects for reuse
    /// </summary>
    /// <typeparam name="T">Type that will be pooled</typeparam>
    public class Pool<T> : IPool<T> where T : class
    {
        protected Queue<T> free = new Queue<T>();
        public int PoolCount { get => free.Count; }
        virtual public T GetPoolable()
        {
            if (free.Count > 0)
            {
                return free.Dequeue();
            }
            else
            {
                return null;
            }
        }

        virtual public void ReturnObject(T obj)
        {
            if (obj != null)
            {
                free.Enqueue(obj);
            }
        }
    }

    /// <summary>
    /// This class adds the better coordination with pooled objects
    /// It will call the activate and deactivate methods of IPoolable automaticaly
    /// </summary>
    public class AutoPool : Pool<IPoolable>
    {
        public T GetPoolable<T>() where T : IPoolable
        {
            return (T)GetPoolable();
        }

        override public IPoolable GetPoolable()
        {
            var poolable = base.GetPoolable();
            poolable?.Activate(this);
            return poolable;
        }
        override public void ReturnObject(IPoolable poolobj)
        {
            poolobj.Deactivate();
            base.ReturnObject(poolobj);
        }
    }
} 


