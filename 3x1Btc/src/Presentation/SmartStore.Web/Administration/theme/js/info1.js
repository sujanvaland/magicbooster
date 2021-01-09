     
     var isChoice = 0;
     
     //Updated by Binal Joshi 04/26/2013 - For Accept resolution message
     //updated by Binal Joshi again on 05/14/2013 - To remove Reassign option
     //updated by Binal Joshi again on 05/16/2013 - Updated again for group problem.
 
     function chkAssgn(fld){     
        document.getElementById("txtTypes").value = ""; //Commented By Rao on 6/10/2015 @ 4:51 PM
		document.getElementById("txtSelectedResol").value = ""; //Commented By Rao on 6/10/2015 @ 4:51 PM
     if (fld[fld.selectedIndex].value == "Complete")
     {
       // document.getElementById("Resolution_Type").style.display = "inline";
      //  document.getElementById("txtTypes").style.display = "inline";
        document.getElementById("tdLblType").style.display = "inline";
        document.getElementById("tdResolution").style.display = "inline";          
        if (document.getElementById("hdn_IsAppUser").value=="True" || document.getElementById("hdn_IsServiceDesk").value=="True")
        {
            document.getElementById("trSecondaryResol").style.display = "inline";
            document.getElementById("trSelectedRes").style.display = "inline"; 
            document.getElementById("trWorkHrs").style.display='inline'; 
            
            //code added for <new changes on April 2016> on date <04/04/2016> by <shashank.sharma> start
            if (document.getElementById("hdn_IsAppUser").value=="True")
            {
              document.getElementById("trCompleteAppdev").style.display='inline'; 
            }
            //code added for <new changes on April 2016> on date <04/04/2016> by <shashank.sharma> end
            
            //----------------------------------------//added by mitesh on 25/11/2015------------------------
            if (document.getElementById("ddl_ticket_area").value == "Desktop" && document.getElementById("ddl_ticket_sub_area").value == "Shifting")
            {
            document.getElementById("trshiftdevice").style.display='inline';   
            }
            //--------------------------------------------------------------------------------------------
                    
        }
        else
            document.getElementById("trWorkHrs").style.display='none';           
     }
     else
     {
       // document.getElementById("Resolution_Type").style.display = "none";
      //  document.getElementById("txtTypes").style.display = "none";
        document.getElementById("tdLblType").style.display = "none";      
        document.getElementById("tdResolution").style.display = "none";  
        document.getElementById("trSecondaryResol").style.display = "none";
        document.getElementById("trSelectedRes").style.display = "none";   
        document.getElementById("trWorkHrs").style.display='none';  
        
        
        //code added for <new changes on April 2016> on date <04/04/2016> by <shashank.sharma> start
        document.getElementById("chkComplete").checked = false;
        document.getElementById("trCompleteAppdev").style.display='none'; 
        //code added for <new changes on April 2016> on date <04/04/2016> by <shashank.sharma> end
        
        
        document.getElementById("trshiftdevice").style.display='none';     //added by mitesh on 25/11/2015  
     }
     
    
    if (fld[fld.selectedIndex].value == "Complete" && (document.getElementById("hdn_IsAppUser").value=="True" || document.getElementById("hdn_IsServiceDesk").value=="True")){
       
       //alert(document.getElementById("pnlWorkHrs").style.display);
       //document.getElementById("pnlWorkHrs").style.display='none';
       document.getElementById("txt_hours").value='';
       document.getElementById("txt_minuts").value='';              
       document.getElementById("Resolution_Description").value=  '\n'  + 'Please check and "accept" resolution on this ticket. Thanks for your support and cooperation.';      
       //document.getElementById("trWorkHrs").style.display =(document.all)? '' : 'table-row';              
          if (typeof(document.getElementById("tdLblType")) != 'undefined')
          {
           document.getElementById("tdLblType").style.display ="inline"    
           document.getElementById("tdTxtType").style.display = "inline" 
         //  document.getElementById("Resolution_Type").style.display = "inline";
           document.getElementById("tdResolution").style.display = "inline";
           document.getElementById("trSecondaryResol").style.display = "inline";
           document.getElementById("trSelectedRes").style.display = "inline";              
          }
          
          
      }    
      
      else {
           //document.getElementById("trWorkHrs").style.display ="none"         
          // document.getElementById("pnlWorkHrs").style.display='none';
      } 
      
      
     }     
     
 
 
     //Added by Binal Joshi
    function chkResolution(frm)
    {
        var count = 0;            
        for (var i = 0; i < frm.Resolution_Type.options.length; i++)
         {
            if (frm.Resolution_Type.options[i].selected)
             {
                count++;
                return true;
             }
        }
         return false;
    }

    function validate(frm) {
        alert('in');
	  var rplc = new RegExp (' ', 'gi')	 
	  
	   if(!chkArea(frm))
	  {
	    return false;
	  }	  
	  
	  else if (frm.Status[frm.Status.selectedIndex].value == 'Rejected'){
	   alert('You cannot submit the ticket with rejected status.')
	   frm.Reassign_To.focus()
	   return false
	  }
	  else if (frm.Status[frm.Status.selectedIndex].value == 'Reassign' && frm.Reassign_To.selectedIndex == 0){
	   alert('Please select the name from dropdown list to reassign this ticket.')
	   frm.Reassign_To.focus()
	   return false
	  }
	  
	  //change by mitesh on 03 june 2015
	 // else if ((frm.Status[frm.Status.selectedIndex].value == 'Complete') && (!chkResolution(frm))){	// hide by mitesh     
	 else if ((frm.Status[frm.Status.selectedIndex].value == 'Complete') && ((!chkResolution(frm)) || frm.txtTypes.value == "")){      
           alert('Resolution is mandatory, if status is complete.')
           frm.Resolution_Type.focus()
           return false   
	  }
	  // JavaScript Added by Anil on 26 March 2012
	  else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' && frm.txt_hours.value == ""){
	   alert('Please enter hours.')
	   frm.txt_hours.focus()
	   return false
	  }	  
	   else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' && frm.txt_minuts.value == ""){
	   alert('Please enter minutes.')
	   frm.txt_minuts.focus()
	   return false
	  }
	   else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' && frm.txt_minuts.value > 59 ){
	   alert('Please enter Minutes value less than 60.')
	   frm.txt_minuts.focus()
	   return false
	  }
	  else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' && (frm.txt_hours.value == 0 && frm.txt_minuts.value == 0)){
	   alert('Both values for Hours and Minutes can not be Zero.')
	   frm.txt_minuts.focus()
	   return false
	  }	  
	  // End of JavaScript Added by Anil on 26 March 2012	
       else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' &&  (document.getElementById('grvHoursHistory') == undefined)) {             
           alert('Please select developer and update hours / minutes before closing ticket in Internal section.')
           frm.ddlDeveloper.focus()
           return false
       }
       
        else if (frm.Status[frm.Status.selectedIndex].value == 'Complete' && frm.hdn_IsAppUser.value=='True' &&  (document.getElementById('rdbRecommed_0').checked == false && document.getElementById('rdbRecommed_1').checked == false)) {             
           alert('Please select Recommend appadmin feature creation/enhancement.')
           document.getElementById('rdbRecommed_0').focus()
           return false
       }
       
	 //else if ((frm.Resolution_Description.value.replace(rplc,'') == '')&&(frm.initArea.value!='MIS-IT')){
	  else if ((frm.Resolution_Description.value.replace(rplc,'') == '')&&(frm.Status[frm.Status.selectedIndex].value != 'Complete')){
	   alert('Please enter description.')
	   frm.Resolution_Description.focus()
	   return false
	  }	  
	   else if (frm.hdnLinkFlag.value == 'yes' && frm.Status[frm.Status.selectedIndex].value == 'Complete')
	   {
            vbMsg('Do you want to change all linked tickets status as Complete?',"Conformation For complete.");
           // alert(isChoice);           
            
            if (isChoice==2){
                //document.getElementById("hdnReturnFlag").value= '0';
                return false;
            }
            else if(isChoice==6){
                document.getElementById("hdnReturnFlag").value= '1';   
               return true;
            }
            else if(isChoice==7){
               document.getElementById("hdnReturnFlag").value= '0';   
               return true;
            }   
                         
	  }	  
	  
		  
	  else{
	   return true;
	  }
     }
 
 function chkArea(frm)
 { 
    if(frm.ddl_ticket_area!=null)
    {
        if(frm.ddl_ticket_area.value=='MIS-IT'||frm.ddl_ticket_sub_area.value=='MIS-IT'||frm.ddl_ticket_sub_area.value=='')
        {
           alert("Please change Area/Sub-Area.");
           frm.ddl_ticket_area.focus();
           return false;
        }        
        else if  (frm.ddlPossibleRootCause.style.display == "inline" && frm.ddlPossibleRootCause.value=='')
        {  
           alert("Please select Possible Root Cause.");
           frm.ddlPossibleRootCause.focus();
           return false;
        }
        else
            return true;
    }
    else
        return true;
 }

 function onlyNumeric(evt)
    {
	    var e = event || evt; // for trans-browser compatibility
	    var charCode = e.which || e.keyCode;

	    if (charCode > 31 && (charCode < 48 || charCode > 57))
		    return false;
	    return true;
    }
    
    
    
    //      else if (fld[fld.selectedIndex].value == "Complete" && document.getElementById("hdn_IsAppUser").value=="True"){
