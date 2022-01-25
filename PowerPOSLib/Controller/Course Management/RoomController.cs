using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace PowerPOS
{
    partial class RoomController
    {
        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void CreateRoom(Room room)
        {
            try
            {
                RoomController roomController = new RoomController();
                if (room != null)
                {
                    //roomController.Insert(room.BuildingName, room.RoomName, room.Floor );
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
