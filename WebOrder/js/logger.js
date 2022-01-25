Logger = {
    fnWriteLog : function (message){
        DAL.fnOpenDB();
        DAL.DB.transaction(function(tx){
           tx.executeSql('insert into powerlog (msg,actiondate) values (?, ?)',
           [message, fnGetCurrentDateTime()],function(tx,result){
               return result;               
           }); 
        });
    }, 
    fnWriteDebug : function (message){
        if (sessionStorage.debugMode == 'true')
        {
            DAL.fnOpenDB();
            DAL.DB.transaction(function(tx){
                tx.executeSql('insert into powerlog (msg,actiondate) values (?, ?)',
                ['DBG:' + message, fnGetCurrentDateTime()],function(tx,result){
                    return result;
                }); 
            });            
        }        
    }, 
    fnViewLog: function(startDate, endDate, callback){
      DAL.fnOpenDB();
      DAL.DB.transaction(function(tx){
         tx.executeSql('select * from powerlog where actiondate >= ? and actiondate <= ? order by actiondate desc',
         [startDate, endDate], function(tx, results){
             var res = new Array();
             for (var i = 0; i < results.rows.length; i++) {                                         
                    res[i] = results.rows.item(i);
                }
             if (callback) callback(res);
         }); 
      });
    },
}