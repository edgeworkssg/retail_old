IF  NOT EXISTS (SELECT * FROM userprivilege WHERE PrivilegeName = 'Outlet Group')
	INSERT INTO UserPrivilege(PrivilegeName, FormName, CreatedBy, CreatedOn, Modifiedby, ModifiedOn, Deleted) Values
	('Outlet Group','OutletGroup.aspx', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE(), 0)	
	
IF  NOT EXISTS (SELECT * FROM userprivilege WHERE PrivilegeName = 'Setting')
	INSERT INTO UserPrivilege(PrivilegeName, FormName, CreatedBy, CreatedOn, Modifiedby, ModifiedOn, Deleted) Values
	('Setting','AppSettingForm.aspx', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE(), 0)	

IF  NOT EXISTS (SELECT * FROM userprivilege WHERE PrivilegeName = 'Freeze POS')
	INSERT INTO UserPrivilege(PrivilegeName, FormName, CreatedBy, CreatedOn, Modifiedby, ModifiedOn, Deleted) Values
	('Freeze POS','FreezePOS.aspx', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE(), 0)	
	
IF  NOT EXISTS (SELECT * FROM userprivilege WHERE PrivilegeName = 'Dashboard Setup')
	INSERT INTO UserPrivilege(PrivilegeName, FormName, CreatedBy, CreatedOn, Modifiedby, ModifiedOn, Deleted) Values
	('Dashboard Setup','DashboardSetup.aspx', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE(), 0)	
	
IF  NOT EXISTS (SELECT * FROM userprivilege WHERE PrivilegeName = 'Edit Counter Close Log')
	INSERT INTO UserPrivilege(PrivilegeName, FormName, CreatedBy, CreatedOn, Modifiedby, ModifiedOn, Deleted, userfld1) Values
	('Edit Counter Close Log','CounterCloseLogReport.aspx', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE(), 0, '01. EQUIPWEB-REPORTS')	