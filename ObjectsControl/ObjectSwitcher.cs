using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argh.Utilities.Control
{
    public class BehaviourWrapSwitchable : ISwitchable
    {
        public string Name => Behaviour.name;

        readonly public Behaviour Behaviour;

        public BehaviourWrapSwitchable(Behaviour behaviour)
        {
            this.Behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour));
        }

        public void SetActive(bool val)
        {
            Behaviour.enabled = val;
        }
    }

    public class BehaviourSwitcher : MonoBehaviour, IObjectSwitcher<Behaviour>
    {
        private ObjectSwitcher<BehaviourWrapSwitchable> internalSwitcher;
        public List<Behaviour> availableObjects => initialBehaviours;
        public List<Behaviour> initialBehaviours;

        public void Awake()
        {
            if (initialBehaviours != null)
            {
                internalSwitcher = new ObjectSwitcher<BehaviourWrapSwitchable>(initialBehaviours.Count);
                foreach (var item in initialBehaviours)
                {
                    internalSwitcher.AddAvailableObject(new BehaviourWrapSwitchable(item));
                }
            }
            else
            {
                internalSwitcher = new ObjectSwitcher<BehaviourWrapSwitchable>();
            }
        }

        public Behaviour ActivateObject(string name) =>
            internalSwitcher.ActivateObject(name)?.Behaviour;

        public void AddAvailableObject(Behaviour obj) =>
            internalSwitcher.AddAvailableObject(new BehaviourWrapSwitchable(obj));

        public void DeactivateAll() =>
            internalSwitcher.DeactivateAll();
    }

    /// <summary>
    /// Generic component that allows to manage activation / deactivation of the objects of choice
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectSwitcher<T> : IObjectSwitcher<T> where T : class, ISwitchable
    {
        public ObjectSwitcher(int initialSize = 0)
        {
            availableObjects = new List<T>(initialSize);
        }

        public ObjectSwitcher(params T[] initialObjects) : this(initialObjects.Length)
        {
            availableObjects.AddRange(initialObjects);
        }

        public List<T> availableObjects { get; private set; }

        public virtual void AddAvailableObject(T obj)
        {
            availableObjects.Add(obj);
        }

        public T ActivateObject(string name)
        {
            var obj = availableObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                obj.SetActive(true);
            }
            return obj;
        }

        public void DeactivateAll() =>
            availableObjects.ForEach(x => x.SetActive(false));
    }

    public interface ISwitchable
    {
        void SetActive(bool val);

        string Name { get; }
    }
}