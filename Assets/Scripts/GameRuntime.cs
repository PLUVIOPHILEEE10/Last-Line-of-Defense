using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class GameRuntime : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private float helpVisibleUntil;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Object.FindFirstObjectByType<FpsPlayerController>() == null) return;
        GameObject runtimeObject = new GameObject("GameRuntime");
        runtimeObject.AddComponent<GameRuntime>();
        ApplyVisualTheme();
    }

    private void Awake()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        helpVisibleUntil = Time.unscaledTime + 7f;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.escapeKey.wasPressedThisFrame) TogglePause();

        PlayerHealth health = Object.FindFirstObjectByType<PlayerHealth>();
        WaveManager waves = Object.FindFirstObjectByType<WaveManager>();
        bool gameEnded = (health != null && health.IsDead) || (waves != null && waves.IsComplete);
        if (gameEnded && keyboard.enterKey.wasPressedThisFrame) RestartScene();
    }

    private void TogglePause()
    {
        PlayerHealth health = Object.FindFirstObjectByType<PlayerHealth>();
        WaveManager waves = Object.FindFirstObjectByType<WaveManager>();
        if ((health != null && health.IsDead) || (waves != null && waves.IsComplete)) return;

        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        if (IsPaused) ReleaseCursor();
        else FpsPlayerController.LockCursor();
    }

    private static void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private static void ApplyVisualTheme()
    {
        SetObjectColor("Ground", new Color(0.07f, 0.11f, 0.16f));
        SetObjectColor("Wall_North", new Color(0.18f, 0.24f, 0.31f));
        SetObjectColor("Wall_South", new Color(0.18f, 0.24f, 0.31f));
        SetObjectColor("Wall_East", new Color(0.13f, 0.18f, 0.25f));
        SetObjectColor("Wall_West", new Color(0.13f, 0.18f, 0.25f));
        SetObjectColor("Weapon", new Color(0.08f, 0.09f, 0.11f));

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.12f, 0.18f, 0.25f);
        RenderSettings.fogStartDistance = 28f;
        RenderSettings.fogEndDistance = 70f;
    }

    private static void SetObjectColor(string objectName, Color color)
    {
        GameObject target = GameObject.Find(objectName);
        if (target == null) return;

        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = color;
    }

    private void OnGUI()
    {
        EnsureStyles();

        if (Time.unscaledTime < helpVisibleUntil && !IsPaused)
        {
            GUI.Box(new Rect(16f, Screen.height - 118f, 310f, 96f), string.Empty);
            GUI.Label(new Rect(30f, Screen.height - 108f, 290f, 30f), "LAST LINE OF DEFENSE", titleStyle);
            GUI.Label(new Rect(30f, Screen.height - 76f, 290f, 48f),
                "WASD Move  |  Shift Sprint  |  Space Jump\nLeft Click Fire  |  R Reload  |  Esc Pause", bodyStyle);
        }

        if (IsPaused)
        {
            GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty);
            GUI.Label(new Rect(0f, Screen.height * 0.39f, Screen.width, 60f), "PAUSED", titleStyle);
            GUI.Label(new Rect(0f, Screen.height * 0.49f, Screen.width, 40f),
                "Press ESC to continue", bodyStyle);
        }
    }

    private void EnsureStyles()
    {
        if (titleStyle != null) return;

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 28,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(1f, 0.7f, 0.25f) }
        };
        bodyStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16,
            normal = { textColor = Color.white }
        };
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
