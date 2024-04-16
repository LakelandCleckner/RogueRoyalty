using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/* AchievementManager.cs
 * Nicolas Kaplan (301261925) 
 * 2024-04-10
 * 
 * Last Modified Date: 2024-04-16
 * Last Modified by: Nicolas Kaplan
 * 
 * 
 * Version History:
 *      -> April 10th, 2024
 *          - Created script using a class based approach
 *      -> April 16th, 2024
 *          - Achievements will no longer repeat in the same session 
 *          
 * This script is an achievement manager, it can be called from specific scripts to give the player an achievement.
 * V 1.1
 */
public class AchievementManager : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;

    [SerializeField] AudioClip achievementJingle;
    [SerializeField] AudioSource playerAudio;
    float displayTime = 4f;
    List<Achievement> achievementList = new List<Achievement>();
    Achievement firstEnemyAchievement = new Achievement(0, "That's gonna leave a mark!", "Defeated your first enemy.", 200, false);
    Achievement firstDeathAchievement = new Achievement(1, "This is Rogue Royalty...", "Died for the first time.", 0, false);
    Achievement secretAchievement = new Achievement(2, "You're not supposed to see this...", "Found a secret area", 500, false);
    Achievement unobtainableAchievement = new Achievement(3, "Something went seriously wrong.", "This achievement means our achievement system broke :)", 1000, false);
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        titleText.text = "a";
        descText.text = "...b";
        canvas.enabled = false; // starts disabled

        achievementList.Add(firstEnemyAchievement);
        achievementList.Add(firstDeathAchievement);
        achievementList.Add(secretAchievement);
        achievementList.Add(unobtainableAchievement);
    }
    IEnumerator AchievementDisplay(Achievement achievement)
    {
        
        titleText.text = achievement.achievementTitle;
        descText.text = achievement.achievementDescription;
        canvas.enabled = true;
        playerAudio.PlayOneShot(achievementJingle);
        Debug.Log($"Achievement Info: {achievement}, Title: {achievement.achievementTitle}, Description: {achievement.achievementDescription}");

        
        yield return new WaitForSeconds(displayTime);
        canvas.enabled = false;
        titleText.text = "";
        descText.text = "";
    }
    public void GiveAchievement(int achievementID)
    {
        
        Achievement chosenAchievement = achievementList[achievementID];
        if (chosenAchievement != null)
        {
            if (!chosenAchievement.obtainedBefore)
            {
                chosenAchievement.obtainedBefore = true;
                StartCoroutine(AchievementDisplay(chosenAchievement));

            }
        }
    }
}
public class Achievement
{
    public int achievementID;
    public string achievementTitle;
    public string achievementDescription;
    public float achievementScore; // how much score will the player recieve from this?
    public bool obtainedBefore; // has the achievement been obtained before?
    public Achievement(int achievementID, string achievementTitle, string achievementDescription, float achievementScore, bool obtainedBefore)
    {
        this.achievementID = achievementID;
        this.achievementTitle = achievementTitle;        
        this.achievementDescription = achievementDescription;
        this.achievementScore = achievementScore;
        this.obtainedBefore = obtainedBefore;
    }



}