using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private const string ProgressKey = "Progress";
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();

        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            for (int index = 0; index < ProgressWriters.Count; index++)
            {
                ISavedProgress progressWriter = ProgressWriters[index];
                progressWriter.UpdateProgress(_progressService.Progress);
            }

            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress()
        {
            return PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                if (!ProgressWriters.Contains(progressWriter))
                    ProgressWriters.Add(progressWriter);
            }

            if (!ProgressReaders.Contains(progressReader))
                ProgressReaders.Add(progressReader);
        }

        public void InformProgressReaders()
        {
            for (int index = 0; index < ProgressReaders.Count; index++)
            {
                ISavedProgressReader progressReader = ProgressReaders[index];
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
    }
}