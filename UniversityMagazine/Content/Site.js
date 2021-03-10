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

function EditFaculty(Id) {
    var url = "/Management/Faculty/Edit?fACULTY_Id=" + Id;
    $("#myModalBodyDivEdit").load(url, function () {
        $("#MyModalEdit").modal();
    });
};
//#endregion Faculty

if (document.getElementById("check") != null) {
    document.getElementById("check").onclick = function () {
        var checkboxes = document.getElementsByName('chkId[]');
        if (document.getElementById("check").checked) {
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = true;
            }
        } else {
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = false;
            }
        }
    };
}

function filterSelection(c) {
    var x, i;
    x = document.getElementsByClassName("filterDiv");
    if (c == "all") c = "";
    // Add the "show" class (display:block) to the filtered elements, and remove the "show" class from the elements that are not selected
    for (i = 0; i < x.length; i++) {
        w3RemoveClass(x[i], "show");
        if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
    }
}

