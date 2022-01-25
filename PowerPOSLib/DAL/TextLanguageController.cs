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
    /// Controller class for TEXT_LANGUAGE
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TextLanguageController
    {
        // Preload our schema..
        TextLanguage thisSchemaLoad = new TextLanguage();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TextLanguageCollection FetchAll()
        {
            TextLanguageCollection coll = new TextLanguageCollection();
            Query qry = new Query(TextLanguage.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TextLanguageCollection FetchByID(object Id)
        {
            TextLanguageCollection coll = new TextLanguageCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TextLanguageCollection FetchByQuery(Query qry)
        {
            TextLanguageCollection coll = new TextLanguageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TextLanguage.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TextLanguage.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Id,string Eng,string Chs,string L1,string L2,string L3,string L4,string L5,string L6,string L7,string L8,string L9,string L10,string L11,string L12,string L13,string L14,string L15,string L16,string L17,string L18,string L19,string L20,string L21,string L22,string L23,string L24,string L25,string L26,string L27,string L28,string L29,string L30,string L31,string L32,string L33,string L34,string L35,string L36,string L37,string L38,string L39,string L40,string L41,string L42,string L43,string L44,string L45,string L46,string L47,string L48,string L49,string L50,string L51,string L52,string L53,string L54,string L55,string L56,string L57,string L58,string L59,string L60,string L61,string L62,string L63,string L64,string L65,string L66,string L67,string L68,string L69,string L70,string L71,string L72,string L73,string L74,string L75,string L76,string L77,string L78,string L79,string L80,string L81,string L82,string L83,string L84,string L85,string L86,string L87,string L88,string L89,string L90,string L91,string L92,string L93,string L94,string L95,string L96,string L97,string L98,string L99,string L100)
	    {
		    TextLanguage item = new TextLanguage();
		    
            item.Id = Id;
            
            item.Eng = Eng;
            
            item.Chs = Chs;
            
            item.L1 = L1;
            
            item.L2 = L2;
            
            item.L3 = L3;
            
            item.L4 = L4;
            
            item.L5 = L5;
            
            item.L6 = L6;
            
            item.L7 = L7;
            
            item.L8 = L8;
            
            item.L9 = L9;
            
            item.L10 = L10;
            
            item.L11 = L11;
            
            item.L12 = L12;
            
            item.L13 = L13;
            
            item.L14 = L14;
            
            item.L15 = L15;
            
            item.L16 = L16;
            
            item.L17 = L17;
            
            item.L18 = L18;
            
            item.L19 = L19;
            
            item.L20 = L20;
            
            item.L21 = L21;
            
            item.L22 = L22;
            
            item.L23 = L23;
            
            item.L24 = L24;
            
            item.L25 = L25;
            
            item.L26 = L26;
            
            item.L27 = L27;
            
            item.L28 = L28;
            
            item.L29 = L29;
            
            item.L30 = L30;
            
            item.L31 = L31;
            
            item.L32 = L32;
            
            item.L33 = L33;
            
            item.L34 = L34;
            
            item.L35 = L35;
            
            item.L36 = L36;
            
            item.L37 = L37;
            
            item.L38 = L38;
            
            item.L39 = L39;
            
            item.L40 = L40;
            
            item.L41 = L41;
            
            item.L42 = L42;
            
            item.L43 = L43;
            
            item.L44 = L44;
            
            item.L45 = L45;
            
            item.L46 = L46;
            
            item.L47 = L47;
            
            item.L48 = L48;
            
            item.L49 = L49;
            
            item.L50 = L50;
            
            item.L51 = L51;
            
            item.L52 = L52;
            
            item.L53 = L53;
            
            item.L54 = L54;
            
            item.L55 = L55;
            
            item.L56 = L56;
            
            item.L57 = L57;
            
            item.L58 = L58;
            
            item.L59 = L59;
            
            item.L60 = L60;
            
            item.L61 = L61;
            
            item.L62 = L62;
            
            item.L63 = L63;
            
            item.L64 = L64;
            
            item.L65 = L65;
            
            item.L66 = L66;
            
            item.L67 = L67;
            
            item.L68 = L68;
            
            item.L69 = L69;
            
            item.L70 = L70;
            
            item.L71 = L71;
            
            item.L72 = L72;
            
            item.L73 = L73;
            
            item.L74 = L74;
            
            item.L75 = L75;
            
            item.L76 = L76;
            
            item.L77 = L77;
            
            item.L78 = L78;
            
            item.L79 = L79;
            
            item.L80 = L80;
            
            item.L81 = L81;
            
            item.L82 = L82;
            
            item.L83 = L83;
            
            item.L84 = L84;
            
            item.L85 = L85;
            
            item.L86 = L86;
            
            item.L87 = L87;
            
            item.L88 = L88;
            
            item.L89 = L89;
            
            item.L90 = L90;
            
            item.L91 = L91;
            
            item.L92 = L92;
            
            item.L93 = L93;
            
            item.L94 = L94;
            
            item.L95 = L95;
            
            item.L96 = L96;
            
            item.L97 = L97;
            
            item.L98 = L98;
            
            item.L99 = L99;
            
            item.L100 = L100;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Id,string Eng,string Chs,string L1,string L2,string L3,string L4,string L5,string L6,string L7,string L8,string L9,string L10,string L11,string L12,string L13,string L14,string L15,string L16,string L17,string L18,string L19,string L20,string L21,string L22,string L23,string L24,string L25,string L26,string L27,string L28,string L29,string L30,string L31,string L32,string L33,string L34,string L35,string L36,string L37,string L38,string L39,string L40,string L41,string L42,string L43,string L44,string L45,string L46,string L47,string L48,string L49,string L50,string L51,string L52,string L53,string L54,string L55,string L56,string L57,string L58,string L59,string L60,string L61,string L62,string L63,string L64,string L65,string L66,string L67,string L68,string L69,string L70,string L71,string L72,string L73,string L74,string L75,string L76,string L77,string L78,string L79,string L80,string L81,string L82,string L83,string L84,string L85,string L86,string L87,string L88,string L89,string L90,string L91,string L92,string L93,string L94,string L95,string L96,string L97,string L98,string L99,string L100)
	    {
		    TextLanguage item = new TextLanguage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Eng = Eng;
				
			item.Chs = Chs;
				
			item.L1 = L1;
				
			item.L2 = L2;
				
			item.L3 = L3;
				
			item.L4 = L4;
				
			item.L5 = L5;
				
			item.L6 = L6;
				
			item.L7 = L7;
				
			item.L8 = L8;
				
			item.L9 = L9;
				
			item.L10 = L10;
				
			item.L11 = L11;
				
			item.L12 = L12;
				
			item.L13 = L13;
				
			item.L14 = L14;
				
			item.L15 = L15;
				
			item.L16 = L16;
				
			item.L17 = L17;
				
			item.L18 = L18;
				
			item.L19 = L19;
				
			item.L20 = L20;
				
			item.L21 = L21;
				
			item.L22 = L22;
				
			item.L23 = L23;
				
			item.L24 = L24;
				
			item.L25 = L25;
				
			item.L26 = L26;
				
			item.L27 = L27;
				
			item.L28 = L28;
				
			item.L29 = L29;
				
			item.L30 = L30;
				
			item.L31 = L31;
				
			item.L32 = L32;
				
			item.L33 = L33;
				
			item.L34 = L34;
				
			item.L35 = L35;
				
			item.L36 = L36;
				
			item.L37 = L37;
				
			item.L38 = L38;
				
			item.L39 = L39;
				
			item.L40 = L40;
				
			item.L41 = L41;
				
			item.L42 = L42;
				
			item.L43 = L43;
				
			item.L44 = L44;
				
			item.L45 = L45;
				
			item.L46 = L46;
				
			item.L47 = L47;
				
			item.L48 = L48;
				
			item.L49 = L49;
				
			item.L50 = L50;
				
			item.L51 = L51;
				
			item.L52 = L52;
				
			item.L53 = L53;
				
			item.L54 = L54;
				
			item.L55 = L55;
				
			item.L56 = L56;
				
			item.L57 = L57;
				
			item.L58 = L58;
				
			item.L59 = L59;
				
			item.L60 = L60;
				
			item.L61 = L61;
				
			item.L62 = L62;
				
			item.L63 = L63;
				
			item.L64 = L64;
				
			item.L65 = L65;
				
			item.L66 = L66;
				
			item.L67 = L67;
				
			item.L68 = L68;
				
			item.L69 = L69;
				
			item.L70 = L70;
				
			item.L71 = L71;
				
			item.L72 = L72;
				
			item.L73 = L73;
				
			item.L74 = L74;
				
			item.L75 = L75;
				
			item.L76 = L76;
				
			item.L77 = L77;
				
			item.L78 = L78;
				
			item.L79 = L79;
				
			item.L80 = L80;
				
			item.L81 = L81;
				
			item.L82 = L82;
				
			item.L83 = L83;
				
			item.L84 = L84;
				
			item.L85 = L85;
				
			item.L86 = L86;
				
			item.L87 = L87;
				
			item.L88 = L88;
				
			item.L89 = L89;
				
			item.L90 = L90;
				
			item.L91 = L91;
				
			item.L92 = L92;
				
			item.L93 = L93;
				
			item.L94 = L94;
				
			item.L95 = L95;
				
			item.L96 = L96;
				
			item.L97 = L97;
				
			item.L98 = L98;
				
			item.L99 = L99;
				
			item.L100 = L100;
				
	        item.Save(UserName);
	    }
    }
}
