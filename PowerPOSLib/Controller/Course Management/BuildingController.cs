using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace PowerPOS
{
    partial class BuildingController
    {
        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void CreateBuilding(Building building)
        {
            try
            {

                //Here all the business rules can be written......

                //Business Rule 1
              
                        
                        BuildingController buildingController = new BuildingController();
                        if (building != null)
                        {
                            buildingController.Insert(building.BuildingName, building.City, building.Country, building.AddressLine1, building.AddressLine2,
                                 building.PinCode);
                        }
                        
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
                {
                    throw new System.ApplicationException(Constants.ORDERNUMBER_ALREADY_EXISTS);
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
