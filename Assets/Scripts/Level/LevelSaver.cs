using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelSaver : IInitializable, IDisposable, ISaver
    {
        private SceneDataCollector _sceneDataCollector;
        
        [Inject]
        private void InstallBindings(SceneDataCollector sceneDataCollector)
        {
            _sceneDataCollector = sceneDataCollector;
        }
        
        public void Save()
        {
            var data = _sceneDataCollector.CollectData();
            
            var json = JsonUtility.ToJson(data, true);
            
            if(!File.Exists(Const.SaveFolderPath))
                Directory.CreateDirectory(Const.SaveFolderPath);
            
            File.WriteAllText(Const.LoadLevelDataPath, json);
        }

        public void Dispose()
        {
            Application.quitting += OnQuitting;
        }

        public void Initialize()
        {
            Debug.Log($"Level Saver initialized");
            Application.quitting += OnQuitting;
        }

        private void OnQuitting()
        {
            Save();
        }
    }

    public interface ISaver
    {
        void Save();
    }
}