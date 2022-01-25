<%@ Page Language="C#" AutoEventWireup="true" Inherits="WebSample.EditorPages.CreateBuilding" CodeBehind="CreateBuilding.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Create Building</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 600px;">
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 2px; margin-right: 2px;">
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 1px; margin-right: 1px;">
        </div>
        <div style="background-color: #C3D9FF; padding: 10px; font-family: Arial, Sans-Serif;
            font-size: 12px;">
            <asp:LinkButton runat="server" ID="lbBack" Text="<< Back to calendar"></asp:LinkButton>
            <asp:Button runat="server" ID="btnSave" Text="Save" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" />
            <asp:Button runat="server" ID="btnDelete" Text="Delete" />
            <br />
            <asp:Label runat="server" ID="lbError" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
            <br />
            <div style="background-color: White; padding: 10px;">
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 2px; margin-right: 2px;">
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 1px; margin-right: 1px;">
                </div>
                <div style="background-color: #D2E6D2;">
                    <table cellpadding="5" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <b>Building Name</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbBuildingName" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Address Line 1</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddrLine1" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Address Line 2</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddrLine2" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>City</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbCity" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Country </b>
                            </td>
                            <td style="border-bottom: solid 2px #E0F1E0; padding: 3px;">
                                <asp:DropDownList runat="server" ID="ddCountry" Width="300px" AutoPostBack="false" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px">
                                    <asp:ListItem>Afghanistan</asp:ListItem><asp:ListItem>Aring land Islands</asp:ListItem><asp:ListItem>Albania</asp:ListItem><asp:ListItem>Algeria</asp:ListItem><asp:ListItem>American Samoa</asp:ListItem><asp:ListItem>Andorra</asp:ListItem><asp:ListItem>Angola</asp:ListItem><asp:ListItem>Anguilla</asp:ListItem><asp:ListItem>Antarctica</asp:ListItem><asp:ListItem>Antigua and Barbuda</asp:ListItem><asp:ListItem>Argentina</asp:ListItem><asp:ListItem>Armenia</asp:ListItem><asp:ListItem>Aruba Australia</asp:ListItem><asp:ListItem>Austria</asp:ListItem><asp:ListItem>Azerbaijan</asp:ListItem><asp:ListItem>Bahamas</asp:ListItem><asp:ListItem>Bahrain</asp:ListItem><asp:ListItem>Bangladesh</asp:ListItem><asp:ListItem>Barbados</asp:ListItem><asp:ListItem>Belarus</asp:ListItem><asp:ListItem>Belgium</asp:ListItem><asp:ListItem>Belize</asp:ListItem><asp:ListItem>Benin</asp:ListItem><asp:ListItem>Bermuda</asp:ListItem><asp:ListItem>Bhutan</asp:ListItem><asp:ListItem>Bolivia</asp:ListItem><asp:ListItem>Bosnia and Herzegovina</asp:ListItem><asp:ListItem>Botswana</asp:ListItem><asp:ListItem>Bouvet Island</asp:ListItem><asp:ListItem>Brazil</asp:ListItem><asp:ListItem>British Indian Ocean territory</asp:ListItem><asp:ListItem>Brunei Darussalam</asp:ListItem><asp:ListItem>Bulgaria</asp:ListItem><asp:ListItem>Burkina Faso</asp:ListItem><asp:ListItem>Burundi</asp:ListItem><asp:ListItem>Cambodia</asp:ListItem><asp:ListItem>Cameroon</asp:ListItem><asp:ListItem>Canada</asp:ListItem><asp:ListItem>Cape Verde</asp:ListItem><asp:ListItem>Cayman Islands</asp:ListItem><asp:ListItem>Central African Republic</asp:ListItem><asp:ListItem>Chad</asp:ListItem><asp:ListItem>Chile</asp:ListItem><asp:ListItem>China</asp:ListItem><asp:ListItem>Christmas Island</asp:ListItem><asp:ListItem>Cocos (Keeling) Islands</asp:ListItem><asp:ListItem>Colombia</asp:ListItem><asp:ListItem>Comoros</asp:ListItem><asp:ListItem>Congo</asp:ListItem><asp:ListItem>Congo, Democratic Republic</asp:ListItem><asp:ListItem>Cook Islands</asp:ListItem><asp:ListItem>Costa Rica</asp:ListItem><asp:ListItem></asp:ListItem><asp:ListItem>Croatia (Hrvatska)</asp:ListItem><asp:ListItem>Cuba</asp:ListItem><asp:ListItem>Cyprus</asp:ListItem><asp:ListItem>Czech Republic</asp:ListItem><asp:ListItem>Denmark</asp:ListItem><asp:ListItem>Djibouti</asp:ListItem><asp:ListItem>Dominica</asp:ListItem><asp:ListItem>Dominican Republic</asp:ListItem><asp:ListItem>East Timor</asp:ListItem><asp:ListItem>Ecuador</asp:ListItem><asp:ListItem>Egypt</asp:ListItem><asp:ListItem>El Salvador</asp:ListItem><asp:ListItem>Equatorial Guinea</asp:ListItem><asp:ListItem>Eritrea</asp:ListItem><asp:ListItem>Estonia</asp:ListItem><asp:ListItem>Ethiopia</asp:ListItem><asp:ListItem>Falkland Islands</asp:ListItem><asp:ListItem>Faroe Islands</asp:ListItem><asp:ListItem>Fiji</asp:ListItem><asp:ListItem>Finland</asp:ListItem><asp:ListItem>France</asp:ListItem><asp:ListItem>French Guiana</asp:ListItem><asp:ListItem>French Polynesia</asp:ListItem><asp:ListItem>French Southern Territories</asp:ListItem><asp:ListItem>Gabon</asp:ListItem><asp:ListItem>Gambia</asp:ListItem><asp:ListItem>Georgia</asp:ListItem><asp:ListItem>Germany</asp:ListItem><asp:ListItem>Ghana</asp:ListItem><asp:ListItem>Gibraltar</asp:ListItem><asp:ListItem>Greece</asp:ListItem><asp:ListItem>Greenland</asp:ListItem><asp:ListItem>Grenada</asp:ListItem><asp:ListItem>Guadeloupe</asp:ListItem><asp:ListItem>Guam</asp:ListItem><asp:ListItem>Guatemala</asp:ListItem><asp:ListItem>Guinea</asp:ListItem><asp:ListItem>Guinea-Bissau</asp:ListItem><asp:ListItem>Guyana</asp:ListItem><asp:ListItem>Haiti</asp:ListItem><asp:ListItem>Heard and McDonald Islands</asp:ListItem><asp:ListItem>Honduras</asp:ListItem><asp:ListItem>Hong Kong</asp:ListItem><asp:ListItem>Hungary</asp:ListItem><asp:ListItem>Iceland</asp:ListItem><asp:ListItem>India</asp:ListItem><asp:ListItem>Indonesia Iran</asp:ListItem><asp:ListItem>Iraq</asp:ListItem><asp:ListItem>Ireland</asp:ListItem><asp:ListItem>Israel</asp:ListItem><asp:ListItem>Italy</asp:ListItem><asp:ListItem>Jamaica</asp:ListItem><asp:ListItem>Japan</asp:ListItem><asp:ListItem>Jordan</asp:ListItem><asp:ListItem>Kazakhstan</asp:ListItem><asp:ListItem>Kenya</asp:ListItem><asp:ListItem>Kiribati</asp:ListItem><asp:ListItem>Korea (north)</asp:ListItem><asp:ListItem>Korea (south)</asp:ListItem><asp:ListItem>Kuwait</asp:ListItem><asp:ListItem>Kyrgyzstan</asp:ListItem><asp:ListItem>Lao People's Democratic Republic</asp:ListItem><asp:ListItem>Latvia</asp:ListItem><asp:ListItem>Lebanon</asp:ListItem><asp:ListItem>Lesotho</asp:ListItem><asp:ListItem>Liberia</asp:ListItem><asp:ListItem>Libyan Arab Jamahiriya</asp:ListItem><asp:ListItem>Liechtenstein</asp:ListItem><asp:ListItem>Lithuania</asp:ListItem><asp:ListItem>Luxembourg</asp:ListItem><asp:ListItem>Macao</asp:ListItem><asp:ListItem>Macedonia, Former Yugoslav Republic Of Madagascar</asp:ListItem><asp:ListItem>Malawi</asp:ListItem><asp:ListItem>Malaysia</asp:ListItem><asp:ListItem>Maldives</asp:ListItem><asp:ListItem>Mali</asp:ListItem><asp:ListItem>Malta</asp:ListItem><asp:ListItem>Marshall Islands</asp:ListItem><asp:ListItem>Martinique</asp:ListItem><asp:ListItem>Mauritania</asp:ListItem><asp:ListItem>Mauritius</asp:ListItem><asp:ListItem>Mayotte</asp:ListItem><asp:ListItem>Mexico</asp:ListItem><asp:ListItem>Micronesia</asp:ListItem><asp:ListItem>Moldova</asp:ListItem><asp:ListItem>Monaco</asp:ListItem><asp:ListItem>Mongolia</asp:ListItem><asp:ListItem>Montserrat</asp:ListItem><asp:ListItem>Morocco</asp:ListItem><asp:ListItem>Mozambique</asp:ListItem><asp:ListItem>Myanmar</asp:ListItem><asp:ListItem>Namibia</asp:ListItem><asp:ListItem>Nauru</asp:ListItem><asp:ListItem>Nepal</asp:ListItem><asp:ListItem>Netherlands</asp:ListItem><asp:ListItem>Netherlands Antilles</asp:ListItem><asp:ListItem>New Caledonia</asp:ListItem><asp:ListItem>New Zealand</asp:ListItem><asp:ListItem>Nicaragua</asp:ListItem><asp:ListItem>Niger</asp:ListItem><asp:ListItem>Nigeria</asp:ListItem><asp:ListItem>Niue</asp:ListItem><asp:ListItem>Norfolk Island</asp:ListItem><asp:ListItem>Northern Mariana Islands</asp:ListItem><asp:ListItem>Norway</asp:ListItem><asp:ListItem>Oman</asp:ListItem><asp:ListItem>Pakistan</asp:ListItem><asp:ListItem>Palau</asp:ListItem><asp:ListItem>Palestinian Territories</asp:ListItem><asp:ListItem>Panama</asp:ListItem><asp:ListItem>Papua New Guinea</asp:ListItem><asp:ListItem>Paraguay</asp:ListItem><asp:ListItem>Peru</asp:ListItem><asp:ListItem>Philippines</asp:ListItem><asp:ListItem>Pitcairn</asp:ListItem><asp:ListItem>Poland</asp:ListItem><asp:ListItem>Portugal</asp:ListItem><asp:ListItem>Puerto Rico</asp:ListItem><asp:ListItem>Qatar</asp:ListItem><asp:ListItem>R&eacute;union</asp:ListItem><asp:ListItem>Romania</asp:ListItem><asp:ListItem>Russian Federation</asp:ListItem><asp:ListItem>Rwanda</asp:ListItem><asp:ListItem>Saint Helena</asp:ListItem><asp:ListItem>Saint Kitts and Nevis</asp:ListItem><asp:ListItem>Saint Lucia</asp:ListItem><asp:ListItem>Saint Pierre and Miquelon</asp:ListItem><asp:ListItem>Saint Vincent and the Grenadines</asp:ListItem><asp:ListItem>Samoa</asp:ListItem><asp:ListItem>San Marino</asp:ListItem><asp:ListItem>Sao Tome and Principe Saudi Arabia</asp:ListItem><asp:ListItem>Senegal</asp:ListItem><asp:ListItem>Serbia and Montenegro</asp:ListItem><asp:ListItem>Seychelles</asp:ListItem><asp:ListItem>Sierra Leone</asp:ListItem>
                                    <asp:ListItem Selected="True">Singapore</asp:ListItem><asp:ListItem>Slovakia</asp:ListItem><asp:ListItem>Slovenia</asp:ListItem><asp:ListItem>Solomon Islands</asp:ListItem><asp:ListItem>Somalia</asp:ListItem><asp:ListItem>South Africa</asp:ListItem><asp:ListItem>South Georgia and the South Sandwich Islands</asp:ListItem><asp:ListItem>Spain</asp:ListItem><asp:ListItem>Sri Lanka</asp:ListItem><asp:ListItem>Sudan</asp:ListItem><asp:ListItem>Suriname</asp:ListItem><asp:ListItem>Svalbard and Jan Mayen Islands</asp:ListItem><asp:ListItem>Swaziland</asp:ListItem><asp:ListItem>Sweden</asp:ListItem><asp:ListItem>Switzerland</asp:ListItem><asp:ListItem>Syria</asp:ListItem><asp:ListItem>Taiwan</asp:ListItem><asp:ListItem>Tajikistan</asp:ListItem><asp:ListItem>Tanzania</asp:ListItem><asp:ListItem>Thailand</asp:ListItem><asp:ListItem>Togo</asp:ListItem><asp:ListItem>Tokelau</asp:ListItem><asp:ListItem>Tonga</asp:ListItem><asp:ListItem>Trinidad and Tobago</asp:ListItem><asp:ListItem>Tunisia</asp:ListItem><asp:ListItem>Turkey</asp:ListItem><asp:ListItem>Turkmenistan</asp:ListItem><asp:ListItem>Turks and Caicos Islands</asp:ListItem><asp:ListItem>Tuvalu</asp:ListItem><asp:ListItem>Uganda</asp:ListItem><asp:ListItem>Ukraine</asp:ListItem><asp:ListItem>United Arab Emirates</asp:ListItem><asp:ListItem>United Kingdom</asp:ListItem><asp:ListItem>United States of America</asp:ListItem><asp:ListItem>Uruguay</asp:ListItem><asp:ListItem>Uzbekistan</asp:ListItem><asp:ListItem>Vanuatu</asp:ListItem><asp:ListItem>Vatican City</asp:ListItem><asp:ListItem>Venezuela</asp:ListItem><asp:ListItem>Vietnam</asp:ListItem><asp:ListItem>Virgin Islands (British)</asp:ListItem><asp:ListItem>Virgin Islands (US)</asp:ListItem><asp:ListItem>Wallis and Futuna Islands</asp:ListItem><asp:ListItem>Western Sahara</asp:ListItem><asp:ListItem>Yemen</asp:ListItem><asp:ListItem>Zaire</asp:ListItem><asp:ListItem>Zambia</asp:ListItem><asp:ListItem>Zimbabwe</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Pin-Code</b>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPinCode" Width="300px" Font-Names="Arial, Sans-Serif"
                                    Font-Size="12px"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        
                    </table>
                    <asp:GridView ID="grdBuilding" runat="server" AutoGenerateEditButton="True" PageSize="5" 
                        ShowFooter="True" Width="393px" onrowediting="grdBuilding_RowEditing">
                                </asp:GridView>
                    <tr runat="server" id="trRecEndType">
                        <td style="border-bottom: solid 2px #E0F1E0; padding: 3px;">
                        </td>
                    </tr>
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 1px; margin-right: 1px;">
                </div>
                <div style="font-size: 1px; line-height: 1px; background-color: #D2E6D2; height: 1px;
                    margin-left: 2px; margin-right: 2px;">
                </div>
            </div>
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 1px; margin-right: 1px;">
        </div>
        <div style="font-size: 1px; line-height: 1px; background-color: #C3D9FF; height: 1px;
            margin-left: 2px; margin-right: 2px;">
        </div>
    </div>
    </form>
</body>
</html>
