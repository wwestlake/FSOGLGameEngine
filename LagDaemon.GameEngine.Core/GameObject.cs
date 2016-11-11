using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    [Serializable]
    public abstract class GameObject
    {
        protected Guid id;
        protected List<GameObject> children = new List<GameObject>();
        protected Transform transform = Transform.Default();
        protected GameObject parent;

        public GameObject()
        {
            id = Guid.NewGuid();
        }

        public abstract void OnLoad();
        public abstract void OnDestroy();
        public abstract void OnInitialize();
        public abstract void OnUpdate(double deltaTime);

        public IEnumerable<T> Find<T>(Func<T,bool> predicate) where T: GameObject
        {
            if (parent != null) yield return parent.Find<T>(predicate) as T;
            else foreach (var child in children)
            {
                if (typeof(T) == child.GetType() && predicate(child as T))
                {
                    yield return child as T;
                    yield return child.Find<T>(predicate) as T;
                }
            }
        }

        public IEnumerable<T> Find<T>() where T: GameObject
        {
            return Find<T>(x => true);
        }

        public IEnumerable<GameObject> All()
        {
            return children;
        }

        public void AddGameObject<T>(T obj) where T: GameObject
        {
            var check = Find<T>(x => x.id == obj.id);
            if (check == null)
            {
                obj.parent = this;
                children.Add(obj);
                obj.OnInitialize();
            } else
            {
                //log the error condition as a warning
            }
        }

        public void RemoveObject(GameObject obj)
        {
            if (children.Contains(obj))
                children.Remove(obj);
        }

        public Transform Transform
        {
            get
            {
                return transform;
            }
        }
    }
}
