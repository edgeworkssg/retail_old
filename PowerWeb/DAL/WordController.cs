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
    /// Controller class for Word
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class WordController
    {
        // Preload our schema..
        Word thisSchemaLoad = new Word();
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
        public WordCollection FetchAll()
        {
            WordCollection coll = new WordCollection();
            Query qry = new Query(Word.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public WordCollection FetchByID(object WordGuid)
        {
            WordCollection coll = new WordCollection().Where("WordGuid", WordGuid).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public WordCollection FetchByQuery(Query qry)
        {
            WordCollection coll = new WordCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object WordGuid)
        {
            return (Word.Delete(WordGuid) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object WordGuid)
        {
            return (Word.Destroy(WordGuid) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid WordGuid,string WordText,byte WordLength,string SoundexGroup,short SoundexValue,short? GroupId,bool IsPalindrome,bool InUnabr,bool InAntworth,bool InCRL,bool InRoget,bool InUnix,bool InKnuthBritish,bool InKnuth,bool InEnglex,bool InShakespeare,bool InPocket,bool InUUNet)
	    {
		    Word item = new Word();
		    
            item.WordGuid = WordGuid;
            
            item.WordText = WordText;
            
            item.WordLength = WordLength;
            
            item.SoundexGroup = SoundexGroup;
            
            item.SoundexValue = SoundexValue;
            
            item.GroupId = GroupId;
            
            item.IsPalindrome = IsPalindrome;
            
            item.InUnabr = InUnabr;
            
            item.InAntworth = InAntworth;
            
            item.InCRL = InCRL;
            
            item.InRoget = InRoget;
            
            item.InUnix = InUnix;
            
            item.InKnuthBritish = InKnuthBritish;
            
            item.InKnuth = InKnuth;
            
            item.InEnglex = InEnglex;
            
            item.InShakespeare = InShakespeare;
            
            item.InPocket = InPocket;
            
            item.InUUNet = InUUNet;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid WordGuid,string WordText,byte WordLength,string SoundexGroup,short SoundexValue,short? GroupId,bool IsPalindrome,bool InUnabr,bool InAntworth,bool InCRL,bool InRoget,bool InUnix,bool InKnuthBritish,bool InKnuth,bool InEnglex,bool InShakespeare,bool InPocket,bool InUUNet)
	    {
		    Word item = new Word();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.WordGuid = WordGuid;
				
			item.WordText = WordText;
				
			item.WordLength = WordLength;
				
			item.SoundexGroup = SoundexGroup;
				
			item.SoundexValue = SoundexValue;
				
			item.GroupId = GroupId;
				
			item.IsPalindrome = IsPalindrome;
				
			item.InUnabr = InUnabr;
				
			item.InAntworth = InAntworth;
				
			item.InCRL = InCRL;
				
			item.InRoget = InRoget;
				
			item.InUnix = InUnix;
				
			item.InKnuthBritish = InKnuthBritish;
				
			item.InKnuth = InKnuth;
				
			item.InEnglex = InEnglex;
				
			item.InShakespeare = InShakespeare;
				
			item.InPocket = InPocket;
				
			item.InUUNet = InUUNet;
				
	        item.Save(UserName);
	    }
    }
}
