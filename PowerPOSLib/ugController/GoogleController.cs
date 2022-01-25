using System.Data;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace PowerPOS
{
    public partial class GoogleController
    {
        private const string ApplicationName = "Edgeworks-EquipWeb-1";

        public struct ListBoxColumns
        {
            public static string DisplayedColumnName = "Displayed";
            public static string ValueColumnName = "Value";
        }
        
        public struct GoogleColors
        {
            /*  COLOR LISTS
             *	#A32929 		#B1365F 		#7A367A 		#5229A3 		#29527A 		#2952A3 		#1B887A
             *	#28754E 		#0D7813 		#528800 		#88880E 		#AB8B00 		#BE6D00 		#B1440E
             *	#865A5A 		#705770 		#4E5D6C 		#5A6986 		#4A716C 		#6E6E41 		#8D6F47
             *	#853104 		#691426 		#5C1158 		#23164E 		#182C57 		#060D5E 		#125A12
             *	#2F6213 		#2F6309 		#5F6B02 		#8C500B 		#8C500B 		#754916 		#6B3304
             *	#5B123B 		#42104A 		#113F47 		#333333 		#0F4B38 		#856508 		#711616
             */

            public static Color[] GoogleColor;
            public static int MinIndex = 0;
            public static int MaxIndex = 41;

            static GoogleColors()
            {
                GoogleColor = new Color[42];
                GoogleColor[0] = Color.FromArgb(10692905); // 	#A32929 	10692905
                GoogleColor[1] = Color.FromArgb(2651470); // 	#28754E 	2651470
                GoogleColor[2] = Color.FromArgb(8804954); // 	#865A5A 	8804954
                GoogleColor[3] = Color.FromArgb(8728836); // 	#853104 	8728836
                GoogleColor[4] = Color.FromArgb(3105299); // 	#2F6213 	3105299
                GoogleColor[5] = Color.FromArgb(5968443); // 	#5B123B 	5968443
                GoogleColor[6] = Color.FromArgb(11613791); // 	#B1365F 	11613791
                GoogleColor[7] = Color.FromArgb(882707); // 	#0D7813 	882707
                GoogleColor[8] = Color.FromArgb(7362416); // 	#705770 	7362416
                GoogleColor[9] = Color.FromArgb(6886438); // 	#691426 	6886438
                GoogleColor[10] = Color.FromArgb(3105545); // 	#2F6309 	3105545
                GoogleColor[11] = Color.FromArgb(4329546); // 	#42104A 	4329546
                GoogleColor[12] = Color.FromArgb(8009338); // 	#7A367A 	8009338
                GoogleColor[13] = Color.FromArgb(5408768); // 	#528800 	5408768
                GoogleColor[14] = Color.FromArgb(5135724); // 	#4E5D6C 	5135724
                GoogleColor[15] = Color.FromArgb(6033752); // 	#5C1158 	6033752
                GoogleColor[16] = Color.FromArgb(6253314); // 	#5F6B02 	6253314
                GoogleColor[17] = Color.FromArgb(1130311); // 	#113F47 	1130311
                GoogleColor[18] = Color.FromArgb(5384611); // 	#5229A3 	5384611
                GoogleColor[19] = Color.FromArgb(8947726); // 	#88880E 	8947726
                GoogleColor[20] = Color.FromArgb(5925254); // 	#5A6986 	5925254
                GoogleColor[21] = Color.FromArgb(2299470); // 	#23164E 	2299470
                GoogleColor[22] = Color.FromArgb(9195531); // 	#8C500B 	9195531
                GoogleColor[23] = Color.FromArgb(3355443); // 	#333333 	3355443
                GoogleColor[24] = Color.FromArgb(2708090); // 	#29527A 	2708090
                GoogleColor[25] = Color.FromArgb(11242240); // 	#AB8B00 	11242240
                GoogleColor[26] = Color.FromArgb(4878700); // 	#4A716C 	4878700
                GoogleColor[27] = Color.FromArgb(1584215); // 	#182C57 	1584215
                GoogleColor[28] = Color.FromArgb(9195531); // 	#8C500B 	9195531
                GoogleColor[29] = Color.FromArgb(1002296); // 	#0F4B38 	1002296
                GoogleColor[30] = Color.FromArgb(2708131); // 	#2952A3 	2708131
                GoogleColor[31] = Color.FromArgb(12479744); // 	#BE6D00 	12479744
                GoogleColor[32] = Color.FromArgb(7237185); // 	#6E6E41 	7237185
                GoogleColor[33] = Color.FromArgb(396638); // 	#060D5E 	396638
                GoogleColor[34] = Color.FromArgb(7686422); // 	#754916 	7686422
                GoogleColor[35] = Color.FromArgb(8742152); // 	#856508 	8742152
                GoogleColor[36] = Color.FromArgb(1804410); // 	#1B887A 	1804410
                GoogleColor[37] = Color.FromArgb(11617294); // 	#B1440E 	11617294
                GoogleColor[38] = Color.FromArgb(9269063); // 	#8D6F47 	9269063
                GoogleColor[39] = Color.FromArgb(1202706); // 	#125A12 	1202706
                GoogleColor[40] = Color.FromArgb(7025412); // 	#6B3304 	7025412
                GoogleColor[41] = Color.FromArgb(7411222); // 	#711616 	7411222
            }
        }

        public static CalendarService GetCalendarServices(string UserName, string Password)
        {
            CalendarService service = new CalendarService(ApplicationName);
            
            #region *) Authenticate: Set Google's User Name & Password
            if (UserName == null || UserName.Length <= 0 || Password == null || Password.Length <= 0)
                throw new Exception("(warning)Cannot login to google.\nReason: User Name or Password are wrong.\nPlease update your credentials");

            service.setUserCredentials(UserName, Password);
            #endregion

            return service;
        }
        public static CalendarService GetCalendarServices()
        {
            CalendarService service = new CalendarService(ApplicationName);

            string UserName = AppSetting.GetSetting(AppSetting.SettingsName.Google.UserID);
            string Password = AppSetting.GetSetting(AppSetting.SettingsName.Google.Password);

            if (UserName == null) return null;
            if (Password == null) return null;


            #region *) Authenticate: Set Google's User Name & Password
            if (UserName == null || UserName.Length <= 0 || Password == null || Password.Length <= 0)
                throw new Exception("(warning)Cannot login to google.\nReason: User Name or Password are wrong.\nPlease update your credentials");

            service.setUserCredentials(UserName, Password);
            #endregion

            return service;
        }

        public static List<CalendarEntry> GetOwnCalendar()
        {
            string UserName = AppSetting.GetSetting(AppSetting.SettingsName.Google.UserID);
            string Password = AppSetting.GetSetting(AppSetting.SettingsName.Google.Password);

            if (UserName == null) return null;
            if (Password == null) return null;

            return GetOwnCalendar(GetCalendarServices(UserName, Password));
        }
        public static List<CalendarEntry> GetOwnCalendar(string UserName, string Password)
        {
            return GetOwnCalendar(GetCalendarServices(UserName, Password));
        }
        public static List<CalendarEntry> GetOwnCalendar(CalendarService service)
        {
            CalendarQuery query = new CalendarQuery();
            query.Uri = new Uri("https://www.google.com/calendar/feeds/default/owncalendars/full");
            CalendarFeed resultFeed = (CalendarFeed)service.Query(query);

            List<CalendarEntry> Output = new List<CalendarEntry>();
            foreach (CalendarEntry entry in resultFeed.Entries)
            {
                Output.Add(entry);
            }

            return Output;
        }

        public static DataTable GetOwnCalendar_forListBox()
        {
            string UserName = AppSetting.GetSetting(AppSetting.SettingsName.Google.UserID);
            string Password = AppSetting.GetSetting(AppSetting.SettingsName.Google.Password);

            if (UserName == null) return null;
            if (Password == null) return null;

            return GetOwnCalendar_forListBox(GetCalendarServices(UserName, Password));
        }
        public static DataTable GetOwnCalendar_forListBox(string UserName, string Password)
        {
            return GetOwnCalendar_forListBox(GetCalendarServices(UserName, Password));
        }
        public static DataTable GetOwnCalendar_forListBox(CalendarService service)
        {
            CalendarQuery query = new CalendarQuery();
            query.Uri = new Uri("https://www.google.com/calendar/feeds/default/owncalendars/full");
            CalendarFeed resultFeed = (CalendarFeed)service.Query(query);

            DataTable Output = new DataTable();
            Output.Columns.Add(ListBoxColumns.ValueColumnName, Type.GetType("System.String"));
            Output.Columns.Add(ListBoxColumns.DisplayedColumnName, Type.GetType("System.String"));

            foreach (CalendarEntry entry in resultFeed.Entries)
            {
                DataRow Rw = Output.NewRow();
                Rw[ListBoxColumns.ValueColumnName] = entry.Id.AbsoluteUri.Substring(63);
                Rw[ListBoxColumns.DisplayedColumnName] = entry.Title.Text;
                Output.Rows.Add(Rw);
            }

            return Output;
        }

        public static void CreateNewCalendar(string UserName)
        {
            CalendarService service = GetCalendarServices();

            CalendarEntry calendar = new CalendarEntry();
            calendar.Title.Text = UserName;
            calendar.Summary.Text = "Appointment for " + UserName;
            calendar.TimeZone = "Singapore";
            calendar.Hidden = false;
            calendar.Location = new Where("", "", "Singapore");

            Uri postUri = new Uri("https://www.google.com/calendar/feeds/default/owncalendars/full");
            CalendarEntry createdCalendar = (CalendarEntry)service.Insert(postUri, calendar);

            createdCalendar.Title.Text = UserName;
            createdCalendar.Update();
        }
    }
}
