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
public class UserCreationBehaviour : MonoBehaviour
{
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private CustomCanvasGroup[] m_Steps;

    [SerializeField]
    private GameObject m_CheckButton;
    [SerializeField]
    private GameObject m_ContinueButton;

    [SerializeField]
    private InputField m_UsernameInputField;
    [SerializeField]
    private InputField m_PasscodeInputField;

    [SerializeField]
    private Text m_UsernameMessageText;
    [SerializeField]
    private Text m_PasscodeMessageText;

    private string m_UsernameChoosed;
    private string m_PasscodeChoosed;

    private int m_Step;

	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        m_Step = 0;
        m_UsernameChoosed = "";
        m_PasscodeChoosed = "";
        m_CheckButton.SetActive(false);
        m_ContinueButton.SetActive(false);
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void Clear()
    {
        StartCoroutine(ClearCoroutine());
    }

    public void CheckUsernameContent()
    {
        switch (m_Step)
        {
            case 0 :
                if (m_UsernameInputField.text.Length < 4)
                {
                    m_UsernameMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                    m_UsernameMessageText.text = "Username must be at least 4 characters long.";
                    m_CheckButton.SetActive(false);
                }
                else if (m_UsernameInputField.text.Length > 32)
                {
                    m_UsernameMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                    m_UsernameMessageText.text = "Usernames max length is 32 characters.";
                    m_CheckButton.SetActive(false);
                }
                else
                {
                    m_ContinueButton.SetActive(false);
                    StopCoroutine(CheckUsernameCoroutine());
                    StartCoroutine(CheckUsernameCoroutine());
                }
                break;

            case 1 :
                if (m_PasscodeChoosed.Equals(""))
                {
                    if (m_PasscodeInputField.text.Length < 4)
                    {
                        m_PasscodeMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                        m_PasscodeMessageText.text = "Passcode must be at least 4 numbers long.";
                        m_CheckButton.SetActive(false);
                    }
                    else if (m_PasscodeInputField.text.Length > 8)
                    {
                        m_PasscodeMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                        m_PasscodeMessageText.text = "Passcodes max length is 8 numbers.";
                        m_CheckButton.SetActive(false);
                    }
                    else
                    {
                        m_PasscodeMessageText.color = new Color(0.165f, 0.165f, 0.949f);
                        m_PasscodeMessageText.text = "Press \"Check\" to verify your passcode.";
                        m_CheckButton.SetActive(true);
                    }
                }
                else
                {
                    if (m_PasscodeInputField.text.Length != m_PasscodeChoosed.Length)
                    {
                        m_PasscodeMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                        m_PasscodeMessageText.text = "Your passcode is " + m_PasscodeChoosed.Length.ToString() + " characters long.";
                        m_ContinueButton.SetActive(false);
                    }
                    else
                    {
                        m_PasscodeMessageText.color = new Color(0.165f, 0.165f, 0.949f);
                        m_PasscodeMessageText.text = "Press \"Continue\" to verify your passcode.";
                        m_ContinueButton.SetActive(true);
                    }
                }
                break;
        }
    }

    public void CheckButton()
    {
        m_CheckButton.SetActive(false);
        m_ContinueButton.SetActive(false);
        if (m_Step == 1)
        {
            m_PasscodeMessageText.color = new Color(0.165f, 0.165f, 0.949f);
            m_PasscodeMessageText.text = "Please re-enter your passcode.";
            m_PasscodeChoosed = m_PasscodeInputField.text;
            m_PasscodeInputField.text = "";
        }
    }

    public void ContinueButton()
    {
        m_CheckButton.SetActive(false);
        m_ContinueButton.SetActive(false);
        if (m_Step == 0)
        {
            m_CheckButton.SetActive(false);
            m_ContinueButton.SetActive(false);
            m_UsernameChoosed = m_UsernameInputField.text;
            m_UsernameInputField.interactable = false;
            StartCoroutine(ChangeStepCoroutine(m_Step + 1));
        }
        else if (m_Step == 1)
        {
            if (!m_PasscodeInputField.text.Equals(m_PasscodeChoosed))
            {
                m_PasscodeMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                m_PasscodeMessageText.text = "Passcodes don't match. Please verify your passcode.";
                m_PasscodeChoosed = "";
            }
            else
            {
                m_PasscodeMessageText.color = new Color(0.165f, 0.949f, 0.165f);
                m_PasscodeMessageText.text = "Verification done.";
                StartCoroutine(MenuController.INSTANCE.CreateUserCoroutine(m_UsernameChoosed, m_PasscodeChoosed));
                Clear();
            }
        }
    }

    private IEnumerator CheckUsernameCoroutine()
    {
        m_UsernameMessageText.color = new Color(0.165f, 0.165f, 0.949f);
        m_UsernameMessageText.text = "Checking availability...\t\t\t\t";

        yield return StartCoroutine(MySQLWrapper.INSTANCE.UsernameAvailabilityCoroutine(m_UsernameInputField.text, (_Callback) =>
        {
            if (_Callback)
            {
                m_UsernameMessageText.color = new Color(0.165f, 0.949f, 0.165f);
                m_UsernameMessageText.text = "Username available, press \"Continue\".";
                m_CheckButton.SetActive(false);
                m_ContinueButton.SetActive(true);
            }
            else
            {
                m_UsernameMessageText.color = new Color(0.949f, 0.165f, 0.165f);
                m_UsernameMessageText.text = "Username already used, please try a different one.";
                m_CheckButton.SetActive(false);
                m_ContinueButton.SetActive(false);
            }
        }));
    }

    private IEnumerator ChangeStepCoroutine(int Step)
    {
        m_Steps[m_Step].FadeOut();
        while (m_Steps[m_Step].IsFading())
        {
            yield return new WaitForEndOfFrame();
        }
        m_Step = Step;
        m_Steps[m_Step].FadeIn();
    }

    private IEnumerator ClearCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        m_UsernameChoosed = "";
        m_PasscodeChoosed = "";
        m_UsernameInputField.text = "";
        m_PasscodeInputField.text = "";
        m_UsernameInputField.interactable = true;
        m_CheckButton.SetActive(false);
        m_ContinueButton.SetActive(false);
        StartCoroutine(ChangeStepCoroutine(0));
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////