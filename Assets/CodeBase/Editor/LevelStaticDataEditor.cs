using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Audio;
using CodeBase.Services.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";
        private const string InitialCameraPointTag = "CameraInitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData) target;

            if (GUILayout.Button("Collect"))
            {
                levelData.PuzzleSpawners = FindObjectsOfType<PuzzleMarker>()
                    .Select(x =>
                        new PuzzleSpawnerData(x.GetComponent<UniqueId>().Id, x.puzzelName, x.transform.position))
                    .ToList();

                levelData.LevelKey = SceneManager.GetActiveScene().name;
                SoundManagerMarker soundManagerMarker = FindObjectOfType<SoundManagerMarker>();
                levelData.SoundManagerData = new SoundManagerData(soundManagerMarker.sounds,soundManagerMarker.clips,
                    soundManagerMarker.soundManagerType);
                
                levelData.InitialHeroPosition = GameObject.FindGameObjectWithTag(InitialPointTag).transform.position;
                levelData.InitialCameraPosition =
                    GameObject.FindGameObjectWithTag(InitialCameraPointTag).transform.position;
            }

            EditorUtility.SetDirty(target);
        }
    }
}