//       document.getElementById("pnlWorkHrs").style.display=(document.all)? 'block' : 'table';
//       document.getElementById("txt_hours").value='';
//       document.getElementById("txt_minuts").value='';
//       document.getElementById("trWorkHrs").style.display =(document.all)? 'block' : 'table-row';
//       document.getElementById("tdLblType").style.display = (document.all)? 'block' : 'table-row';
//       document.getElementById("tdTxtType").style.display = (document.all)? 'block' : 'table-row';
//       document.getElementById("trReassign").style.display = "none"
//       var rType = document.getElementById("Resolution_Type")
//       rType.selectedIndex = 0
//      }




function expandMainBlock_training(imgID,divName)
{
    animatedcollapse.addDiv(divName, 'fade=1')
    
    animatedcollapse.ontoggle=function($, divName, state){ //fires each time a DIV is expanded/contracted
	//$: Access to jQuery
	//divobj: DOM reference to DIV being expanded/ collapsed. Use "divobj.id" to get its ID
	//state: "block" or "none", depending on state
    }
    animatedcollapse.init()
    
    var img=document.getElementById(imgID).src;
    var imgName=img.substr(img.lastIndexOf('/')+1);
    
    if(imgName=="minus.gif")
    {
        document.getElementById(imgID).src='images/plus.gif';
        document.getElementById(imgID).title="Click here to collapse this block"
    }
    else
    {
        document.getElementById(imgID).src='images/minus.gif';
        document.getElementById(imgID).title="Click here to collapse this block"
    }
    animatedcollapse.toggle(divName)
}


