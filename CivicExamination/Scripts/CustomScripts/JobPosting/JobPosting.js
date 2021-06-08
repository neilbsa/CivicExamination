(function () {

    let baseUrl = SystemVariables.baseUrl + '/JobPosting/'
    let availableExaminations = []
    let examinationNames = []

    updateExaminationList();


    $('#createJobPosting').on('click', function () {
        var option = {
            url: baseUrl + 'Create',
            datatype: 'HTML',
            success: function (data) {
                globalMethods.OpenModal('Create Job Posting', data, createJobPostingFunc, 'Save')
                $.validator.unobtrusive.parse('#createJobPostingFrm');
            }
        }
        $.ajax(option)
    })



    $('.updateJobPosting').on('click', function () {
        var dom = $(this)
        var jobid = dom.data('jobpostingid')
        var option = {
            url: baseUrl + 'Update',
            datatype: 'HTML',
            data: { Id : jobid },
            success: function (data) {
                globalMethods.OpenModal('Update Posting', data, updateJobPostingFunc,'Update')
            }
        }

        $.ajax(option)

    })





    function updateExaminationList() {
        var options = {
            url: SystemVariables.baseUrl + '/Examination/GetExaminationList',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {

                availableExaminations = data
                console.log(availableExaminations)
                examinationNames = availableExaminations.map(function (item) { return item.ExaminationName; })

            }
        }
        $.ajax(options)
    }

    $(document).on('focusin', '.examinationTags', function () {
        $(this).autocomplete({
            source: examinationNames
        })
    })


    function getExamDetail(str) {
        console.log(str)
        var item = $.grep(availableExaminations, function (n, i) {

            //console.log(";"+n+";")
            //console.log(i)
            return n.ExaminationName === str
        })
       
        return item[0];
    }


    $(document).on('focusout', '.examinationTags', function (evt) {
        evt.stopImmediatePropagation()
        var dom = $(this)
        var parent = dom.parent().parent().parent()
        var noItem = parent.find('.serverItems')
        var examinationId = parent.find('.examinationId')
        var search = dom.val()
        var serverItems = 0

        search = $.trim(search)

        if (search.length > 0) {

            serverItems = getExamDetail(search)
        }
        console.log(serverItems)
        //noItem.html(serverItems.count)
        examinationId.val(serverItems.entityId)
    })


    $(document).on("click", "#addExaminationBtn", function () {
        var examList = $("#jobExaminationList")
        var options = {
            url: baseUrl + "JobPostingExaminationItem",
            datatype: "HTML",
            method: "GET",
            success: function (data) {

                examList.append(data);
                componentHandler.upgradeDom()
            }

        }

        $.ajax(options);
    })

    let updateJobPostingFunc = function () {
        var frm = $('form:first')
        var option = {
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {
                if (!data.success) {
                    addErrorList(data.errors)
                } else {
                    globalMethods.CloseModal();
                    globalMethods.AddSnackBar('Job Posting Updated')                   
                }
            }
        }
        frm.ajaxSubmit(option)
    }


    function updateIndexList(Id) {
        let list = $('#jobPostingMainList')

        $.ajax({
            url: baseUrl + 'IndexViewItem',
            datatype: 'HTML',
            method: 'POST',
            data: { Id: Id },
            success: function (data) {
                if (data.length > 0) {
                    list.append(data)
                    componentHandler.upgradeDom()
               
                }
            }
        })
    }


    function addErrorList(str) {
        var dom = $('#errorList');
        dom.html('')
        dom.fadeIn(1000)
        var noItem = 0
        $.each(str, function () {
            var strError = '<div class="listErrorCode">' + str[noItem] + '</div>';
            dom.append(strError)
            noItem = noItem + 1
        })
        dom.fadeOut(4000)
    }


    let createJobPostingFunc = function () {
        var frm = $('form:first')
     

       var options = {
           method: 'POST',
           datatype:'JSON',
           success: function (data) {
         
               if (!data.success) {
                   addErrorList(data.errors)
               } else {
                   updateIndexList(data.id)
                   globalMethods.AddSnackBar('New JobPosting Added')
                   globalMethods.CloseModal()
               }
           }
        }
       frm.ajaxSubmit(options)
    }



})()