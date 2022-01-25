using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
	public partial class Appointment
	{
		public string BuildDescription()
		{
            var result = "";

			var member = Membership;
			var services = AppointmentItemRecords();

            if (member != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result+= member.NameToAppear;
            }

			var servicesSb = new StringBuilder();
			foreach (var service in services)
			{
				if (servicesSb.Length > 0) servicesSb.Append(", ");
				servicesSb.Append(service.Item.ItemName);
			}

            if (!string.IsNullOrEmpty(result))
                result += Environment.NewLine;
			result+=servicesSb;

            if (ResourceID != null && ResourceID != string.Empty)
            {
                Resource res = new Resource(ResourceID);

                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += res.ResourceName;
            }

            if (Description != string.Empty)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += Description;
            }

            if (CheckInTime != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Check In : {0:dd MMM yyyy hh:mm tt}", CheckInTime); 
            }

            if (TimeExtension != null && TimeExtension > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Extend : {0}", TimeExtension.ToString());
            }

            return result;
		}

        public string BuildDescriptionWithSetting(bool IsDrawResource)
        {
            var result = "";

            var member = Membership;
            var services = AppointmentItemRecords();

            if (member != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += member.NameToAppear;
            }

            var servicesSb = new StringBuilder();
            foreach (var service in services)
            {
                if (servicesSb.Length > 0) servicesSb.Append(", ");
                servicesSb.Append(service.Item.ItemName);
            }

            if (!string.IsNullOrEmpty(result))
                result += Environment.NewLine;
            result += servicesSb;
            if (IsDrawResource)
            {
                if (ResourceID != null && ResourceID != string.Empty)
                {
                    Resource res = new Resource(ResourceID);

                    if (!string.IsNullOrEmpty(result))
                        result += Environment.NewLine;
                    result += res.ResourceName;
                }
            }

            if (Description != string.Empty)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += Description;
            }

            if (CheckInTime != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Check In : {0:dd MMM yyyy hh:mm tt}", CheckInTime);
            }

            if (TimeExtension != null && TimeExtension > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Extend : {0}", TimeExtension.ToString());
            }

            if (CheckOutTime != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Check Out : {0:dd MMM yyyy hh:mm tt}", CheckOutTime);
            }

            return result;
        }

        public string BuildDescriptionResource()
        {
            var result = "";

            var member = Membership;
            var services = AppointmentItemRecords();

            if (member != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += member.NameToAppear;
            }

            var servicesSb = new StringBuilder();
            foreach (var service in services)
            {
                if (servicesSb.Length > 0) servicesSb.Append(", ");
                servicesSb.Append(service.Item.ItemName);
            }

            if (!string.IsNullOrEmpty(result))
                result += Environment.NewLine;
            result += servicesSb;

            if (SalesPersonID != null && SalesPersonID != string.Empty)
            {
                UserMst res = new UserMst(SalesPersonID);

                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += res.DisplayName;
            }

            if (Description != string.Empty)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += Description;
            }

            if (CheckInTime != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Check In : {0:dd MMM yyyy hh:mm tt}", CheckInTime);
            }

            if (TimeExtension != null && TimeExtension > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Extend : {0}", TimeExtension.ToString());
            }

            if (CheckOutTime != null)
            {
                if (!string.IsNullOrEmpty(result))
                    result += Environment.NewLine;
                result += string.Format("Check Out : {0:dd MMM yyyy hh:mm tt}", CheckOutTime);
            }

            return result;
        }

        public string BuildCompleteDescriptionWEB()
        {
            var descSb = new StringBuilder();

            DateTime dtStart = this.StartTime;
            //string time = (string.Format("{0} - {1} ",
            //    dtStart.ToString("hh:mm tt"),
            //    dtStart.AddMinutes(this.Duration).ToString("hh:mm tt")));

            //descSb.Append(time);

            var member = Membership;
            var services = AppointmentItemRecords();

            if (member != null)
                descSb.AppendLine("<br/>" + member.NameToAppear);

            var servicesSb = new StringBuilder();
            foreach (var service in services)
            {
                if (servicesSb.Length > 0) servicesSb.Append(", ");
                servicesSb.Append(service.Item.ItemName);
            }
            descSb.Append("<br/>" + servicesSb);

            if (Description != null && Description != string.Empty)
                descSb.AppendLine("<br/>" + Description);

            if (CheckInTime != null)
            {
                 descSb.AppendLine("<br/>" + string.Format("Check In : {0:dd MMM yyyy hh:mm tt}", CheckInTime));
            }

            //return "---";
            return descSb.ToString();
        }

        public string BuildCompleteDescriptionWEEKLY()
        {
            var descSb = new StringBuilder();

            DateTime dtStart = this.StartTime;
            string time = (string.Format("{0} - {1} ",
                dtStart.ToString("hh:mm tt"),
                dtStart.AddMinutes(this.Duration).ToString("hh:mm tt")));

            descSb.Append(time);

            var member = Membership;
            var services = AppointmentItemRecords();

            if (member != null)
                descSb.AppendLine(Environment.NewLine + member.NameToAppear);

            var servicesSb = new StringBuilder();
            foreach (var service in services)
            {
                if (servicesSb.Length > 0) servicesSb.Append(", ");
                servicesSb.Append(service.Item.ItemName);
            }
            descSb.Append(Environment.NewLine + servicesSb);

            if (Description != string.Empty)
                descSb.AppendLine(Environment.NewLine + Description);

            return descSb.ToString();
        }

        private string _services;
        public string Service
        {
            get
            {
                if (string.IsNullOrEmpty(_services))
                {
                    string sql = @"SELECT  TOP 1 ItemNo
                                    FROM	AppointmentItem 
                                    WHERE	AppointmentId = '{0}'
		                                    AND ISNULL(Deleted,0) = 0
                                    ORDER BY CreatedOn DESC";
                    sql = string.Format(sql, this.Id.ToString());
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                    if (dt.Rows.Count > 0)
                    {
                        _services = (string)dt.Rows[0]["ItemNo"];
                    }
                }
                return _services;
            }
            set
            {
                _services = value;
            }
        }
	}
}