function openWindowNoResize(win,width,height)
{   
    var width = width;
    var height = height;

    var left = parseInt((screen.availWidth/2) - (width/2));
    var top = parseInt((screen.availHeight/2) - (height/2));
    var windowFeatures = "width=" + width + ",height=" + height + ",resizable=0,scrollbars=1,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;

    
  window.open (win,"clientwindow",windowFeatures); 
  return false;
}  



  function chkCnfrm(fld){
      rstRd();
      document.frmRequest.Rejection_Reason.value = ""
      document.getElementById("trService_Level").style.display = "none"
      document.getElementById("trRejection_Reason").style.display = "none"      
      if (fld[fld.selectedIndex].value == "Yes"){
       document.getElementById("trService_Level").style.display = (document.all)? 'block' : 'table-row';
      }
      else if (fld[fld.selectedIndex].value == "No"){
       document.getElementById("trRejection_Reason").style.display = (document.all)? 'block' : 'table-row';
      }
     }
     
     function rstRd(){
      for (var i=0;i<4;i++){
       document.frmRequest.Service_Level[i].checked=false
       document.frmRequest.Service_Courteous[i].checked=false
       document.frmRequest.Service_Knowledgeable[i].checked=false
       document.frmRequest.Service_Timeframe[i].checked=false
      }
     }

     function ichkSrvc(fld){
      for (var i=0;i<4;i++){
       if (fld[i].checked===true) return true;
      }
      return false;
     }

     function validateInfo1(frm){
    
      var rplc = new RegExp (' ', 'gi')
      if (document.frmRequest.Confirm_Status.selectedIndex == 0){
       alert('Please select the resolution status.')
       document.frmRequest.Confirm_Status.focus()
       return false
      }
      if (document.frmRequest.Confirm_Status[document.frmRequest.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Level) == false){
       alert('Please select service level.')
       document.frmRequest.Service_Level[0].focus()
       return false
      }
      if (document.frmRequest.Confirm_Status[document.frmRequest.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Courteous) == false){
       alert('Please select whether contact was helpful and courteous.')
       document.frmRequest.Service_Courteous[0].focus()
       return false
      }
      if (document.frmRequest.Confirm_Status[document.frmRequest.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Knowledgeable) == false){
       alert('Please select whether contact was knowledgeable and able to get you answers to your questions.')
       document.frmRequest.Service_Knowledgeable[0].focus()
       return false
      }
      if (document.frmRequest.Confirm_Status[document.frmRequest.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Timeframe) == false){
       alert('Please select whether your questions answered within the timeframe anticipated.')
       document.frmRequest.Service_Timeframe[0].focus()
       return false
      }
      if (document.frmRequest.Confirm_Status[document.frmRequest.Confirm_Status.selectedIndex].value == 'No' && document.frmRequest.Rejection_Reason.value.replace(rplc,'') == ''){
       alert('Please enter the rejection reason.')
       document.frmRequest.Rejection_Reason.focus()
       return false
      }
      if (document.frmRequest.Rejection_Reason.value.length>1500){
       alert('You cannot enter more than 1500 characte in the rejection reason field.')
       document.frmRequest.Rejection_Reason.focus()
       return false
      }
      else return true
     }

     function validateThis(frm){
      var rplc = new RegExp (' ', 'gi')
      if (frm.UserInput.value.replace(rplc,'') == ''){
       alert('Please enter description.')
       frm.UserInput.focus()
       return false
      }
      else{
       return true;
      }
     }
     
     
     
     
     
     
