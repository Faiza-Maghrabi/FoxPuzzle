using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneCompletion : MonoBehaviour
{
    public Slider completionSlider;
    public Slider easeCompletionSlider;
    private static TextMeshProUGUI completionText;
    private static float maxFood;
    private static float foodCollected;
    private static double completionPercent;
    private readonly float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        maxFood = FoodTracker.numFoodInScene(gameObject.scene.name);
        completionSlider.maxValue = maxFood; 
        completionSlider.minValue = 0; 
        easeCompletionSlider.maxValue = maxFood; 
        easeCompletionSlider.minValue = 0;
        foodCollected = FoodTracker.numFoodCollectedInScene(gameObject.scene.name);
        completionPercent = 0;
        completionText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        completionText.text = completionPercent.ToString() + "%";
    }

    // Update is called once per frame
    void Update(){

        // Check if health slider value is equal to player health and update value appropriately
        if(completionSlider.value != foodCollected){
            completionSlider.value = foodCollected;
        }  

        // Check if health slider value equals ease health slider value, and update it appropriate using lerp to show the damage taken
        if(completionSlider.value != easeCompletionSlider.value){
            easeCompletionSlider.value = Mathf.Lerp(easeCompletionSlider.value, foodCollected, lerpSpeed);
        }


    }

    public static void increaseFoodCount(){
        foodCollected++;
        completionPercent = Math.Truncate(foodCollected/maxFood * 100);
        completionText.text = completionPercent.ToString() + "%";
    }

    public static int getDangerLevel(){
        return (int)(completionPercent/30);
    }
}
