
     function ichkSrvc(fld){
      for (var i=0;i<4;i++){
       if (fld[i].checked===true) return true;
      }
      return false;
     }

     function validate(frm) {
      var rplc = new RegExp (' ', 'gi')
      if (frm.Confirm_Status.selectedIndex == 0){
       alert('Please select the resolution status.')
       frm.Confirm_Status.focus()
       return false
      }
      else if (frm.Confirm_Status[frm.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Level) == false){
       alert('Please select service level.')
       frm.Service_Level[0].focus()
       return false
      }
      else if (frm.Confirm_Status[frm.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Courteous) == false){
       alert('Please select whether contact was helpful and courteous.')
       frm.Service_Courteous[0].focus()
       return false
      }
      else if (frm.Confirm_Status[frm.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Knowledgeable) == false){
       alert('Please select whether contact was knowledgeable and able to get you answers to your questions.')
       frm.Service_Knowledgeable[0].focus()
       return false
      }
      else if (frm.Confirm_Status[frm.Confirm_Status.selectedIndex].value == 'Yes' && ichkSrvc(document.frmRequest.Service_Timeframe) == false){
       alert('Please select whether your questions answered within the timeframe anticipated.')
       frm.Service_Timeframe[0].focus()
       return false
      }
      else if (frm.Confirm_Status[frm.Confirm_Status.selectedIndex].value == 'No' && frm.Rejection_Reason.value.replace(rplc,'') == ''){
       alert('Please enter the rejection reason.')
       frm.Rejection_Reason.focus()
       return false
      }
      else if (frm.Rejection_Reason.value.length>1500){
       alert('You cannot enter more than 1500 characte in the rejection reason field.')
       frm.Rejection_Reason.focus()
       return false
      }
      else return true
     }

     function validateThis(frm) {
         var rplc = new RegExp(' ', 'gi')
      if (frm.UserInput.value.replace(rplc,'') == ''){
       frm.UserInput.focus()
       return false
      }
      else{
       return true;
      }
     }