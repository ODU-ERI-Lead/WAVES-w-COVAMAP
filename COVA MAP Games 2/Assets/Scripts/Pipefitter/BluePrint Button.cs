
using UnityEngine;
using UnityEngine.UI;

public class BluePrintButton : MonoBehaviour
{
    public Button BluePrint_Button;
    public Toggle BP_Pressed; // chaned this to toggle from bool
    public Button BP_close;
   // public Toggle 
    public GameObject BluePrint_Image;
    //public Image BluePrint_Image;


   
    public void Onopen()
    {
       
        if (BP_Pressed)
        {
            BluePrint_Image.SetActive(true);
           
        }
    }

    public void Onclose()
    {
        if (!BP_Pressed)
        {
            Debug.Log("Button clicked, object is being deactivated");
            BluePrint_Image.SetActive(false);
           // might need one for close button as well 
        }

    }





  //  public Onopen()
  //  {
   //     if ()
     //   {
     //       BP_Pressed = true;
     //       this.BluePrint_Image = GameObject.active
     //   }
  //  }

}
