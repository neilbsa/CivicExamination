$(document).ready(function () {
    var baseUrl = $('#SystemBaseURL').val()


    checkMdlValues()
    $('.genderSelect li').on('click', function () {
        var item = $(this);
        var text = item.text();
        var input = $('#selectedGender');
        var parentInput = input.closest('.mdl-textfield--floating-label');
        var inputText = input.val(text)
        parentInput.addClass('is-dirty');
    })




    $(document).on('click','#SubmitRegisterBtn', function () {


        validateUserDetails();


    })


    function validateUserDetails() {
        var frm = $('#ExaminerData');
        var UserData = frm.serializeArray();
        var options = {
            url: baseUrl + '/Examiner/ValidateUserForRegistration',
            datatype: 'HTML',
            method: 'POST',
            data:  UserData ,
            success: function (data)
            {
                console.log(data)

                if (data.trim().length > 0) {
                    globalMethods.OpenModal("Existing Account Found", data, sbtFunc, 'Continue Anyway');
                }else {
                    frm.submit();
                }

            }
        }
        $.ajax(options)
    }

    let sbtFunc = function () {
        var frm = $('#ExaminerData');
        frm.submit();
    }

    
   function checkMdlValues() {
        $('.childValue').each(
            function () {
                var dom = $(this)
                var isChecked = dom.prop('checked')
                var childView = dom.parent().next()

       
                if (isChecked) {
                    dom.parent().addClass('is-checked')
                    childView.toggle()
                } else {
                    dom.parent().removeClass('is-checked')
                }


            }


        )
    }





    $(document).on('click', '.deleteCharRefRow', function () {
        var dom = $(this)
        var parentThumb = dom.parent().parent().parent()

        parentThumb.remove()




    })


    $(document).on('click', '.deleteProfessionalLicense', function () {
        var dom = $(this)
        var parentThumb = dom.parent().parent().parent()

        parentThumb.remove()




    })



    $(document).on('click', '.removePanel', function () {
        var dom = $(this)
        var parentThumb = dom.parent().parent()
        parentThumb.remove()
    })


    

    

    $(document).on('change','.chkBoxWithChildView', function () {
        var dom = $(this)
        var parent = dom.next()
        var inputs = parent.find('input')
        var children = dom.find('.childValue');
        parent.toggle()
        var isVisible = parent.is(':visible')
    

        if (!isVisible) {
            dom.removeClass('is-checked')
        } else {
            dom.addClass('is-checked')
        }



 
        if (dom.hasClass('is-checked')) {
            children.attr("checked", true)
        } else {
            children.attr("checked", false)
        }

        inputs.each(function () {
            var domChild = $(this)
            domChild.parent().removeClass('is-dirty')
            domChild.val('')

        })

    })


    $(document).on('click', '.selectMenu', function () {
        var dom = $(this)
        var domParent = dom.parent().parent().parent()
        var input = domParent.find('.mdl-textfield__input')
        var inputLabel = domParent.find('.mdl-textfield--floating-label')

        input.val(dom.text())
        domParent.addClass('is-dirty')


    })


    var updateEducList = function () {
        var dom = $('.educationHistories')
        var count = dom.count
        var currentCount = 1
        dom.each(function () {
            var childDom = $(this)
            var schoolLevel = childDom.find('.schoolLevel')
            var schoolLevelSelect = childDom.find('.schoolLevelSelect')
            schoolLevel.attr("Id", currentCount)
            schoolLevelSelect.attr("for", currentCount)
            currentCount = currentCount + 1
        })
    }

    $('#addEducAttainment').on('click', function () {
        var domList = $('#educAttainmentList')
        var options = {
            url: baseUrl + '/Examiner/GetView',
            datatype: 'HTML',
            data: { viewName: 'EducationAttained' },
            success: function (data) {
           
                domList.append(data)
                $.validator.unobtrusive.parse(data);
                updateEducList()
                componentHandler.upgradeDom()
            }
        }

        $.ajax(options)
    
    })

    $('#addProLicenses').on('click', function () {

        var domList = $('#prolicensesList')
        var options = {
            url: baseUrl + '/Examiner/GetView',
            datatype: 'HTML',
            data: { viewName: 'ProfessionalLicenses' },
            success: function (data) {

                domList.append(data)
                $.validator.unobtrusive.parse(data);
                componentHandler.upgradeDom()
            }
        }

        $.ajax(options)

    })

    $('#addEmpHistory').on('click', function () {

        var domList = $('#employmentHistoryList')
        var options = {
            url: baseUrl + '/Examiner/GetView',
            datatype: 'HTML',
            data: { viewName: 'EmploymentHistory' },
            success: function (data) {

                domList.prepend(data)
                $.validator.unobtrusive.parse(data);
                componentHandler.upgradeDom()
            }
        }

        $.ajax(options)

    })

    $('#addCharacterRef').on('click', function () {

        var domList = $('#characterReferenceList')
        var options = {
            url: baseUrl + '/Examiner/GetView',
            datatype: 'HTML',
            data: { viewName: 'CharacterReference' },
            success: function (data) {
                domList.prepend(data)
                $.validator.unobtrusive.parse(data);
                componentHandler.upgradeDom()
            }
        }

        $.ajax(options)

    })

    

    $('.datePickerMdl').bootstrapMaterialDatePicker
        ({
            time: false,
            clearButton: true

        }).on('change', function (date) {

            var dom = $(this)
            var parent = dom.parent();

            if (dom.val() === "") {
                parent.removeClass('is-dirty')
            } else {
                parent.addClass('is-dirty')

            }


        });


    $('.IsActiveUser').on('change', function () {
        var isActive = $(this).hasClass("is-checked");
        var domId = $(this).parent().prev().prev().val()
    
        if (isActive) {
          console.log(updateUserStatus(domId,"Active"))
        } else {
            console.log(updateUserStatus(domId,"Deactivate"))
        }



    })

    function updateUserStatus(userId,strStatus) {
        var options = {
            url: baseUrl + "/Examiner/UpdateUserStatus",
            method: "POST",
            data: { UserId: userId, status: strStatus },
            success : function (data){
                if (data.success) {
                  globalMethods.AddSnackBar("User is now " + strStatus)
                } else {
                    globalMethods.AddSnackBar("Something went wrong call Administrator");
                }
             
            },
            error: function () {
                globalMethods.AddSnackBar("Something went wrong call Administrator");
            }
        }
        $.ajax(options);
    }


    $('.mdl-checkbox').on('change', function () {
        var dom = $(this)
        var children = dom.find('.childValue');
        if (dom.hasClass('is-checked')) {
            children.attr("checked", true)
        } else {
            children.attr("checked", false)
        }
    })





    $('.civilStatusSelect li').on('click', function () {
        var item = $(this);
        var text = item.text();
        var input = $('#civilStatus');
        var parentInput = input.closest('.mdl-textfield--floating-label');
        var inputText = input.val(text)
        var addtnlreference = $('#notSingleRequirments')

        if (text === 'Married' ) {
            addtnlreference.toggle()
        } else {
          
            addtnlreference.hide()
        }
        parentInput.addClass('is-dirty');
    })

   

    var handler=function () {
        alert('sss')
    }



    $('#submitCredUpdates').on('click', function () {
        //var toast = $('#toastNotif')
        var updateForm = $('#UpdatePasswordForm')
     

        $.ajax({
            url: baseUrl + '/Account/UpdatePassword',
            type: 'POST',
            datatype: 'JSON',
            data: updateForm.serialize(),
            success: function (data) {
                if (data === true) {
                    globalMethods.AddSnackBar('The user credential updated.')
                    //addSnackbar('The user credential updated.')
                } else {
                    globalMethods.AddSnackBar('The user credential failed updated.')
      
                }
            }
        })

    })


    

    $('#submitUserUpdate').on('click', function () {
        var toast = $('#toastNotif')
        var updateForm = $('#UpdateUserForm')


        $.ajax({
            url: baseUrl + '/Examiner/UpdateJson',
            type: 'POST',
            datatype: 'JSON',
            data: updateForm.serialize(),
            success: function (data) {
   
                if (data === true) {
              
                   globalMethods.AddSnackBar('The User updated.')
                } else {
          
                   globalMethods.AddSnackBar('The User failed updated.')
                }
            }
        })

    })

    $('#submitAdminCredUpdates').on('click', function () {
        var toast = $('#toastNotif')
        var updateForm = $('#UpdateAdminPasswordForm')


        $.ajax({
            url: baseUrl + '/Account/UpdateAdminPassword',
            type: 'POST',
            datatype: 'JSON',
            data: updateForm.serialize(),
            success: function (data) {
                if (data === true) {
                   globalMethods.AddSnackBar('The user credential updated.')
                } else {
                   globalMethods.AddSnackBar('The user credential failed updated.')
                }
            }
        })

    })



    $('#submitAdminAccount').on('click', function () {
        var toast = $('#toastNotif')
        var updateForm = $('#UpdateAdminAccount')


        $.ajax({
            url: baseUrl + '/Account/UpdateAdmin',
            type: 'POST',
            datatype: 'JSON',
            data: updateForm.serialize(),
            success: function (data) {
                if (data === true) {
                    globalMethods.AddSnackBar('The User updated.')
                //   globalMethods.AddSnackBar()
                } else {
                    globalMethods.AddSnackBar('The User failed updated.')
                   // addSnackbar('The User failed updated.')
                }
            }
        })

    })


    $('.chkIsAdministrator').on('click', function () {

        var checkBox = $(this)
        var checkboxParent = checkBox.parent();
        var isCheck = checkboxParent.hasClass('is-checked')
        var isAdmins = $('#isAdministratorChk')
        var adminCred = $('#adminAccess')
        if (!isCheck) {
        
            //isAdmins.prop('checked', true)
        } else {
         
            //isAdmins.prop('checked', false)
        }
        adminCred.toggle('fold')
      

    })


    




    $('#generatePassword').on('click', function () {

        var passwordFields = $('.passwordDetail');
        var fieldParent = passwordFields.parent();
 

        $.ajax({
            url: baseUrl + '/Account/GenerateRandomPassword',
            type: 'POST',
            datatype: 'JSON',
            success: function (data) {
                if (data !== null) {

                    passwordFields.val(data);
                    fieldParent.addClass('is-dirty')
                } else {
                    passwordFields.val('No');
                }
            }
        })
    })





    $('.companyTable').on('change', function () {
        $('.companyTable > tbody > tr > td > .chkCompanySelection').each(function () {
            var itemParent = $(this).parent();
            var parentParent = itemParent.parent();
            var item = $(this)
            if (parentParent.hasClass('is-selected') === true) {
                item.val('true')
            } else {
                item.val('false')
            }
        })
    })

    $('.userRoleTable').on('change', function () {
        $('.userRoleTable > tbody > tr > td > .chkUserSelection').each(function () {
            var itemParent = $(this).parent();
            var parentParent = itemParent.parent();
            var item = $(this)
            if (parentParent.hasClass('is-selected') === true) {
                item.val('true')
            } else {
                item.val('false')
            }
        })
    })






})