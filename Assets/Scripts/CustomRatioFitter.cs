#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: be.linkedin.com/in/roybodereau
************************************************************************************************************/
#endregion

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0 - Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class CustomRatioFitter : AspectRatioFitter
{
    #region Variables
    //////////////////////////////////////////////////////////////////////////
    //VARIABLES
    //////////////////////////////////////////////////////////////////////////
    
    //////////////////////////////////////////////////////////////////////////
    #endregion

    #region Handlers
    //////////////////////////////////////////////////////////////////////////
    //HANDLERS
    //////////////////////////////////////////////////////////////////////////
    void Update ()
    {
        if (Screen.width > Screen.height)
        {
            if (!aspectMode.Equals(AspectMode.HeightControlsWidth))
            {
                aspectMode = AspectMode.HeightControlsWidth;
                GetComponent<RectTransform>().offsetMin = new Vector2(GetComponent<RectTransform>().offsetMin.x, 0.0f);
                GetComponent<RectTransform>().offsetMax = new Vector2(GetComponent<RectTransform>().offsetMax.x, 0.0f);
            }
        }
        else
        {
            if (!aspectMode.Equals(AspectMode.WidthControlsHeight))
            {
                aspectMode = AspectMode.WidthControlsHeight;
                GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, GetComponent<RectTransform>().offsetMin.y);
                GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, GetComponent<RectTransform>().offsetMax.y);
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////
    #endregion

    #region Methods
    //////////////////////////////////////////////////////////////////////////
    //METHODS
    //////////////////////////////////////////////////////////////////////////
    #endregion
}
//////////////////////////////////////////////////////////////////////////