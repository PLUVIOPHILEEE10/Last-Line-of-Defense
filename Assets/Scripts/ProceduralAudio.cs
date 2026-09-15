using UnityEngine;

public static class ProceduralAudio
{
    private const int SampleRate = 22050;
    private static AudioClip shotClip;
    private static AudioClip hitClip;
    private static AudioClip reloadClip;
    private static AudioClip playerHitClip;
    private static AudioClip victoryClip;
    private static AudioClip defeatClip;

    public static void PlayShot()
    {
        if (shotClip == null)
        {
            shotClip = CreateClip("Shot", 0.10f, (time, progress) =>
                (Random.value * 2f - 1f) * (1f - progress) * 0.75f +
                Mathf.Sin(time * 620f) * (1f - progress) * 0.25f);
        }
        Play(shotClip, 0.42f);
    }

    public static void PlayHit()
    {
        if (hitClip == null)
        {
            hitClip = CreateClip("Hit", 0.08f, (time, progress) =>
                Mathf.Sin(time * 1100f) * (1f - progress));
        }
        Play(hitClip, 0.28f);
    }

    public static void PlayReload()
    {
        if (reloadClip == null)
        {
            reloadClip = CreateClip("Reload", 0.22f, (time, progress) =>
                Mathf.Sin(time * (260f + progress * 500f)) * (1f - progress) * 0.55f);
        }
        Play(reloadClip, 0.24f);
    }

    public static void PlayPlayerHit()
    {
        if (playerHitClip == null)
        {
            playerHitClip = CreateClip("PlayerHit", 0.18f, (time, progress) =>
                Mathf.Sin(time * 150f) * (1f - progress) * 0.8f);
        }
        Play(playerHitClip, 0.36f);
    }

    public static void PlayVictory()
    {
        if (victoryClip == null)
        {
            victoryClip = CreateClip("Victory", 0.75f, (time, progress) =>
            {
                float frequency = progress < 0.34f ? 523.25f : progress < 0.67f ? 659.25f : 783.99f;
                return Mathf.Sin(time * frequency * Mathf.PI * 2f) * (1f - progress) * 0.6f;
            });
        }
        Play(victoryClip, 0.42f);
    }

    public static void PlayDefeat()
    {
        if (defeatClip == null)
        {
            defeatClip = CreateClip("Defeat", 0.65f, (time, progress) =>
            {
                float frequency = Mathf.Lerp(240f, 85f, progress);
                return Mathf.Sin(time * frequency * Mathf.PI * 2f) * (1f - progress) * 0.65f;
            });
        }
        Play(defeatClip, 0.42f);
    }

    private static AudioClip CreateClip(string name, float duration, System.Func<float, float, float> sample)
    {
        int sampleCount = Mathf.CeilToInt(duration * SampleRate);
        float[] data = new float[sampleCount];
        for (int index = 0; index < sampleCount; index++)
        {
            float time = index / (float)SampleRate;
            float progress = index / (float)sampleCount;
            data[index] = Mathf.Clamp(sample(time, progress), -1f, 1f);
        }

        AudioClip clip = AudioClip.Create(name, sampleCount, 1, SampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    private static void Play(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject audioObject = new GameObject("OneShotAudio");
        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();
        Object.Destroy(audioObject, clip.length + 0.1f);
    }
}
