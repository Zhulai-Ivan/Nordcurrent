using System;
using Modules;
using UnityEngine;

namespace Level.Data
{
    [Serializable]
    public struct ViewData
    {
        public string name;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public ModuleType trackType;
    }
}