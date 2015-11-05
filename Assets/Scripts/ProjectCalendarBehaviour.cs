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
using System;

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class ProjectCalendarBehaviour : MonoBehaviour
{
	#region Variables
    //////////////////////////////////////////////////////////////////////////
    //STATIC
    //////////////////////////////////////////////////////////////////////////
    public static ProjectCalendarBehaviour INSTANCE;

    public struct ProjectStruct
    {
        public string Name;
        public DateTime StartDate;
        public int TimeUnits;
        public int ID;
    }
    //////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private Text m_MonthDisplay;
    [SerializeField]
    private CalendarDay[] m_Days;

    private bool m_Started;

    private int m_CurrentYear;
    private int m_CurrentMonth;

    private ProjectStruct[] m_Projects;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        INSTANCE = this;
        m_Started = false;
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
    public bool IsStarted()
    {
        return m_Started;
    }
    
    public IEnumerator BeginCalendar()
    {
        m_CurrentYear = DateTime.Now.Year;
        m_CurrentMonth = DateTime.Now.Month;

        for (int i = 0; i < m_Days.Length; i ++)
        {
            if (m_Days[i])
            {
                m_Days[i].Begin();
                while (!m_Days[i].IsSet())
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        m_Started = true;
    }

    public void SetProjects(string[] Projects, string[] StartDates, int[] TimeUnits, int[] IDs)
    {
        m_Projects = new ProjectStruct[Projects.Length];

        for (int i = 0; i < m_Projects.Length; i++)
        {
            m_Projects[i].Name = Projects[i];
            DateTime.TryParse(StartDates[i], out m_Projects[i].StartDate);
            m_Projects[i].TimeUnits = TimeUnits[i];
            m_Projects[i].ID = IDs[i];
        }
    }
    
    public void SetCalendar()
    {
        int _DayIndex = 0;

        DateTime _FirstVisibleDay = new DateTime(m_CurrentYear, m_CurrentMonth, 1);

        for (int i = 0; i < m_Days.Length; i ++)
        {
            if (i < (((int)_FirstVisibleDay.DayOfWeek + 6) % 7) || _DayIndex > DateTime.DaysInMonth(m_CurrentYear, m_CurrentMonth) - 1)
            {
                if (m_Days[i])
                {
                    m_Days[i].gameObject.SetActive(false);
                }
            }
            else
            {
                _DayIndex++;
                if (m_Days[i])
                {
                    m_Days[i].gameObject.SetActive(true);
                    m_Days[i].SetDay(_DayIndex, new bool[4] { 
                        m_Projects.Length > 0 ? IsDateInProject(new DateTime(m_CurrentYear, m_CurrentMonth, _DayIndex), m_Projects[0].StartDate, m_Projects[0].TimeUnits) : false, 
                        m_Projects.Length > 1 ? IsDateInProject(new DateTime(m_CurrentYear, m_CurrentMonth, _DayIndex), m_Projects[1].StartDate, m_Projects[1].TimeUnits) : false, 
                        m_Projects.Length > 2 ? IsDateInProject(new DateTime(m_CurrentYear, m_CurrentMonth, _DayIndex), m_Projects[2].StartDate, m_Projects[2].TimeUnits) : false, 
                        m_Projects.Length > 3 ? IsDateInProject(new DateTime(m_CurrentYear, m_CurrentMonth, _DayIndex), m_Projects[3].StartDate, m_Projects[3].TimeUnits) : false });
                }
            }
        }

        m_MonthDisplay.text = System.Globalization.DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames[m_CurrentMonth - 1] + ". " + m_CurrentYear.ToString();
    }

    public void PreviousMonth()
    {
        m_CurrentMonth --;
        if (m_CurrentMonth == 0)
        {
            m_CurrentMonth = 12;
            m_CurrentYear --;
        }

        SetCalendar();
    }

    public void NextMonth()
    {
        m_CurrentMonth ++;
        if (m_CurrentMonth == 13)
        {
            m_CurrentMonth = 1;
            m_CurrentYear ++;
        }

        SetCalendar();
    }

    private bool IsDateInProject(DateTime DateToCheck, DateTime StartDate, int TimeUnits)
    {
        return DateTime.Compare(StartDate, DateToCheck) <= 0 && GetBusinessDays(StartDate, DateToCheck) <= TimeUnits;
    }

    private int GetBusinessDays(DateTime StartDate, DateTime EndDate)
    {
        double _BusinessDays = 1 + ((EndDate - StartDate).TotalDays * 5 - (StartDate.DayOfWeek - EndDate.DayOfWeek) * 2) / 7;

        if ((int)EndDate.DayOfWeek == 6)
            _BusinessDays --;
        if ((int)StartDate.DayOfWeek == 0)
            _BusinessDays --;

        return (int)_BusinessDays;
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////