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
public class CalendarDay : MonoBehaviour
{
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    private GameObject[] m_Projects;

    private bool m_Set;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        m_Set = false;
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public bool IsSet()
    {
        return m_Set;
    }
    
    public void Begin()
    {
        m_Projects = new GameObject[4];

        Image[] _Images = new Image[m_Projects.Length];
        _Images = GetComponentsInChildren<Image>();

        for (int i = 0; i < _Images.Length; i++)
        {
            if (!_Images[i].transform.Equals(transform))
            {
                int _Project;
                int.TryParse(_Images[i].gameObject.name.Replace("Project_", ""), out _Project);

                m_Projects[_Project] = _Images[i].gameObject;
            }
        }

        m_Set = true;
    }
    
    public void SetDay(int DayNumber, bool[] Projects)
    {
        GetComponentInChildren<Text>().text = DayNumber.ToString();

        for (int i = 0; i < m_Projects.Length; i++)
        {
            m_Projects[i].GetComponent<Image>().enabled = Projects[i];
        }
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////