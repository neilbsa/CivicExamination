$(function () {


    let baseUrl = SystemVariables.baseUrl + '/Evaluation/'

    $('#createNewAppraisalBtn').on('click', function () {
        var frm = $('#createBatchFrm')
        var options = {
            url: baseUrl + "Create",
            method: "POST",
            dataType: "JSON",
            beforeSend: function () {
                console.log('test')
                globalMethods.OpenStaticModal("Currently Processing Request");
            },
            success: function (data) {

                if (data.status) {
                    globalMethods.CloseStaticModal();
                    globalMethods.AddSnackBar("Creating batch success")
                } else {
                    globalMethods.AddSnackBar("Fatal Error: Processing request failed contact Administrator or call 911")
                }

               
            }, complete: function () {
                globalMethods.CloseStaticModal();

            }, error: function () {
                globalMethods.CloseStaticModal();
                globalMethods.AddSnackBar("Error: Processing request please contact Administrator")
            }
        }

        frm.ajaxSubmit(options);



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




});