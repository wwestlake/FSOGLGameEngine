using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public abstract class GameObject
    {
        protected Guid id;
        protected List<GameObject> children = new List<GameObject>();
        protected Transform transform = Transform.Default();

        public GameObject()
        {

        }

        public abstract void OnLoad();
        public abstract void OnDestroy();
        public abstract void OnInitialize();
        public abstract void OnUpdate(double deltaTime);

        public IEnumerable<T> Find<T>(Func<T,bool> predicate) where T: GameObject
        {
            foreach (var child in children)
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

        public void AddGameObject(GameObject obj)
        {
            children.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
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
