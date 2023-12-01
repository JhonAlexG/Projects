using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PointsController : MonoBehaviour
{
    public CollaiderController collaiderController1;
    public CollaiderController collaiderController2;
    public CollaiderController collaiderController3;
    public int totalPointsDefinitive = 0;
    public TextMeshProUGUI textPoints;
    public Animator animatorDoor;

    private void Update()
    {
        totalPointsDefinitive = collaiderController1.totalPoints + collaiderController2.totalPoints + collaiderController3.totalPoints;
        textPoints.text = totalPointsDefinitive.ToString() + " = 15";

        if (totalPointsDefinitive == 15)
        {
            animatorDoor.SetBool("isOpen", true);
        }
        else
        {
            animatorDoor.SetBool("isOpen", false);
        }
    }

}
