using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementsManager{

    public static void updateAcheivements(int touchdowns, int totalTouchdowns, int boosts)
    {
        if (touchdowns > 0)
        {
            submitAcheivement(GPGConstants.achievement_touchdown);
        }
        if (touchdowns > 2)
        {
            submitAcheivement(GPGConstants.achievement_3_in_a_row);
        }
        if (touchdowns > 5)
        {
            submitAcheivement(GPGConstants.achievement_reservation_for_6);
        }
        if (touchdowns > 9)
        {
            submitAcheivement(GPGConstants.achievement_top_ten);
        }
        if (touchdowns > 19)
        {
            submitAcheivement(GPGConstants.achievement_twenty);
        }

        if (totalTouchdowns > 9)
        {
            submitAcheivement(GPGConstants.achievement_novice);
        }
        if (totalTouchdowns > 49)
        {
            submitAcheivement(GPGConstants.achievement_apprentice);
        }
        if (totalTouchdowns > 99)
        {
            submitAcheivement(GPGConstants.achievement_enthusiast);
        }
        if (totalTouchdowns > 499)
        {
            submitAcheivement(GPGConstants.achievement_professional);
        }
        if (totalTouchdowns > 999)
        {
            submitAcheivement(GPGConstants.achievement_guru);
        }

        if (boosts > 9)
        {
            submitAcheivement(GPGConstants.achievement_boost);
        }
        if (boosts > 49)
        {
            submitAcheivement(GPGConstants.achievement_turbo);
        }
        if (boosts > 99)
        {
            submitAcheivement(GPGConstants.achievement_charged);
        }
        if (boosts > 499)
        {
            submitAcheivement(GPGConstants.achievement_energized);
        }
        if (boosts > 999)
        {
            submitAcheivement(GPGConstants.achievement_supersonic);
        }
    }

    public static void submitAcheivement(string achievement) {
        Social.ReportProgress(achievement, 100.0f, (bool success) => {
            // handle success or failure
        });
    }
}
