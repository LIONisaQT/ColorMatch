using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Shared/Sound Library", order = 1)]
public class SoundLibrary : ScriptableObject
{
    private static System.Random rng = new System.Random();

    [SerializeField] private List<SoundCollection> _soundCollection = new List<SoundCollection>();

    private Dictionary<string, List<AudioClip>> _collectionDictionary;

    public void InitializeDictionary()
    {
        _collectionDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (var collection in _soundCollection)
        {
            _collectionDictionary.Add(collection.name, collection.clips);
        }
    }

    public AudioClip GetClip(string name)
    {
        var hasClips = _collectionDictionary.TryGetValue(name, out var clips);
        
        if (!hasClips)
        {
            Debug.LogWarning($"There are no clips for {name}!");
            return null;
        }

        return clips[rng.Next(clips.Count)];
    }

    [Serializable]
    private class SoundCollection
    {
        [SerializeField] internal string name;
        [SerializeField] internal List<AudioClip> clips = new List<AudioClip>();
    }
}

