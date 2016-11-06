using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.Core
{
    public class Scene : GameObject
    {
        private string _name;

        public Scene(string name) : base()
        {
            _name = name;
        }

        public override void OnDestroy()
        {
            foreach (var child in children) child.OnDestroy();
        }

        public override void OnInitialize()
        {
            foreach (var child in children) child.OnInitialize();
        }

        public override void OnLoad()
        {
            foreach (var child in children) child.OnLoad();
        }

        public override void OnUpdate(double deltaTime)
        {
            foreach (var child in children) child.OnUpdate(deltaTime);
        }
    }
}
