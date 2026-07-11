using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreManagerTests
{
    private ScoreManager scoreManager;

    [SetUp]
    public void SetUp()
    {
        // Using a dummy GameObject because ScoreManager inherits from MonoBehaviour
        GameObject go = new GameObject();
        scoreManager = go.AddComponent<ScoreManager>();
        // We skip Awake since it relies on instance and DontDestroyOnLoad which might be tricky in unit tests,
        // but for SubmitScore/GetMaxScore we just need the logic to work with PlayerPrefs.
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(scoreManager.gameObject);
        PlayerPrefs.DeleteAll();
    }

    [Test]
    public void SubmitScore_HigherThanMax_UpdatesMaxScore()
    {
        int initialMax = 100;
        PlayerPrefs.SetInt("MaxScore", initialMax);
        PlayerPrefs.Save();

        scoreManager.SubmitScore(200);

        Assert.AreEqual(200, PlayerPrefs.GetInt("MaxScore"));
    }

    [Test]
    public void SubmitScore_LowerThanMax_DoesNotUpdateMaxScore()
    {
        int initialMax = 500;
        PlayerPrefs.SetInt("MaxScore", initialMax);
        PlayerPrefs.Save();

        scoreManager.SubmitScore(300);

        Assert.AreEqual(500, PlayerPrefs.GetInt("MaxScore"));
    }

}
