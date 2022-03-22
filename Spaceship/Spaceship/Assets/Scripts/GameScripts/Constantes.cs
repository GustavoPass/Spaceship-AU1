public sealed class Constantes {

    public const string masterVolumeString = "masterVolume";
    public const string musicVolumeString = "musicVolume";
    public const string effectsVolumeString = "effectsVolume";

    public const string screenWidthString = "screenWidth";
    public const string screenHeightString = "screenHeight";
    public const string screenResolutionIndex = "resolutionIndex";

    public const string scoreStringFormat = "SCORE: \n {0:000000000000000}";

    public const string sceneCanvasLevel = "CanvasLevel";
    public const string scenePosGame = "PosGame";
    public const string sceneLoad = "LoadScene";

    public const float XPvelocity = 5f;
    public const int minXP = 2;
    public const int maxXP = 4;

    public const float materialDamageTimeReset = 0.02f;

    public const int startLife = 100;
    public const int startVelocity = 30;

    public const float projecaoLevel = 1.15f;
    public const int xpLevel1 = 500;
    public const int levelMax = 30;

    public const int pointsLevelUp = 7;


    public static int XpProjection(int level) {
        if (level >= levelMax) return 0;
        return (int) (xpLevel1 * (System.Math.Pow(projecaoLevel, level - 1)));
    }

}