//Code added for <cares ticket #192225> By <shashank.sharma> on date <12/09/2014> start
function validateThisBJ(frm)
{
     
//if (frm[fldStrtNm+'ticket_area'].value=='MIS-IT')
//{
    if ( (document.frmRequest.ddlFeature[0].checked==false) && (document.frmRequest.ddlFeature[1].checked==false) )
    {
    alert("Please select any value for 'Are you requesting a feature enhancement?'")
    document.frmRequest.ddlFeature[0].focus()
    return false   
    }
    
    if ( (document.frmRequest.ddlProcessChange[0].checked==false) && (document.frmRequest.ddlProcessChange[1].checked==false) )
    {
    alert("Please select any value for 'Is this ticket the result of a regulatory/policy/process change?'")
    document.frmRequest.ddlProcessChange[0].focus()
    return false   
    }
    
    if ( (document.frmRequest.ddlSystemCreation[0].checked==false) && (document.frmRequest.ddlSystemCreation[1].checked==false) )
    {
    alert("Please select any value for 'Are you requesting creation or change of an application/system?'")
    document.frmRequest.ddlSystemCreation[0].focus()
    return false   
    }
    
    if ( (document.frmRequest.ddlReportCreation[0].checked==false) && (document.frmRequest.ddlReportCreation[1].checked==false) )
    {
    alert("Please select any value for 'Are you requesting creation or change of report(s)?'")
    document.frmRequest.ddlReportCreation[0].focus()
    return false   
    }
    
   if ( (document.frmRequest.ddlFeature[0].checked==true) || (document.frmRequest.ddlProcessChange[0].checked==true) || (document.frmRequest.ddlSystemCreation[0].checked==true) || (document.frmRequest.ddlReportCreation[0].checked==true) )
   {
       if (document.frmRequest.txtBusinessJust.value=="")
       {
            alert("Please enter Business Justification")
            document.frmRequest.txtBusinessJust.focus()
            return false
       }
   }
   
   if ( (document.frmRequest.ddlFeature[0].checked==true) || (document.frmRequest.ddlProcessChange[0].checked==true) || (document.frmRequest.ddlSystemCreation[0].checked==true) || (document.frmRequest.ddlReportCreation[0].checked==true) )
   {
        if ( (document.frmRequest.ddlRiskMitigation[0].checked==false) && (document.frmRequest.ddlRiskMitigation[1].checked==false) )
        {
            alert("Please select any value for 'Will this result in Risk Mitigation for Collabera?'")
            document.frmRequest.ddlRiskMitigation[0].focus()
            return false   
        }
        
        if ( (document.frmRequest.ddlDollarSaving[0].checked==false) && (document.frmRequest.ddlDollarSaving[1].checked==false) )
        {
            alert("Please select any value for 'Will this result in Cost Savings for Collabera? '")
            document.frmRequest.ddlDollarSaving[0].focus()
            return false   
        }
        
        if ( (document.frmRequest.ddlPersonHoursSaving[0].checked==false) && (document.frmRequest.ddlPersonHoursSaving[1].checked==false) )
        {
            alert("Please select any value for 'Will this result in Person Hours Savings for Collabera?'")
            document.frmRequest.ddlPersonHoursSaving[0].focus()
            return false   
        }
   }
   
   if ( (document.frmRequest.ddlFeature[0].checked==true) || (document.frmRequest.ddlProcessChange[0].checked==true) || (document.frmRequest.ddlSystemCreation[0].checked==true) || (document.frmRequest.ddlReportCreation[0].checked==true) )
   {
       if ( (document.frmRequest.ddlDollarSaving[1].checked==true) && (document.frmRequest.ddlPersonHoursSaving[1].checked==true) )
       {
        alert("Either Dollar Saving Or Hours Saving should be Yes.")        
        document.frmRequest.ddlDollarSaving[1].focus()
        return false
       }
   }
   
    if ( (document.frmRequest.ddlDollarSaving[0].checked == true) &&  ((document.frmRequest.txtDollarSaving.value =='') || (parseFloat(document.frmRequest.txtDollarSaving.value)==0)))
    {
        alert("Please enter $ Amount. It should be greater than zero.")
        document.frmRequest.txtDollarSaving.focus()
        return false
    }

    if ( (document.frmRequest.ddlDollarSaving[0].checked == true) &&  ((document.frmRequest.ddlPerDollar.selectedIndex == 0)))
    {
        alert("Please select Per $ Saving.")
        document.frmRequest.ddlPerDollar.focus()
        return false
    }
  
    if ( (document.frmRequest.ddlPersonHoursSaving[0].checked == true) &&  ((document.frmRequest.txtHoursSaving.value =='') || (parseFloat(document.frmRequest.txtHoursSaving.value)==0)))
    {
        alert("Please enter # Hours. It should be greater than zero.")
        document.frmRequest.txtHoursSaving.focus()
        return false
    }
    
    if ( (document.frmRequest.ddlPersonHoursSaving[0].checked == true) &&  (document.frmRequest.ddlPeriod.selectedIndex == 0))
    {
        alert("Please select period")
        document.frmRequest.ddlPeriod.focus()
        return false
    }
   
    if ( (document.frmRequest.ddlClientRequest[0].checked==false) && (document.frmRequest.ddlClientRequest[1].checked==false) )
    {
    alert("Please select any value for 'Is this required for any urgent client request/client audit?'")
    document.frmRequest.ddlClientRequest[0].focus()
    return false   
    }
        
    if ( (document.frmRequest.ddlCrititcal[0].checked==false) && (document.frmRequest.ddlCrititcal[1].checked==false) )
    {
    alert("Please select any value for 'Is this required to address a critical legal/regulatory/government audit or compliance?'")
    document.frmRequest.ddlCrititcal[0].focus()
    return false   
    }
        
    if ( (document.frmRequest.ddlStrategic[0].checked==false) && (document.frmRequest.ddlStrategic[1].checked==false) )
    {
    alert("Please select any value for 'Is this a strategic company initiative?'")
    document.frmRequest.ddlStrategic[0].focus()
    return false   
    }
    
}

