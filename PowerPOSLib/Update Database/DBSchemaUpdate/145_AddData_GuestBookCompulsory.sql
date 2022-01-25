IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Header')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark, Label)
	VALUES('Header', 0, 1,'','PARENT''S PARTICULARS')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Surname')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Surname', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'GivenName')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('GivenName', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'NRIC')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('NRIC', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'HomeAddress')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('HomeAddress', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'ContactNo')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('ContactNo', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Email')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Email', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'PassCode')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('PassCode', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'ConfirmPassCode')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('ConfirmPassCode', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'IsLifeTimeMember')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('IsLifeTimeMember', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Child1GivenName')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Child1GivenName', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Child1DateBirth')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Child1DateBirth', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Child2GivenName')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Child2GivenName', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Child2DateBirth')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Child2DateBirth', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'HearAboutUs')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('HearAboutUs', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkMagazines')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkMagazines', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkOnlineSearch')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkOnlineSearch', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkOnlineMedia')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkOnlineMedia', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkFriends')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkFriends', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkOther')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkOther', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'StatementAgreement')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('StatementAgreement', 1, 1,'I have read and agree to abide by the Play Rules of Clover Beauty Wellness during all visits to Clover Beauty Wellness.\n I understand and agree that all information provided may be used by Clover Beauty Wellness for its operational and marketing purposes.\n To the extent permitted by law, I shall not hold Clover Beauty Wellness, its directors, staff and contractors liable for any injuries, damages or death resulting from the use of its services and equipment at Clover Beauty Wellness. I shall indemnify Clover Beauty Wellness, its directors, staff and contractors against any loss of or damage to any property or injury or death to any person as a result of a wilful or negligent act by my child or I during any visit to Clover Beauty Wellness.\n')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'chkAgreement')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('chkAgreement', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Nationality')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Nationality', 1, 1,'')
END

IF  NOT EXISTS (SELECT * FROM GuestBookCompulsory WHERE FieldName = 'Remark')
BEGIN
	INSERT INTO GuestBookCompulsory(FieldName, IsCompulsory, IsVisible, Remark)
	VALUES('Remark', 1, 1,'')
END
