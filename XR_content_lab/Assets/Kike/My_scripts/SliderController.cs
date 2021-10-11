using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{ 
    float newValue;
    public Slider slider;

    void start(){

        //GameObject.Find ("Slider").GetComponent <Slider> ().value;
        ///print(newValue);
    }
    /*
    public float slidernumber = SliderGet. ;


    public void changeHertz(float slidernumber)
    {
        FlickerSprite.cycleHz = slidernumber;

    }
    */
    void update(){
        print("X");
    }


    public void OnValueChanged(float newValue)
    {
        //FlickerSprite.SetHz(newValue);
        
        print(slider.value);
        FlickerSprite.cycleHz = newValue;
        print(FlickerSprite.cycleHz);
        print(newValue);
    }

}