//Code added for <cares ticket #192225> By <shashank.sharma> on date <12/09/2014> end


function GetSubArea(e)
{
  var Command = e +"|area";  
  var context = new Object();
  context.CommandName = "area";    
  //document.getElementById("hdnArea_new").value = e;
  WebForm_DoCallback('__Page',Command,CallBackHandler,context,null,false);
}

//function GetPossibleRootCause(e)
//{
//  var Command = document.getElementById("hdnArea_new").value + "|" +  e +"|subarea";  
//  var context = new Object();
//  context.CommandName = "subarea";    
//  WebForm_DoCallback('__Page',Command,CallBackHandlerPossible,context,null,false);
//}


function CallBackHandler(result, context) 
{
    var objID="ddl_ticket_sub_area";
    if (document.getElementById(objID)==undefined)
        objID="ddl_ticket_sub_area";
        
    var objIDSec="ddlSecSubArea";
    if (document.getElementById(objIDSec)==undefined)
        objIDSec="ddlSecSubArea";
    
     var objIDTer="ddlTertiarySubArea";
    if (document.getElementById(objIDTer)==undefined)
        objIDTer="ddlTertiarySubArea";
        
        

    while (document.getElementById(objID).length>0)
    {
        document.getElementById(objID).options[0]=null;
    }
     
     while (document.getElementById(objIDSec).length>0)
    {
        document.getElementById(objIDSec).options[0]=null;
    }   
    
      while (document.getElementById(objIDTer).length>0)
    {
        document.getElementById(objIDTer).options[0]=null;
    }   
    
    var Item;      
    Item = result.split(',');
    
    
    if (result == "MIS-IT")
    {     
       document.getElementById(objID).options[0]= new Option('MIS-IT','MIS-IT');
       document.getElementById(objID).options[0].title='MIS-IT';
       document.getElementById(objIDSec).options[0]= new Option('Select','');
       document.getElementById(objIDSec).options[0].title='Select';
         document.getElementById(objIDSec).options[1]= new Option('MIS-IT','MIS-IT');
       document.getElementById(objIDSec).options[1].title='MIS-IT';
       document.getElementById(objIDTer).options[0]= new Option('Select','');
       document.getElementById(objIDTer).options[0].title='Select';
       
         document.getElementById(objIDTer).options[1]= new Option('MIS-IT','MIS-IT');
       document.getElementById(objIDTer).options[1].title='MIS-IT';
       return;
    }
    
    
    if(Item.length>=1)
    {
        document.getElementById(objID).options[0]= new Option("Select","");
        document.getElementById(objIDSec).options[0]= new Option("Select","");
        document.getElementById(objIDTer).options[0]= new Option("Select","");
      
        for (var i = 0; i < Item.length; ++i)
        { 
           document.getElementById(objID).options[i+1]= new Option(Item[i].replace('+',','),Item[i].replace('+',','));
           document.getElementById(objID).options[i+1].title=Item[i].replace('+',',');
                     
           document.getElementById(objIDSec).options[i+1]= new Option(Item[i].replace('+',','),Item[i].replace('+',','));
           document.getElementById(objIDSec).options[i+1].title=Item[i].replace('+',',');
           
           document.getElementById(objIDTer).options[i+1]= new Option(Item[i].replace('+',','),Item[i].replace('+',','));
           document.getElementById(objIDTer).options[i+1].title=Item[i].replace('+',',');                  
         }  
    }
   
   document.getElementById(objID).options[i+1]= new Option("MIS-IT","MIS-IT");
   document.getElementById(objID).options[i+1].title="MIS-IT";
  
   document.getElementById(objIDSec).options[i+1]= new Option("MIS-IT","MIS-IT");
   document.getElementById(objIDSec).options[i+1].title="MIS-IT"; 
   
   document.getElementById(objIDTer).options[i+1]= new Option("MIS-IT","MIS-IT");
   document.getElementById(objIDTer).options[i+1].title="MIS-IT";   
         
 }
 
 
 
 
