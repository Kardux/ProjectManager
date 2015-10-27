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
public class CustomCanvasGroup : MonoBehaviour
{
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private float m_FadingSpeed;

    [SerializeField]
    private bool m_DisplayedOnStart;

    private int m_TargetAlpha = 0;

    private bool m_Fading;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	public virtual void Start()
	{
        GetComponent<CanvasGroup>().alpha = (m_DisplayedOnStart ? 1.0f : 0.0f);
        //GetComponent<CanvasGroup>().interactable = m_DisplayedOnStart;
        GetComponent<CanvasGroup>().blocksRaycasts = m_DisplayedOnStart;
	}
	
	public virtual void Update()
	{
	    if (m_Fading)
        {
            float _CurrentAlpha = GetComponent<CanvasGroup>().alpha;
            if (m_TargetAlpha == 0)
            {
                _CurrentAlpha -= m_FadingSpeed * Time.deltaTime;
                if (_CurrentAlpha <= 0.0f)
                {
                    _CurrentAlpha = 0.0f;
                    m_Fading = false;
                }
            }
            else
            {
                _CurrentAlpha += m_FadingSpeed * Time.deltaTime;
                if (_CurrentAlpha >= 1.0f)
                {
                    _CurrentAlpha = 1.0f;
                    m_Fading = false;
                    //GetComponent<CanvasGroup>().interactable = true;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

            GetComponent<CanvasGroup>().alpha = _CurrentAlpha;
        }
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void FadeIn()
    {
        m_TargetAlpha = 1;
        m_Fading = true;
    }

    public void FadeOut()
    {
        //GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        m_TargetAlpha = 0;
        m_Fading = true;
    }

    public bool IsFading()
    {
        return m_Fading;
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////