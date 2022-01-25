using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;
using System.Collections.Generic;

public partial class CounterCloseLogReport : PageBase
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    private const int Edit = 0;
    private const int CollectedPOS = 8;
    private const int RecordedPOS = 7;
    private const int DefiPOS = 9;
    private const int CashRecordedPOS = 11;
    private const int CashCollectedPOS = 12;

    private const int TotalForeignCurrency = 18;
    private const int ForeignCurrency1Recorded = 19;
    private const int ForeignCurrency1Collected = 20;
    private const int ForeignCurrency2Recorded = 21;
    private const int ForeignCurrency2Collected = 22;
    private const int ForeignCurrency3Recorded = 23;
    private const int ForeignCurrency3Collected = 24;
    private const int ForeignCurrency4Recorded = 25;
    private const int ForeignCurrency4Collected = 26;
    private const int ForeignCurrency5Recorded = 27;
    private const int ForeignCurrency5Collected = 28;


    private const int NetsRecordedPOS = 29;
    private const int NetsCollectedPOS = 30;

    private const int NetsCashCardRecordedPOS = 31;
    private const int NetsCashCardCollectedPOS = 32;
    private const int NetsFlashPayRecordedPOS = 33;
    private const int NetsFlashPayCollectedPOS = 34;
    private const int NetsATMCardRecordedPOS = 35;
    private const int NetsATMCardCollectedPOS = 36;

    private const int VisaRecordedPOS = 37;
    private const int VisaCollectedPOS = 38;
    private const int AmexRecordedPOS = 41;
    private const int AmexCollectedPOS = 42;
    private const int ChinaNetsRecordedPOS = 39;
    private const int ChinaNetsCollectedPOS = 40;
    private const int Payment5RecordedPOS = 43;
    private const int Payment5CollectedPOS = 44;
    private const int Payment6RecordedPOS = 45;
    private const int Payment6CollectedPOS = 46;
    private const int Payment7RecordedPOS = 47;
    private const int Payment7CollectedPOS = 48;
    private const int Payment8RecordedPOS = 49;
    private const int Payment8CollectedPOS = 50;
    private const int Payment9RecordedPOS = 51;
    private const int Payment9CollectedPOS = 52;
    private const int Payment10RecordedPOS = 53;
    private const int Payment10CollectedPOS = 54;
    private const int TotalGst = 69;
    private const int PointRecorded = 66;

    private const int VoucherRecordedPOS = 55;
    private const int VoucherCollectedPOS = 56;
    private const int ChequeRecordedPOS = 57;
    private const int ChequeCollectedPOS = 58;

    private const int SMFRecordedPOS = 60;
    private const int SMFCollectedPOS = 61;
    private const int PAMedRecordedPOS = 62;
    private const int PAMedCollectedPOS = 63;
    private const int PWFRecordedPOS = 64;
    private const int PWFCollectedPOS = 65;

    private const int OpeningBalancePOS = 10;
    private const int CashInPOS = 13;
    private const int CashOutPOS = 14;
    //private const int FloatBalancePOS = 24;
    private const int ClosingCashOutPOS = 16;

    private string CurrencySymbol1 = "$";
    private string CurrencySymbol2 = "$";
    private string CurrencySymbol3 = "$";
    private string CurrencySymbol4 = "$";
    private string CurrencySymbol5 = "$";

    private bool enableNetsIntegration;
    private bool enableNETSATMCard;
    private bool enableNETSCashCard;
    private bool enableNETSFlashPay;

    protected void Page_Init(object sender, EventArgs e)
    {
        OutletDropdownList.setIsUseAllBreakdownOutlet(false);
        OutletDropdownList.setIsUseAllBreakdownPOS(false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["id"] != "0")
        {
            if (!Page.IsPostBack)
            {
                CheckNetsIntegration();
                ShowEditor(true);
                LoadEditor(Request.QueryString["id"]);
            }
        }
        else
        {
            if (!Page.IsPostBack)
            {
                CheckNetsIntegration();
                ShowEditor(false);
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                ViewState["sortBy"] = "ENDTIME";
                ViewState[SORT_DIRECTION] = "DESC";

                DataSet ds = new DataSet();
                ds.ReadXml(Server.MapPath("~/PaymentTypes.xml"));
                PaymentTypes = ds.Tables[0];


                BindGrid();
            }
        }
    }

    private void ShowEditor(bool ShowIt)
    {
        pnlEdit.Visible = ShowIt;
        pnlSearch.Visible = !ShowIt;
        pnlGrid.Visible = !ShowIt;
    }

    private void LoadEditor(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            CounterCloseLog ccl = new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, id);

            lblCounterClose.Text = ccl.CounterCloseID.ToString();
            lblOpeningTime.Text = ccl.StartTime.ToString("dd MMMM yyyy HH:mm:ss");
            lblClosingTime.Text = ccl.EndTime.ToString("dd MMMM yyyy HH:mm:ss");
            lblOutlet.Text = ccl.PointOfSale.OutletName;
            LblPOSID.Text = ccl.PointOfSale.PointOfSaleName;

            lblTotalActualCollected.Text = ccl.TotalActualCollected.ToString("N2");
            lblTotalSystemRecorded.Text = ccl.TotalSystemRecorded.ToString("N2");
            lblVariance.Text = ccl.Variance.ToString("N2");
            lblOpeningBalance.Text = ccl.OpeningBalance.ToString("N2");

            txtCashIn.Text = ccl.CashIn.ToString("N2");
            txtCashOut.Text = ccl.CashOut.ToString("N2");
            txtDepositBag.Text = ccl.DepositBagNo;

            txtCash.Text = ccl.CashCollected.ToString("N2");
            lblCashRecorded.Text = ccl.CashRecorded.GetValueOrDefault(0).ToString("N2");
            lblCashSurplus.Text = (ccl.CashCollected - ccl.CashRecorded.GetValueOrDefault(0)).ToString("N2");

            if (enableNETSATMCard)
            {
                divNetsATMCard.Visible = true;
                divNetsAtmCardHeader.Visible = true;
                divNetsAtmCardSurplus.Visible = true;
                divNetsAtmCardRecorded.Visible = true;
                txtNetsAtmCard.Text = ccl.NetsATMCardCollected.GetValueOrDefault(0).ToString("N2");
                lblNetsAtmCardRecorded.Text = ccl.NetsATMCardRecorded.GetValueOrDefault(0).ToString("N2");
                lblNetsAtmCardSurplus.Text = (ccl.NetsATMCardCollected.GetValueOrDefault(0) - ccl.NetsATMCardRecorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divNetsATMCard.Visible = false;
                divNetsAtmCardHeader.Visible = false;
                divNetsAtmCardSurplus.Visible = false;
                divNetsAtmCardRecorded.Visible = false;
            }

            if (enableNETSCashCard)
            {
                divNetsCashCard.Visible = true;
                divNetsCashCardHeader.Visible = true;
                divNetsCashCardSurplus.Visible = true;
                divNetsCashCardRecorded.Visible = true;
                txtNetsCashCard.Text = ccl.NetsCashCardCollected.GetValueOrDefault(0).ToString("N2");
                lblNetsCashCardRecorded.Text = ccl.NetsCashCardRecorded.GetValueOrDefault(0).ToString("N2");
                lblNetsCashCardSurplus.Text = (ccl.NetsCashCardCollected.GetValueOrDefault(0) - ccl.NetsCashCardRecorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divNetsCashCard.Visible = false;
                divNetsCashCardHeader.Visible = false;
                divNetsCashCardSurplus.Visible = false;
                divNetsCashCardRecorded.Visible = false;
            }


            if (enableNETSFlashPay)
            {
                divNetsFlashPay.Visible = true;
                divNetsFlashPayHeader.Visible = true;
                divNetsFlashPayRecorded.Visible = true;
                divNetsFlashPaySurplus.Visible = true;
                txtNetsFlashPay.Text = ccl.NetsFlashPayCollected.GetValueOrDefault(0).ToString("N2");
                lblNetsCashCardRecorded.Text = ccl.NetsFlashPayRecorded.GetValueOrDefault(0).ToString("N2");
                lblNetsCashCardSurplus.Text = (ccl.NetsFlashPayCollected.GetValueOrDefault(0) - ccl.NetsFlashPayRecorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divNetsFlashPay.Visible = false;
                divNetsFlashPayHeader.Visible = false;
                divNetsFlashPayRecorded.Visible = false;
                divNetsFlashPaySurplus.Visible = false;

            }

            string tmp = FetchPaymentByID("1");
            divPayment1.Visible = false;
            divPayment1Header.Visible = false;
            divPayment1Recorded.Visible = false;
            divPayment1Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment1Header.Visible = true;
                    divPayment1Recorded.Visible = true;
                    divPayment1Surplus.Visible = true;
                    divPayment1.Visible = true;
                    lblPayment1.Text = tmp;
                    txtPayment1.Text = ccl.NetsCollected.ToString("N2");
                    lblPayment1Recorded.Text = ccl.NetsRecorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment1Surplus.Text = (ccl.NetsCollected - ccl.NetsRecorded.GetValueOrDefault(0)).ToString("N2");
                }
            }

            tmp = FetchPaymentByID("2");
            divPayment2.Visible = false;
            divPayment2Header.Visible = false;
            divPayment2Recorded.Visible = false;
            divPayment2Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment2.Visible = true;
                    divPayment2Header.Visible = true;
                    divPayment2Recorded.Visible = true;
                    divPayment2Surplus.Visible = true;
                    lblPayment2.Text = tmp;
                    txtPayment2.Text = ccl.VisaCollected.ToString("N2");
                    lblPayment2Recorded.Text = ccl.VisaRecorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment2Surplus.Text = (ccl.VisaCollected - ccl.VisaRecorded.GetValueOrDefault(0)).ToString("N2");
                }
            }

            tmp = FetchPaymentByID("3");
            divPayment3.Visible = false;
            divPayment3Header.Visible = false;
            divPayment3Recorded.Visible = false;
            divPayment3Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment3.Visible = true;
                    divPayment3Header.Visible = true;
                    divPayment3Recorded.Visible = true;
                    divPayment3Surplus.Visible = true;
                    lblPayment3.Text = tmp;
                    txtPayment3.Text = ccl.ChinaNetsCollected.ToString("N2");
                    lblPayment3Recorded.Text = ccl.ChinaNetsRecorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment3Surplus.Text = (ccl.ChinaNetsCollected - ccl.ChinaNetsRecorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("4");
            divPayment4.Visible = false;
            divPayment4Header.Visible = false;
            divPayment4Recorded.Visible = false;
            divPayment4Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment4.Visible = true;
                    divPayment4Header.Visible = true;
                    divPayment4Recorded.Visible = true;
                    divPayment4Surplus.Visible = true;
                    lblPayment4.Text = tmp;
                    txtPayment4.Text = ccl.AmexCollected.ToString("N2");
                    lblPayment4Recorded.Text = ccl.AmexRecorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment4Surplus.Text = (ccl.AmexCollected - ccl.AmexRecorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("5");
            divPayment5.Visible = false;
            divPayment5Header.Visible = false;
            divPayment5Recorded.Visible = false;
            divPayment5Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment5.Visible = true;
                    divPayment5Header.Visible = true;
                    divPayment5Recorded.Visible = true;
                    divPayment5Surplus.Visible = true;
                    lblPayment5.Text = tmp;
                    txtPayment5.Text = ccl.Pay5Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment5Recorded.Text = ccl.Pay5Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment5Surplus.Text = (ccl.Pay5Collected.GetValueOrDefault(0) - ccl.Pay5Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("6");
            divPayment6.Visible = false;
            divPayment6Header.Visible = false;
            divPayment6Recorded.Visible = false;
            divPayment6Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment6.Visible = true;
                    divPayment6Header.Visible = true;
                    divPayment6Recorded.Visible = true;
                    divPayment6Surplus.Visible = true;
                    lblPayment6.Text = tmp;
                    txtPayment6.Text = ccl.Pay6Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment6Recorded.Text = ccl.Pay6Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment6Surplus.Text = (ccl.Pay6Collected.GetValueOrDefault(0) - ccl.Pay6Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("7");
            divPayment7.Visible = false;
            divPayment7Header.Visible = false;
            divPayment7Recorded.Visible = false;
            divPayment7Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment7.Visible = true;
                    divPayment7Header.Visible = true;
                    divPayment7Recorded.Visible = true;
                    divPayment7Surplus.Visible = true;
                    lblPayment7.Text = tmp;
                    txtPayment7.Text = ccl.Pay7Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment7Recorded.Text = ccl.Pay7Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment7Surplus.Text = (ccl.Pay7Collected.GetValueOrDefault(0) - ccl.Pay7Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("8");
            divPayment8.Visible = false;
            divPayment8Header.Visible = false;
            divPayment8Recorded.Visible = false;
            divPayment8Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment8.Visible = true;
                    divPayment8Header.Visible = true;
                    divPayment8Recorded.Visible = true;
                    divPayment8Surplus.Visible = true;
                    lblPayment8.Text = tmp;
                    txtPayment8.Text = ccl.Pay8Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment8Recorded.Text = ccl.Pay8Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment8Surplus.Text = (ccl.Pay8Collected.GetValueOrDefault(0) - ccl.Pay8Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }

            tmp = FetchPaymentByID("9");
            divPayment9.Visible = false;
            divPayment9Header.Visible = false;
            divPayment9Recorded.Visible = false;
            divPayment9Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment9.Visible = true;
                    divPayment9Header.Visible = true;
                    divPayment9Recorded.Visible = true;
                    divPayment9Surplus.Visible = true;
                    lblPayment9.Text = tmp;
                    txtPayment9.Text = ccl.Pay9Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment9Recorded.Text = ccl.Pay9Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment9Surplus.Text = (ccl.Pay9Collected.GetValueOrDefault(0) - ccl.Pay9Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }


            tmp = FetchPaymentByID("10");
            divPayment10.Visible = false;
            divPayment10Header.Visible = false;
            divPayment10Recorded.Visible = false;
            divPayment10Surplus.Visible = false;
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    divPayment10.Visible = true;
                    divPayment10Header.Visible = true;
                    divPayment10Recorded.Visible = true;
                    divPayment10Surplus.Visible = true;
                    lblPayment10.Text = tmp;
                    txtPayment10.Text = ccl.Pay10Collected.GetValueOrDefault(0).ToString("N2");
                    lblPayment10Recorded.Text = ccl.Pay10Recorded.GetValueOrDefault(0).ToString("N2");
                    lblPayment10Surplus.Text = (ccl.Pay10Collected.GetValueOrDefault(0) - ccl.Pay10Recorded.GetValueOrDefault(0)).ToString("N2");
                }
            }
            else
            {
                divPayment10.Visible = false;
            }

            if (PaymentTypes.Select("Name = 'Voucher'").Length == 0)
            {
                divVoucher.Visible = true;
                divVoucherHeader.Visible = true;
                divVoucherRecorded.Visible = true;
                divVoucherSurplus.Visible = true;
                txtVoucher.Text = ccl.VoucherCollected.ToString("N2");
                lblVoucherRecorded.Text = ccl.VoucherRecorded.GetValueOrDefault(0).ToString("N2");
                lblVoucherSurplus.Text = (ccl.VoucherCollected - ccl.VoucherRecorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divVoucher.Visible = false;
                divVoucherHeader.Visible = false;
                divVoucherRecorded.Visible = false;
                divVoucherSurplus.Visible = false;
            }

            if (ccl.ChequeRecorded != 0)
            {
                divCheque.Visible = true;
                divChequeHeader.Visible = true;
                divChequeRecorded.Visible = true;
                divChequeSurplus.Visible = true;
                txtCheque.Text = ccl.ChequeCollected.GetValueOrDefault(0).ToString("N2");
                lblChequeRecorded.Text = ccl.ChequeRecorded.GetValueOrDefault(0).ToString("N2");
                lblChequeSurplus.Text = (ccl.ChequeCollected.GetValueOrDefault(0) - ccl.ChequeRecorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divCheque.Visible = false;
                divChequeHeader.Visible = false;
                divChequeRecorded.Visible = false;
                divChequeSurplus.Visible = false;
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency1))
            {
                divForeignCurrency1.Visible = true;
                divForeignCurrency1Header.Visible = true;
                divForeignCurrency1Recorded.Visible = true;
                divForeignCurrency1Surplus.Visible = true;
                lblForeignCurrency1.Text = ccl.ForeignCurrency1;
                txtForeignCurrency1.Text = ccl.ForeignCurrency1Collected.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency1Recorded.Text = ccl.ForeignCurrency1Recorded.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency1Surplus.Text = (ccl.ForeignCurrency1Collected.GetValueOrDefault(0) - ccl.ForeignCurrency1Recorded.GetValueOrDefault(0)).ToString("N2");

            }
            else
            {
                divForeignCurrency1.Visible = false;
                divForeignCurrency1Header.Visible = false;
                divForeignCurrency1Recorded.Visible = false;
                divForeignCurrency1Surplus.Visible = false;
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency2))
            {
                divForeignCurrency2.Visible = true;
                divForeignCurrency2Header.Visible = true;
                divForeignCurrency2Recorded.Visible = true;
                divForeignCurrency2Surplus.Visible = true;
                lblForeignCurrency2.Text = ccl.ForeignCurrency2;
                txtForeignCurrency2.Text = ccl.ForeignCurrency2Collected.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency2Recorded.Text = ccl.ForeignCurrency2Recorded.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency2Surplus.Text = (ccl.ForeignCurrency2Collected.GetValueOrDefault(0) - ccl.ForeignCurrency2Recorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divForeignCurrency2.Visible = false;
                divForeignCurrency2Header.Visible = false;
                divForeignCurrency2Recorded.Visible = false;
                divForeignCurrency2Surplus.Visible = false;
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency3))
            {
                divForeignCurrency3.Visible = true;
                divForeignCurrency3Header.Visible = true;
                divForeignCurrency3Recorded.Visible = true;
                divForeignCurrency3Surplus.Visible = true;
                lblForeignCurrency3.Text = ccl.ForeignCurrency3;
                txtForeignCurrency3.Text = ccl.ForeignCurrency3Collected.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency3Recorded.Text = ccl.ForeignCurrency3Recorded.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency3Surplus.Text = (ccl.ForeignCurrency3Collected.GetValueOrDefault(0) - ccl.ForeignCurrency3Recorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divForeignCurrency3.Visible = false;
                divForeignCurrency3Header.Visible = false;
                divForeignCurrency3Recorded.Visible = false;
                divForeignCurrency3Surplus.Visible = false;
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency4))
            {
                divForeignCurrency4.Visible = true;
                divForeignCurrency4Header.Visible = true;
                divForeignCurrency4Recorded.Visible = true;
                divForeignCurrency4Surplus.Visible = true;
                lblForeignCurrency4.Text = ccl.ForeignCurrency4;
                txtForeignCurrency4.Text = ccl.ForeignCurrency4Collected.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency4Recorded.Text = ccl.ForeignCurrency4Recorded.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency4Surplus.Text = (ccl.ForeignCurrency4Collected.GetValueOrDefault(0) - ccl.ForeignCurrency4Recorded.GetValueOrDefault(0)).ToString("N2");
            }
            else
            {
                divForeignCurrency4.Visible = false;
                divForeignCurrency4Header.Visible = false;
                divForeignCurrency4Recorded.Visible = false;
                divForeignCurrency4Surplus.Visible = false;
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency5))
            {
                divForeignCurrency5.Visible = true;
                divForeignCurrency4Header.Visible = true;
                divForeignCurrency4Recorded.Visible = true;
                divForeignCurrency4Surplus.Visible = true;
                lblForeignCurrency5.Text = ccl.ForeignCurrency5;
                txtForeignCurrency5.Text = ccl.ForeignCurrency5Collected.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency5Recorded.Text = ccl.ForeignCurrency5Recorded.GetValueOrDefault(0).ToString("N2");
                lblForeignCurrency5Surplus.Text = (ccl.ForeignCurrency5Collected.GetValueOrDefault(0) - ccl.ForeignCurrency5Recorded.GetValueOrDefault(0)).ToString("N2");

            }
            else
            {
                divForeignCurrency5.Visible = false;
                divForeignCurrency5Header.Visible = false;
                divForeignCurrency5Recorded.Visible = false;
                divForeignCurrency5Surplus.Visible = false;
            }

            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enableSMF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            bool enablePAMed = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);

            if (enableFunding && enableSMF)
            {
                divSMFMed.Visible = true;
                divSMFMedHeader.Visible = true;
                divSMFMedRecorded.Visible = true;
                divSMFMedSurplus.Visible = true;
                txtSMFMed.Text = ccl.SMFCollected.GetValueOrDefault(0).ToString("N2");
                lblSMFMedRecorded.Text = ccl.SMFCollected.GetValueOrDefault(0).ToString("N2");
                lblSMFMedSurplus.Text = (ccl.SMFCollected.GetValueOrDefault(0) - ccl.SMFRecorded.GetValueOrDefault(0)).ToString("N2");

            }
            else
            {
                divSMFMed.Visible = false;
                divSMFMedHeader.Visible = false;
                divSMFMedRecorded.Visible = false;
                divSMFMedSurplus.Visible = false;
            }

            if (enableFunding && enablePAMed)
            {
                divPAMed.Visible = true;
                divPAMedHeader.Visible = true;
                divPAMedRecorded.Visible = true;
                divPAMedSurplus.Visible = true;
                txtPAMed.Text = ccl.PAMedCollected.GetValueOrDefault(0).ToString("N2");
                lblPAMedRecorded.Text = ccl.PAMedCollected.GetValueOrDefault(0).ToString("N2");
                lblPAMedSurplus.Text = (ccl.PAMedCollected.GetValueOrDefault(0) - ccl.PAMedRecorded.GetValueOrDefault(0)).ToString("N2");

            }
            else
            {
                divPAMed.Visible = false;
                divPAMedHeader.Visible = false;
                divPAMedRecorded.Visible = false;
                divPAMedSurplus.Visible = false;
            }

            if (enableFunding && enablePWF)
            {
                divPWF.Visible = true;
                divPWFHeader.Visible = true;
                divPWFRecorded.Visible = true;
                divPWFSurplus.Visible = true;
                txtPWF.Text = ccl.PWFCollected.GetValueOrDefault(0).ToString("N2");
                lblPWFRecorded.Text = ccl.PWFCollected.GetValueOrDefault(0).ToString("N2");
                lblPWFSurplus.Text = (ccl.PWFCollected.GetValueOrDefault(0) - ccl.PWFRecorded.GetValueOrDefault(0)).ToString("N2");

            }
            else
            {
                divPWF.Visible = false;
                divPWFHeader.Visible = false;
                divPWFRecorded.Visible = false;
                divPWFSurplus.Visible = false;
            }



        }
    }

    private void CheckNetsIntegration()
    {
        bool enableNetsIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
        bool enableNETSATMCard = enableNetsIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
        bool enableNETSCashCard = enableNetsIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
        bool enableNETSFlashPay = enableNetsIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);



        gvReport.Columns[NetsCashCardRecordedPOS].Visible = enableNETSCashCard;
        gvReport.Columns[NetsCashCardCollectedPOS].Visible = enableNETSCashCard;
        gvReport.Columns[NetsFlashPayRecordedPOS].Visible = enableNETSFlashPay;
        gvReport.Columns[NetsFlashPayCollectedPOS].Visible = enableNETSFlashPay;
        gvReport.Columns[NetsATMCardRecordedPOS].Visible = enableNETSATMCard;
        gvReport.Columns[NetsATMCardCollectedPOS].Visible = enableNETSATMCard;

        gvReport.Columns[NetsRecordedPOS].Visible = !enableNetsIntegration;
        gvReport.Columns[NetsCollectedPOS].Visible = !enableNetsIntegration;
    }

    private static DataTable PaymentTypes;
    public static string FetchPaymentByID(string ID)
    {
        if (PaymentTypes == null)
        {
            return "";
        }
        DataRow[] dr = PaymentTypes.Select("ID = '" + ID.ToString() + "'");
        if (dr.Length > 0)
        {
            string name = dr[0]["Name"].ToString();
            if (name != "" && name != "-" && name.ToLower() != "points" && name.ToLower() != "installment")
            {
                return name;
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }
    private void BindGrid()
    {
        //if (false)
        {
            if (ViewState["sortBy"] == null)
            {
                ViewState["sortBy"] = "EndTime";
                ViewState[SORT_DIRECTION] = "ASC";
            }

            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);
            string SelectedPOS = OutletDropdownList.GetDdlPOSSelectedItemText;
            if (SelectedPOS == "ALL")
            {
                SelectedPOS = "%";
            }

            int selectedPOSID = OutletDropdownList.GetDdlPOSSelectedValue.GetIntValue();

            if (PaymentTypes.Select("Name = 'Voucher'").Length > 0)
            {
                // Hide Voucher if there's already a payment type named "Voucher" in PaymentTypes.xml
                gvReport.Columns[VoucherRecordedPOS].Visible = false;
                gvReport.Columns[VoucherCollectedPOS].Visible = false;
            }

            #region *) Show/hide Funding Method
            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enableSMF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            bool enablePAMed = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            gvReport.Columns[SMFRecordedPOS].Visible = (enableFunding && enableSMF);
            gvReport.Columns[SMFCollectedPOS].Visible = (enableFunding && enableSMF);
            gvReport.Columns[PAMedRecordedPOS].Visible = (enableFunding && enablePAMed);
            gvReport.Columns[PAMedCollectedPOS].Visible = (enableFunding && enablePAMed);
            gvReport.Columns[PWFRecordedPOS].Visible = (enableFunding && enablePWF);
            gvReport.Columns[PWFCollectedPOS].Visible = (enableFunding && enablePWF);
            #endregion

            DataTable dt = ReportController.FetchCounterCloseLogReport(
                cbUseStartDate.Checked, cbUseEndDate.Checked,
                startDate, endDate.AddSeconds(86399),
                "", txtCashier.Text, txtSupervisor.Text,
                selectedPOSID, OutletDropdownList.GetDdlOutletSelectedItemText, ddDept.SelectedValue.GetIntValue(),
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());

            dt = UtilityController.RoundToTwoDecimalDigits(dt);

            #region *) Check Foreign Currency

            string currencyCode1 = "";
            string currencyCode2 = "";
            string currencyCode3 = "";
            string currencyCode4 = "";
            string currencyCode5 = "";


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencyCode1"] + ""))
                    currencyCode1 = dt.Rows[i]["CurrencyCode1"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencyCode2"] + ""))
                    currencyCode2 = dt.Rows[i]["CurrencyCode2"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencyCode3"] + ""))
                    currencyCode3 = dt.Rows[i]["CurrencyCode3"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencyCode4"] + ""))
                    currencyCode4 = dt.Rows[i]["CurrencyCode4"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencyCode5"] + ""))
                    currencyCode5 = dt.Rows[i]["CurrencyCode5"] + "";

                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencySymbol1"] + ""))
                    CurrencySymbol1 = dt.Rows[i]["CurrencySymbol1"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencySymbol2"] + ""))
                    CurrencySymbol2 = dt.Rows[i]["CurrencySymbol2"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencySymbol3"] + ""))
                    CurrencySymbol3 = dt.Rows[i]["CurrencySymbol3"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencySymbol4"] + ""))
                    CurrencySymbol4 = dt.Rows[i]["CurrencySymbol4"] + "";
                if (!string.IsNullOrEmpty(dt.Rows[i]["CurrencySymbol5"] + ""))
                    CurrencySymbol5 = dt.Rows[i]["CurrencySymbol5"] + "";
            }

            gvReport.Columns[ForeignCurrency1Recorded].HeaderText = currencyCode1;
            gvReport.Columns[ForeignCurrency2Recorded].HeaderText = currencyCode2;
            gvReport.Columns[ForeignCurrency3Recorded].HeaderText = currencyCode3;
            gvReport.Columns[ForeignCurrency4Recorded].HeaderText = currencyCode4;
            gvReport.Columns[ForeignCurrency5Recorded].HeaderText = currencyCode5;

            gvReport.Columns[ForeignCurrency1Recorded].Visible = !string.IsNullOrEmpty(currencyCode1);
            gvReport.Columns[ForeignCurrency2Recorded].Visible = !string.IsNullOrEmpty(currencyCode2);
            gvReport.Columns[ForeignCurrency3Recorded].Visible = !string.IsNullOrEmpty(currencyCode3);
            gvReport.Columns[ForeignCurrency4Recorded].Visible = !string.IsNullOrEmpty(currencyCode4);
            gvReport.Columns[ForeignCurrency5Recorded].Visible = !string.IsNullOrEmpty(currencyCode5);
            gvReport.Columns[ForeignCurrency1Collected].Visible = !string.IsNullOrEmpty(currencyCode1);
            gvReport.Columns[ForeignCurrency2Collected].Visible = !string.IsNullOrEmpty(currencyCode2);
            gvReport.Columns[ForeignCurrency3Collected].Visible = !string.IsNullOrEmpty(currencyCode3);
            gvReport.Columns[ForeignCurrency4Collected].Visible = !string.IsNullOrEmpty(currencyCode4);
            gvReport.Columns[ForeignCurrency5Collected].Visible = !string.IsNullOrEmpty(currencyCode5);

            #endregion

            gvReport.DataSource = dt;
            gvReport.DataBind();

            List<DateTime> AllClosingTime = ReportController.FetchCounterClosingReport_DateOnly(
                cbUseStartDate.Checked, cbUseStartDate.Checked,
                startDate, endDate.AddSeconds(86399),
                "", txtCashier.Text, txtSupervisor.Text, 0, SelectedPOS,
                OutletDropdownList.GetDdlOutletSelectedItemText, ddDept.SelectedValue.ToString(), ViewState["sortBy"].ToString(),
                ViewState[SORT_DIRECTION].ToString());

            AllClosingTime.Sort();

            List<DateTime> MissingClosingTime = new List<DateTime>();
            for (int Counter = 1; Counter < AllClosingTime.Count; Counter++)
            {
                if (AllClosingTime[Counter].AddDays(-1) == AllClosingTime[Counter - 1])
                    continue;

                for (DateTime dCounter = AllClosingTime[Counter - 1].AddDays(1); dCounter < AllClosingTime[Counter]; dCounter = dCounter.AddDays(1))
                {
                    MissingClosingTime.Add(dCounter);
                }
            }

            if (MissingClosingTime.Count <= 0)
            {
                txtMissingDate.Text = "";
            }
            else
            {
                txtMissingDate.Text = "Warning - Sales data might not be updated on dates: ";
                foreach (DateTime Counter in MissingClosingTime)
                {
                    txtMissingDate.Text += Counter.ToString("dd MMM, ");
                }
                txtMissingDate.Text = txtMissingDate.Text.Substring(0, txtMissingDate.Text.Length - 2) + ". Please check.";
            }
        }
        //else
        //BindChart();
    }

    private void BindChart()
    {
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "EndTime";
            ViewState[SORT_DIRECTION] = "ASC";
        }

        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        string SelectedPOS = OutletDropdownList.GetDdlPOSSelectedItemText;
        if (SelectedPOS == "ALL")
        {
            SelectedPOS = "%";
        }
        DataTable dt = ReportController.FetchCounterClosingReport(
            cbUseStartDate.Checked, cbUseStartDate.Checked,
            startDate, endDate.AddSeconds(86399),
            "", txtCashier.Text, txtSupervisor.Text, 0, SelectedPOS,
            OutletDropdownList.GetDdlOutletSelectedItemText, ddDept.SelectedValue.ToString(), ViewState["sortBy"].ToString(),
            ViewState[SORT_DIRECTION].ToString());

        //iframe2.Attributes["src"] = ChartController.CreateLineChart(dt, "cashcollected", "endtime");
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvReport_DataBound(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        if (gvrPager == null)
        {
            return;
        }

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");
        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvReport.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == gvReport.PageIndex)
                {
                    lstItem.Selected = true;
                }

                ddlPages.Items.Add(lstItem);
            }

        }

        int itemCount = 0;
        // populate page count
        if (lblPageCount != null)
        {
            //pull the datasource
            DataSet ds = gvReport.DataSource as DataSet;
            if (ds != null)
            {
                itemCount = ds.Tables[0].Rows.Count;
            }

            string pageCount = "<b>" + gvReport.PageCount.ToString() + "</b>";
            lblPageCount.Text = pageCount;
        }

        Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
        Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
        Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
        Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
        //now figure out what page we're on
        if (gvReport.PageIndex == 0)
        {
            btnPrev.Enabled = false;
            btnFirst.Enabled = false;
        }

        else if (gvReport.PageIndex + 1 == gvReport.PageCount)
        {
            btnLast.Enabled = false;
            btnNext.Enabled = false;
        }
        else
        {
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnPrev.Enabled = true;
            btnFirst.Enabled = true;
        }

    }
    protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["sortBy"] = e.SortExpression;
        //rebind the grid
        if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
        {
            ViewState[SORT_DIRECTION] = SqlFragment.DESC;
        }

        else
        {
            ViewState[SORT_DIRECTION] = SqlFragment.ASC;
        }

        BindGrid();
    }
    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvReport.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid();
    }
    string GetSortDirection(string sortBy)
    {
        string sortDir = " ASC";
        if (ViewState["sortBy"] != null)
        {
            string sortedBy = ViewState["sortBy"].ToString();
            if (sortedBy == sortBy)
            {
                //the direction should be desc
                sortDir = " DESC";
                //reset the sorter to null
                ViewState["sortBy"] = null;
            }

            else
            {
                //this is the first sort for this row
                //put it to the ViewState
                ViewState["sortBy"] = sortBy;
            }
        }
        else
        {
            //it's null, so this is the first sort
            ViewState["sortBy"] = sortBy;
        }

        return sortDir;
    }
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!PrivilegesController.HasPrivilege("Edit Counter Close Log", (DataTable)Session["privileges"]))
        {
            if (e.Row.RowType != DataControlRowType.Pager)
                e.Row.Cells[Edit].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {

            string test = FetchPaymentByID("1");
            if (test != "")
            {
                e.Row.Cells[NetsRecordedPOS].Text = test;
                gvReport.Columns[NetsRecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[NetsRecordedPOS].Visible = false;
                gvReport.Columns[NetsRecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("2");
            if (test != "")
            {
                e.Row.Cells[VisaRecordedPOS].Text = test;
                gvReport.Columns[VisaRecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[VisaRecordedPOS].Visible = false;
                gvReport.Columns[VisaRecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("3");
            if (test != "")
            {
                e.Row.Cells[ChinaNetsRecordedPOS].Text = test;
                gvReport.Columns[ChinaNetsRecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[ChinaNetsRecordedPOS].Visible = false;
                gvReport.Columns[ChinaNetsRecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("4");
            if (test != "")
            {
                e.Row.Cells[AmexRecordedPOS].Text = test;
                gvReport.Columns[AmexRecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[AmexRecordedPOS].Visible = false;
                gvReport.Columns[AmexRecordedPOS + 1].Visible = false;

            }
            test = FetchPaymentByID("5");
            if (test != "")
            {
                e.Row.Cells[Payment5RecordedPOS].Text = test;
                gvReport.Columns[Payment5RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment5RecordedPOS].Visible = false;
                gvReport.Columns[Payment5RecordedPOS + 1].Visible = false;

            }
            test = FetchPaymentByID("6");
            if (test != "")
            {
                e.Row.Cells[Payment6RecordedPOS].Text = test;
                gvReport.Columns[Payment6RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment6RecordedPOS].Visible = false;
                gvReport.Columns[Payment6RecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("7");
            if (test != "")
            {
                e.Row.Cells[Payment7RecordedPOS].Text = test;
                gvReport.Columns[Payment7RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment7RecordedPOS].Visible = false;
                gvReport.Columns[Payment7RecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("8");
            if (test != "")
            {
                e.Row.Cells[Payment8RecordedPOS].Text = test;
                gvReport.Columns[Payment8RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment8RecordedPOS].Visible = false;
                gvReport.Columns[Payment8RecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("9");
            if (test != "")
            {
                e.Row.Cells[Payment9RecordedPOS].Text = test;
                gvReport.Columns[Payment9RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment9RecordedPOS].Visible = false;
                gvReport.Columns[Payment9RecordedPOS + 1].Visible = false;
            }
            test = FetchPaymentByID("10");
            if (test != "")
            {
                e.Row.Cells[Payment10RecordedPOS].Text = test;
                gvReport.Columns[Payment10RecordedPOS].HeaderText = test;
            }
            else
            {
                gvReport.Columns[Payment10RecordedPOS].Visible = false;
                gvReport.Columns[Payment10RecordedPOS + 1].Visible = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CommonWebUILib.SetVideoBrowserClientScript(e, 1, int.Parse(((DataTable)gvReport.DataSource).Rows[e.Row.RowIndex+gvReport.PageIndex * gvReport.PageSize]["PointOfSaleID"].ToString()));
            decimal Collected, Recorded, Defi, CashRecorded, CashCollected,
                    NetsRecorded, NetsCollected, VisaRecorded, VisaCollected,
                    AmexRecorded, AmexCollected, ChinaNetsRecorded, ChinaNetsCollected,
                    OpeningBalance, CashIn, CashOut, FloatBalance, ClosingCashOut,
                    VoucherRecorded, VoucherCollected, ChequeCollected, ChequeRecorded, tmp, GSTRecorded;

            Collected = Decimal.Parse(e.Row.Cells[CollectedPOS].Text);
            e.Row.Cells[CollectedPOS].Text = String.Format("{0:N2}", Collected);

            Recorded = Decimal.Parse(e.Row.Cells[RecordedPOS].Text);
            e.Row.Cells[RecordedPOS].Text = String.Format("{0:N2}", Recorded);

            Defi = Decimal.Parse(e.Row.Cells[DefiPOS].Text);
            e.Row.Cells[DefiPOS].Text = String.Format("{0:N2}", Defi);

            CashRecorded = Decimal.Parse(e.Row.Cells[CashRecordedPOS].Text);
            e.Row.Cells[CashRecordedPOS].Text = String.Format("{0:N2}", CashRecorded);

            CashCollected = Decimal.Parse(e.Row.Cells[CashCollectedPOS].Text);
            e.Row.Cells[CashCollectedPOS].Text = String.Format("{0:N2}", CashCollected);

            if (gvReport.Columns[TotalGst].Visible)
            {
                GSTRecorded = Decimal.Parse(e.Row.Cells[TotalGst].Text);
                e.Row.Cells[TotalGst].Text = String.Format("{0:N2}", GSTRecorded);
            }
            if (gvReport.Columns[NetsRecordedPOS].Visible)
            {

                NetsRecorded = Decimal.Parse(e.Row.Cells[NetsRecordedPOS].Text);
                e.Row.Cells[NetsRecordedPOS].Text = String.Format("{0:N2}", NetsRecorded);

                NetsCollected = Decimal.Parse(e.Row.Cells[NetsCollectedPOS].Text);
                e.Row.Cells[NetsCollectedPOS].Text = String.Format("{0:N2}", NetsCollected);
            }
            if (gvReport.Columns[VisaRecordedPOS].Visible)
            {

                VisaRecorded = Decimal.Parse(e.Row.Cells[VisaRecordedPOS].Text);
                e.Row.Cells[VisaRecordedPOS].Text = String.Format("{0:N2}", VisaRecorded);

                VisaCollected = Decimal.Parse(e.Row.Cells[VisaCollectedPOS].Text);
                e.Row.Cells[VisaCollectedPOS].Text = String.Format("{0:N2}", VisaCollected);
            }
            if (gvReport.Columns[AmexRecordedPOS].Visible)
            {
                AmexRecorded = Decimal.Parse(e.Row.Cells[AmexRecordedPOS].Text);
                e.Row.Cells[AmexRecordedPOS].Text = String.Format("{0:N2}", AmexRecorded);

                AmexCollected = Decimal.Parse(e.Row.Cells[AmexCollectedPOS].Text);
                e.Row.Cells[AmexCollectedPOS].Text = String.Format("{0:N2}", AmexCollected);
            }
            if (gvReport.Columns[ChinaNetsRecordedPOS].Visible)
            {

                ChinaNetsRecorded = Decimal.Parse(e.Row.Cells[ChinaNetsRecordedPOS].Text);
                e.Row.Cells[ChinaNetsRecordedPOS].Text = String.Format("{0:N2}", ChinaNetsRecorded);

                ChinaNetsCollected = Decimal.Parse(e.Row.Cells[ChinaNetsCollectedPOS].Text);
                e.Row.Cells[ChinaNetsCollectedPOS].Text = String.Format("{0:N2}", ChinaNetsCollected);
            }

            VoucherRecorded = 0;
            decimal.TryParse(e.Row.Cells[VoucherRecordedPOS].Text, out VoucherRecorded);
            e.Row.Cells[VoucherRecordedPOS].Text = String.Format("{0:N2}", VoucherRecorded);

            VoucherCollected = 0;
            decimal.TryParse(e.Row.Cells[VoucherCollectedPOS].Text, out VoucherCollected);
            e.Row.Cells[VoucherCollectedPOS].Text = String.Format("{0:N2}", VoucherCollected);

            if (gvReport.Columns[Payment5RecordedPOS].Visible)
            {

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment5RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment5RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment5CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment5CollectedPOS].Text = String.Format("{0:N2}", tmp);

            }
            tmp = 0;
            if (gvReport.Columns[Payment6RecordedPOS].Visible)
            {

                decimal.TryParse(e.Row.Cells[Payment6RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment6RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment6CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment6CollectedPOS].Text = String.Format("{0:N2}", tmp);
            }

            tmp = 0;
            if (gvReport.Columns[Payment7RecordedPOS].Visible)
            {

                decimal.TryParse(e.Row.Cells[Payment7RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment7RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment7CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment7CollectedPOS].Text = String.Format("{0:N2}", tmp);
            }

            tmp = 0;
            if (gvReport.Columns[Payment8RecordedPOS].Visible)
            {

                decimal.TryParse(e.Row.Cells[Payment8RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment8RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment8CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment8CollectedPOS].Text = String.Format("{0:N2}", tmp);
            }

            tmp = 0;
            if (gvReport.Columns[Payment9RecordedPOS].Visible)
            {

                decimal.TryParse(e.Row.Cells[Payment9RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment9RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment9CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment9CollectedPOS].Text = String.Format("{0:N2}", tmp);
            }

            tmp = 0;
            if (gvReport.Columns[Payment10RecordedPOS].Visible)
            {

                decimal.TryParse(e.Row.Cells[Payment10RecordedPOS].Text, out tmp);
                e.Row.Cells[Payment10RecordedPOS].Text = String.Format("{0:N2}", tmp);

                tmp = 0;
                decimal.TryParse(e.Row.Cells[Payment10CollectedPOS].Text, out tmp);
                e.Row.Cells[Payment10CollectedPOS].Text = String.Format("{0:N2}", tmp);
            }

            if (gvReport.Columns[ForeignCurrency1Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency1Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol1,
                    (e.Row.Cells[ForeignCurrency1Collected].Text + "").GetDecimalValue());
                e.Row.Cells[ForeignCurrency1Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol1,
                    (e.Row.Cells[ForeignCurrency1Recorded].Text + "").GetDecimalValue());
            }
            if (gvReport.Columns[ForeignCurrency2Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency2Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol2,
                    (e.Row.Cells[ForeignCurrency2Collected].Text + "").GetDecimalValue());
                e.Row.Cells[ForeignCurrency2Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol2,
                    (e.Row.Cells[ForeignCurrency2Recorded].Text + "").GetDecimalValue());
            }
            if (gvReport.Columns[ForeignCurrency3Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency3Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol3,
                    (e.Row.Cells[ForeignCurrency3Collected].Text + "").GetDecimalValue());
                e.Row.Cells[ForeignCurrency3Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol3,
                    (e.Row.Cells[ForeignCurrency3Recorded].Text + "").GetDecimalValue());
            }
            if (gvReport.Columns[ForeignCurrency4Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency4Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol4,
                    (e.Row.Cells[ForeignCurrency4Collected].Text + "").GetDecimalValue());
                e.Row.Cells[ForeignCurrency4Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol4,
                    (e.Row.Cells[ForeignCurrency4Recorded].Text + "").GetDecimalValue());
            }
            if (gvReport.Columns[ForeignCurrency5Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency5Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol5,
                    (e.Row.Cells[ForeignCurrency5Collected].Text + "").GetDecimalValue());
                e.Row.Cells[ForeignCurrency5Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol5,
                    (e.Row.Cells[ForeignCurrency5Recorded].Text + "").GetDecimalValue());
            }


            tmp = 0;
            decimal.TryParse(e.Row.Cells[ChequeRecordedPOS].Text, out ChequeRecorded);
            e.Row.Cells[ChequeRecordedPOS].Text = String.Format("{0:N2}", ChequeRecorded);

            ChequeCollected = 0;
            decimal.TryParse(e.Row.Cells[ChequeCollectedPOS].Text, out ChequeCollected);
            e.Row.Cells[ChequeCollectedPOS].Text = String.Format("{0:N2}", ChequeCollected);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[SMFRecordedPOS].Text, out tmp);
            e.Row.Cells[SMFRecordedPOS].Text = String.Format("{0:N2}", tmp);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[SMFCollectedPOS].Text, out tmp);
            e.Row.Cells[SMFCollectedPOS].Text = String.Format("{0:N2}", tmp);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[PAMedRecordedPOS].Text, out tmp);
            e.Row.Cells[PAMedRecordedPOS].Text = String.Format("{0:N2}", tmp);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[PAMedCollectedPOS].Text, out tmp);
            e.Row.Cells[PAMedCollectedPOS].Text = String.Format("{0:N2}", tmp);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[PWFRecordedPOS].Text, out tmp);
            e.Row.Cells[PWFRecordedPOS].Text = String.Format("{0:N2}", tmp);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[PWFCollectedPOS].Text, out tmp);
            e.Row.Cells[PWFCollectedPOS].Text = String.Format("{0:N2}", tmp);

            OpeningBalance = Decimal.Parse(e.Row.Cells[OpeningBalancePOS].Text);
            e.Row.Cells[OpeningBalancePOS].Text = String.Format("{0:N2}", OpeningBalance);

            CashIn = Decimal.Parse(e.Row.Cells[CashInPOS].Text);
            e.Row.Cells[CashInPOS].Text = String.Format("{0:N2}", CashIn);

            CashOut = Decimal.Parse(e.Row.Cells[CashOutPOS].Text);
            e.Row.Cells[CashOutPOS].Text = String.Format("{0:N2}", CashOut);
            /*
            FloatBalance = Decimal.Parse(e.Row.Cells[FloatBalancePOS].Text);
            e.Row.Cells[FloatBalancePOS].Text = String.Format("{0:N2}", FloatBalance);
            */
            ClosingCashOut = Decimal.Parse(e.Row.Cells[ClosingCashOutPOS].Text);
            e.Row.Cells[ClosingCashOutPOS].Text = String.Format("{0:N2}", ClosingCashOut);

            tmp = 0;
            decimal.TryParse(e.Row.Cells[PointRecorded].Text, out tmp);
            e.Row.Cells[PointRecorded].Text = String.Format("{0:N2}", tmp);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            calculateSum();
            e.Row.Cells[RecordedPOS].Text = String.Format("{0:N2}", TotalRecorded);
            e.Row.Cells[CollectedPOS].Text = String.Format("{0:N2}", TotalCollected);
            e.Row.Cells[DefiPOS].Text = String.Format("{0:N2}", TotalDefi);
            e.Row.Cells[CashRecordedPOS].Text = String.Format("{0:N2}", TotalCashRecorded);
            e.Row.Cells[CashCollectedPOS].Text = String.Format("{0:N2}", TotalCashCollected);
            e.Row.Cells[NetsRecordedPOS].Text = String.Format("{0:N2}", TotalNetsRecorded);
            e.Row.Cells[NetsCollectedPOS].Text = String.Format("{0:N2}", TotalNetsCollected);
            e.Row.Cells[VisaRecordedPOS].Text = String.Format("{0:N2}", TotalVisaRecorded);
            e.Row.Cells[VisaCollectedPOS].Text = String.Format("{0:N2}", TotalVisaCollected);
            e.Row.Cells[AmexRecordedPOS].Text = String.Format("{0:N2}", TotalAmexRecorded);
            e.Row.Cells[AmexCollectedPOS].Text = String.Format("{0:N2}", TotalAmexCollected);
            e.Row.Cells[ChinaNetsRecordedPOS].Text = String.Format("{0:N2}", TotalChinaNetsRecorded);
            e.Row.Cells[ChinaNetsCollectedPOS].Text = String.Format("{0:N2}", TotalChinaNetsCollected);
            e.Row.Cells[VoucherRecordedPOS].Text = String.Format("{0:N2}", TotalVoucherRecorded);
            e.Row.Cells[VoucherCollectedPOS].Text = String.Format("{0:N2}", TotalVoucherCollected);
            e.Row.Cells[ChequeRecordedPOS].Text = String.Format("{0:N2}", TotalChequeRecorded);
            e.Row.Cells[ChequeCollectedPOS].Text = String.Format("{0:N2}", TotalChequeCollected);
            e.Row.Cells[Payment5RecordedPOS].Text = String.Format("{0:N2}", TotalPayment5Recorded);
            e.Row.Cells[Payment5CollectedPOS].Text = String.Format("{0:N2}", TotalPayment5Collected);
            e.Row.Cells[Payment6RecordedPOS].Text = String.Format("{0:N2}", TotalPayment6Recorded);
            e.Row.Cells[Payment6CollectedPOS].Text = String.Format("{0:N2}", TotalPayment6Collected);

            e.Row.Cells[Payment7RecordedPOS].Text = String.Format("{0:N2}", TotalPayment7Recorded);
            e.Row.Cells[Payment7CollectedPOS].Text = String.Format("{0:N2}", TotalPayment7Collected);
            e.Row.Cells[Payment8RecordedPOS].Text = String.Format("{0:N2}", TotalPayment8Recorded);
            e.Row.Cells[Payment8CollectedPOS].Text = String.Format("{0:N2}", TotalPayment8Collected);
            e.Row.Cells[Payment9RecordedPOS].Text = String.Format("{0:N2}", TotalPayment9Recorded);
            e.Row.Cells[Payment9CollectedPOS].Text = String.Format("{0:N2}", TotalPayment9Collected);
            e.Row.Cells[Payment10RecordedPOS].Text = String.Format("{0:N2}", TotalPayment10Recorded);
            e.Row.Cells[Payment10CollectedPOS].Text = String.Format("{0:N2}", TotalPayment10Collected);

            e.Row.Cells[SMFRecordedPOS].Text = String.Format("{0:N2}", TotalSMFRecorded);
            e.Row.Cells[SMFCollectedPOS].Text = String.Format("{0:N2}", TotalSMFCollected);
            e.Row.Cells[PAMedRecordedPOS].Text = String.Format("{0:N2}", TotalPAMedRecorded);
            e.Row.Cells[PAMedCollectedPOS].Text = String.Format("{0:N2}", TotalPAMedCollected);
            e.Row.Cells[PWFRecordedPOS].Text = String.Format("{0:N2}", TotalPWFRecorded);
            e.Row.Cells[PWFCollectedPOS].Text = String.Format("{0:N2}", TotalPWFCollected);
            e.Row.Cells[OpeningBalancePOS].Text = String.Format("{0:N2}", TotalOpeningBalance);
            e.Row.Cells[CashInPOS].Text = String.Format("{0:N2}", TotalCashIn);
            e.Row.Cells[CashOutPOS].Text = String.Format("{0:N2}", TotalCashOut);
            //e.Row.Cells[FloatBalancePOS].Text = String.Format("{0:N2}", TotalFloatBalance);
            e.Row.Cells[ClosingCashOutPOS].Text = String.Format("{0:N2}", TotalClosingCashOut);
            e.Row.Cells[TotalGst].Text = String.Format("{0:N2}", TotalGSTRecorded);
            e.Row.Cells[PointRecorded].Text = String.Format("{0:N2}", TotalPointRecorded);
            e.Row.Cells[NetsATMCardCollectedPOS].Text = String.Format("{0:N2}", NetsATMCardCollected);
            e.Row.Cells[NetsATMCardRecordedPOS].Text = String.Format("{0:N2}", NetsATMCardRecorded);
            e.Row.Cells[NetsCashCardCollectedPOS].Text = String.Format("{0:N2}", NetsCashCardCollected);
            e.Row.Cells[NetsCashCardRecordedPOS].Text = String.Format("{0:N2}", NetsCashCardRecorded);
            e.Row.Cells[NetsFlashPayCollectedPOS].Text = String.Format("{0:N2}", NetsFlashPayCollected);
            e.Row.Cells[NetsFlashPayRecordedPOS].Text = String.Format("{0:N2}", NetsFlashPayRecorded);
            e.Row.Cells[TotalForeignCurrency].Text = string.Format("{0:N2}", TotalForeignAmount);
            if (gvReport.Columns[ForeignCurrency1Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency1Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol1, CollectedAmount1.ToString("N2"));
                e.Row.Cells[ForeignCurrency1Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol1, RecordedAmount1.ToString("N2"));
            }
            if (gvReport.Columns[ForeignCurrency2Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency2Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol2, CollectedAmount2.ToString("N2"));
                e.Row.Cells[ForeignCurrency2Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol2, RecordedAmount2.ToString("N2"));
            }
            if (gvReport.Columns[ForeignCurrency3Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency3Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol3, CollectedAmount3.ToString("N2"));
                e.Row.Cells[ForeignCurrency3Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol3, RecordedAmount3.ToString("N2"));
            }
            if (gvReport.Columns[ForeignCurrency4Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency4Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol4, CollectedAmount4.ToString("N2"));
                e.Row.Cells[ForeignCurrency4Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol4, RecordedAmount4.ToString("N2"));
            }
            if (gvReport.Columns[ForeignCurrency5Collected].Visible)
            {
                e.Row.Cells[ForeignCurrency5Collected].Text =
                    string.Format("{0}{1}", CurrencySymbol5, CollectedAmount5.ToString("N2"));
                e.Row.Cells[ForeignCurrency5Recorded].Text =
                    string.Format("{0}{1}", CurrencySymbol5, RecordedAmount5.ToString("N2"));
            }
        }
    }
    decimal TotalCollected, TotalRecorded, TotalDefi, TotalCashRecorded, TotalCashCollected,
            TotalNetsRecorded, TotalNetsCollected, TotalVisaRecorded, TotalVisaCollected, TotalVoucherRecorded, TotalVoucherCollected, TotalChequeCollected, TotalChequeRecorded,
            TotalAmexRecorded, TotalAmexCollected, TotalChinaNetsRecorded, TotalChinaNetsCollected,
            TotalOpeningBalance, TotalCashIn, TotalCashOut, TotalFloatBalance, TotalClosingCashOut, TotalPayment5Recorded,
            TotalPayment5Collected,
            TotalPayment6Recorded, TotalPayment6Collected,
            TotalPayment7Recorded, TotalPayment7Collected,
            TotalPayment8Recorded, TotalPayment8Collected,
            TotalPayment9Recorded, TotalPayment9Collected,
            TotalPayment10Recorded, TotalPayment10Collected,
            TotalGSTRecorded, TotalPointRecorded,
            TotalSMFRecorded, TotalSMFCollected, TotalPAMedRecorded, TotalPAMedCollected, TotalPWFRecorded, TotalPWFCollected,
            NetsCashCardCollected, NetsCashCardRecorded,
            NetsFlashPayCollected, NetsFlashPayRecorded,
            NetsATMCardCollected, NetsATMCardRecorded,
            TotalForeignAmount,
            CollectedAmount1, RecordedAmount1,
            CollectedAmount2, RecordedAmount2,
            CollectedAmount3, RecordedAmount3,
            CollectedAmount4, RecordedAmount4,
            CollectedAmount5, RecordedAmount5;

    public void calculateSum()
    {
        TotalRecorded = 0;
        TotalCollected = 0;
        TotalDefi = 0;
        TotalCashRecorded = 0;
        TotalCashCollected = 0;
        TotalNetsRecorded = 0;
        TotalNetsCollected = 0;
        TotalVisaRecorded = 0;
        TotalVisaCollected = 0;
        TotalAmexRecorded = 0;
        TotalAmexCollected = 0;
        TotalChinaNetsRecorded = 0;
        TotalChinaNetsCollected = 0;
        TotalVoucherCollected = 0;
        TotalVoucherRecorded = 0;
        TotalChequeCollected = 0;
        TotalChequeRecorded = 0;
        TotalPayment5Recorded = 0;
        TotalPayment5Collected = 0;
        TotalPayment6Recorded = 0;
        TotalPayment6Collected = 0;
        TotalPayment7Recorded = 0;
        TotalPayment7Collected = 0;
        TotalPayment8Recorded = 0;
        TotalPayment8Collected = 0;
        TotalPayment9Recorded = 0;
        TotalPayment9Collected = 0;
        TotalPayment10Recorded = 0;
        TotalPayment10Collected = 0;
        TotalOpeningBalance = 0;
        TotalCashIn = 0;
        TotalCashOut = 0;
        TotalFloatBalance = 0;
        TotalClosingCashOut = 0;
        TotalVoucherCollected = 0;
        TotalVoucherRecorded = 0;
        TotalSMFRecorded = 0;
        TotalSMFCollected = 0;
        TotalPAMedRecorded = 0;
        TotalPAMedCollected = 0;
        TotalPWFRecorded = 0;
        TotalPWFCollected = 0;
        NetsCashCardCollected = 0;
        NetsCashCardRecorded = 0;
        NetsFlashPayCollected = 0;
        NetsFlashPayRecorded = 0;
        NetsATMCardCollected = 0;
        NetsATMCardRecorded = 0;
        TotalForeignAmount = 0;
        CollectedAmount1 = 0;
        RecordedAmount1 = 0;
        CollectedAmount2 = 0;
        RecordedAmount2 = 0;
        CollectedAmount3 = 0;
        RecordedAmount3 = 0;
        CollectedAmount4 = 0;
        RecordedAmount4 = 0;
        CollectedAmount5 = 0;
        RecordedAmount5 = 0;
        TotalPointRecorded = 0;

        DataTable dt = (DataTable)gvReport.DataSource;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TotalRecorded += (dt.Rows[i]["TotalSystemRecorded"] + "").GetDecimalValue();
            TotalCollected += (dt.Rows[i]["TotalActualCollected"] + "").GetDecimalValue();
            TotalDefi += (dt.Rows[i]["Variance"] + "").GetDecimalValue();
            TotalCashRecorded += (dt.Rows[i]["CashRecorded"] + "").GetDecimalValue();
            TotalCashCollected += (dt.Rows[i]["CashCollected"] + "").GetDecimalValue();
            TotalNetsRecorded += (dt.Rows[i]["NetsRecorded"] + "").GetDecimalValue();
            TotalNetsCollected += (dt.Rows[i]["NetsCollected"] + "").GetDecimalValue();
            TotalVisaRecorded += (dt.Rows[i]["VisaRecorded"] + "").GetDecimalValue();
            TotalVisaCollected += (dt.Rows[i]["VisaCollected"] + "").GetDecimalValue();
            TotalAmexRecorded += (dt.Rows[i]["AmexRecorded"] + "").GetDecimalValue();
            TotalAmexCollected += (dt.Rows[i]["AmexCollected"] + "").GetDecimalValue();
            TotalChinaNetsRecorded += (dt.Rows[i]["ChinaNetsRecorded"] + "").GetDecimalValue();
            TotalChinaNetsCollected += (dt.Rows[i]["ChinaNetsCollected"] + "").GetDecimalValue();
            NetsCashCardCollected += (dt.Rows[i]["NetsCashCardCollected"] + "").GetDecimalValue();
            NetsCashCardRecorded += (dt.Rows[i]["NetsCashCardRecorded"] + "").GetDecimalValue();
            NetsFlashPayCollected += (dt.Rows[i]["NetsFlashPayCollected"] + "").GetDecimalValue();
            NetsFlashPayRecorded += (dt.Rows[i]["NetsFlashPayRecorded"] + "").GetDecimalValue();
            NetsATMCardCollected += (dt.Rows[i]["NetsATMCardCollected"] + "").GetDecimalValue();
            NetsATMCardRecorded += (dt.Rows[i]["NetsATMCardRecorded"] + "").GetDecimalValue();
            TotalVoucherRecorded += (dt.Rows[i]["VoucherRecorded"] + "").GetDecimalValue();
            TotalVoucherCollected += (dt.Rows[i]["VoucherCollected"] + "").GetDecimalValue();
            TotalChequeRecorded += (dt.Rows[i]["ChequeRecorded"] + "").GetDecimalValue();
            TotalChequeCollected += (dt.Rows[i]["ChequeCollected"] + "").GetDecimalValue();
            TotalPayment5Recorded += (dt.Rows[i]["Payment5Recorded"] + "").GetDecimalValue();
            TotalPayment5Collected += (dt.Rows[i]["Payment5Collected"] + "").GetDecimalValue();
            TotalPayment6Recorded += (dt.Rows[i]["Payment6Recorded"] + "").GetDecimalValue();
            TotalPayment6Collected += (dt.Rows[i]["Payment6Collected"] + "").GetDecimalValue();
            TotalPayment7Recorded += (dt.Rows[i]["Payment7Recorded"] + "").GetDecimalValue();
            TotalPayment7Collected += (dt.Rows[i]["Payment7Collected"] + "").GetDecimalValue();
            TotalPayment8Recorded += (dt.Rows[i]["Payment8Recorded"] + "").GetDecimalValue();
            TotalPayment8Collected += (dt.Rows[i]["Payment8Collected"] + "").GetDecimalValue();
            TotalPayment9Recorded += (dt.Rows[i]["Payment9Recorded"] + "").GetDecimalValue();
            TotalPayment9Collected += (dt.Rows[i]["Payment9Collected"] + "").GetDecimalValue();
            TotalPayment10Recorded += (dt.Rows[i]["Payment10Recorded"] + "").GetDecimalValue();
            TotalPayment10Collected += (dt.Rows[i]["Payment10Collected"] + "").GetDecimalValue();
            TotalSMFRecorded += (dt.Rows[i]["SMFRecorded"] + "").GetDecimalValue();
            TotalSMFCollected += (dt.Rows[i]["SMFCollected"] + "").GetDecimalValue();
            TotalPAMedRecorded += (dt.Rows[i]["PAMedRecorded"] + "").GetDecimalValue();
            TotalPAMedCollected += (dt.Rows[i]["PAMedCollected"] + "").GetDecimalValue();
            TotalPWFRecorded += (dt.Rows[i]["PWFRecorded"] + "").GetDecimalValue();
            TotalPWFCollected += (dt.Rows[i]["PWFCollected"] + "").GetDecimalValue();
            TotalOpeningBalance += (dt.Rows[i]["OpeningBalance"] + "").GetDecimalValue();
            TotalCashIn += (dt.Rows[i]["CashIn"] + "").GetDecimalValue();
            TotalCashOut += (dt.Rows[i]["CashOut"] + "").GetDecimalValue();
            TotalFloatBalance += (dt.Rows[i]["FloatBalance"] + "").GetDecimalValue();
            TotalClosingCashOut += (dt.Rows[i]["ClosingCashOut"] + "").GetDecimalValue();
            TotalGSTRecorded += (dt.Rows[i]["TotalGST"] + "").GetDecimalValue();
            TotalForeignAmount += (dt.Rows[i]["TotalForeignCurrency"] + "").GetDecimalValue();
            CollectedAmount1 += (dt.Rows[i]["CollectedAmount1"] + "").GetDecimalValue();
            RecordedAmount1 += (dt.Rows[i]["RecordedAmount1"] + "").GetDecimalValue();
            CollectedAmount2 += (dt.Rows[i]["CollectedAmount2"] + "").GetDecimalValue();
            RecordedAmount2 += (dt.Rows[i]["RecordedAmount2"] + "").GetDecimalValue();
            CollectedAmount3 += (dt.Rows[i]["CollectedAmount3"] + "").GetDecimalValue();
            RecordedAmount3 += (dt.Rows[i]["RecordedAmount3"] + "").GetDecimalValue();
            CollectedAmount4 += (dt.Rows[i]["CollectedAmount4"] + "").GetDecimalValue();
            RecordedAmount4 += (dt.Rows[i]["RecordedAmount4"] + "").GetDecimalValue();
            CollectedAmount5 += (dt.Rows[i]["CollectedAmount5"] + "").GetDecimalValue();
            RecordedAmount5 += (dt.Rows[i]["RecordedAmount5"] + "").GetDecimalValue();
            TotalPointRecorded += (dt.Rows[i]["PointRecorded"] + "").GetDecimalValue();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }
    protected void cldStartDate_SelectionChanged(object sender, EventArgs e)
    {
        cbUseStartDate.Checked = true;
    }
    protected void cldEndDate_SelectionChanged(object sender, EventArgs e)
    {
        cbUseEndDate.Checked = true;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        cbUseStartDate.Checked = false;
        cbUseEndDate.Checked = false;
        OutletDropdownList.ResetDdlOutlet();
        OutletDropdownList.ResetDdlPOS();
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtCashier.Text = "";
        txtSupervisor.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddDept.WhereField = "DepartmentID";
            ddDept.WhereValue = Session["DeptID"].ToString();
        }
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        //if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        //{
        //    ddPOS.WhereField = "DepartmentID";
        //    ddPOS.WhereValue = Session["DeptID"].ToString();
        //}  
    }
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        ShowEditor(false);
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        ViewState["sortBy"] = "ENDTIME";
        ViewState[SORT_DIRECTION] = "DESC";

        DataSet ds = new DataSet();
        ds.ReadXml(Server.MapPath("~/PaymentTypes.xml"));
        PaymentTypes = ds.Tables[0];


        BindGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string ID = lblCounterClose.Text;
            QueryCommandCollection col = new QueryCommandCollection();
            decimal TotalCollected = 0;

            CounterCloseLog ccl = new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, ID);
            if (ccl == null)
                throw new Exception("Closing with ID:" + ID + " doesn't exist");

            string updateCashIn = String.Format("Update CounterCloseLog set CashIn = {0}  where CounterCloseID= '{1}'", txtCashIn.Text.GetDecimalValue(), ID);
            col.Add(new QueryCommand(updateCashIn));
            //TotalCollected += txtCashIn.Text.GetDecimalValue();

            string updateCashOut = String.Format("Update CounterCloseLog set CashOut = {0}  where CounterCloseID= '{1}'", txtCashOut.Text.GetDecimalValue(), ID);
            col.Add(new QueryCommand(updateCashOut));
            //TotalCollected -= txtCashOut.Text.GetDecimalValue();

            string updateDepositBag = String.Format("Update CounterCloseLog set DepositBagNo = '{0}'  where CounterCloseID= '{1}'", txtDepositBag.Text, ID);
            col.Add(new QueryCommand(updateDepositBag));

            string updateCash = String.Format("Update CounterCloseLog set CashCollected = {0}  where CounterCloseID= '{1}'", txtCash.Text.GetDecimalValue(), ID);
            col.Add(new QueryCommand(updateCash));
            TotalCollected += txtCash.Text.GetDecimalValue();

            if (enableNETSATMCard)
            {
                string updateNetsATMCardCollected = String.Format("Update CounterCloseLog set NetsATMCardCollected = {0}  where CounterCloseID= '{1}'", txtNetsAtmCard.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateNetsATMCardCollected));
            }

            if (enableNETSCashCard)
            {
                string updateNETSCashCard = String.Format("Update CounterCloseLog set NetsCashCardCollected = {0}  where CounterCloseID= '{1}'", txtNetsCashCard.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateNETSCashCard));
            }

            if (enableNETSFlashPay)
            {
                string updateNetsFlashPay = String.Format("Update CounterCloseLog set NetsFlashPayCollected = {0}  where CounterCloseID= '{1}'", txtNetsFlashPay.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateNetsFlashPay));
            }

            if (enableNetsIntegration)
            {
                decimal totalNets = txtNetsAtmCard.Text.GetDecimalValue() + txtNetsCashCard.Text.GetDecimalValue() + txtNetsFlashPay.Text.GetDecimalValue();
                string updateNets = String.Format("Update CounterCloseLog set NetsCollected = {0}  where CounterCloseID= '{1}'", totalNets, ID);
                col.Add(new QueryCommand(updateNets));
                TotalCollected += totalNets;
            }

            string tmp = FetchPaymentByID("1");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment1 = String.Format("Update CounterCloseLog set NetsCollected = {0}  where CounterCloseID= '{1}'", txtPayment1.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment1));
                    TotalCollected += txtPayment1.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("2");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment2 = String.Format("Update CounterCloseLog set VisaCollected = {0}  where CounterCloseID= '{1}'", txtPayment2.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment2));
                    TotalCollected += txtPayment2.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("3");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment3 = String.Format("Update CounterCloseLog set ChinaNetsCollected = {0}  where CounterCloseID= '{1}'", txtPayment3.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment3));
                    TotalCollected += txtPayment3.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("4");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment4 = String.Format("Update CounterCloseLog set AmexCollected = {0}  where CounterCloseID= '{1}'", txtPayment4.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment4));
                    TotalCollected += txtPayment4.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("5");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment5 = String.Format("Update CounterCloseLog set Userfld4 = '{0}' where CounterCloseID= '{1}'", txtPayment5.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment5));
                    TotalCollected += txtPayment5.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("6");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment6 = String.Format("Update CounterCloseLog set Userfld6 = '{0}'  where CounterCloseID= '{1}'", txtPayment6.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment6));
                    TotalCollected += txtPayment6.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("7");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment7 = String.Format("Update CounterCloseLog set Pay7Collected = {0} where CounterCloseID= '{1}'", txtPayment7.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment7));
                    TotalCollected += txtPayment7.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("8");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment8 = String.Format("Update CounterCloseLog set Pay8Collected = {0} where CounterCloseID = '{1}'", txtPayment8.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment8));
                    TotalCollected += txtPayment8.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("9");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment9 = String.Format("Update CounterCloseLog set Pay9Collected = {0} where CounterCloseID = '{1}'", txtPayment9.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment9));
                    TotalCollected += txtPayment9.Text.GetDecimalValue();
                }
            }

            tmp = FetchPaymentByID("10");
            if (tmp != "" && tmp != "-" && tmp.ToUpper() != "POINTS" && tmp.ToUpper() != "INSTALLMENT")
            {
                if (((tmp.ToUpper() != "NETS" && tmp.ToUpper() != "NETSX") || !enableNetsIntegration))
                {
                    string updatePayment10 = String.Format("Update CounterCloseLog set Pay10Collected = {0} where CounterCloseID = '{1}'", txtPayment10.Text.GetDecimalValue(), ID);
                    col.Add(new QueryCommand(updatePayment10));
                    TotalCollected += txtPayment10.Text.GetDecimalValue();
                }
            }


            if (PaymentTypes.Select("Name = 'Voucher'").Length == 0)
            {
                string updateVoucher = String.Format("Update CounterCloseLog set VoucherCollected = {0} where CounterCloseID = '{1}'", txtVoucher.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateVoucher));
                TotalCollected += txtVoucher.Text.GetDecimalValue();
            }

            if (ccl.ChequeRecorded != 0)
            {
                string updateCheque = String.Format("Update CounterCloseLog set Userfloat2 = {0} where CounterCloseID = '{1}'", txtCheque.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateCheque));
                TotalCollected += txtCheque.Text.GetDecimalValue();
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency1))
            {
                string updateForeignCurrency1 = String.Format("Update CounterCloseLog set ForeignCurrency1Collected = {0} where CounterCloseID = '{1}'", txtForeignCurrency1.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateForeignCurrency1));
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency2))
            {
                string updateForeignCurrency2 = String.Format("Update CounterCloseLog set ForeignCurrency2Collected = {0} where CounterCloseID = '{1}'", txtForeignCurrency2.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateForeignCurrency2));
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency3))
            {
                string updateForeignCurrency3 = String.Format("Update CounterCloseLog set ForeignCurrency3Collected = {0} where CounterCloseID = '{1}'", txtForeignCurrency3.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateForeignCurrency3));
            }


            if (!string.IsNullOrEmpty(ccl.ForeignCurrency4))
            {
                string updateForeignCurrency4 = String.Format("Update CounterCloseLog set ForeignCurrency4Collected = {0} where CounterCloseID = '{1}'", txtForeignCurrency4.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateForeignCurrency4));
            }

            if (!string.IsNullOrEmpty(ccl.ForeignCurrency5))
            {
                string updateForeignCurrency5 = String.Format("Update CounterCloseLog set ForeignCurrency5Collected = {0} where CounterCloseID = '{1}'", txtForeignCurrency5.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateForeignCurrency5));
            }

            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enableSMF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            bool enablePAMed = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);

            if (enableFunding && enableSMF)
            {
                string updateSMF = String.Format("Update CounterCloseLog set Userfloat7 = {0} where CounterCloseID = '{1}'", txtSMFMed.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updateSMF));
                TotalCollected += txtSMFMed.Text.GetDecimalValue();
            }

            if (enableFunding && enablePAMed)
            {
                string updatePAMed = String.Format("Update CounterCloseLog set Userfloat9 = {0} where CounterCloseID = '{1}'", txtPAMed.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updatePAMed));
                TotalCollected += txtPAMed.Text.GetDecimalValue();
            }

            if (enableFunding && enablePWF)
            {
                string updatePWF = String.Format("Update CounterCloseLog set Userfloat11 = {0} where CounterCloseID = '{1}'", txtPWF.Text.GetDecimalValue(), ID);
                col.Add(new QueryCommand(updatePWF));
                TotalCollected += txtPWF.Text.GetDecimalValue();
            }
            TotalCollected -= ccl.OpeningBalance;
            string updateModifiedBy = String.Format("Update CounterCloseLog set ModifiedBy = '{0}', TotalActualCollected = {2}, Variance = {2} - TotalSystemRecorded where CounterCloseID = '{1}'", Session["UserName"].ToString(), ID, TotalCollected);
            col.Add(new QueryCommand(updateModifiedBy));

            if (col.Count > 0)
                DataService.ExecuteTransaction(col);

            AccessLogController.AddAccessLog(AccessSource.WEB, Session["UserName"].ToString(), "-", string.Format("Update CounterCloseLog with ID : '{0}'", ID), "");

            lblResult.Text = string.Format("Closing with ID : '{0}' is saved", ID);
            LoadEditor(ID);
        }
        catch (Exception ex)
        {
            Logger.writeLog("Error Update Counter Close Log: " + ex.Message);
            lblResult.Text = "Error Update Counter Close Log: " + ex.Message;
        }
    }


}
