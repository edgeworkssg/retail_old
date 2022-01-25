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
	/// <summary>
	/// Strongly-typed collection for the TextLanguage class.
	/// </summary>
    [Serializable]
	public partial class TextLanguageCollection : ActiveList<TextLanguage, TextLanguageCollection>
	{	   
		public TextLanguageCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>TextLanguageCollection</returns>
		public TextLanguageCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                TextLanguage o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the TEXT_LANGUAGE table.
	/// </summary>
	[Serializable]
	public partial class TextLanguage : ActiveRecord<TextLanguage>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public TextLanguage()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public TextLanguage(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public TextLanguage(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public TextLanguage(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("TEXT_LANGUAGE", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.String;
				colvarId.MaxLength = 50;
				colvarId.AutoIncrement = false;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarEng = new TableSchema.TableColumn(schema);
				colvarEng.ColumnName = "ENG";
				colvarEng.DataType = DbType.String;
				colvarEng.MaxLength = 100;
				colvarEng.AutoIncrement = false;
				colvarEng.IsNullable = true;
				colvarEng.IsPrimaryKey = false;
				colvarEng.IsForeignKey = false;
				colvarEng.IsReadOnly = false;
				colvarEng.DefaultSetting = @"";
				colvarEng.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEng);
				
				TableSchema.TableColumn colvarChs = new TableSchema.TableColumn(schema);
				colvarChs.ColumnName = "CHS";
				colvarChs.DataType = DbType.String;
				colvarChs.MaxLength = 100;
				colvarChs.AutoIncrement = false;
				colvarChs.IsNullable = true;
				colvarChs.IsPrimaryKey = false;
				colvarChs.IsForeignKey = false;
				colvarChs.IsReadOnly = false;
				colvarChs.DefaultSetting = @"";
				colvarChs.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChs);
				
				TableSchema.TableColumn colvarL1 = new TableSchema.TableColumn(schema);
				colvarL1.ColumnName = "L1";
				colvarL1.DataType = DbType.String;
				colvarL1.MaxLength = 100;
				colvarL1.AutoIncrement = false;
				colvarL1.IsNullable = true;
				colvarL1.IsPrimaryKey = false;
				colvarL1.IsForeignKey = false;
				colvarL1.IsReadOnly = false;
				colvarL1.DefaultSetting = @"";
				colvarL1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL1);
				
				TableSchema.TableColumn colvarL2 = new TableSchema.TableColumn(schema);
				colvarL2.ColumnName = "L2";
				colvarL2.DataType = DbType.String;
				colvarL2.MaxLength = 100;
				colvarL2.AutoIncrement = false;
				colvarL2.IsNullable = true;
				colvarL2.IsPrimaryKey = false;
				colvarL2.IsForeignKey = false;
				colvarL2.IsReadOnly = false;
				colvarL2.DefaultSetting = @"";
				colvarL2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL2);
				
				TableSchema.TableColumn colvarL3 = new TableSchema.TableColumn(schema);
				colvarL3.ColumnName = "L3";
				colvarL3.DataType = DbType.String;
				colvarL3.MaxLength = 100;
				colvarL3.AutoIncrement = false;
				colvarL3.IsNullable = true;
				colvarL3.IsPrimaryKey = false;
				colvarL3.IsForeignKey = false;
				colvarL3.IsReadOnly = false;
				colvarL3.DefaultSetting = @"";
				colvarL3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL3);
				
				TableSchema.TableColumn colvarL4 = new TableSchema.TableColumn(schema);
				colvarL4.ColumnName = "L4";
				colvarL4.DataType = DbType.String;
				colvarL4.MaxLength = 100;
				colvarL4.AutoIncrement = false;
				colvarL4.IsNullable = true;
				colvarL4.IsPrimaryKey = false;
				colvarL4.IsForeignKey = false;
				colvarL4.IsReadOnly = false;
				colvarL4.DefaultSetting = @"";
				colvarL4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL4);
				
				TableSchema.TableColumn colvarL5 = new TableSchema.TableColumn(schema);
				colvarL5.ColumnName = "L5";
				colvarL5.DataType = DbType.String;
				colvarL5.MaxLength = 100;
				colvarL5.AutoIncrement = false;
				colvarL5.IsNullable = true;
				colvarL5.IsPrimaryKey = false;
				colvarL5.IsForeignKey = false;
				colvarL5.IsReadOnly = false;
				colvarL5.DefaultSetting = @"";
				colvarL5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL5);
				
				TableSchema.TableColumn colvarL6 = new TableSchema.TableColumn(schema);
				colvarL6.ColumnName = "L6";
				colvarL6.DataType = DbType.String;
				colvarL6.MaxLength = 100;
				colvarL6.AutoIncrement = false;
				colvarL6.IsNullable = true;
				colvarL6.IsPrimaryKey = false;
				colvarL6.IsForeignKey = false;
				colvarL6.IsReadOnly = false;
				colvarL6.DefaultSetting = @"";
				colvarL6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL6);
				
				TableSchema.TableColumn colvarL7 = new TableSchema.TableColumn(schema);
				colvarL7.ColumnName = "L7";
				colvarL7.DataType = DbType.String;
				colvarL7.MaxLength = 100;
				colvarL7.AutoIncrement = false;
				colvarL7.IsNullable = true;
				colvarL7.IsPrimaryKey = false;
				colvarL7.IsForeignKey = false;
				colvarL7.IsReadOnly = false;
				colvarL7.DefaultSetting = @"";
				colvarL7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL7);
				
				TableSchema.TableColumn colvarL8 = new TableSchema.TableColumn(schema);
				colvarL8.ColumnName = "L8";
				colvarL8.DataType = DbType.String;
				colvarL8.MaxLength = 100;
				colvarL8.AutoIncrement = false;
				colvarL8.IsNullable = true;
				colvarL8.IsPrimaryKey = false;
				colvarL8.IsForeignKey = false;
				colvarL8.IsReadOnly = false;
				colvarL8.DefaultSetting = @"";
				colvarL8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL8);
				
				TableSchema.TableColumn colvarL9 = new TableSchema.TableColumn(schema);
				colvarL9.ColumnName = "L9";
				colvarL9.DataType = DbType.String;
				colvarL9.MaxLength = 100;
				colvarL9.AutoIncrement = false;
				colvarL9.IsNullable = true;
				colvarL9.IsPrimaryKey = false;
				colvarL9.IsForeignKey = false;
				colvarL9.IsReadOnly = false;
				colvarL9.DefaultSetting = @"";
				colvarL9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL9);
				
				TableSchema.TableColumn colvarL10 = new TableSchema.TableColumn(schema);
				colvarL10.ColumnName = "L10";
				colvarL10.DataType = DbType.String;
				colvarL10.MaxLength = 100;
				colvarL10.AutoIncrement = false;
				colvarL10.IsNullable = true;
				colvarL10.IsPrimaryKey = false;
				colvarL10.IsForeignKey = false;
				colvarL10.IsReadOnly = false;
				colvarL10.DefaultSetting = @"";
				colvarL10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL10);
				
				TableSchema.TableColumn colvarL11 = new TableSchema.TableColumn(schema);
				colvarL11.ColumnName = "L11";
				colvarL11.DataType = DbType.String;
				colvarL11.MaxLength = 100;
				colvarL11.AutoIncrement = false;
				colvarL11.IsNullable = true;
				colvarL11.IsPrimaryKey = false;
				colvarL11.IsForeignKey = false;
				colvarL11.IsReadOnly = false;
				colvarL11.DefaultSetting = @"";
				colvarL11.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL11);
				
				TableSchema.TableColumn colvarL12 = new TableSchema.TableColumn(schema);
				colvarL12.ColumnName = "L12";
				colvarL12.DataType = DbType.String;
				colvarL12.MaxLength = 100;
				colvarL12.AutoIncrement = false;
				colvarL12.IsNullable = true;
				colvarL12.IsPrimaryKey = false;
				colvarL12.IsForeignKey = false;
				colvarL12.IsReadOnly = false;
				colvarL12.DefaultSetting = @"";
				colvarL12.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL12);
				
				TableSchema.TableColumn colvarL13 = new TableSchema.TableColumn(schema);
				colvarL13.ColumnName = "L13";
				colvarL13.DataType = DbType.String;
				colvarL13.MaxLength = 100;
				colvarL13.AutoIncrement = false;
				colvarL13.IsNullable = true;
				colvarL13.IsPrimaryKey = false;
				colvarL13.IsForeignKey = false;
				colvarL13.IsReadOnly = false;
				colvarL13.DefaultSetting = @"";
				colvarL13.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL13);
				
				TableSchema.TableColumn colvarL14 = new TableSchema.TableColumn(schema);
				colvarL14.ColumnName = "L14";
				colvarL14.DataType = DbType.String;
				colvarL14.MaxLength = 100;
				colvarL14.AutoIncrement = false;
				colvarL14.IsNullable = true;
				colvarL14.IsPrimaryKey = false;
				colvarL14.IsForeignKey = false;
				colvarL14.IsReadOnly = false;
				colvarL14.DefaultSetting = @"";
				colvarL14.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL14);
				
				TableSchema.TableColumn colvarL15 = new TableSchema.TableColumn(schema);
				colvarL15.ColumnName = "L15";
				colvarL15.DataType = DbType.String;
				colvarL15.MaxLength = 100;
				colvarL15.AutoIncrement = false;
				colvarL15.IsNullable = true;
				colvarL15.IsPrimaryKey = false;
				colvarL15.IsForeignKey = false;
				colvarL15.IsReadOnly = false;
				colvarL15.DefaultSetting = @"";
				colvarL15.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL15);
				
				TableSchema.TableColumn colvarL16 = new TableSchema.TableColumn(schema);
				colvarL16.ColumnName = "L16";
				colvarL16.DataType = DbType.String;
				colvarL16.MaxLength = 100;
				colvarL16.AutoIncrement = false;
				colvarL16.IsNullable = true;
				colvarL16.IsPrimaryKey = false;
				colvarL16.IsForeignKey = false;
				colvarL16.IsReadOnly = false;
				colvarL16.DefaultSetting = @"";
				colvarL16.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL16);
				
				TableSchema.TableColumn colvarL17 = new TableSchema.TableColumn(schema);
				colvarL17.ColumnName = "L17";
				colvarL17.DataType = DbType.String;
				colvarL17.MaxLength = 100;
				colvarL17.AutoIncrement = false;
				colvarL17.IsNullable = true;
				colvarL17.IsPrimaryKey = false;
				colvarL17.IsForeignKey = false;
				colvarL17.IsReadOnly = false;
				colvarL17.DefaultSetting = @"";
				colvarL17.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL17);
				
				TableSchema.TableColumn colvarL18 = new TableSchema.TableColumn(schema);
				colvarL18.ColumnName = "L18";
				colvarL18.DataType = DbType.String;
				colvarL18.MaxLength = 100;
				colvarL18.AutoIncrement = false;
				colvarL18.IsNullable = true;
				colvarL18.IsPrimaryKey = false;
				colvarL18.IsForeignKey = false;
				colvarL18.IsReadOnly = false;
				colvarL18.DefaultSetting = @"";
				colvarL18.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL18);
				
				TableSchema.TableColumn colvarL19 = new TableSchema.TableColumn(schema);
				colvarL19.ColumnName = "L19";
				colvarL19.DataType = DbType.String;
				colvarL19.MaxLength = 100;
				colvarL19.AutoIncrement = false;
				colvarL19.IsNullable = true;
				colvarL19.IsPrimaryKey = false;
				colvarL19.IsForeignKey = false;
				colvarL19.IsReadOnly = false;
				colvarL19.DefaultSetting = @"";
				colvarL19.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL19);
				
				TableSchema.TableColumn colvarL20 = new TableSchema.TableColumn(schema);
				colvarL20.ColumnName = "L20";
				colvarL20.DataType = DbType.String;
				colvarL20.MaxLength = 100;
				colvarL20.AutoIncrement = false;
				colvarL20.IsNullable = true;
				colvarL20.IsPrimaryKey = false;
				colvarL20.IsForeignKey = false;
				colvarL20.IsReadOnly = false;
				colvarL20.DefaultSetting = @"";
				colvarL20.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL20);
				
				TableSchema.TableColumn colvarL21 = new TableSchema.TableColumn(schema);
				colvarL21.ColumnName = "L21";
				colvarL21.DataType = DbType.String;
				colvarL21.MaxLength = 100;
				colvarL21.AutoIncrement = false;
				colvarL21.IsNullable = true;
				colvarL21.IsPrimaryKey = false;
				colvarL21.IsForeignKey = false;
				colvarL21.IsReadOnly = false;
				colvarL21.DefaultSetting = @"";
				colvarL21.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL21);
				
				TableSchema.TableColumn colvarL22 = new TableSchema.TableColumn(schema);
				colvarL22.ColumnName = "L22";
				colvarL22.DataType = DbType.String;
				colvarL22.MaxLength = 100;
				colvarL22.AutoIncrement = false;
				colvarL22.IsNullable = true;
				colvarL22.IsPrimaryKey = false;
				colvarL22.IsForeignKey = false;
				colvarL22.IsReadOnly = false;
				colvarL22.DefaultSetting = @"";
				colvarL22.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL22);
				
				TableSchema.TableColumn colvarL23 = new TableSchema.TableColumn(schema);
				colvarL23.ColumnName = "L23";
				colvarL23.DataType = DbType.String;
				colvarL23.MaxLength = 100;
				colvarL23.AutoIncrement = false;
				colvarL23.IsNullable = true;
				colvarL23.IsPrimaryKey = false;
				colvarL23.IsForeignKey = false;
				colvarL23.IsReadOnly = false;
				colvarL23.DefaultSetting = @"";
				colvarL23.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL23);
				
				TableSchema.TableColumn colvarL24 = new TableSchema.TableColumn(schema);
				colvarL24.ColumnName = "L24";
				colvarL24.DataType = DbType.String;
				colvarL24.MaxLength = 100;
				colvarL24.AutoIncrement = false;
				colvarL24.IsNullable = true;
				colvarL24.IsPrimaryKey = false;
				colvarL24.IsForeignKey = false;
				colvarL24.IsReadOnly = false;
				colvarL24.DefaultSetting = @"";
				colvarL24.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL24);
				
				TableSchema.TableColumn colvarL25 = new TableSchema.TableColumn(schema);
				colvarL25.ColumnName = "L25";
				colvarL25.DataType = DbType.String;
				colvarL25.MaxLength = 100;
				colvarL25.AutoIncrement = false;
				colvarL25.IsNullable = true;
				colvarL25.IsPrimaryKey = false;
				colvarL25.IsForeignKey = false;
				colvarL25.IsReadOnly = false;
				colvarL25.DefaultSetting = @"";
				colvarL25.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL25);
				
				TableSchema.TableColumn colvarL26 = new TableSchema.TableColumn(schema);
				colvarL26.ColumnName = "L26";
				colvarL26.DataType = DbType.String;
				colvarL26.MaxLength = 100;
				colvarL26.AutoIncrement = false;
				colvarL26.IsNullable = true;
				colvarL26.IsPrimaryKey = false;
				colvarL26.IsForeignKey = false;
				colvarL26.IsReadOnly = false;
				colvarL26.DefaultSetting = @"";
				colvarL26.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL26);
				
				TableSchema.TableColumn colvarL27 = new TableSchema.TableColumn(schema);
				colvarL27.ColumnName = "L27";
				colvarL27.DataType = DbType.String;
				colvarL27.MaxLength = 100;
				colvarL27.AutoIncrement = false;
				colvarL27.IsNullable = true;
				colvarL27.IsPrimaryKey = false;
				colvarL27.IsForeignKey = false;
				colvarL27.IsReadOnly = false;
				colvarL27.DefaultSetting = @"";
				colvarL27.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL27);
				
				TableSchema.TableColumn colvarL28 = new TableSchema.TableColumn(schema);
				colvarL28.ColumnName = "L28";
				colvarL28.DataType = DbType.String;
				colvarL28.MaxLength = 100;
				colvarL28.AutoIncrement = false;
				colvarL28.IsNullable = true;
				colvarL28.IsPrimaryKey = false;
				colvarL28.IsForeignKey = false;
				colvarL28.IsReadOnly = false;
				colvarL28.DefaultSetting = @"";
				colvarL28.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL28);
				
				TableSchema.TableColumn colvarL29 = new TableSchema.TableColumn(schema);
				colvarL29.ColumnName = "L29";
				colvarL29.DataType = DbType.String;
				colvarL29.MaxLength = 100;
				colvarL29.AutoIncrement = false;
				colvarL29.IsNullable = true;
				colvarL29.IsPrimaryKey = false;
				colvarL29.IsForeignKey = false;
				colvarL29.IsReadOnly = false;
				colvarL29.DefaultSetting = @"";
				colvarL29.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL29);
				
				TableSchema.TableColumn colvarL30 = new TableSchema.TableColumn(schema);
				colvarL30.ColumnName = "L30";
				colvarL30.DataType = DbType.String;
				colvarL30.MaxLength = 100;
				colvarL30.AutoIncrement = false;
				colvarL30.IsNullable = true;
				colvarL30.IsPrimaryKey = false;
				colvarL30.IsForeignKey = false;
				colvarL30.IsReadOnly = false;
				colvarL30.DefaultSetting = @"";
				colvarL30.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL30);
				
				TableSchema.TableColumn colvarL31 = new TableSchema.TableColumn(schema);
				colvarL31.ColumnName = "L31";
				colvarL31.DataType = DbType.String;
				colvarL31.MaxLength = 100;
				colvarL31.AutoIncrement = false;
				colvarL31.IsNullable = true;
				colvarL31.IsPrimaryKey = false;
				colvarL31.IsForeignKey = false;
				colvarL31.IsReadOnly = false;
				colvarL31.DefaultSetting = @"";
				colvarL31.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL31);
				
				TableSchema.TableColumn colvarL32 = new TableSchema.TableColumn(schema);
				colvarL32.ColumnName = "L32";
				colvarL32.DataType = DbType.String;
				colvarL32.MaxLength = 100;
				colvarL32.AutoIncrement = false;
				colvarL32.IsNullable = true;
				colvarL32.IsPrimaryKey = false;
				colvarL32.IsForeignKey = false;
				colvarL32.IsReadOnly = false;
				colvarL32.DefaultSetting = @"";
				colvarL32.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL32);
				
				TableSchema.TableColumn colvarL33 = new TableSchema.TableColumn(schema);
				colvarL33.ColumnName = "L33";
				colvarL33.DataType = DbType.String;
				colvarL33.MaxLength = 100;
				colvarL33.AutoIncrement = false;
				colvarL33.IsNullable = true;
				colvarL33.IsPrimaryKey = false;
				colvarL33.IsForeignKey = false;
				colvarL33.IsReadOnly = false;
				colvarL33.DefaultSetting = @"";
				colvarL33.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL33);
				
				TableSchema.TableColumn colvarL34 = new TableSchema.TableColumn(schema);
				colvarL34.ColumnName = "L34";
				colvarL34.DataType = DbType.String;
				colvarL34.MaxLength = 100;
				colvarL34.AutoIncrement = false;
				colvarL34.IsNullable = true;
				colvarL34.IsPrimaryKey = false;
				colvarL34.IsForeignKey = false;
				colvarL34.IsReadOnly = false;
				colvarL34.DefaultSetting = @"";
				colvarL34.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL34);
				
				TableSchema.TableColumn colvarL35 = new TableSchema.TableColumn(schema);
				colvarL35.ColumnName = "L35";
				colvarL35.DataType = DbType.String;
				colvarL35.MaxLength = 100;
				colvarL35.AutoIncrement = false;
				colvarL35.IsNullable = true;
				colvarL35.IsPrimaryKey = false;
				colvarL35.IsForeignKey = false;
				colvarL35.IsReadOnly = false;
				colvarL35.DefaultSetting = @"";
				colvarL35.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL35);
				
				TableSchema.TableColumn colvarL36 = new TableSchema.TableColumn(schema);
				colvarL36.ColumnName = "L36";
				colvarL36.DataType = DbType.String;
				colvarL36.MaxLength = 100;
				colvarL36.AutoIncrement = false;
				colvarL36.IsNullable = true;
				colvarL36.IsPrimaryKey = false;
				colvarL36.IsForeignKey = false;
				colvarL36.IsReadOnly = false;
				colvarL36.DefaultSetting = @"";
				colvarL36.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL36);
				
				TableSchema.TableColumn colvarL37 = new TableSchema.TableColumn(schema);
				colvarL37.ColumnName = "L37";
				colvarL37.DataType = DbType.String;
				colvarL37.MaxLength = 100;
				colvarL37.AutoIncrement = false;
				colvarL37.IsNullable = true;
				colvarL37.IsPrimaryKey = false;
				colvarL37.IsForeignKey = false;
				colvarL37.IsReadOnly = false;
				colvarL37.DefaultSetting = @"";
				colvarL37.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL37);
				
				TableSchema.TableColumn colvarL38 = new TableSchema.TableColumn(schema);
				colvarL38.ColumnName = "L38";
				colvarL38.DataType = DbType.String;
				colvarL38.MaxLength = 100;
				colvarL38.AutoIncrement = false;
				colvarL38.IsNullable = true;
				colvarL38.IsPrimaryKey = false;
				colvarL38.IsForeignKey = false;
				colvarL38.IsReadOnly = false;
				colvarL38.DefaultSetting = @"";
				colvarL38.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL38);
				
				TableSchema.TableColumn colvarL39 = new TableSchema.TableColumn(schema);
				colvarL39.ColumnName = "L39";
				colvarL39.DataType = DbType.String;
				colvarL39.MaxLength = 100;
				colvarL39.AutoIncrement = false;
				colvarL39.IsNullable = true;
				colvarL39.IsPrimaryKey = false;
				colvarL39.IsForeignKey = false;
				colvarL39.IsReadOnly = false;
				colvarL39.DefaultSetting = @"";
				colvarL39.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL39);
				
				TableSchema.TableColumn colvarL40 = new TableSchema.TableColumn(schema);
				colvarL40.ColumnName = "L40";
				colvarL40.DataType = DbType.String;
				colvarL40.MaxLength = 100;
				colvarL40.AutoIncrement = false;
				colvarL40.IsNullable = true;
				colvarL40.IsPrimaryKey = false;
				colvarL40.IsForeignKey = false;
				colvarL40.IsReadOnly = false;
				colvarL40.DefaultSetting = @"";
				colvarL40.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL40);
				
				TableSchema.TableColumn colvarL41 = new TableSchema.TableColumn(schema);
				colvarL41.ColumnName = "L41";
				colvarL41.DataType = DbType.String;
				colvarL41.MaxLength = 100;
				colvarL41.AutoIncrement = false;
				colvarL41.IsNullable = true;
				colvarL41.IsPrimaryKey = false;
				colvarL41.IsForeignKey = false;
				colvarL41.IsReadOnly = false;
				colvarL41.DefaultSetting = @"";
				colvarL41.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL41);
				
				TableSchema.TableColumn colvarL42 = new TableSchema.TableColumn(schema);
				colvarL42.ColumnName = "L42";
				colvarL42.DataType = DbType.String;
				colvarL42.MaxLength = 100;
				colvarL42.AutoIncrement = false;
				colvarL42.IsNullable = true;
				colvarL42.IsPrimaryKey = false;
				colvarL42.IsForeignKey = false;
				colvarL42.IsReadOnly = false;
				colvarL42.DefaultSetting = @"";
				colvarL42.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL42);
				
				TableSchema.TableColumn colvarL43 = new TableSchema.TableColumn(schema);
				colvarL43.ColumnName = "L43";
				colvarL43.DataType = DbType.String;
				colvarL43.MaxLength = 100;
				colvarL43.AutoIncrement = false;
				colvarL43.IsNullable = true;
				colvarL43.IsPrimaryKey = false;
				colvarL43.IsForeignKey = false;
				colvarL43.IsReadOnly = false;
				colvarL43.DefaultSetting = @"";
				colvarL43.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL43);
				
				TableSchema.TableColumn colvarL44 = new TableSchema.TableColumn(schema);
				colvarL44.ColumnName = "L44";
				colvarL44.DataType = DbType.String;
				colvarL44.MaxLength = 100;
				colvarL44.AutoIncrement = false;
				colvarL44.IsNullable = true;
				colvarL44.IsPrimaryKey = false;
				colvarL44.IsForeignKey = false;
				colvarL44.IsReadOnly = false;
				colvarL44.DefaultSetting = @"";
				colvarL44.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL44);
				
				TableSchema.TableColumn colvarL45 = new TableSchema.TableColumn(schema);
				colvarL45.ColumnName = "L45";
				colvarL45.DataType = DbType.String;
				colvarL45.MaxLength = 100;
				colvarL45.AutoIncrement = false;
				colvarL45.IsNullable = true;
				colvarL45.IsPrimaryKey = false;
				colvarL45.IsForeignKey = false;
				colvarL45.IsReadOnly = false;
				colvarL45.DefaultSetting = @"";
				colvarL45.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL45);
				
				TableSchema.TableColumn colvarL46 = new TableSchema.TableColumn(schema);
				colvarL46.ColumnName = "L46";
				colvarL46.DataType = DbType.String;
				colvarL46.MaxLength = 100;
				colvarL46.AutoIncrement = false;
				colvarL46.IsNullable = true;
				colvarL46.IsPrimaryKey = false;
				colvarL46.IsForeignKey = false;
				colvarL46.IsReadOnly = false;
				colvarL46.DefaultSetting = @"";
				colvarL46.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL46);
				
				TableSchema.TableColumn colvarL47 = new TableSchema.TableColumn(schema);
				colvarL47.ColumnName = "L47";
				colvarL47.DataType = DbType.String;
				colvarL47.MaxLength = 100;
				colvarL47.AutoIncrement = false;
				colvarL47.IsNullable = true;
				colvarL47.IsPrimaryKey = false;
				colvarL47.IsForeignKey = false;
				colvarL47.IsReadOnly = false;
				colvarL47.DefaultSetting = @"";
				colvarL47.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL47);
				
				TableSchema.TableColumn colvarL48 = new TableSchema.TableColumn(schema);
				colvarL48.ColumnName = "L48";
				colvarL48.DataType = DbType.String;
				colvarL48.MaxLength = 100;
				colvarL48.AutoIncrement = false;
				colvarL48.IsNullable = true;
				colvarL48.IsPrimaryKey = false;
				colvarL48.IsForeignKey = false;
				colvarL48.IsReadOnly = false;
				colvarL48.DefaultSetting = @"";
				colvarL48.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL48);
				
				TableSchema.TableColumn colvarL49 = new TableSchema.TableColumn(schema);
				colvarL49.ColumnName = "L49";
				colvarL49.DataType = DbType.String;
				colvarL49.MaxLength = 100;
				colvarL49.AutoIncrement = false;
				colvarL49.IsNullable = true;
				colvarL49.IsPrimaryKey = false;
				colvarL49.IsForeignKey = false;
				colvarL49.IsReadOnly = false;
				colvarL49.DefaultSetting = @"";
				colvarL49.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL49);
				
				TableSchema.TableColumn colvarL50 = new TableSchema.TableColumn(schema);
				colvarL50.ColumnName = "L50";
				colvarL50.DataType = DbType.String;
				colvarL50.MaxLength = 100;
				colvarL50.AutoIncrement = false;
				colvarL50.IsNullable = true;
				colvarL50.IsPrimaryKey = false;
				colvarL50.IsForeignKey = false;
				colvarL50.IsReadOnly = false;
				colvarL50.DefaultSetting = @"";
				colvarL50.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL50);
				
				TableSchema.TableColumn colvarL51 = new TableSchema.TableColumn(schema);
				colvarL51.ColumnName = "L51";
				colvarL51.DataType = DbType.String;
				colvarL51.MaxLength = 100;
				colvarL51.AutoIncrement = false;
				colvarL51.IsNullable = true;
				colvarL51.IsPrimaryKey = false;
				colvarL51.IsForeignKey = false;
				colvarL51.IsReadOnly = false;
				colvarL51.DefaultSetting = @"";
				colvarL51.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL51);
				
				TableSchema.TableColumn colvarL52 = new TableSchema.TableColumn(schema);
				colvarL52.ColumnName = "L52";
				colvarL52.DataType = DbType.String;
				colvarL52.MaxLength = 100;
				colvarL52.AutoIncrement = false;
				colvarL52.IsNullable = true;
				colvarL52.IsPrimaryKey = false;
				colvarL52.IsForeignKey = false;
				colvarL52.IsReadOnly = false;
				colvarL52.DefaultSetting = @"";
				colvarL52.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL52);
				
				TableSchema.TableColumn colvarL53 = new TableSchema.TableColumn(schema);
				colvarL53.ColumnName = "L53";
				colvarL53.DataType = DbType.String;
				colvarL53.MaxLength = 100;
				colvarL53.AutoIncrement = false;
				colvarL53.IsNullable = true;
				colvarL53.IsPrimaryKey = false;
				colvarL53.IsForeignKey = false;
				colvarL53.IsReadOnly = false;
				colvarL53.DefaultSetting = @"";
				colvarL53.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL53);
				
				TableSchema.TableColumn colvarL54 = new TableSchema.TableColumn(schema);
				colvarL54.ColumnName = "L54";
				colvarL54.DataType = DbType.String;
				colvarL54.MaxLength = 100;
				colvarL54.AutoIncrement = false;
				colvarL54.IsNullable = true;
				colvarL54.IsPrimaryKey = false;
				colvarL54.IsForeignKey = false;
				colvarL54.IsReadOnly = false;
				colvarL54.DefaultSetting = @"";
				colvarL54.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL54);
				
				TableSchema.TableColumn colvarL55 = new TableSchema.TableColumn(schema);
				colvarL55.ColumnName = "L55";
				colvarL55.DataType = DbType.String;
				colvarL55.MaxLength = 100;
				colvarL55.AutoIncrement = false;
				colvarL55.IsNullable = true;
				colvarL55.IsPrimaryKey = false;
				colvarL55.IsForeignKey = false;
				colvarL55.IsReadOnly = false;
				colvarL55.DefaultSetting = @"";
				colvarL55.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL55);
				
				TableSchema.TableColumn colvarL56 = new TableSchema.TableColumn(schema);
				colvarL56.ColumnName = "L56";
				colvarL56.DataType = DbType.String;
				colvarL56.MaxLength = 100;
				colvarL56.AutoIncrement = false;
				colvarL56.IsNullable = true;
				colvarL56.IsPrimaryKey = false;
				colvarL56.IsForeignKey = false;
				colvarL56.IsReadOnly = false;
				colvarL56.DefaultSetting = @"";
				colvarL56.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL56);
				
				TableSchema.TableColumn colvarL57 = new TableSchema.TableColumn(schema);
				colvarL57.ColumnName = "L57";
				colvarL57.DataType = DbType.String;
				colvarL57.MaxLength = 100;
				colvarL57.AutoIncrement = false;
				colvarL57.IsNullable = true;
				colvarL57.IsPrimaryKey = false;
				colvarL57.IsForeignKey = false;
				colvarL57.IsReadOnly = false;
				colvarL57.DefaultSetting = @"";
				colvarL57.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL57);
				
				TableSchema.TableColumn colvarL58 = new TableSchema.TableColumn(schema);
				colvarL58.ColumnName = "L58";
				colvarL58.DataType = DbType.String;
				colvarL58.MaxLength = 100;
				colvarL58.AutoIncrement = false;
				colvarL58.IsNullable = true;
				colvarL58.IsPrimaryKey = false;
				colvarL58.IsForeignKey = false;
				colvarL58.IsReadOnly = false;
				colvarL58.DefaultSetting = @"";
				colvarL58.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL58);
				
				TableSchema.TableColumn colvarL59 = new TableSchema.TableColumn(schema);
				colvarL59.ColumnName = "L59";
				colvarL59.DataType = DbType.String;
				colvarL59.MaxLength = 100;
				colvarL59.AutoIncrement = false;
				colvarL59.IsNullable = true;
				colvarL59.IsPrimaryKey = false;
				colvarL59.IsForeignKey = false;
				colvarL59.IsReadOnly = false;
				colvarL59.DefaultSetting = @"";
				colvarL59.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL59);
				
				TableSchema.TableColumn colvarL60 = new TableSchema.TableColumn(schema);
				colvarL60.ColumnName = "L60";
				colvarL60.DataType = DbType.String;
				colvarL60.MaxLength = 100;
				colvarL60.AutoIncrement = false;
				colvarL60.IsNullable = true;
				colvarL60.IsPrimaryKey = false;
				colvarL60.IsForeignKey = false;
				colvarL60.IsReadOnly = false;
				colvarL60.DefaultSetting = @"";
				colvarL60.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL60);
				
				TableSchema.TableColumn colvarL61 = new TableSchema.TableColumn(schema);
				colvarL61.ColumnName = "L61";
				colvarL61.DataType = DbType.String;
				colvarL61.MaxLength = 100;
				colvarL61.AutoIncrement = false;
				colvarL61.IsNullable = true;
				colvarL61.IsPrimaryKey = false;
				colvarL61.IsForeignKey = false;
				colvarL61.IsReadOnly = false;
				colvarL61.DefaultSetting = @"";
				colvarL61.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL61);
				
				TableSchema.TableColumn colvarL62 = new TableSchema.TableColumn(schema);
				colvarL62.ColumnName = "L62";
				colvarL62.DataType = DbType.String;
				colvarL62.MaxLength = 100;
				colvarL62.AutoIncrement = false;
				colvarL62.IsNullable = true;
				colvarL62.IsPrimaryKey = false;
				colvarL62.IsForeignKey = false;
				colvarL62.IsReadOnly = false;
				colvarL62.DefaultSetting = @"";
				colvarL62.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL62);
				
				TableSchema.TableColumn colvarL63 = new TableSchema.TableColumn(schema);
				colvarL63.ColumnName = "L63";
				colvarL63.DataType = DbType.String;
				colvarL63.MaxLength = 100;
				colvarL63.AutoIncrement = false;
				colvarL63.IsNullable = true;
				colvarL63.IsPrimaryKey = false;
				colvarL63.IsForeignKey = false;
				colvarL63.IsReadOnly = false;
				colvarL63.DefaultSetting = @"";
				colvarL63.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL63);
				
				TableSchema.TableColumn colvarL64 = new TableSchema.TableColumn(schema);
				colvarL64.ColumnName = "L64";
				colvarL64.DataType = DbType.String;
				colvarL64.MaxLength = 100;
				colvarL64.AutoIncrement = false;
				colvarL64.IsNullable = true;
				colvarL64.IsPrimaryKey = false;
				colvarL64.IsForeignKey = false;
				colvarL64.IsReadOnly = false;
				colvarL64.DefaultSetting = @"";
				colvarL64.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL64);
				
				TableSchema.TableColumn colvarL65 = new TableSchema.TableColumn(schema);
				colvarL65.ColumnName = "L65";
				colvarL65.DataType = DbType.String;
				colvarL65.MaxLength = 100;
				colvarL65.AutoIncrement = false;
				colvarL65.IsNullable = true;
				colvarL65.IsPrimaryKey = false;
				colvarL65.IsForeignKey = false;
				colvarL65.IsReadOnly = false;
				colvarL65.DefaultSetting = @"";
				colvarL65.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL65);
				
				TableSchema.TableColumn colvarL66 = new TableSchema.TableColumn(schema);
				colvarL66.ColumnName = "L66";
				colvarL66.DataType = DbType.String;
				colvarL66.MaxLength = 100;
				colvarL66.AutoIncrement = false;
				colvarL66.IsNullable = true;
				colvarL66.IsPrimaryKey = false;
				colvarL66.IsForeignKey = false;
				colvarL66.IsReadOnly = false;
				colvarL66.DefaultSetting = @"";
				colvarL66.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL66);
				
				TableSchema.TableColumn colvarL67 = new TableSchema.TableColumn(schema);
				colvarL67.ColumnName = "L67";
				colvarL67.DataType = DbType.String;
				colvarL67.MaxLength = 100;
				colvarL67.AutoIncrement = false;
				colvarL67.IsNullable = true;
				colvarL67.IsPrimaryKey = false;
				colvarL67.IsForeignKey = false;
				colvarL67.IsReadOnly = false;
				colvarL67.DefaultSetting = @"";
				colvarL67.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL67);
				
				TableSchema.TableColumn colvarL68 = new TableSchema.TableColumn(schema);
				colvarL68.ColumnName = "L68";
				colvarL68.DataType = DbType.String;
				colvarL68.MaxLength = 100;
				colvarL68.AutoIncrement = false;
				colvarL68.IsNullable = true;
				colvarL68.IsPrimaryKey = false;
				colvarL68.IsForeignKey = false;
				colvarL68.IsReadOnly = false;
				colvarL68.DefaultSetting = @"";
				colvarL68.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL68);
				
				TableSchema.TableColumn colvarL69 = new TableSchema.TableColumn(schema);
				colvarL69.ColumnName = "L69";
				colvarL69.DataType = DbType.String;
				colvarL69.MaxLength = 100;
				colvarL69.AutoIncrement = false;
				colvarL69.IsNullable = true;
				colvarL69.IsPrimaryKey = false;
				colvarL69.IsForeignKey = false;
				colvarL69.IsReadOnly = false;
				colvarL69.DefaultSetting = @"";
				colvarL69.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL69);
				
				TableSchema.TableColumn colvarL70 = new TableSchema.TableColumn(schema);
				colvarL70.ColumnName = "L70";
				colvarL70.DataType = DbType.String;
				colvarL70.MaxLength = 100;
				colvarL70.AutoIncrement = false;
				colvarL70.IsNullable = true;
				colvarL70.IsPrimaryKey = false;
				colvarL70.IsForeignKey = false;
				colvarL70.IsReadOnly = false;
				colvarL70.DefaultSetting = @"";
				colvarL70.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL70);
				
				TableSchema.TableColumn colvarL71 = new TableSchema.TableColumn(schema);
				colvarL71.ColumnName = "L71";
				colvarL71.DataType = DbType.String;
				colvarL71.MaxLength = 100;
				colvarL71.AutoIncrement = false;
				colvarL71.IsNullable = true;
				colvarL71.IsPrimaryKey = false;
				colvarL71.IsForeignKey = false;
				colvarL71.IsReadOnly = false;
				colvarL71.DefaultSetting = @"";
				colvarL71.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL71);
				
				TableSchema.TableColumn colvarL72 = new TableSchema.TableColumn(schema);
				colvarL72.ColumnName = "L72";
				colvarL72.DataType = DbType.String;
				colvarL72.MaxLength = 100;
				colvarL72.AutoIncrement = false;
				colvarL72.IsNullable = true;
				colvarL72.IsPrimaryKey = false;
				colvarL72.IsForeignKey = false;
				colvarL72.IsReadOnly = false;
				colvarL72.DefaultSetting = @"";
				colvarL72.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL72);
				
				TableSchema.TableColumn colvarL73 = new TableSchema.TableColumn(schema);
				colvarL73.ColumnName = "L73";
				colvarL73.DataType = DbType.String;
				colvarL73.MaxLength = 100;
				colvarL73.AutoIncrement = false;
				colvarL73.IsNullable = true;
				colvarL73.IsPrimaryKey = false;
				colvarL73.IsForeignKey = false;
				colvarL73.IsReadOnly = false;
				colvarL73.DefaultSetting = @"";
				colvarL73.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL73);
				
				TableSchema.TableColumn colvarL74 = new TableSchema.TableColumn(schema);
				colvarL74.ColumnName = "L74";
				colvarL74.DataType = DbType.String;
				colvarL74.MaxLength = 100;
				colvarL74.AutoIncrement = false;
				colvarL74.IsNullable = true;
				colvarL74.IsPrimaryKey = false;
				colvarL74.IsForeignKey = false;
				colvarL74.IsReadOnly = false;
				colvarL74.DefaultSetting = @"";
				colvarL74.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL74);
				
				TableSchema.TableColumn colvarL75 = new TableSchema.TableColumn(schema);
				colvarL75.ColumnName = "L75";
				colvarL75.DataType = DbType.String;
				colvarL75.MaxLength = 100;
				colvarL75.AutoIncrement = false;
				colvarL75.IsNullable = true;
				colvarL75.IsPrimaryKey = false;
				colvarL75.IsForeignKey = false;
				colvarL75.IsReadOnly = false;
				colvarL75.DefaultSetting = @"";
				colvarL75.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL75);
				
				TableSchema.TableColumn colvarL76 = new TableSchema.TableColumn(schema);
				colvarL76.ColumnName = "L76";
				colvarL76.DataType = DbType.String;
				colvarL76.MaxLength = 100;
				colvarL76.AutoIncrement = false;
				colvarL76.IsNullable = true;
				colvarL76.IsPrimaryKey = false;
				colvarL76.IsForeignKey = false;
				colvarL76.IsReadOnly = false;
				colvarL76.DefaultSetting = @"";
				colvarL76.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL76);
				
				TableSchema.TableColumn colvarL77 = new TableSchema.TableColumn(schema);
				colvarL77.ColumnName = "L77";
				colvarL77.DataType = DbType.String;
				colvarL77.MaxLength = 100;
				colvarL77.AutoIncrement = false;
				colvarL77.IsNullable = true;
				colvarL77.IsPrimaryKey = false;
				colvarL77.IsForeignKey = false;
				colvarL77.IsReadOnly = false;
				colvarL77.DefaultSetting = @"";
				colvarL77.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL77);
				
				TableSchema.TableColumn colvarL78 = new TableSchema.TableColumn(schema);
				colvarL78.ColumnName = "L78";
				colvarL78.DataType = DbType.String;
				colvarL78.MaxLength = 100;
				colvarL78.AutoIncrement = false;
				colvarL78.IsNullable = true;
				colvarL78.IsPrimaryKey = false;
				colvarL78.IsForeignKey = false;
				colvarL78.IsReadOnly = false;
				colvarL78.DefaultSetting = @"";
				colvarL78.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL78);
				
				TableSchema.TableColumn colvarL79 = new TableSchema.TableColumn(schema);
				colvarL79.ColumnName = "L79";
				colvarL79.DataType = DbType.String;
				colvarL79.MaxLength = 100;
				colvarL79.AutoIncrement = false;
				colvarL79.IsNullable = true;
				colvarL79.IsPrimaryKey = false;
				colvarL79.IsForeignKey = false;
				colvarL79.IsReadOnly = false;
				colvarL79.DefaultSetting = @"";
				colvarL79.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL79);
				
				TableSchema.TableColumn colvarL80 = new TableSchema.TableColumn(schema);
				colvarL80.ColumnName = "L80";
				colvarL80.DataType = DbType.String;
				colvarL80.MaxLength = 100;
				colvarL80.AutoIncrement = false;
				colvarL80.IsNullable = true;
				colvarL80.IsPrimaryKey = false;
				colvarL80.IsForeignKey = false;
				colvarL80.IsReadOnly = false;
				colvarL80.DefaultSetting = @"";
				colvarL80.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL80);
				
				TableSchema.TableColumn colvarL81 = new TableSchema.TableColumn(schema);
				colvarL81.ColumnName = "L81";
				colvarL81.DataType = DbType.String;
				colvarL81.MaxLength = 100;
				colvarL81.AutoIncrement = false;
				colvarL81.IsNullable = true;
				colvarL81.IsPrimaryKey = false;
				colvarL81.IsForeignKey = false;
				colvarL81.IsReadOnly = false;
				colvarL81.DefaultSetting = @"";
				colvarL81.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL81);
				
				TableSchema.TableColumn colvarL82 = new TableSchema.TableColumn(schema);
				colvarL82.ColumnName = "L82";
				colvarL82.DataType = DbType.String;
				colvarL82.MaxLength = 100;
				colvarL82.AutoIncrement = false;
				colvarL82.IsNullable = true;
				colvarL82.IsPrimaryKey = false;
				colvarL82.IsForeignKey = false;
				colvarL82.IsReadOnly = false;
				colvarL82.DefaultSetting = @"";
				colvarL82.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL82);
				
				TableSchema.TableColumn colvarL83 = new TableSchema.TableColumn(schema);
				colvarL83.ColumnName = "L83";
				colvarL83.DataType = DbType.String;
				colvarL83.MaxLength = 100;
				colvarL83.AutoIncrement = false;
				colvarL83.IsNullable = true;
				colvarL83.IsPrimaryKey = false;
				colvarL83.IsForeignKey = false;
				colvarL83.IsReadOnly = false;
				colvarL83.DefaultSetting = @"";
				colvarL83.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL83);
				
				TableSchema.TableColumn colvarL84 = new TableSchema.TableColumn(schema);
				colvarL84.ColumnName = "L84";
				colvarL84.DataType = DbType.String;
				colvarL84.MaxLength = 100;
				colvarL84.AutoIncrement = false;
				colvarL84.IsNullable = true;
				colvarL84.IsPrimaryKey = false;
				colvarL84.IsForeignKey = false;
				colvarL84.IsReadOnly = false;
				colvarL84.DefaultSetting = @"";
				colvarL84.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL84);
				
				TableSchema.TableColumn colvarL85 = new TableSchema.TableColumn(schema);
				colvarL85.ColumnName = "L85";
				colvarL85.DataType = DbType.String;
				colvarL85.MaxLength = 100;
				colvarL85.AutoIncrement = false;
				colvarL85.IsNullable = true;
				colvarL85.IsPrimaryKey = false;
				colvarL85.IsForeignKey = false;
				colvarL85.IsReadOnly = false;
				colvarL85.DefaultSetting = @"";
				colvarL85.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL85);
				
				TableSchema.TableColumn colvarL86 = new TableSchema.TableColumn(schema);
				colvarL86.ColumnName = "L86";
				colvarL86.DataType = DbType.String;
				colvarL86.MaxLength = 100;
				colvarL86.AutoIncrement = false;
				colvarL86.IsNullable = true;
				colvarL86.IsPrimaryKey = false;
				colvarL86.IsForeignKey = false;
				colvarL86.IsReadOnly = false;
				colvarL86.DefaultSetting = @"";
				colvarL86.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL86);
				
				TableSchema.TableColumn colvarL87 = new TableSchema.TableColumn(schema);
				colvarL87.ColumnName = "L87";
				colvarL87.DataType = DbType.String;
				colvarL87.MaxLength = 100;
				colvarL87.AutoIncrement = false;
				colvarL87.IsNullable = true;
				colvarL87.IsPrimaryKey = false;
				colvarL87.IsForeignKey = false;
				colvarL87.IsReadOnly = false;
				colvarL87.DefaultSetting = @"";
				colvarL87.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL87);
				
				TableSchema.TableColumn colvarL88 = new TableSchema.TableColumn(schema);
				colvarL88.ColumnName = "L88";
				colvarL88.DataType = DbType.String;
				colvarL88.MaxLength = 100;
				colvarL88.AutoIncrement = false;
				colvarL88.IsNullable = true;
				colvarL88.IsPrimaryKey = false;
				colvarL88.IsForeignKey = false;
				colvarL88.IsReadOnly = false;
				colvarL88.DefaultSetting = @"";
				colvarL88.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL88);
				
				TableSchema.TableColumn colvarL89 = new TableSchema.TableColumn(schema);
				colvarL89.ColumnName = "L89";
				colvarL89.DataType = DbType.String;
				colvarL89.MaxLength = 100;
				colvarL89.AutoIncrement = false;
				colvarL89.IsNullable = true;
				colvarL89.IsPrimaryKey = false;
				colvarL89.IsForeignKey = false;
				colvarL89.IsReadOnly = false;
				colvarL89.DefaultSetting = @"";
				colvarL89.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL89);
				
				TableSchema.TableColumn colvarL90 = new TableSchema.TableColumn(schema);
				colvarL90.ColumnName = "L90";
				colvarL90.DataType = DbType.String;
				colvarL90.MaxLength = 100;
				colvarL90.AutoIncrement = false;
				colvarL90.IsNullable = true;
				colvarL90.IsPrimaryKey = false;
				colvarL90.IsForeignKey = false;
				colvarL90.IsReadOnly = false;
				colvarL90.DefaultSetting = @"";
				colvarL90.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL90);
				
				TableSchema.TableColumn colvarL91 = new TableSchema.TableColumn(schema);
				colvarL91.ColumnName = "L91";
				colvarL91.DataType = DbType.String;
				colvarL91.MaxLength = 100;
				colvarL91.AutoIncrement = false;
				colvarL91.IsNullable = true;
				colvarL91.IsPrimaryKey = false;
				colvarL91.IsForeignKey = false;
				colvarL91.IsReadOnly = false;
				colvarL91.DefaultSetting = @"";
				colvarL91.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL91);
				
				TableSchema.TableColumn colvarL92 = new TableSchema.TableColumn(schema);
				colvarL92.ColumnName = "L92";
				colvarL92.DataType = DbType.String;
				colvarL92.MaxLength = 100;
				colvarL92.AutoIncrement = false;
				colvarL92.IsNullable = true;
				colvarL92.IsPrimaryKey = false;
				colvarL92.IsForeignKey = false;
				colvarL92.IsReadOnly = false;
				colvarL92.DefaultSetting = @"";
				colvarL92.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL92);
				
				TableSchema.TableColumn colvarL93 = new TableSchema.TableColumn(schema);
				colvarL93.ColumnName = "L93";
				colvarL93.DataType = DbType.String;
				colvarL93.MaxLength = 100;
				colvarL93.AutoIncrement = false;
				colvarL93.IsNullable = true;
				colvarL93.IsPrimaryKey = false;
				colvarL93.IsForeignKey = false;
				colvarL93.IsReadOnly = false;
				colvarL93.DefaultSetting = @"";
				colvarL93.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL93);
				
				TableSchema.TableColumn colvarL94 = new TableSchema.TableColumn(schema);
				colvarL94.ColumnName = "L94";
				colvarL94.DataType = DbType.String;
				colvarL94.MaxLength = 100;
				colvarL94.AutoIncrement = false;
				colvarL94.IsNullable = true;
				colvarL94.IsPrimaryKey = false;
				colvarL94.IsForeignKey = false;
				colvarL94.IsReadOnly = false;
				colvarL94.DefaultSetting = @"";
				colvarL94.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL94);
				
				TableSchema.TableColumn colvarL95 = new TableSchema.TableColumn(schema);
				colvarL95.ColumnName = "L95";
				colvarL95.DataType = DbType.String;
				colvarL95.MaxLength = 100;
				colvarL95.AutoIncrement = false;
				colvarL95.IsNullable = true;
				colvarL95.IsPrimaryKey = false;
				colvarL95.IsForeignKey = false;
				colvarL95.IsReadOnly = false;
				colvarL95.DefaultSetting = @"";
				colvarL95.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL95);
				
				TableSchema.TableColumn colvarL96 = new TableSchema.TableColumn(schema);
				colvarL96.ColumnName = "L96";
				colvarL96.DataType = DbType.String;
				colvarL96.MaxLength = 100;
				colvarL96.AutoIncrement = false;
				colvarL96.IsNullable = true;
				colvarL96.IsPrimaryKey = false;
				colvarL96.IsForeignKey = false;
				colvarL96.IsReadOnly = false;
				colvarL96.DefaultSetting = @"";
				colvarL96.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL96);
				
				TableSchema.TableColumn colvarL97 = new TableSchema.TableColumn(schema);
				colvarL97.ColumnName = "L97";
				colvarL97.DataType = DbType.String;
				colvarL97.MaxLength = 100;
				colvarL97.AutoIncrement = false;
				colvarL97.IsNullable = true;
				colvarL97.IsPrimaryKey = false;
				colvarL97.IsForeignKey = false;
				colvarL97.IsReadOnly = false;
				colvarL97.DefaultSetting = @"";
				colvarL97.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL97);
				
				TableSchema.TableColumn colvarL98 = new TableSchema.TableColumn(schema);
				colvarL98.ColumnName = "L98";
				colvarL98.DataType = DbType.String;
				colvarL98.MaxLength = 100;
				colvarL98.AutoIncrement = false;
				colvarL98.IsNullable = true;
				colvarL98.IsPrimaryKey = false;
				colvarL98.IsForeignKey = false;
				colvarL98.IsReadOnly = false;
				colvarL98.DefaultSetting = @"";
				colvarL98.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL98);
				
				TableSchema.TableColumn colvarL99 = new TableSchema.TableColumn(schema);
				colvarL99.ColumnName = "L99";
				colvarL99.DataType = DbType.String;
				colvarL99.MaxLength = 100;
				colvarL99.AutoIncrement = false;
				colvarL99.IsNullable = true;
				colvarL99.IsPrimaryKey = false;
				colvarL99.IsForeignKey = false;
				colvarL99.IsReadOnly = false;
				colvarL99.DefaultSetting = @"";
				colvarL99.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL99);
				
				TableSchema.TableColumn colvarL100 = new TableSchema.TableColumn(schema);
				colvarL100.ColumnName = "L100";
				colvarL100.DataType = DbType.String;
				colvarL100.MaxLength = 100;
				colvarL100.AutoIncrement = false;
				colvarL100.IsNullable = true;
				colvarL100.IsPrimaryKey = false;
				colvarL100.IsForeignKey = false;
				colvarL100.IsReadOnly = false;
				colvarL100.DefaultSetting = @"";
				colvarL100.ForeignKeyTableName = "";
				schema.Columns.Add(colvarL100);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("TEXT_LANGUAGE",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public string Id 
		{
			get { return GetColumnValue<string>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("Eng")]
		[Bindable(true)]
		public string Eng 
		{
			get { return GetColumnValue<string>(Columns.Eng); }
			set { SetColumnValue(Columns.Eng, value); }
		}
		  
		[XmlAttribute("Chs")]
		[Bindable(true)]
		public string Chs 
		{
			get { return GetColumnValue<string>(Columns.Chs); }
			set { SetColumnValue(Columns.Chs, value); }
		}
		  
		[XmlAttribute("L1")]
		[Bindable(true)]
		public string L1 
		{
			get { return GetColumnValue<string>(Columns.L1); }
			set { SetColumnValue(Columns.L1, value); }
		}
		  
		[XmlAttribute("L2")]
		[Bindable(true)]
		public string L2 
		{
			get { return GetColumnValue<string>(Columns.L2); }
			set { SetColumnValue(Columns.L2, value); }
		}
		  
		[XmlAttribute("L3")]
		[Bindable(true)]
		public string L3 
		{
			get { return GetColumnValue<string>(Columns.L3); }
			set { SetColumnValue(Columns.L3, value); }
		}
		  
		[XmlAttribute("L4")]
		[Bindable(true)]
		public string L4 
		{
			get { return GetColumnValue<string>(Columns.L4); }
			set { SetColumnValue(Columns.L4, value); }
		}
		  
		[XmlAttribute("L5")]
		[Bindable(true)]
		public string L5 
		{
			get { return GetColumnValue<string>(Columns.L5); }
			set { SetColumnValue(Columns.L5, value); }
		}
		  
		[XmlAttribute("L6")]
		[Bindable(true)]
		public string L6 
		{
			get { return GetColumnValue<string>(Columns.L6); }
			set { SetColumnValue(Columns.L6, value); }
		}
		  
		[XmlAttribute("L7")]
		[Bindable(true)]
		public string L7 
		{
			get { return GetColumnValue<string>(Columns.L7); }
			set { SetColumnValue(Columns.L7, value); }
		}
		  
		[XmlAttribute("L8")]
		[Bindable(true)]
		public string L8 
		{
			get { return GetColumnValue<string>(Columns.L8); }
			set { SetColumnValue(Columns.L8, value); }
		}
		  
		[XmlAttribute("L9")]
		[Bindable(true)]
		public string L9 
		{
			get { return GetColumnValue<string>(Columns.L9); }
			set { SetColumnValue(Columns.L9, value); }
		}
		  
		[XmlAttribute("L10")]
		[Bindable(true)]
		public string L10 
		{
			get { return GetColumnValue<string>(Columns.L10); }
			set { SetColumnValue(Columns.L10, value); }
		}
		  
		[XmlAttribute("L11")]
		[Bindable(true)]
		public string L11 
		{
			get { return GetColumnValue<string>(Columns.L11); }
			set { SetColumnValue(Columns.L11, value); }
		}
		  
		[XmlAttribute("L12")]
		[Bindable(true)]
		public string L12 
		{
			get { return GetColumnValue<string>(Columns.L12); }
			set { SetColumnValue(Columns.L12, value); }
		}
		  
		[XmlAttribute("L13")]
		[Bindable(true)]
		public string L13 
		{
			get { return GetColumnValue<string>(Columns.L13); }
			set { SetColumnValue(Columns.L13, value); }
		}
		  
		[XmlAttribute("L14")]
		[Bindable(true)]
		public string L14 
		{
			get { return GetColumnValue<string>(Columns.L14); }
			set { SetColumnValue(Columns.L14, value); }
		}
		  
		[XmlAttribute("L15")]
		[Bindable(true)]
		public string L15 
		{
			get { return GetColumnValue<string>(Columns.L15); }
			set { SetColumnValue(Columns.L15, value); }
		}
		  
		[XmlAttribute("L16")]
		[Bindable(true)]
		public string L16 
		{
			get { return GetColumnValue<string>(Columns.L16); }
			set { SetColumnValue(Columns.L16, value); }
		}
		  
		[XmlAttribute("L17")]
		[Bindable(true)]
		public string L17 
		{
			get { return GetColumnValue<string>(Columns.L17); }
			set { SetColumnValue(Columns.L17, value); }
		}
		  
		[XmlAttribute("L18")]
		[Bindable(true)]
		public string L18 
		{
			get { return GetColumnValue<string>(Columns.L18); }
			set { SetColumnValue(Columns.L18, value); }
		}
		  
		[XmlAttribute("L19")]
		[Bindable(true)]
		public string L19 
		{
			get { return GetColumnValue<string>(Columns.L19); }
			set { SetColumnValue(Columns.L19, value); }
		}
		  
		[XmlAttribute("L20")]
		[Bindable(true)]
		public string L20 
		{
			get { return GetColumnValue<string>(Columns.L20); }
			set { SetColumnValue(Columns.L20, value); }
		}
		  
		[XmlAttribute("L21")]
		[Bindable(true)]
		public string L21 
		{
			get { return GetColumnValue<string>(Columns.L21); }
			set { SetColumnValue(Columns.L21, value); }
		}
		  
		[XmlAttribute("L22")]
		[Bindable(true)]
		public string L22 
		{
			get { return GetColumnValue<string>(Columns.L22); }
			set { SetColumnValue(Columns.L22, value); }
		}
		  
		[XmlAttribute("L23")]
		[Bindable(true)]
		public string L23 
		{
			get { return GetColumnValue<string>(Columns.L23); }
			set { SetColumnValue(Columns.L23, value); }
		}
		  
		[XmlAttribute("L24")]
		[Bindable(true)]
		public string L24 
		{
			get { return GetColumnValue<string>(Columns.L24); }
			set { SetColumnValue(Columns.L24, value); }
		}
		  
		[XmlAttribute("L25")]
		[Bindable(true)]
		public string L25 
		{
			get { return GetColumnValue<string>(Columns.L25); }
			set { SetColumnValue(Columns.L25, value); }
		}
		  
		[XmlAttribute("L26")]
		[Bindable(true)]
		public string L26 
		{
			get { return GetColumnValue<string>(Columns.L26); }
			set { SetColumnValue(Columns.L26, value); }
		}
		  
		[XmlAttribute("L27")]
		[Bindable(true)]
		public string L27 
		{
			get { return GetColumnValue<string>(Columns.L27); }
			set { SetColumnValue(Columns.L27, value); }
		}
		  
		[XmlAttribute("L28")]
		[Bindable(true)]
		public string L28 
		{
			get { return GetColumnValue<string>(Columns.L28); }
			set { SetColumnValue(Columns.L28, value); }
		}
		  
		[XmlAttribute("L29")]
		[Bindable(true)]
		public string L29 
		{
			get { return GetColumnValue<string>(Columns.L29); }
			set { SetColumnValue(Columns.L29, value); }
		}
		  
		[XmlAttribute("L30")]
		[Bindable(true)]
		public string L30 
		{
			get { return GetColumnValue<string>(Columns.L30); }
			set { SetColumnValue(Columns.L30, value); }
		}
		  
		[XmlAttribute("L31")]
		[Bindable(true)]
		public string L31 
		{
			get { return GetColumnValue<string>(Columns.L31); }
			set { SetColumnValue(Columns.L31, value); }
		}
		  
		[XmlAttribute("L32")]
		[Bindable(true)]
		public string L32 
		{
			get { return GetColumnValue<string>(Columns.L32); }
			set { SetColumnValue(Columns.L32, value); }
		}
		  
		[XmlAttribute("L33")]
		[Bindable(true)]
		public string L33 
		{
			get { return GetColumnValue<string>(Columns.L33); }
			set { SetColumnValue(Columns.L33, value); }
		}
		  
		[XmlAttribute("L34")]
		[Bindable(true)]
		public string L34 
		{
			get { return GetColumnValue<string>(Columns.L34); }
			set { SetColumnValue(Columns.L34, value); }
		}
		  
		[XmlAttribute("L35")]
		[Bindable(true)]
		public string L35 
		{
			get { return GetColumnValue<string>(Columns.L35); }
			set { SetColumnValue(Columns.L35, value); }
		}
		  
		[XmlAttribute("L36")]
		[Bindable(true)]
		public string L36 
		{
			get { return GetColumnValue<string>(Columns.L36); }
			set { SetColumnValue(Columns.L36, value); }
		}
		  
		[XmlAttribute("L37")]
		[Bindable(true)]
		public string L37 
		{
			get { return GetColumnValue<string>(Columns.L37); }
			set { SetColumnValue(Columns.L37, value); }
		}
		  
		[XmlAttribute("L38")]
		[Bindable(true)]
		public string L38 
		{
			get { return GetColumnValue<string>(Columns.L38); }
			set { SetColumnValue(Columns.L38, value); }
		}
		  
		[XmlAttribute("L39")]
		[Bindable(true)]
		public string L39 
		{
			get { return GetColumnValue<string>(Columns.L39); }
			set { SetColumnValue(Columns.L39, value); }
		}
		  
		[XmlAttribute("L40")]
		[Bindable(true)]
		public string L40 
		{
			get { return GetColumnValue<string>(Columns.L40); }
			set { SetColumnValue(Columns.L40, value); }
		}
		  
		[XmlAttribute("L41")]
		[Bindable(true)]
		public string L41 
		{
			get { return GetColumnValue<string>(Columns.L41); }
			set { SetColumnValue(Columns.L41, value); }
		}
		  
		[XmlAttribute("L42")]
		[Bindable(true)]
		public string L42 
		{
			get { return GetColumnValue<string>(Columns.L42); }
			set { SetColumnValue(Columns.L42, value); }
		}
		  
		[XmlAttribute("L43")]
		[Bindable(true)]
		public string L43 
		{
			get { return GetColumnValue<string>(Columns.L43); }
			set { SetColumnValue(Columns.L43, value); }
		}
		  
		[XmlAttribute("L44")]
		[Bindable(true)]
		public string L44 
		{
			get { return GetColumnValue<string>(Columns.L44); }
			set { SetColumnValue(Columns.L44, value); }
		}
		  
		[XmlAttribute("L45")]
		[Bindable(true)]
		public string L45 
		{
			get { return GetColumnValue<string>(Columns.L45); }
			set { SetColumnValue(Columns.L45, value); }
		}
		  
		[XmlAttribute("L46")]
		[Bindable(true)]
		public string L46 
		{
			get { return GetColumnValue<string>(Columns.L46); }
			set { SetColumnValue(Columns.L46, value); }
		}
		  
		[XmlAttribute("L47")]
		[Bindable(true)]
		public string L47 
		{
			get { return GetColumnValue<string>(Columns.L47); }
			set { SetColumnValue(Columns.L47, value); }
		}
		  
		[XmlAttribute("L48")]
		[Bindable(true)]
		public string L48 
		{
			get { return GetColumnValue<string>(Columns.L48); }
			set { SetColumnValue(Columns.L48, value); }
		}
		  
		[XmlAttribute("L49")]
		[Bindable(true)]
		public string L49 
		{
			get { return GetColumnValue<string>(Columns.L49); }
			set { SetColumnValue(Columns.L49, value); }
		}
		  
		[XmlAttribute("L50")]
		[Bindable(true)]
		public string L50 
		{
			get { return GetColumnValue<string>(Columns.L50); }
			set { SetColumnValue(Columns.L50, value); }
		}
		  
		[XmlAttribute("L51")]
		[Bindable(true)]
		public string L51 
		{
			get { return GetColumnValue<string>(Columns.L51); }
			set { SetColumnValue(Columns.L51, value); }
		}
		  
		[XmlAttribute("L52")]
		[Bindable(true)]
		public string L52 
		{
			get { return GetColumnValue<string>(Columns.L52); }
			set { SetColumnValue(Columns.L52, value); }
		}
		  
		[XmlAttribute("L53")]
		[Bindable(true)]
		public string L53 
		{
			get { return GetColumnValue<string>(Columns.L53); }
			set { SetColumnValue(Columns.L53, value); }
		}
		  
		[XmlAttribute("L54")]
		[Bindable(true)]
		public string L54 
		{
			get { return GetColumnValue<string>(Columns.L54); }
			set { SetColumnValue(Columns.L54, value); }
		}
		  
		[XmlAttribute("L55")]
		[Bindable(true)]
		public string L55 
		{
			get { return GetColumnValue<string>(Columns.L55); }
			set { SetColumnValue(Columns.L55, value); }
		}
		  
		[XmlAttribute("L56")]
		[Bindable(true)]
		public string L56 
		{
			get { return GetColumnValue<string>(Columns.L56); }
			set { SetColumnValue(Columns.L56, value); }
		}
		  
		[XmlAttribute("L57")]
		[Bindable(true)]
		public string L57 
		{
			get { return GetColumnValue<string>(Columns.L57); }
			set { SetColumnValue(Columns.L57, value); }
		}
		  
		[XmlAttribute("L58")]
		[Bindable(true)]
		public string L58 
		{
			get { return GetColumnValue<string>(Columns.L58); }
			set { SetColumnValue(Columns.L58, value); }
		}
		  
		[XmlAttribute("L59")]
		[Bindable(true)]
		public string L59 
		{
			get { return GetColumnValue<string>(Columns.L59); }
			set { SetColumnValue(Columns.L59, value); }
		}
		  
		[XmlAttribute("L60")]
		[Bindable(true)]
		public string L60 
		{
			get { return GetColumnValue<string>(Columns.L60); }
			set { SetColumnValue(Columns.L60, value); }
		}
		  
		[XmlAttribute("L61")]
		[Bindable(true)]
		public string L61 
		{
			get { return GetColumnValue<string>(Columns.L61); }
			set { SetColumnValue(Columns.L61, value); }
		}
		  
		[XmlAttribute("L62")]
		[Bindable(true)]
		public string L62 
		{
			get { return GetColumnValue<string>(Columns.L62); }
			set { SetColumnValue(Columns.L62, value); }
		}
		  
		[XmlAttribute("L63")]
		[Bindable(true)]
		public string L63 
		{
			get { return GetColumnValue<string>(Columns.L63); }
			set { SetColumnValue(Columns.L63, value); }
		}
		  
		[XmlAttribute("L64")]
		[Bindable(true)]
		public string L64 
		{
			get { return GetColumnValue<string>(Columns.L64); }
			set { SetColumnValue(Columns.L64, value); }
		}
		  
		[XmlAttribute("L65")]
		[Bindable(true)]
		public string L65 
		{
			get { return GetColumnValue<string>(Columns.L65); }
			set { SetColumnValue(Columns.L65, value); }
		}
		  
		[XmlAttribute("L66")]
		[Bindable(true)]
		public string L66 
		{
			get { return GetColumnValue<string>(Columns.L66); }
			set { SetColumnValue(Columns.L66, value); }
		}
		  
		[XmlAttribute("L67")]
		[Bindable(true)]
		public string L67 
		{
			get { return GetColumnValue<string>(Columns.L67); }
			set { SetColumnValue(Columns.L67, value); }
		}
		  
		[XmlAttribute("L68")]
		[Bindable(true)]
		public string L68 
		{
			get { return GetColumnValue<string>(Columns.L68); }
			set { SetColumnValue(Columns.L68, value); }
		}
		  
		[XmlAttribute("L69")]
		[Bindable(true)]
		public string L69 
		{
			get { return GetColumnValue<string>(Columns.L69); }
			set { SetColumnValue(Columns.L69, value); }
		}
		  
		[XmlAttribute("L70")]
		[Bindable(true)]
		public string L70 
		{
			get { return GetColumnValue<string>(Columns.L70); }
			set { SetColumnValue(Columns.L70, value); }
		}
		  
		[XmlAttribute("L71")]
		[Bindable(true)]
		public string L71 
		{
			get { return GetColumnValue<string>(Columns.L71); }
			set { SetColumnValue(Columns.L71, value); }
		}
		  
		[XmlAttribute("L72")]
		[Bindable(true)]
		public string L72 
		{
			get { return GetColumnValue<string>(Columns.L72); }
			set { SetColumnValue(Columns.L72, value); }
		}
		  
		[XmlAttribute("L73")]
		[Bindable(true)]
		public string L73 
		{
			get { return GetColumnValue<string>(Columns.L73); }
			set { SetColumnValue(Columns.L73, value); }
		}
		  
		[XmlAttribute("L74")]
		[Bindable(true)]
		public string L74 
		{
			get { return GetColumnValue<string>(Columns.L74); }
			set { SetColumnValue(Columns.L74, value); }
		}
		  
		[XmlAttribute("L75")]
		[Bindable(true)]
		public string L75 
		{
			get { return GetColumnValue<string>(Columns.L75); }
			set { SetColumnValue(Columns.L75, value); }
		}
		  
		[XmlAttribute("L76")]
		[Bindable(true)]
		public string L76 
		{
			get { return GetColumnValue<string>(Columns.L76); }
			set { SetColumnValue(Columns.L76, value); }
		}
		  
		[XmlAttribute("L77")]
		[Bindable(true)]
		public string L77 
		{
			get { return GetColumnValue<string>(Columns.L77); }
			set { SetColumnValue(Columns.L77, value); }
		}
		  
		[XmlAttribute("L78")]
		[Bindable(true)]
		public string L78 
		{
			get { return GetColumnValue<string>(Columns.L78); }
			set { SetColumnValue(Columns.L78, value); }
		}
		  
		[XmlAttribute("L79")]
		[Bindable(true)]
		public string L79 
		{
			get { return GetColumnValue<string>(Columns.L79); }
			set { SetColumnValue(Columns.L79, value); }
		}
		  
		[XmlAttribute("L80")]
		[Bindable(true)]
		public string L80 
		{
			get { return GetColumnValue<string>(Columns.L80); }
			set { SetColumnValue(Columns.L80, value); }
		}
		  
		[XmlAttribute("L81")]
		[Bindable(true)]
		public string L81 
		{
			get { return GetColumnValue<string>(Columns.L81); }
			set { SetColumnValue(Columns.L81, value); }
		}
		  
		[XmlAttribute("L82")]
		[Bindable(true)]
		public string L82 
		{
			get { return GetColumnValue<string>(Columns.L82); }
			set { SetColumnValue(Columns.L82, value); }
		}
		  
		[XmlAttribute("L83")]
		[Bindable(true)]
		public string L83 
		{
			get { return GetColumnValue<string>(Columns.L83); }
			set { SetColumnValue(Columns.L83, value); }
		}
		  
		[XmlAttribute("L84")]
		[Bindable(true)]
		public string L84 
		{
			get { return GetColumnValue<string>(Columns.L84); }
			set { SetColumnValue(Columns.L84, value); }
		}
		  
		[XmlAttribute("L85")]
		[Bindable(true)]
		public string L85 
		{
			get { return GetColumnValue<string>(Columns.L85); }
			set { SetColumnValue(Columns.L85, value); }
		}
		  
		[XmlAttribute("L86")]
		[Bindable(true)]
		public string L86 
		{
			get { return GetColumnValue<string>(Columns.L86); }
			set { SetColumnValue(Columns.L86, value); }
		}
		  
		[XmlAttribute("L87")]
		[Bindable(true)]
		public string L87 
		{
			get { return GetColumnValue<string>(Columns.L87); }
			set { SetColumnValue(Columns.L87, value); }
		}
		  
		[XmlAttribute("L88")]
		[Bindable(true)]
		public string L88 
		{
			get { return GetColumnValue<string>(Columns.L88); }
			set { SetColumnValue(Columns.L88, value); }
		}
		  
		[XmlAttribute("L89")]
		[Bindable(true)]
		public string L89 
		{
			get { return GetColumnValue<string>(Columns.L89); }
			set { SetColumnValue(Columns.L89, value); }
		}
		  
		[XmlAttribute("L90")]
		[Bindable(true)]
		public string L90 
		{
			get { return GetColumnValue<string>(Columns.L90); }
			set { SetColumnValue(Columns.L90, value); }
		}
		  
		[XmlAttribute("L91")]
		[Bindable(true)]
		public string L91 
		{
			get { return GetColumnValue<string>(Columns.L91); }
			set { SetColumnValue(Columns.L91, value); }
		}
		  
		[XmlAttribute("L92")]
		[Bindable(true)]
		public string L92 
		{
			get { return GetColumnValue<string>(Columns.L92); }
			set { SetColumnValue(Columns.L92, value); }
		}
		  
		[XmlAttribute("L93")]
		[Bindable(true)]
		public string L93 
		{
			get { return GetColumnValue<string>(Columns.L93); }
			set { SetColumnValue(Columns.L93, value); }
		}
		  
		[XmlAttribute("L94")]
		[Bindable(true)]
		public string L94 
		{
			get { return GetColumnValue<string>(Columns.L94); }
			set { SetColumnValue(Columns.L94, value); }
		}
		  
		[XmlAttribute("L95")]
		[Bindable(true)]
		public string L95 
		{
			get { return GetColumnValue<string>(Columns.L95); }
			set { SetColumnValue(Columns.L95, value); }
		}
		  
		[XmlAttribute("L96")]
		[Bindable(true)]
		public string L96 
		{
			get { return GetColumnValue<string>(Columns.L96); }
			set { SetColumnValue(Columns.L96, value); }
		}
		  
		[XmlAttribute("L97")]
		[Bindable(true)]
		public string L97 
		{
			get { return GetColumnValue<string>(Columns.L97); }
			set { SetColumnValue(Columns.L97, value); }
		}
		  
		[XmlAttribute("L98")]
		[Bindable(true)]
		public string L98 
		{
			get { return GetColumnValue<string>(Columns.L98); }
			set { SetColumnValue(Columns.L98, value); }
		}
		  
		[XmlAttribute("L99")]
		[Bindable(true)]
		public string L99 
		{
			get { return GetColumnValue<string>(Columns.L99); }
			set { SetColumnValue(Columns.L99, value); }
		}
		  
		[XmlAttribute("L100")]
		[Bindable(true)]
		public string L100 
		{
			get { return GetColumnValue<string>(Columns.L100); }
			set { SetColumnValue(Columns.L100, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varId,string varEng,string varChs,string varL1,string varL2,string varL3,string varL4,string varL5,string varL6,string varL7,string varL8,string varL9,string varL10,string varL11,string varL12,string varL13,string varL14,string varL15,string varL16,string varL17,string varL18,string varL19,string varL20,string varL21,string varL22,string varL23,string varL24,string varL25,string varL26,string varL27,string varL28,string varL29,string varL30,string varL31,string varL32,string varL33,string varL34,string varL35,string varL36,string varL37,string varL38,string varL39,string varL40,string varL41,string varL42,string varL43,string varL44,string varL45,string varL46,string varL47,string varL48,string varL49,string varL50,string varL51,string varL52,string varL53,string varL54,string varL55,string varL56,string varL57,string varL58,string varL59,string varL60,string varL61,string varL62,string varL63,string varL64,string varL65,string varL66,string varL67,string varL68,string varL69,string varL70,string varL71,string varL72,string varL73,string varL74,string varL75,string varL76,string varL77,string varL78,string varL79,string varL80,string varL81,string varL82,string varL83,string varL84,string varL85,string varL86,string varL87,string varL88,string varL89,string varL90,string varL91,string varL92,string varL93,string varL94,string varL95,string varL96,string varL97,string varL98,string varL99,string varL100)
		{
			TextLanguage item = new TextLanguage();
			
			item.Id = varId;
			
			item.Eng = varEng;
			
			item.Chs = varChs;
			
			item.L1 = varL1;
			
			item.L2 = varL2;
			
			item.L3 = varL3;
			
			item.L4 = varL4;
			
			item.L5 = varL5;
			
			item.L6 = varL6;
			
			item.L7 = varL7;
			
			item.L8 = varL8;
			
			item.L9 = varL9;
			
			item.L10 = varL10;
			
			item.L11 = varL11;
			
			item.L12 = varL12;
			
			item.L13 = varL13;
			
			item.L14 = varL14;
			
			item.L15 = varL15;
			
			item.L16 = varL16;
			
			item.L17 = varL17;
			
			item.L18 = varL18;
			
			item.L19 = varL19;
			
			item.L20 = varL20;
			
			item.L21 = varL21;
			
			item.L22 = varL22;
			
			item.L23 = varL23;
			
			item.L24 = varL24;
			
			item.L25 = varL25;
			
			item.L26 = varL26;
			
			item.L27 = varL27;
			
			item.L28 = varL28;
			
			item.L29 = varL29;
			
			item.L30 = varL30;
			
			item.L31 = varL31;
			
			item.L32 = varL32;
			
			item.L33 = varL33;
			
			item.L34 = varL34;
			
			item.L35 = varL35;
			
			item.L36 = varL36;
			
			item.L37 = varL37;
			
			item.L38 = varL38;
			
			item.L39 = varL39;
			
			item.L40 = varL40;
			
			item.L41 = varL41;
			
			item.L42 = varL42;
			
			item.L43 = varL43;
			
			item.L44 = varL44;
			
			item.L45 = varL45;
			
			item.L46 = varL46;
			
			item.L47 = varL47;
			
			item.L48 = varL48;
			
			item.L49 = varL49;
			
			item.L50 = varL50;
			
			item.L51 = varL51;
			
			item.L52 = varL52;
			
			item.L53 = varL53;
			
			item.L54 = varL54;
			
			item.L55 = varL55;
			
			item.L56 = varL56;
			
			item.L57 = varL57;
			
			item.L58 = varL58;
			
			item.L59 = varL59;
			
			item.L60 = varL60;
			
			item.L61 = varL61;
			
			item.L62 = varL62;
			
			item.L63 = varL63;
			
			item.L64 = varL64;
			
			item.L65 = varL65;
			
			item.L66 = varL66;
			
			item.L67 = varL67;
			
			item.L68 = varL68;
			
			item.L69 = varL69;
			
			item.L70 = varL70;
			
			item.L71 = varL71;
			
			item.L72 = varL72;
			
			item.L73 = varL73;
			
			item.L74 = varL74;
			
			item.L75 = varL75;
			
			item.L76 = varL76;
			
			item.L77 = varL77;
			
			item.L78 = varL78;
			
			item.L79 = varL79;
			
			item.L80 = varL80;
			
			item.L81 = varL81;
			
			item.L82 = varL82;
			
			item.L83 = varL83;
			
			item.L84 = varL84;
			
			item.L85 = varL85;
			
			item.L86 = varL86;
			
			item.L87 = varL87;
			
			item.L88 = varL88;
			
			item.L89 = varL89;
			
			item.L90 = varL90;
			
			item.L91 = varL91;
			
			item.L92 = varL92;
			
			item.L93 = varL93;
			
			item.L94 = varL94;
			
			item.L95 = varL95;
			
			item.L96 = varL96;
			
			item.L97 = varL97;
			
			item.L98 = varL98;
			
			item.L99 = varL99;
			
			item.L100 = varL100;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varId,string varEng,string varChs,string varL1,string varL2,string varL3,string varL4,string varL5,string varL6,string varL7,string varL8,string varL9,string varL10,string varL11,string varL12,string varL13,string varL14,string varL15,string varL16,string varL17,string varL18,string varL19,string varL20,string varL21,string varL22,string varL23,string varL24,string varL25,string varL26,string varL27,string varL28,string varL29,string varL30,string varL31,string varL32,string varL33,string varL34,string varL35,string varL36,string varL37,string varL38,string varL39,string varL40,string varL41,string varL42,string varL43,string varL44,string varL45,string varL46,string varL47,string varL48,string varL49,string varL50,string varL51,string varL52,string varL53,string varL54,string varL55,string varL56,string varL57,string varL58,string varL59,string varL60,string varL61,string varL62,string varL63,string varL64,string varL65,string varL66,string varL67,string varL68,string varL69,string varL70,string varL71,string varL72,string varL73,string varL74,string varL75,string varL76,string varL77,string varL78,string varL79,string varL80,string varL81,string varL82,string varL83,string varL84,string varL85,string varL86,string varL87,string varL88,string varL89,string varL90,string varL91,string varL92,string varL93,string varL94,string varL95,string varL96,string varL97,string varL98,string varL99,string varL100)
		{
			TextLanguage item = new TextLanguage();
			
				item.Id = varId;
			
				item.Eng = varEng;
			
				item.Chs = varChs;
			
				item.L1 = varL1;
			
				item.L2 = varL2;
			
				item.L3 = varL3;
			
				item.L4 = varL4;
			
				item.L5 = varL5;
			
				item.L6 = varL6;
			
				item.L7 = varL7;
			
				item.L8 = varL8;
			
				item.L9 = varL9;
			
				item.L10 = varL10;
			
				item.L11 = varL11;
			
				item.L12 = varL12;
			
				item.L13 = varL13;
			
				item.L14 = varL14;
			
				item.L15 = varL15;
			
				item.L16 = varL16;
			
				item.L17 = varL17;
			
				item.L18 = varL18;
			
				item.L19 = varL19;
			
				item.L20 = varL20;
			
				item.L21 = varL21;
			
				item.L22 = varL22;
			
				item.L23 = varL23;
			
				item.L24 = varL24;
			
				item.L25 = varL25;
			
				item.L26 = varL26;
			
				item.L27 = varL27;
			
				item.L28 = varL28;
			
				item.L29 = varL29;
			
				item.L30 = varL30;
			
				item.L31 = varL31;
			
				item.L32 = varL32;
			
				item.L33 = varL33;
			
				item.L34 = varL34;
			
				item.L35 = varL35;
			
				item.L36 = varL36;
			
				item.L37 = varL37;
			
				item.L38 = varL38;
			
				item.L39 = varL39;
			
				item.L40 = varL40;
			
				item.L41 = varL41;
			
				item.L42 = varL42;
			
				item.L43 = varL43;
			
				item.L44 = varL44;
			
				item.L45 = varL45;
			
				item.L46 = varL46;
			
				item.L47 = varL47;
			
				item.L48 = varL48;
			
				item.L49 = varL49;
			
				item.L50 = varL50;
			
				item.L51 = varL51;
			
				item.L52 = varL52;
			
				item.L53 = varL53;
			
				item.L54 = varL54;
			
				item.L55 = varL55;
			
				item.L56 = varL56;
			
				item.L57 = varL57;
			
				item.L58 = varL58;
			
				item.L59 = varL59;
			
				item.L60 = varL60;
			
				item.L61 = varL61;
			
				item.L62 = varL62;
			
				item.L63 = varL63;
			
				item.L64 = varL64;
			
				item.L65 = varL65;
			
				item.L66 = varL66;
			
				item.L67 = varL67;
			
				item.L68 = varL68;
			
				item.L69 = varL69;
			
				item.L70 = varL70;
			
				item.L71 = varL71;
			
				item.L72 = varL72;
			
				item.L73 = varL73;
			
				item.L74 = varL74;
			
				item.L75 = varL75;
			
				item.L76 = varL76;
			
				item.L77 = varL77;
			
				item.L78 = varL78;
			
				item.L79 = varL79;
			
				item.L80 = varL80;
			
				item.L81 = varL81;
			
				item.L82 = varL82;
			
				item.L83 = varL83;
			
				item.L84 = varL84;
			
				item.L85 = varL85;
			
				item.L86 = varL86;
			
				item.L87 = varL87;
			
				item.L88 = varL88;
			
				item.L89 = varL89;
			
				item.L90 = varL90;
			
				item.L91 = varL91;
			
				item.L92 = varL92;
			
				item.L93 = varL93;
			
				item.L94 = varL94;
			
				item.L95 = varL95;
			
				item.L96 = varL96;
			
				item.L97 = varL97;
			
				item.L98 = varL98;
			
				item.L99 = varL99;
			
				item.L100 = varL100;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn EngColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ChsColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn L1Column
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn L2Column
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn L3Column
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn L4Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn L5Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn L6Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn L7Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn L8Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn L9Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn L10Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn L11Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn L12Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn L13Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn L14Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn L15Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn L16Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn L17Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn L18Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn L19Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn L20Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn L21Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn L22Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn L23Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn L24Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn L25Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn L26Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn L27Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn L28Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn L29Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn L30Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn L31Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn L32Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn L33Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn L34Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn L35Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn L36Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn L37Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn L38Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn L39Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn L40Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn L41Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn L42Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn L43Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn L44Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn L45Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn L46Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn L47Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn L48Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn L49Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn L50Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn L51Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn L52Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn L53Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn L54Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn L55Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn L56Column
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn L57Column
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn L58Column
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn L59Column
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn L60Column
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        public static TableSchema.TableColumn L61Column
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn L62Column
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn L63Column
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn L64Column
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn L65Column
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn L66Column
        {
            get { return Schema.Columns[68]; }
        }
        
        
        
        public static TableSchema.TableColumn L67Column
        {
            get { return Schema.Columns[69]; }
        }
        
        
        
        public static TableSchema.TableColumn L68Column
        {
            get { return Schema.Columns[70]; }
        }
        
        
        
        public static TableSchema.TableColumn L69Column
        {
            get { return Schema.Columns[71]; }
        }
        
        
        
        public static TableSchema.TableColumn L70Column
        {
            get { return Schema.Columns[72]; }
        }
        
        
        
        public static TableSchema.TableColumn L71Column
        {
            get { return Schema.Columns[73]; }
        }
        
        
        
        public static TableSchema.TableColumn L72Column
        {
            get { return Schema.Columns[74]; }
        }
        
        
        
        public static TableSchema.TableColumn L73Column
        {
            get { return Schema.Columns[75]; }
        }
        
        
        
        public static TableSchema.TableColumn L74Column
        {
            get { return Schema.Columns[76]; }
        }
        
        
        
        public static TableSchema.TableColumn L75Column
        {
            get { return Schema.Columns[77]; }
        }
        
        
        
        public static TableSchema.TableColumn L76Column
        {
            get { return Schema.Columns[78]; }
        }
        
        
        
        public static TableSchema.TableColumn L77Column
        {
            get { return Schema.Columns[79]; }
        }
        
        
        
        public static TableSchema.TableColumn L78Column
        {
            get { return Schema.Columns[80]; }
        }
        
        
        
        public static TableSchema.TableColumn L79Column
        {
            get { return Schema.Columns[81]; }
        }
        
        
        
        public static TableSchema.TableColumn L80Column
        {
            get { return Schema.Columns[82]; }
        }
        
        
        
        public static TableSchema.TableColumn L81Column
        {
            get { return Schema.Columns[83]; }
        }
        
        
        
        public static TableSchema.TableColumn L82Column
        {
            get { return Schema.Columns[84]; }
        }
        
        
        
        public static TableSchema.TableColumn L83Column
        {
            get { return Schema.Columns[85]; }
        }
        
        
        
        public static TableSchema.TableColumn L84Column
        {
            get { return Schema.Columns[86]; }
        }
        
        
        
        public static TableSchema.TableColumn L85Column
        {
            get { return Schema.Columns[87]; }
        }
        
        
        
        public static TableSchema.TableColumn L86Column
        {
            get { return Schema.Columns[88]; }
        }
        
        
        
        public static TableSchema.TableColumn L87Column
        {
            get { return Schema.Columns[89]; }
        }
        
        
        
        public static TableSchema.TableColumn L88Column
        {
            get { return Schema.Columns[90]; }
        }
        
        
        
        public static TableSchema.TableColumn L89Column
        {
            get { return Schema.Columns[91]; }
        }
        
        
        
        public static TableSchema.TableColumn L90Column
        {
            get { return Schema.Columns[92]; }
        }
        
        
        
        public static TableSchema.TableColumn L91Column
        {
            get { return Schema.Columns[93]; }
        }
        
        
        
        public static TableSchema.TableColumn L92Column
        {
            get { return Schema.Columns[94]; }
        }
        
        
        
        public static TableSchema.TableColumn L93Column
        {
            get { return Schema.Columns[95]; }
        }
        
        
        
        public static TableSchema.TableColumn L94Column
        {
            get { return Schema.Columns[96]; }
        }
        
        
        
        public static TableSchema.TableColumn L95Column
        {
            get { return Schema.Columns[97]; }
        }
        
        
        
        public static TableSchema.TableColumn L96Column
        {
            get { return Schema.Columns[98]; }
        }
        
        
        
        public static TableSchema.TableColumn L97Column
        {
            get { return Schema.Columns[99]; }
        }
        
        
        
        public static TableSchema.TableColumn L98Column
        {
            get { return Schema.Columns[100]; }
        }
        
        
        
        public static TableSchema.TableColumn L99Column
        {
            get { return Schema.Columns[101]; }
        }
        
        
        
        public static TableSchema.TableColumn L100Column
        {
            get { return Schema.Columns[102]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Eng = @"ENG";
			 public static string Chs = @"CHS";
			 public static string L1 = @"L1";
			 public static string L2 = @"L2";
			 public static string L3 = @"L3";
			 public static string L4 = @"L4";
			 public static string L5 = @"L5";
			 public static string L6 = @"L6";
			 public static string L7 = @"L7";
			 public static string L8 = @"L8";
			 public static string L9 = @"L9";
			 public static string L10 = @"L10";
			 public static string L11 = @"L11";
			 public static string L12 = @"L12";
			 public static string L13 = @"L13";
			 public static string L14 = @"L14";
			 public static string L15 = @"L15";
			 public static string L16 = @"L16";
			 public static string L17 = @"L17";
			 public static string L18 = @"L18";
			 public static string L19 = @"L19";
			 public static string L20 = @"L20";
			 public static string L21 = @"L21";
			 public static string L22 = @"L22";
			 public static string L23 = @"L23";
			 public static string L24 = @"L24";
			 public static string L25 = @"L25";
			 public static string L26 = @"L26";
			 public static string L27 = @"L27";
			 public static string L28 = @"L28";
			 public static string L29 = @"L29";
			 public static string L30 = @"L30";
			 public static string L31 = @"L31";
			 public static string L32 = @"L32";
			 public static string L33 = @"L33";
			 public static string L34 = @"L34";
			 public static string L35 = @"L35";
			 public static string L36 = @"L36";
			 public static string L37 = @"L37";
			 public static string L38 = @"L38";
			 public static string L39 = @"L39";
			 public static string L40 = @"L40";
			 public static string L41 = @"L41";
			 public static string L42 = @"L42";
			 public static string L43 = @"L43";
			 public static string L44 = @"L44";
			 public static string L45 = @"L45";
			 public static string L46 = @"L46";
			 public static string L47 = @"L47";
			 public static string L48 = @"L48";
			 public static string L49 = @"L49";
			 public static string L50 = @"L50";
			 public static string L51 = @"L51";
			 public static string L52 = @"L52";
			 public static string L53 = @"L53";
			 public static string L54 = @"L54";
			 public static string L55 = @"L55";
			 public static string L56 = @"L56";
			 public static string L57 = @"L57";
			 public static string L58 = @"L58";
			 public static string L59 = @"L59";
			 public static string L60 = @"L60";
			 public static string L61 = @"L61";
			 public static string L62 = @"L62";
			 public static string L63 = @"L63";
			 public static string L64 = @"L64";
			 public static string L65 = @"L65";
			 public static string L66 = @"L66";
			 public static string L67 = @"L67";
			 public static string L68 = @"L68";
			 public static string L69 = @"L69";
			 public static string L70 = @"L70";
			 public static string L71 = @"L71";
			 public static string L72 = @"L72";
			 public static string L73 = @"L73";
			 public static string L74 = @"L74";
			 public static string L75 = @"L75";
			 public static string L76 = @"L76";
			 public static string L77 = @"L77";
			 public static string L78 = @"L78";
			 public static string L79 = @"L79";
			 public static string L80 = @"L80";
			 public static string L81 = @"L81";
			 public static string L82 = @"L82";
			 public static string L83 = @"L83";
			 public static string L84 = @"L84";
			 public static string L85 = @"L85";
			 public static string L86 = @"L86";
			 public static string L87 = @"L87";
			 public static string L88 = @"L88";
			 public static string L89 = @"L89";
			 public static string L90 = @"L90";
			 public static string L91 = @"L91";
			 public static string L92 = @"L92";
			 public static string L93 = @"L93";
			 public static string L94 = @"L94";
			 public static string L95 = @"L95";
			 public static string L96 = @"L96";
			 public static string L97 = @"L97";
			 public static string L98 = @"L98";
			 public static string L99 = @"L99";
			 public static string L100 = @"L100";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
