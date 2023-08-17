using UnityEngine;

public static class HighscoreManager
{
    private static int _highscore = -1;

    public static void CompareAndSaveHighscore(int score)
    {
        if (_highscore == -1)
        {
            _highscore = PlayerPrefs.GetInt("Highscore", 0);
        }            

        if (score > _highscore)
        {
            _highscore = score;
            PlayerPrefs.SetInt("Highscore", score);
            Debug.Log("Highscore set: " + _highscore);
        }
    }   

    public static int GetHighscore()
    {
        return _highscore;
    }
}
