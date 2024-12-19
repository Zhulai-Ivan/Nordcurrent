using System.Collections.Generic;
using Level.Data;
using UnityEngine;
using View;

namespace Level
{
    public class SceneDataCollector : MonoBehaviour
    {
        public LevelData CollectData()
        {
            var levelData = new LevelData();
            var views = FindObjectsOfType<TankView>();
            var viewsData = new List<ViewData>();
            
            
            foreach (var view in views)
            {
                var viewData = new ViewData()
                {
                    name = view.Name,
                    position = view.transform.position,
                    rotation = view.transform.rotation.eulerAngles,
                    scale = view.transform.localScale,
                    trackType = view.TrackType
                };
                
                viewsData.Add(viewData);
            }
            
            levelData.views = viewsData.ToArray();
            return levelData;
        }
    }
}