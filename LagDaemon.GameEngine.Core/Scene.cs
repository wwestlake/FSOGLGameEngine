using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public class Scene
    {
        private string _name;
        private List<GameObject> _sceneGraph = new List<GameObject>();

        public Scene(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Returns all of the objects from the scene graph of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Find<T>(Func<T, bool> predicate) where T : GameObject
        {
            foreach (var child in _sceneGraph)
            {
                if (typeof(T) == child.GetType() && predicate(child as T))
                {
                    yield return child as T;
                    yield return child.Find<T>(predicate) as T;
                }

            }
        }

        public IEnumerable<T> Find<T>() where T : GameObject
        {
            return Find<T>(x => true);
        }

        public IEnumerable<GameObject> All()
        {
            return _sceneGraph;
        }

        public void AddGameObject(GameObject obj)
        {
            _sceneGraph.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
            _sceneGraph.Remove(obj);
        }

    }
}
