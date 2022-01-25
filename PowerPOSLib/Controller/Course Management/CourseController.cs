using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;
namespace PowerPOS
{
    partial class CourseController
    {
        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public int InsertWithID
            (string CourseName, string Description, string CourseType, 
            DateTime? StartDate, DateTime? EndDate, 
            bool? IsAllDay, int? Place)
        {
                        
            Course item = new Course();

            item.CourseName = CourseName;

            item.Description = Description;

            item.CourseType = CourseType;

            item.StartDate = StartDate;

            item.EndDate = EndDate;

            item.IsAllDay = IsAllDay;

            item.Place = Place;


            item.Save(UserName);
            return item.Id;
        }

        public static ArrayList GetRecurranceDates(int courseID)
        {
            RecurrenceController recContr = new RecurrenceController();
            RecurrenceCollection recCol = new RecurrenceCollection();
            recCol.Where(Recurrence.Columns.ItemID, courseID);
            recCol.Load();
            if (recCol.Count == 0)
                return null;
            PowerPOS.Recurrence selectedRecurrence = recCol[0];
            
            RecurrenceLogic recinUI = new RecurrenceLogic();
            if (selectedRecurrence.DayofMonth.HasValue)
                recinUI.DayOfMonth = (byte)selectedRecurrence.DayofMonth.Value;
            if (selectedRecurrence.EndAfter.HasValue)
                recinUI.EndAfter = selectedRecurrence.EndAfter.Value;
            if (selectedRecurrence.EndType.HasValue)
                recinUI.EndType = (RecurrenceEndType)selectedRecurrence.EndType.Value;
            if (selectedRecurrence.Frequency.HasValue)
                recinUI.Frequency = selectedRecurrence.Frequency.Value;
            if (selectedRecurrence.StartDate.HasValue)
                recinUI.StartDate = selectedRecurrence.StartDate.Value.AddDays(-1);
            if (selectedRecurrence.Pattern.HasValue)
                recinUI.Pattern = (RecurrencePattern)selectedRecurrence.Pattern.Value;
            if (selectedRecurrence.SubPattern.HasValue)
                recinUI.SubPattern = (RecurrenceSubPattern)selectedRecurrence.SubPattern.Value;
            if (selectedRecurrence.WeekDays.HasValue)
                recinUI.WeekDays = (byte)selectedRecurrence.WeekDays.Value;
            if (selectedRecurrence.WeekNum.HasValue)
                recinUI.WeekNum = (WeekNumber)selectedRecurrence.WeekNum.Value;
            if (selectedRecurrence.EndDate.HasValue)
                recinUI.EndDate = selectedRecurrence.EndDate.Value;

            
            if (selectedRecurrence.EndType == 0)
                recinUI.EndDate = selectedRecurrence.StartDate.Value.AddDays(100);
            ArrayList arr = recinUI.GetRecurrenceDates
                (selectedRecurrence.StartDate.Value.AddDays(-1), recinUI.EndDate, 
                selectedRecurrence.StartDate.Value.AddDays(-1));
            ArrayList arr1 = new ArrayList();
            foreach (Object o in arr)
            {
                arr1.Add(((DateTime)o).ToString("dd-MMMM-yyyy"));
            }
            return arr1;
        }
    }
}
