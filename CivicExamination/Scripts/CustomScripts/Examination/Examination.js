(function () {

    let baseUrl = SystemVariables.baseUrl + '/Examination/'
    let availableCategories = []
    let categoryNames = []
    updateCategoryList()

    
    $('#examinationIndex').on('click', '#createExamination', function () {
        var options = {
            url: baseUrl + 'Create',
            datatype: 'HTML',
            method: 'GET',         
            success: function (data) {
                  globalMethods.OpenModal('Create Examination', data, createExaminationFunc)                
            }
        }
        $.ajax(options)
    })


    $(document).on('click', '.updateExamination', function () {

        var dom = $(this)
        var examId = dom.data('examinationid')
        var options = {
            url: baseUrl + '/Update/',
            datatype: 'HTML',
            method: 'GET',
            data: { id: examId },
            success: function (data) {
               globalMethods.OpenModal('Update Exam', data, funcUpdateCommand)
               functionUpdateTable()
            }
        }
        $.ajax(options)
    })


    let funcUpdateCommand = function () {
        var frm = $('Form:first')
        var options = {
            url: baseUrl + '/Update/',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {
                if (!data.success) {
                    addErrorList(data.errors)
                } else {          
                    globalMethods.CloseModal()
                    location.reload(true);
                    globalMethods.AddSnackBar('New Examination has been updated')
                }
            }
        }
        frm.ajaxSubmit(options)
    }
    


    let functionUpdateTable = function () {
        var rows = $("#examinationCategoryMainList > tbody > tr")
        rows.each(
            function () {
            var dom = $(this)
            var items = dom.find('.serverItems')
            var CategoryTag = dom.find('.categoryTags')
            var serverItems

            serverItems = getNumItems(CategoryTag.val())
            items.html(serverItems.count)
    
        })

    }
    



    let createExaminationFunc = function () {
        var frm = $('Form:first')
        var options = {
            success: function (data) {
                if (!data.success)
                {           
                    addErrorList(data.errors)
                } else {
                  
                    globalMethods.CloseModal()
                    globalMethods.AddSnackBar('New Examination has been added')
                }
            }
        }
        frm.ajaxSubmit(options)
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
        
    $(document).on('click', '#examinationAddNewCategory', function () {
        var tbl = $('#examinationCategoryMainList tbody')
        var options = {
            url: baseUrl + 'GetView',
            datatype: 'HTML',
            data: { viewName: 'ExaminationCategoryBuild' },
            method: 'GET',
            success: function (data) {
                tbl.append(data)
                componentHandler.upgradeDom()
            }
        }
        $.ajax(options)
    })

    function updateCategoryList() {
        var options = {
            url: SystemVariables.baseUrl + '/Category/GetCategoryList',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {               
          
                availableCategories = data
            
                categoryNames = availableCategories.map(function (item) { return item.CategoryName; })  

            }
        }
        $.ajax(options)
    }  

    $(document).on('focusin', '.categoryTags', function () {
        $(this).autocomplete({
            source: categoryNames
        })
    })

    $(document).on('focusout', '.categoryTags', function (evt) {
        evt.stopImmediatePropagation()     
        var dom = $(this)
        var parent = dom.parent().parent().parent()
        var noItem = parent.find('.serverItems')
        var categoryId = parent.find('.categoryId')
        var search = dom.val()
        var serverItems = 0

        search =  $.trim(search)
   
        if (search.length > 0) {
     
            serverItems = getNumItems(search)
        }
        noItem.html(serverItems.count)
        categoryId.val(serverItems.entityId)
    })


    $(document).on('click', '.removeCategory', function () {
        var dom = $(this);
        var parentTr = dom.parent().parent()
        parentTr.remove()

    })

    function getNumItems(str) {
      var item = $.grep(availableCategories, function (n, i) {
            return n.CategoryName === str
        })
        return item[0]
    }
  


})()