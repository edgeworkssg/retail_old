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
    partial class DeliveryPersonnelController
    {
        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void CreateDeliveryPersonnel(DeliveryPersonnel deliveryPersonnel)
        {
            try
            {
                deliveryPersonnel.Save();

                /*Create the new delivery personnel
                //Time from and time to designates the working hours of the personnel
                DeliveryPersonnelController delPersonnelContr = new DeliveryPersonnelController();
                delPersonnelContr.Create(deliveryPersonnel.PersonnelId, deliveryPersonnel.PersonnelName, 
                    deliveryPersonnel.Remarks, deliveryPersonnel.TimeFrom, deliveryPersonnel.TimeTo);
                 */ 
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERDETAILNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        public static void UpdateDeliveryPersonnel(DeliveryPersonnel deliveryPersonnel)
        {
            try
            {
                // Update the existing order detail record
                DeliveryPersonnelController dvc = new DeliveryPersonnelController();
                dvc.Update(deliveryPersonnel.PersonnelId, deliveryPersonnel.PersonnelName,
                    deliveryPersonnel.Remarks, deliveryPersonnel.TimeFrom, deliveryPersonnel.TimeTo);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERDETAILNUMBER_ALREADY_EXISTS);
                }
                else
                    throw new System.ApplicationException(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }
    }
}
