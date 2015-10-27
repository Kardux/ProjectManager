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
public class NumpadBehaviour : MonoBehaviour
{
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private Text m_UsernameField;
    [SerializeField]
    private Text m_PasswordField;
    [SerializeField]
    private Text m_PasswordPlaceholder;

    [SerializeField]
    private GameObject m_ButtonCorrection;
    [SerializeField]
    private GameObject m_ButtonValidation;

    private string m_Password;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        m_Password = "";
        UpdatePasswordField();
	}

    void Update()
    {
        m_PasswordPlaceholder.text = (m_Password.Length > 0 ? "" : "Password");

        m_ButtonCorrection.SetActive(m_Password.Length > 0);
        m_ButtonValidation.SetActive(m_UsernameField.text.Length >= 4 && m_Password.Length >= 4);
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void AddNumber(int Number)
    {
        if (m_Password.Length < 8)
        {
            m_Password += Number.ToString();
            UpdatePasswordField();
        }
    }

    public void Correction()
    {
        m_Password = m_Password.Substring(0, m_Password.Length - 1);
        UpdatePasswordField();
    }

    public void Validation()
    {
        MySQLWrapper.INSTANCE.CheckUser(m_UsernameField.text, m_Password);
        m_Password = "";
        UpdatePasswordField();
        MenuController.INSTANCE.ChangeCanvas(2);
    }

    private void UpdatePasswordField()
    {
        m_PasswordField.text = "";
        for (int i = 0; i < m_Password.Length; i ++)
        {
            m_PasswordField.text += "*";
        }
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////