// function CallBackHandlerPossible(result, context) 
//{
//    var objID="ddlPossibleRootCause";
//    if (document.getElementById(objID)==undefined)
//        objID="ddlPossibleRootCause";
//               

//    while (document.getElementById(objID).length>0)
//    {
//        document.getElementById(objID).options[0]=null;
//    }
//     
//      
//    var Item;      
//    Item = result.split(',');
//        
//    if(Item.length>=1)
//    {
//        document.getElementById(objID).options[0]= new Option("Select","");
//           
//        for (var i = 0; i < Item.length; ++i)
//        { 
//           document.getElementById(objID).options[i+1]= new Option(Item[i],Item[i]);
//           document.getElementById(objID).options[i+1].title=Item[i];
//         }  
//    }
//         
// }
// 
// 

function SetSubArea(subarea)
{ 
    document.getElementById('hdnSubAreaNew').value = subarea;
     //----------------------------------------//added by mitesh on 25/11/2015------------------------
            if (document.getElementById("ddl_ticket_area").value == "Desktop" && document.getElementById("ddl_ticket_sub_area").value == "Shifting")
            {
            document.getElementById("trshiftdevice").style.display='inline';   
            }
            else
            {
            document.getElementById("trshiftdevice").style.display='none';  
            }
            //--------------------------------------------------------------------------------------------
}

function updatePossible(possibleRootCause)
{
    document.getElementById('hdnPossibleRootCause').value = possibleRootCause;
}