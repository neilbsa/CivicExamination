(function () {

    let baseUrl = SystemVariables.baseUrl + '/Question/'


    $(document).on('click', '.questionViewDetails', function () {
        var frmId = $(this).attr('Id')
        $.ajax({
            url: baseUrl + 'Details',
            datatype: 'HTML',
            method: 'GET',
            data: { Id: frmId },
            success: function (data) {
                globalMethods.OpenModal("View Details", data, null)
            }
        })
    })

    var forDeleteId = 0;
    let deleteQuestionDetails = function () {
        var dom = $('#deleteQuestionFrm');

        dom.submit();

    }

    $(document).on('click', '.questionViewDeleteItem', function () {
        var frmId = $(this).attr('Id')
        //$.ajax({
        //    url: baseUrl + 'Details',
        //    datatype: 'HTML',
        //    method: 'GET',
        //    data: { Id: frmId },
        //    success: function (data) {
        //        globalMethods.OpenModal("View Details", data, null)
        //    }
        //})

        $.ajax({
            url: baseUrl + 'Delete',
            datatype: 'HTML',
            method: 'GET',
            data: { Id: frmId },
            success: function (data) {
                forDeleteId = frmId;
                globalMethods.OpenModal("Delete", data, deleteQuestionDetails, "Confirm");
            }
        })
      
     
    })






    $(document).on('click', '.updateQuestionBtn', function () {
        var frmId = $(this).attr('Id')
        //var dataType = $(this).attr('data-QuestionType')
        var dataType = $('#selectType').val()

        $.ajax({
            url: baseUrl + 'Update',
            datatype: 'HTML',
            method: 'GET',
            data: { Id: frmId }

        }).done(function (data) {
            globalMethods.OpenModal("Update Question", data, updateQuestionFunc)
            updateTable()

        })

    })



    let updateQuestionFunc = function () {

        var frm = $('#updateQuestionFrm')
        checkTable()
        var options = {
            url: baseUrl + "Update",
            type: "POST",
            success: function (data) {
                if (!data.success) {
                    addErrorList(data.errors)
                    globalMethods.AddSnackBar('Error: Updating not success')
                }
                else {

                    globalMethods.CloseModal()
                    globalMethods.AddSnackBar('Question updated')
                }
            }
        }
        frm.ajaxSubmit(options);
    }

    $(document).on('click', '#resetMainQuestionImage', function () {
        var inputFile = $('#mainImageUpload')
        var imageContainer = $('#mainImageContainer')
        imageContainer.html('')
        inputFile.val('')
    })

    $(document).on('click', '.resetChoiceImage', function () {
        var thisBtn = $(this)
        var imageContainer = thisBtn.prev()
        var inputFile = imageContainer.prev().find('.choiceImageUpload')
        imageContainer.html('')
        inputFile.val('')
    })


    function updateQuestionTable(dataId) {
        var list = $('#QuestionListTable')
        $.ajax({
            url: baseUrl + 'GetViewWithModel',
            datatype: 'HTML',
            method: 'GET',
            data: { viewName: 'QuestionListView', Id: dataId },
            success: function (reply) {
                list.append(reply)
                componentHandler.upgradeDom()
            }
        })
    }


    $(document).on('click', '.typeSelect li', function () {
        var item = $(this);
        var text = item.text();
        var input = $('#selectType');

        var parentInput = input.closest('.mdl-textfield--floating-label');
        var inputText = input.val(text)

        updateQuestionBody(text)

        parentInput.addClass('is-dirty');
    })


    $('#createCategoryQuestions').on('click', function () {
        $.ajax({
            url: baseUrl + 'GetView',
            data: { viewName: 'CreateQuestion' },
            datatype: 'HTML',
            method: 'GET',
            success: function (data) {
                globalMethods.OpenModal('Create Question', data, createQuestionFunction)
                $.validator.unobtrusive.parse('#createQuestionFrm')
            }
        })
    })

    var createQuestionFunction = function () {
        var frm = $('#createQuestionFrm')
        var dataSend = frm.serializeArray()
        var dataSendJson = dataSend[1]
        var QuestionBody = $('.questionBody')
        checkTable()
        var option = {
            datatype: 'JSON',

            success: function (data) {
                if (!data.success) {
                    addErrorList(data.errors)
                } else {
                    updateQuestionTable(data.Id)
                    globalMethods.CloseModal()
                    globalMethods.AddSnackBar('New Question has been added')
                }
            }
        };
        frm.ajaxSubmit(option)
    }

    function checkTable() {
        var rows = $('#choicesTable > tbody > tr')
        var currentRow = 0;
        $.each(rows, function () {

            var currRow = $(rows[currentRow])
            var indi = currRow.find('.isCorrectChoices')
            var check = currRow.find('.hiddenCheck')
            var indiPArent = indi.parent();
            var selectType = $('#selectType').val()

            if (selectType === 'Multiple_Choice') {
                if (indi.hasClass('is-checked')) {
                    check.prop('checked', true)
                    check.prop('value', true)
                } else {
                    check.prop('checked', false)
                    check.prop('value', false)
                }
            } else if (selectType === 'Matching') {
                if (indiPArent.hasClass('is-checked')) {
                    check.prop('checked', true)
                    check.prop('value', true)
                } else {
                    check.prop('checked', false)
                    check.prop('value', false)
                }
            }
            currentRow = currentRow + 1
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



    function updateQuestionBody(type) {

        var QuestionBody = $('.questionBody')
        var CategoryId = $('#categoryId').val()
        var catId = $('#catId')

        $.ajax({
            url: baseUrl + 'GetPartialQuestionView',
            type: 'GET',
            datatype: 'HTML',
            data: { viewName: type },
            success: function (data) {

                QuestionBody.html(data);

                catId.val(CategoryId)

                componentHandler.upgradeDom()
            }
        })
    }




    $(document).on('click', '#addChoicesBtn', function () {
        var tblBody = $('#choicesTable tbody')
        var selectType = $('#selectType').val();

        var currentView = null
        if (selectType === "Multiple_Choice") {
            currentView = 'ExamChoicesTemplate';
        } else if (selectType == 'Matching') {
            currentView = 'ExamChoicesTemplateCheckbox';
        }
        $.ajax({
            url: baseUrl + 'GetView',
            datatype: 'HTML',
            method: 'POST',
            data: { viewName: currentView },
            success: function (data) {

                tblBody.append(data);
                componentHandler.upgradeDom()
                updateTable()
            }
        })
    })






    function updateTableUpdate(forUpdate) {
        var tblBody = $('#choicesTable > tbody')
        var tblBodyRow = $('#choicesTable > tbody tr')
        var tblBodyRowCount = tblBodyRow.length
        var selectType = forUpdate
        tblBodyRow.each(function () {



            var currentRowChoice = currentRow.find('.isCorrectChoices')
            var currentRowChoiceParent = currentRowChoice.parent()
            var currentRowChoiceInput = currentRowChoice.find('.isCorrectChoiceInput')
            if (selectType === 'Multiple_Choice') {

                currentRowChoice.attr('for', currentRow.index())
                currentRowChoiceInput.attr('id', currentRow.index())
            }
            else if (selectType === 'Matching') {

                currentRowChoiceParent.attr('for', currentRow.index())
                currentRowChoice.attr('id', currentRow.index())
            }
        })
    }

    function updateTable() {
        var tblBody = $('#choicesTable > tbody')
        var tblBodyRow = $('#choicesTable > tbody tr')
        var tblBodyRowCount = tblBodyRow.length
        var selectType = $('#selectType').val();
        tblBodyRow.each(function () {

            var currentRow = $(this)

            var currentRowChoice = currentRow.find('.isCorrectChoices')
            var currentRowChoiceParent = currentRowChoice.parent()
            var currentRowChoiceInput = currentRowChoice.find('.isCorrectChoiceInput')
            var hiddencheck = currentRow.find('.hiddenCheck')
            if (selectType === 'Multiple_Choice') {

                currentRowChoice.attr('for', currentRow.index())
                currentRowChoiceInput.attr('id', currentRow.index())
            }
            else if (selectType === 'Matching') {
                currentRowChoiceParent.attr('for', currentRow.index())
                currentRowChoice.attr('id', currentRow.index())

            }
        })
    }

    $(document).on('change', '#mainImageUpload', function () {
        var file = $(this)[0]
        var imageContainer = $('#mainImageContainer');
        var reader = new FileReader();
        reader.onloadend = function () {

            imageContainer.html(createMainImage(reader.result))
        }
        if (file) {

            var FileSize = file.files[0].size;
            console.log(FileSize)
            if (FileSize > 300000) {
                imageContainer.html('</br></br><center><b>ERROR: Cannot load image , Image size is to large</b></center>')

            } else {
                reader.readAsDataURL(file.files[0]);
            }

        } else {
            imageContainer.html('')
        }
    })

    function createMainImage(src) {
        return '<img id="mainImage" height="200" width="100%" src="' + src + '"/>';
    }



})()