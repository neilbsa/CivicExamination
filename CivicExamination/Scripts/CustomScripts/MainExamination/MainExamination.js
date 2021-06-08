(function () {

    let baseUrl = SystemVariables.baseUrl + '/MainExamination/'
    let selectedExamId;
    let selectedId = $('#scheduleId').val()



    $('.submitStartPrepare').on('click', function () {

        var dom = $(this)
        var frm = dom.parent()
        var data = frm.serializeArray()
        var schedId = data[0].value;

        selectedExamId = schedId;
        var options = {
            url: baseUrl + 'PrepareExamination',
            beforeSend: function () {
             
                globalMethods.OpenModal('Instruction', 'This will show the instruction', funcStartExam,'Start')
            },
            success: function(data) {
                console.log('proessing success')
            },
            complete: function (data) {
                console.log('proessing complete')
            }
        }
        frm.ajaxSubmit(options)
    })


    let funcStartExam = function () {
        window.location.assign(baseUrl + 'OnExamination?schedId=' + selectedExamId)
      
    }

    
  



    $(document).on('click', '.updateEssay', function () {
        var item = $(this)
        var type = item.data('actiontype')
        var choiceid = item.data('questionid')
        var schedId = $('#ScheduleId').val()
        var userId = $('#userSchedId').val()


        var options = {
            url: baseUrl + "UpdateEssayAnswer",
            datatype: 'JSON',
            method: 'POST',
            data: { type: type, questionId: choiceid, schedId: schedId, userId: userId },
            success: function (data) {
        
                console.log(data)
                if (data.success) {
                    var dom = item.parent().parent().parent().parent();
                    console.log(dom)
                    if (type === 'Correct')
                    {
                        dom.removeClass('panel-default')    
                        dom.removeClass('panel-danger')  
                        dom.addClass('panel-success')
                    }
                    else if (type === 'Wrong')
                    {
                        dom.removeClass('panel-default')
                        dom.removeClass('panel-success')
                        dom.addClass('panel-danger')
                    
                    }
                    else if (type === 'NotChecked')
                    {
                        dom.removeClass('panel-success')
                        dom.removeClass('panel-danger')
                        dom.addClass('panel-default')
                      
                    }
                   
                } else {
                    globalMethods.AddSnackBar("Error submiting Action: Contact Administrator")
                }
            }
        }


        $.ajax(options);


      //  console.log(type + ' ' + choiceid + ' ' + schedId + ' ' + userId)
    })

})()