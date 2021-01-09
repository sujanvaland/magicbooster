 function validateVls(frm){
    var txtEmailValue;
    var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\/~`-=";
    function check(string) {
        for (i = 0; i < specialChars.length; i++) {
            if (string.indexOf(specialChars[i]) > -1) {
                return true
            }
        }
        return false;
    }

    var rplc = new RegExp (' ', 'gi');
    var isEmail  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if ($('#ddlArea').val() == "0") {
        toastr.error("Please select area");
        $('#ddlArea').focus();
        $('#ddlArea').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false
    }
    else{
        $('#ddlArea').removeAttr('style');
    }

    if ($('#ddlSubArea').val() == "0" && ($('#ddlSubArea').val() != 'MIS-IT')) {
        toastr.error("<b style='color:black'>SubArea</b>: Please select sub area");
        $('#ddlSubArea').focus();
        $('#ddlSubArea').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false
    }
    else{
        $('#ddlSubArea').removeAttr('style');
    }

    if (($("#ddlArea option:selected").text() == 'Candidate Referrals') && ($("#ddlSubArea option:selected").text() == 'Request to find Assignment')) {
        if ($('#txtCandidateName').val() == '') {
            toastr.error("<b style='color:black'>Candidate name</b>: Please enter candidate name.");
            $('#txtCandidateName').focus();
            $('#txtCandidateName').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtCandidateName').removeAttr('style');
        }
        if (check($('#txtCandidateName').val())) {
            toastr.error("<b style='color:black'>Candidate Name</b>: Candidate name cannot contain illegal characters");
            $('#txtCandidateName').focus();
            $('#txtCandidateName').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtCandidateName').removeAttr('style');
        }
        if ($('#txtReferral').val() == '') {
            toastr.error("<b style='color:black'>Candidate Email</b>: Please enter candidate email.");
            $('#txtReferral').focus();
            $('#txtReferral').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtReferral').removeAttr('style');
        }
        if ($('#txtReferral').val() != '') {
            var text = $('#txtReferral').val();
            txtEmailValue = $('#txtReferral').val($('#txtReferral').val().replace(',', ';'));
            var toids = $('#txtReferral').val().split(';');
            for (i = 0; i < toids.length; i++) {
                var email = toids[i];
                var atpos = email.indexOf("@");
                var dotpos = email.lastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
                    toastr.error("<b style='color:black'>Email</b>: Please enter valid email for Candidate Email");
                    $('#txtReferral').focus();
                    $('#txtReferral').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                    return false;
                }
                else {
                    $('#txtReferral').removeAttr('style');
                }
            }
        }
        if ($('#txtRefByEmail').val() == '') {
            toastr.error("<b style='color:black'>Email</b>: Please enter referred by email");
            $('#txtRefByEmail').focus();
            $('#txtRefByEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtRefByEmail').removeAttr('style');
        }
        if ($('#txtRefByEmail').val() != '') {
            var text = $('#txtRefByEmail').val();
            txtEmailValue = $('#txtRefByEmail').val($('#txtRefByEmail').val().replace(',', ';'));
            var toids = $('#txtRefByEmail').val().split(';');
            for (i = 0; i < toids.length; i++) {
                var email = toids[i];
                var atpos = email.indexOf("@");
                var dotpos = email.lastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
                    toastr.error("<b style='color:black'>Email</b>: Please enter valid email for Ref. By Email");
                    $('#txtRefByEmail').focus();
                    $('#txtRefByEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                    return false;
                }
                else {
                    $('#txtRefByEmail').removeAttr('style');
                }
            }
        }
    }

    if ($('#txtSubject').val() == "") {
        toastr.error("<b style='color:black'>Subject</b>: Please enter subject");
        $('#txtSubject').focus();
        $('#txtSubject').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false    
    }
    else{
        $('#txtSubject').removeAttr('style');
    }

    if (check($('#txtSubject').val())) {
        toastr.error("<b style='color:black'>Subject</b>: Please enter alphanumeric characters only ");
        $('#txtSubject').focus();
        $('#txtSubject').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else{
        $('#txtSubject').removeAttr('style');
    }

    if ($('#txtDescription').val() == "") {
        toastr.error("<b style='color:black'>Description</b>: Please enter description");
        $('#txtDescription').focus();
        $('#txtDescription').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false
    }
    else{
        $('#txtDescription').removeAttr('style');
    }

    if (($("#ddlArea option:selected").text() == 'MIS-IT') && ($("#ddlSubArea option:selected").text() == 'MIS-IT'))
    {
        if (($('#txtLocationDetails').val() == ""))
        {
            toastr.error("<b style='color:black'>Description</b>: Please enter Location");
            $('#txtLocationDetails').focus()
            $('#txtLocationDetails').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtLocationDetails').removeAttr('style');
        }
        if (check($('#txtLocationDetails').val())) {
            toastr.error("<b style='color:black'>Location</b>: Please enter alphanumeric characters only ");
            $('#txtLocationDetails').focus();
            $('#txtLocationDetails').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtLocationDetails').removeAttr('style');
        }
        if (($('#txtNoOfImpUsers').val()== "")) {
            toastr.error("<b style='color:black'>Impacted Users</b>: Please enter Number of impacted users.");
            $('#txtNoOfImpUsers').focus()
            $('#txtNoOfImpUsers').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtNoOfImpUsers').removeAttr('style');
        }
        if (check($('#txtNoOfImpUsers').val())) {
            toastr.error("<b style='color:black'>Impacted Users</b>: Please enter alphanumeric characters only ");
            $('#txtNoOfImpUsers').focus();
            $('#txtNoOfImpUsers').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtNoOfImpUsers').removeAttr('style');
        }
        if (($('#ddlentity').val() == "")) {
            toastr.error("<b style='color:black'>Entity</b>: Please select Entity.");
            $('#ddlentity').focus()
            $('#ddlentity').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#ddlentity').removeAttr('style');
        }
    }

    if ($("#ddlArea option:selected").text() == 'MIS-IT' || $("#ddlArea option:selected").text() == 'MIS-Internal applications')
    {
        if ($('#ddlFeature').is(":checked") || $('#ddlProcessChange').is(":checked") || $('#ddlSystemCreation').is(":checked") || $('#ddlReportCreation').is(":checked")) {
            if ($('#txtBusinessJust').val() == "") {
                toastr.error("<b style='color:black'>Business Justification</b>: Please enter Business Justification");
                $('#txtBusinessJust').focus();
                $('#txtBusinessJust').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                return false
            }
            else {
                $('#txtBusinessJust').removeAttr('style');
            }
            if (check($('#txtBusinessJust').val())) {
                toastr.error("<b style='color:black'>Business Justification</b>: Please enter alphanumeric characters only ");
                $('#txtBusinessJust').focus();
                $('#txtBusinessJust').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                return false;
            }
            else {
                $('#txtBusinessJust').removeAttr('style');
            }
        }

        if ($('#ddlFeature').is(":checked") || $('#ddlProcessChange').is(":checked") || $('#ddlSystemCreation').is(":checked") || $('#ddlReportCreation').is(":checked")) {
            if ($('#ddlDollarSaving').is(":checked") == false && $('#ddlPersonHoursSaving').is(":checked") == false) {
                toastr.error("<b style='color:black'>Dollar Saving</b>: Either Dollar Saving Or Hours Saving should be tick.");
                return false
            }
            else {
                $('#ddlDollarSaving').removeAttr('style');
            }
        }

        if ($('#ddlDollarSaving').is(":checked") && ($('#txtDollarSaving').val() == '' || $('#txtDollarSaving').val() == "0")) {
            toastr.error("<b style='color:black'>Dollar Saving</b>:Please enter $ Amount. It should be greater than zero.");
            $('#txtDollarSaving').focus()
            $('#txtDollarSaving').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#ddlDollarSaving').removeAttr('style');
        }

        if ($('#ddlDollarSaving').is(":checked") && (($('#ddlPerDollar').val() == ""))) {
            toastr.error("<b style='color:black'>Dollar Saving</b>: Please select Per $ Saving.");
            $('#ddlPerDollar').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#ddlPerDollar').removeAttr('style');
        }
        if ($('#ddlPersonHoursSaving').is(":checked") && (($('#txtHoursSaving').val() == '') || (parseFloat($('#txtHoursSaving').val()) == 0))) {
            toastr.error("<b style='color:black'>Person Hour Saving</b>: Please enter # Hours. It should be greater than zero.");
            $('#txtHoursSaving').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtHoursSaving').removeAttr('style');
        }

        if (check($('#txtHoursSaving').val())) {
            toastr.error("<b style='color:black'>Hours Saving</b>: Please enter alphanumeric characters only ");
            $('#txtHoursSaving').focus();
            $('#txtHoursSaving').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtHoursSaving').removeAttr('style');
        }
        if ($('#ddlPersonHoursSaving').is(":checked") && ($('#ddlPeriod').val() == 0)) {
            toastr.error("<b style='color:black'>Period</b>: Please select period");
            $('#ddlPeriod').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#ddlPeriodtxtHoursSaving').removeAttr('style');
        }
    }

    if ($('#fileTempUpload').val() != "")
    {
        if($('#ddlAttachment').val() =="")
        {
            toastr.error("<b style='color:black'>Attachment</b>: Select file type");
            $('#ddlAttachment').focus();
            $('#ddlAttachment').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        if(!$('#divgr').is(":visible"))
        {
            toastr.error("<b style='color:black'>Attachment</b>: Click upload button to add an attachment");
            $('#fileTempUpload').focus();
            $('#fileTempUpload').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
    }
    if ($('#ddlAttachment').val() != "") 
    {
        if ($('#fileTempUpload').val() == "")
        {
            toastr.error("<b style='color:black'>Attachment</b>: Select File to upload");
            $('#fileTempUpload').focus();
            $('#fileTempUpload').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
    }
    
    if (check($('#txtContactName').val())) {
        toastr.error("<b style='color:black'>Name</b>: Please enter alphanumeric characters only ");
        $('#txtContactName').focus();
        $('#txtContactName').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtContactName').removeAttr('style');
    }
    if (check($('#txtContactPhone').val())) {
        toastr.error("<b style='color:black'>Phone</b>: Please enter alphanumeric characters only ");
        $('#txtContactPhone').focus();
        $('#txtContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtContactPhone').removeAttr('style');
    }
    if ($('#txtContactPhone').val() == "") {
        toastr.error("<b style='color:black'>Phone</b>: Please enter phone");
        $('#txtContactPhone').focus()
        $('#txtContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
       return false   
    }
    else {
        $('#txtContactPhone').removeAttr('style');
    }
    if ($('#txtContactPhone').val().length > 10) {
        toastr.error("<b style='color:black'>Phone</b>: Phone Number cannot be more then 10 digit");
        $('#txtContactPhone').focus();
        $('#txtContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtContactPhone').removeAttr('style');
    }
    if ($('#txtContactPhone').val().length < 10) {
        toastr.error("<b style='color:black'>Phone</b>: Phone Number cannot be less then 10 digit");
        $('#txtContactPhone').focus();
        $('#txtContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtContactPhone').removeAttr('style');
    }
    if ($('#txtVOIP').val().length > 4) {
        toastr.error("<b style='color:black'>VOIP</b>: VOIP cannot be more then 4 digit");
        $('#txtVOIP').focus();
        $('#txtVOIP').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtVOIP').removeAttr('style');
    }
    if ($('#txtVOIP').val() != "" && $('#txtVOIP').val().length < 4) {
        toastr.error("<b style='color:black'>VOIP</b>: VOIP cannot be less then 4 digit");
        $('#txtVOIP').focus();
        $('#txtVOIP').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false;
    }
    else {
        $('#txtVOIP').removeAttr('style');
    }
    if ($('#txtContactEmail').val() == "") {
        toastr.error("<b style='color:black'>Email</b>: Please enter Email ID");
        $('#txtContactEmail').focus()
        $('#txtContactEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
        return false
    }
    else {
        $('#txtContactEmail').removeAttr('style');
    }
    if ($('#txtContactEmail').val() != '') {
        var text = $('#txtContactEmail').val();
        txtEmailValue = $('#txtContactEmail').val($('#txtContactEmail').val().replace(',', ';'));
        var toids = $('#txtContactEmail').val().split(';');
        for (i = 0; i < toids.length; i++) {
            var email = toids[i];
            var atpos = email.indexOf("@");
            var dotpos = email.lastIndexOf(".");
            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
                toastr.error("<b style='color:black'>Email</b>: Please enter valid email for contact Email");
                $('#txtContactEmail').focus();
                $('#txtContactEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                return false;
            }
            else {
                $('#txtContactEmail').removeAttr('style');
            }
        }
    }
    debugger;
    if ($('#trBranch').is(":visible")) {
        if ($('#ddlBranch').val() == "") {
            toastr.error("<b style='color:black'>Branch</b>: Select branch");
            $('#ddlBranch').focus()
            $('#ddlBranch').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
    }
    if ($('#panelOnBhalfContact').is(":visible")) {
        if (check($('#txtOnBehalfContactName').val())) {
            toastr.error("<b style='color:black'>Name</b>: Please enter alphanumeric characters only ");
            $('#txtOnBehalfContactName').focus();
            $('#txtOnBehalfContactName').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfContactName').removeAttr('style');
        }
        if (check($('#txtOnBehalfContactPhone').val())) {
            toastr.error("<b style='color:black'>Phone</b>: Please enter alphanumeric characters only ");
            $('#txtOnBehalfContactPhone').focus();
            $('#txtOnBehalfContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfContactPhone').removeAttr('style');
        }
        if ($('#txtOnBehalfContactPhone').val().length > 10) {
            toastr.error("<b style='color:black'>Phone</b>: On Behalf of Contact Number cannot be more then 10 digit");
            $('#txtOnBehalfContactPhone').focus();
            $('#txtOnBehalfContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfContactPhone').removeAttr('style');
        }
        if ($('#txtOnBehalfContactPhone').val().length < 10) {
            toastr.error("<b style='color:black'>Phone</b>: On Behalf of Contact Number cannot be less then 10 digit");
            $('#txtOnBehalfContactPhone').focus();
            $('#txtOnBehalfContactPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfContactPhone').removeAttr('style');
        }
        if ($('#txtOnBehalfVOIP').val().length > 4) {
            toastr.error("<b style='color:black'>VOIP</b>: On Behalf of VOIP cannot be more then 4 digit");
            $('#txtOnBehalfVOIP').focus();
            $('#txtOnBehalfVOIP').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfVOIP').removeAttr('style');
        }
        if ($('#txtOnBehalfVOIP').val() != "" && $('#txtOnBehalfVOIP').val().length < 4) {
            toastr.error("<b style='color:black'>VOIP</b>: On Behalf of VOIP cannot be less then 4 digit");
            $('#txtOnBehalfVOIP').focus();
            $('#txtOnBehalfVOIP').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtOnBehalfVOIP').removeAttr('style');
        }
        if ($('#txtOnBehalfContactEmail').val() != '') {
            var text = $('#txtOnBehalfContactEmail').val();
            txtEmailValue = $('#txtOnBehalfContactEmail').val($('#txtOnBehalfContactEmail').val().replace(',', ';'));
            var toids = $('#txtOnBehalfContactEmail').val().split(';');
            for (i = 0; i < toids.length; i++) {
                var email = toids[i];
                var atpos = email.indexOf("@");
                var dotpos = email.lastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
                    toastr.error("<b style='color:black'>Email</b>: Please enter valid email for On Behalf of contact Email");
                    $('#txtOnBehalfContactEmail').focus();
                    $('#txtOnBehalfContactEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                    return false;
                }
                else {
                    $('#txtOnBehalfContactEmail').removeAttr('style');
                }
            }
        }
    }

    
    if ($('#panelBussinessExcellenc').is(":visible"))
    {
        if ($('#ddlIssueRequestFor').val() == "")
        {
            toastr.error("<b style='color:black'>Issue Request For</b>: Please select issue request for.");
            $('#ddlIssueRequestFor').focus();
            $('#ddlIssueRequestFor').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#ddlIssueRequestFor').removeAttr('style');
        }
        if ($('#txtReportedByPhone').val() == "") {
            toastr.error("<b style='color:black'>Phone</b>: Please enter reported by phone.");
            $('#txtReportedByPhone').focus();
            $('#txtReportedByPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtReportedByPhone').removeAttr('style');
        }
        if ($('#txtReportedByPhone').val().length > 10) {
            toastr.error("<b style='color:black'>Phone</b>: Reported By Phone Number cannot be more then 10 digit");
            $('#txtReportedByPhone').focus();
            $('#txtReportedByPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtReportedByPhone').removeAttr('style');
        }
        if ($('#txtReportedByPhone').val().length < 10) {
            toastr.error("<b style='color:black'>Phone</b>: Reported By Phone Number cannot be less then 10 digit");
            $('#txtReportedByPhone').focus();
            $('#txtReportedByPhone').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtReportedByPhone').removeAttr('style');
        }
        if ($('#txtReportedByEmail').val() == "") {
            toastr.error("<b style='color:black'>Email</b>: Please enter reported by email.");
            $('#txtReportedByEmail').focus();
            $('#txtReportedByEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false
        }
        else {
            $('#txtReportedByEmail').removeAttr('style');
        }
        if ($('#txtReportedByEmail').val() != '') {
            var text = $('#txtReportedByEmail').val();
            txtEmailValue = $('#txtReportedByEmail').val($('#txtReportedByEmail').val().replace(',', ';'));
            var toids = $('#txtReportedByEmail').val().split(';');
            for (i = 0; i < toids.length; i++) {
                var email = toids[i];
                var atpos = email.indexOf("@");
                var dotpos = email.lastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
                    toastr.error("<b style='color:black'>Email</b>: Please enter valid email for Reported By Email");
                    $('#txtReportedByEmail').focus();
                    $('#txtReportedByEmail').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
                    return false;
                }
                else {
                    $('#txtReportedByEmail').removeAttr('style');
                }
            }
        }
    }
    if ($('#pnlCVE1').is(":visible"))
    {
        if ($('#txtName1').val() == '') {
            toastr.error("<b style='color:black'>"+ $('#lblInfo1').text() +"</b>: Please enter " + $('#lblInfo1').text() + " using lookup button.");
            $('#txtName1').focus();
            $('#txtName1').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName1').removeAttr('style');
        }
        if (check($('#txtName1').val())) {
            toastr.error("<b style='color:black'>" + $('#lblInfo1').text() + "</b>: Please enter alphanumeric characters only ");
            $('#txtName1').focus();
            $('#txtName1').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName1').removeAttr('style');
        }
    }
    if ($('#pnlCVE2').is(":visible"))
    {
        if ($('#txtName2').val() == '') {
            toastr.error("<b style='color:black'>" + $('#lblInfo2').text() + "</b>: Please enter " + $('#lblInfo2').text() + " using lookup button.");
            $('#txtName2').focus();
            $('#txtName2').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName2').removeAttr('style');
        }
        if (check($('#txtName2').val())) {
            toastr.error("<b style='color:black'>" + $('#lblInfo2').text() + "</b>: Please enter alphanumeric characters only ");
            $('#txtName2').focus();
            $('#txtName2').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName2').removeAttr('style');
        }
    }
    if ($('#pnlCVE3').is(":visible"))
    {
        if ($('#txtName3').val() == '') {
            toastr.error("<b style='color:black'>" + $('#lblInfo3').text() + "</b>: Please enter " + $('#lblInfo3').text() + " using lookup button.");
            $('#txtName3').focus();
            $('#txtName3').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName3').removeAttr('style');
        }
        if (check($('#txtName3').val())) {
            toastr.error("<b style='color:black'>" + $('#lblInfo3').text() + "</b>: Please enter alphanumeric characters only ");
            $('#txtName3').focus();
            $('#txtName3').attr('style', "border-radius: 5px; border:#FF0000 1px solid;");
            return false;
        }
        else {
            $('#txtName3').removeAttr('style');
        }
    }
    
    if (IsSelected(frm, $('#ddlGCILocations'), 'location') == false) {
        return false
    }
    else if (IsSelected(frm, $('ddlAssignedTo'), 'user') == false) {
        return false
    }
    else{
        return true;
    }
 }
 
function IsSelected(frm, fld, msg){
 if(!frm[fld]){
  return true;
 }
 else {
  if (frm[fld].selectedIndex == 0){
   alert("Please select " + msg)
   frm[fld].focus()
   return false;
  }
  else return true;
 }
} 



