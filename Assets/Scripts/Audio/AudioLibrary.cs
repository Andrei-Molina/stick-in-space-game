using UnityEngine;

[System.Serializable]
public class AudioClipData
{
    public string Id;
    public AudioClip Clip;
    public float Volume = 1f;
}

[CreateAssetMenu(menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public AudioClipData[] MusicClips;
    public AudioClipData[] SfxClips;

    public AudioClipData GetClip(string id, bool isMusic)
    {
        AudioClipData[] list = isMusic ? MusicClips : SfxClips;
        foreach (var data in list)
        {
            if (data.Id == id) return data;
        }
        Debug.LogWarning($"Audio ID not found: {id}");
        return null;
    }
}
