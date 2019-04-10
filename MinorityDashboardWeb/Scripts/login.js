

function EncryptionPwd() {

    var seed = document.getElementById("RandomNumer").value;
    var gid = document.getElementById("AuthToken").value;
    var objnewpwd = document.getElementById("Password");
  //  var objusername = document.getElementById("UserName");
    var saltVal = document.getElementById("UserName").value;
    var sPasswordnew = objnewpwd.value;

    if (sPasswordnew !== '') {
        var sha256Value = EncryptWithSalt(sPasswordnew, saltVal);
       
        objnewpwd.value = seed + sha256Value + gid;


        //var sha256Value = EncryptWithSalt(saltVal, saltVal);

        //objusername.value = seed + sha256Value + gid;

    }



}

function EncryptionPwdReg() {



    var seed = document.getElementById("RandomNumer").value;
    var gid = document.getElementById("AuthToken").value;

    var objnewpwd = document.getElementById("Password");
    var objnewpwdc = document.getElementById("ConfirmPassword");
   // var objusername = document.getElementById("UserName");
    var saltVal = document.getElementById("UserName").value;

    var sPasswordnew = objnewpwd.value;
    var sPasswordnewc = objnewpwdc.value;

    if (sPasswordnew !== '' && sPasswordnewc !== '' && saltVal !== '') {
        var FirstMd6Value = EncryptWithSalt(sPasswordnew, saltVal).toString();
        objnewpwd.value = seed + FirstMd6Value + gid;


        var FirstMd6Valuec = EncryptWithSalt(sPasswordnewc, saltVal).toString();
        objnewpwdc.value = seed + FirstMd6Valuec + gid;

        //var FirstMd6Valueuser = EncryptWithSalt(saltVal, saltVal).toString();
        //objusername.value = seed + FirstMd6Valueuser + gid;

    }
}