
using UnityEngine;
using UnityEngine.UI;

public class BluePrintButton : MonoBehaviour
{
    public Button BluePrint_Button;
    public bool BP_Pressed = false;
    public Button BP_close;
   // public Toggle 
    public GameObject BluePrint_Image;
    //public Image BluePrint_Image;


   /// public void ImageAlphaSetOne(bool alphaActive)
    //{
     //   var curColor = BluePrint_Image.color;
     //   float alpha = 1f;
     //   if (!alphaActive)
      //  {
     //       alpha = 0;
     //   }
    //    BluePrint_Image.color = new Color(curColor.r, curColor.g, curColor.b, alpha);
  //  }
    public void Onopen()
    {
       
        if (BP_Pressed = true)
        {
            BluePrint_Image.SetActive(true);
           
        }
    }

    public void Onclose()
    {
        if (BP_Pressed = false)
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
