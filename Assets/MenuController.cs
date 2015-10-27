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

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class MenuController : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////
    //STATIC
    //////////////////////////////////////////////////////////////////////////
    public static MenuController INSTANCE;
    //////////////////////////////////////////////////////////////////////////

	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private CustomCanvasGroup m_LoadingCanvas;

    [SerializeField]
    private CustomCanvasGroup[] m_Canvas;

    private int m_MenuIndex;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        INSTANCE = this;
        m_MenuIndex = 0;
	}
	
	void Update()
	{
	
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void ChangeCanvas(int Index)
    {
        if (Index >= 0 && Index < m_Canvas.Length)
        {
            StartCoroutine(ChangeCanvasCoroutine(Index));
        }
    }

    public IEnumerator CreateUserCoroutine(string Username, string Passcode)
    {
        if (m_MenuIndex == 9)
        {
            m_Canvas[m_MenuIndex].FadeOut();
            while (m_Canvas[m_MenuIndex].IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            m_LoadingCanvas.FadeIn();
            while (m_LoadingCanvas.IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            StartCoroutine(MySQLWrapper.INSTANCE.CreateUserCoroutine(Username, Passcode));

            yield return new WaitForSeconds(0.5f);

            while (MessageBoxBehaviour.INSTANCE.IsDisplayed())
            {
                yield return new WaitForEndOfFrame();
            }

            m_LoadingCanvas.FadeOut();
            while (m_LoadingCanvas.IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            m_MenuIndex = 1;

            m_Canvas[m_MenuIndex].FadeIn();
        }
    }

    private IEnumerator ChangeCanvasCoroutine(int Index)
    {
        m_Canvas[m_MenuIndex].FadeOut();
        while (m_Canvas[m_MenuIndex].IsFading())
        {
            yield return new WaitForEndOfFrame();
        }

        switch (Index)
        {
            case 0:
                //Back to splashscreen
                m_MenuIndex = Index;
                break;

            case 1 :
                //To login screen
                MySQLWrapper.INSTANCE.ClearData();
                m_MenuIndex = Index;
                break;

            case 2 :
                //To welcome page
                m_LoadingCanvas.FadeIn();
                while (m_LoadingCanvas.IsFading())
                {
                    yield return new WaitForEndOfFrame();
                }

                while (MySQLWrapper.INSTANCE.GetUserState().Equals(MySQLWrapper.UserState.Disconnected))
                {
                    yield return new WaitForEndOfFrame();
                }

                if (MySQLWrapper.INSTANCE.GetUserState().Equals(MySQLWrapper.UserState.Logged))
                {
                    m_MenuIndex = Index;
                }
                else
                {
                    m_LoadingCanvas.FadeOut();
                    while (m_LoadingCanvas.IsFading())
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    MessageBoxBehaviour.INSTANCE.SetMessageTest("Wrong password or username provided please try again (or create your account if necessary).");
                    while (MessageBoxBehaviour.INSTANCE.IsDisplayed())
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                break;

            default :
                m_MenuIndex = Index;
                break;
        }

        m_LoadingCanvas.FadeOut();
        while (m_LoadingCanvas.IsFading())
        {
            yield return new WaitForEndOfFrame();
        }

        if (!m_Canvas[m_MenuIndex].isActiveAndEnabled)
        {
            m_Canvas[m_MenuIndex].gameObject.SetActive(true);
        }

        m_Canvas[m_MenuIndex].FadeIn();
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////