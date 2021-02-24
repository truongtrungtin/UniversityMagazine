$('#toast-container').removeClass('hide');
$('#toast-container').delay(2000).slideUp(300);

//Kiểm tra Delete
$("#Delete").click(function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (!confirm("Are you sure you want to delete?")) {
            return false;
        }
    } else {
        alert("No value is selected for deletion !!");
        return false;
    }
});

//#region Role
var CreateRole = function () {
    var url = "/Credential/Role/Create";
    $("#myModalBodyDivCreate").load(url, function () {
        $("#MyModalCreate").modal();
    });
};

var EditRole = function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (checkboxes == 1) {
            var Id = $('input[name="chkId[]"]:checked').val()
            var url = "/Credential/Role/Edit?rOLE_Id=" + Id;
            $("#myModalBodyDivEdit").load(url, function () {
                $("#MyModalEdit").modal();
            });
        } else {
            alert("Chỉ chọn một giá trị!");
            return false;
        }
    } else {
        alert("Không có giá trị nào được chọn!!");
        return false;
    }
};
//#endregion Role

//#region Role Group
var CreateRoleGroup = function () {
    var url = "/Credential/RoleGroup/Create";
    $("#myModalBodyDivCreate").load(url, function () {
        $("#MyModalCreate").modal();
    });
};

var EditRoleGroup = function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (checkboxes == 1) {
            var Id = $('input[name="chkId[]"]:checked').val()
            var url = "/Credential/RoleGroup/Edit?rOLEGROUP_Id=" + Id;
            $("#myModalBodyDivEdit").load(url, function () {
                $("#MyModalEdit").modal();
            });
        } else {
            alert("Chỉ chọn một giá trị!");
            return false;
        }
    } else {
        alert("Không có giá trị nào được chọn!!");
        return false;
    }
};


var Credential = function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (checkboxes == 1) {
            var Id = $('input[name="chkId[]"]:checked').val()
            var url = "/Credential/RoleGroup/Credential?rOLEGROUP_Id=" + Id;
            $("#myModalBodyDivCredential").load(url, function () {
                $("#MyModalCredential").modal();
            });
        } else {
            alert("Chỉ chọn một giá trị!");
            return false;
        }
    } else {
        alert("Không có giá trị nào được chọn!!");
        return false;
    }
};
//#endregion Role Group

//#region Account
var CreateAccount = function () {
    var url = "/Management/Account/Create";
    $("#myModalBodyDivCreate").load(url, function () {
        $("#MyModalCreate").modal();
    });
};

var EditAccount = function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (checkboxes == 1) {
            var Id = $('input[name="chkId[]"]:checked').val()
            var url = "/Management/Account/Edit?aCCOUNT_Id=" + Id;
            $("#myModalBodyDivEdit").load(url, function () {
                $("#MyModalEdit").modal();
            });
        } else {
            alert("Chỉ chọn một giá trị!");
            return false;
        }
    } else {
        alert("Không có giá trị nào được chọn!!");
        return false;
    }
};
//#endregion Account

//#region Faculty
var CreateFaculty = function () {
    var url = "/Management/Faculty/Create";
    $("#myModalBodyDivCreate").load(url, function () {
        $("#MyModalCreate").modal();
    });
};

var EditFaculty = function () {
    var checkboxes = document.querySelectorAll('input[name="chkId[]"]:checked').length;
    if (checkboxes > 0) {
        if (checkboxes == 1) {
            var Id = $('input[name="chkId[]"]:checked').val()
            var url = "/Management/Faculty/Edit?fACULTY_Id=" + Id;
            $("#myModalBodyDivEdit").load(url, function () {
                $("#MyModalEdit").modal();
            });
        } else {
            alert("Chỉ chọn một giá trị!");
            return false;
        }
    } else {
        alert("Không có giá trị nào được chọn!!");
        return false;
    }
};
//#endregion Faculty