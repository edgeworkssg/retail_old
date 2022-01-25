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
    partial class DeliveryOrderDetail
    {
        ///// <summary>
        ///// Inserts a record, can be used with the Object Data Source
        ///// </summary>
        //public static void CreateOrderDetail(string varDetailsId, string varProductId, decimal? varQuantity,
        //    string varSerialNumber, string varOrderId, string OrderDetID, string varRemarks,
        //    DateTime varCreatedOn, DateTime varModifiedOn, string varCreatedBy, string varModifiedBy, Guid varUniqueID, bool? varDeleted)
        //{
        //    try
        //    {
        //        //Create the new order detail for the selected order
        //        Insert(varDetailsId, varProductId, varQuantity, varSerialNumber, varOrderId, OrderDetID, varRemarks,
        //            varCreatedOn, varModifiedOn, varCreatedBy, varModifiedBy, varUniqueID, varDeleted);
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
        //        {
        //            throw new System.ApplicationException(Constants.ORDERDETAILNUMBER_ALREADY_EXISTS);
        //        }
        //        else
        //            throw new System.ApplicationException(sqlEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new System.ApplicationException(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Updates a record, can be used with the Object Data Source
        ///// </summary>
        //public static void UpdateOrderDetail(string varDetailsId, string varProductId, decimal? varQuantity,
        //    string varSerialNumber, string varOrderId, string OrderDetID, string varRemarks,
        //    DateTime varCreatedOn, DateTime varModifiedOn, string varCreatedBy, string varModifiedBy, Guid varUniqueID, bool? varDeleted)
        //{
        //    try
        //    {
        //        // Update the existing order detail record
        //        DeliveryOrderDetailController dvc = new DeliveryOrderDetailController();
        //        dvc.Update(varDetailsId, varProductId, varQuantity, varSerialNumber, varOrderId, OrderDetID, varRemarks,
        //            varCreatedOn, varModifiedOn, varCreatedBy, varModifiedBy, varUniqueID, varDeleted);
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        if (sqlEx.Number.Equals(Constants.PRIMARY_KEY_VOILATION))
        //        {
        //            throw new System.ApplicationException(Constants.ORDERDETAILNUMBER_ALREADY_EXISTS);
        //        }
        //        else
        //            throw new System.ApplicationException(sqlEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new System.ApplicationException(ex.Message);
        //    }
        //}
    }
}
