//*******************************************************************************************************************
//sample project for connecting to the PUPPI HTTP Listener server and issuing commands via console.
//use with server in PUPPITestClientServer
//No Warranty: THE SOFTWARE IS A WORK IN PROGRESS AND IS PROVIDED "AS IS".
//see commands in Getting Started With PUPPI document
//http://visualprogramminglanguage.com
//Advanced client-server project samples available to PUPPI subscribers. Contact us at sales@pupi.co
//*******************************************************************************************************************

 
 
 
 $(function () {
    
    $("#runSample1").click(function () {
        var IPaddJS = $("#ipadd").val();
        var portJS= $("#portt").val();
		var pJS = $("#passw").val();
		var commandJS = $("#commd").val();
		
		var connString=IPaddJS +":" + portJS + "/";
		
		var dataReturn="nada";
		//alert(connString);
        $.post(connString, { ssw: pJS, command: commandJS }, function (data) {
            
			dataReturn=data;
			alert(dataReturn);
        });
		
    });
});