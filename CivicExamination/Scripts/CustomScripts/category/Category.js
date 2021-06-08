var CategoryCodes = (function () {
    let baseUrl = SystemVariables.baseUrl +'/Category/'



    $('#createCategoryBtn').on('click',function () {
        $.ajax({
            url: baseUrl + 'GetView',
            data: { viewName: 'Create' },
            datatype: 'HTML',
            method: 'GET',
            success: function (data) {
                globalMethods.OpenModal('Create Category', data, createCategoryFunc)
                $.validator.unobtrusive.parse('#createCategoryFrm');
            }
        })
    })






    function updateIndexList(json) {
        let list = $('#categoryMainList');
        $.ajax({
            url: baseUrl + 'IndexItemView',
            datatype: 'HTML',
            method: 'POST',
            data: { CategoryName : json.value},
            success: function (data) {
                if (data.length > 0) {
                    list.append(data)
                    componentHandler.upgradeDom()
                    globalMethods.AddSnackBar('Category created')
                }
            }
        })
    }


    var createCategoryFunc = function () {
        var frm = $('#createCategoryFrm')
        var dataSend = frm.serializeArray()
        var dataSendJson = dataSend[1]
        $.ajax({
            url: baseUrl + 'Create',
            datatype: 'HTML',
            method: 'POST',
            data: frm.serialize(),
            success: function (data) {
                if (data.length > 0) {
                    globalMethods.UpdateModalBody(data)
                } else {
                    updateIndexList(dataSendJson)
                    globalMethods.CloseModal()
                }
            }
        })
    }



    //$('#createQuestionBtn').on('click', function () {

    //    var dialog = $('#CreateQuestionModal')
    //    $.ajax({
    //        url: baseUrl + 'CreateQuestion',
    //        type: 'POST',
    //        datatype: 'HTML',
    //        success: function (data) {
    //            globalMethods.OpenModal("Create Question", data, funcCreateQuestionModal)
    //        }
    //    })
    //})


    //let funcCreateQuestionModal = function () {
    //    alert('helloAgain')
    //}

 




}())




