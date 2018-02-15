

public static class GameInstance
{
    //setting data
    public static float volum = 1;
    public static float brightness = 1;

    //game data
    public static float timer = 0;
    public static int currentScore = 0;
    public static int currentHP = 3;
    public static int level = 1;

    //save data
    public static string[] saveInfo = new string[3]{string.Empty,string.Empty,string.Empty};

    //format: name:score
    public static string[] scores = new string[3] { string.Empty, string.Empty, string.Empty };
}
