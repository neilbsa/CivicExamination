(function () {
    let baseUrl = SystemVariables.baseUrl + '/MainExamination/'
    var schedId = $('#scheduleId').val()

    var status = $('#status').val()
    checkStatus()


    function checkStatus() {
        if (status === 'false') {
            $('#examinationTimer').TimeCircles(timeOptions).start()
        }
    }





    function navigateTo(index) {
        $('.btnquestion')[index].click()
    }

    $(document).on('focusout', '.essayAnswer', function () {
        var dom = $(this)
        var textArea = dom.find('.userAnswer')
        var questionId = dom.data('quesid')
        var userchoice = dom.data('choiceid')
        var options = {
            url: baseUrl + 'updateEssayAnswerOnExam',
            datatype: 'JSON',
            data: { quesId: questionId, choiceId: userchoice, Answer: textArea.val() },
            method: 'POST'
        }

        $.ajax(options)


    })
    var timeOptions = {

        start: false,
        animation: "smooth",
        count_past_zero: false,
        circle_bg_color: "#000000",
        time: {
            Days: { show: false }

        }

    }

    $('#examinationTimer').TimeCircles(timeOptions).addListener(function (unit,value,total) {
        if (total === 0) {
            endCode()
        }
        updateServerTime(total)
    })







    function endCode() {
        var domModal = $('#endModal')
        var navigateEnd = $('#navigateSave');
        domModal.modal({ backdrop: 'static', keyboard: false })
        setTimeout(function () {

            var mySched = $('#scheduleId').val();
     
            window.location.replace(baseUrl + 'FinishExamination/'+ mySched)
            //$.ajax(option)
            //alert('test')
        }, 5000)
    }




    function updateServerTime(str) {
        var options = {
            url: baseUrl + "updateServerTime",
            datatype: "JSON",
            data: { schedId: schedId, Remaining: str }
        }

        $.ajax(options)
    }










    $(document).on('click', '.matchingChoicesPanel', function () {
        var dom = $(this);
        var selectGroup = $('.isChoosen')
        var domChoices = $('.choicesPanel');
        var domSelected = dom.find('.isChoosen')


        if (domSelected.hasClass('is-checked')) {
            dom.removeClass('isSelectedThumbnail');
            domSelected.removeClass('is-checked').change();
        }else{
            dom.addClass('isSelectedThumbnail');
            domSelected.addClass('is-checked').change();
        } 
        //console.log(dom);
        //console.log(domSelected);
    });

    $(document).on('click','.choicesPanel', function () {
        var dom = $(this);
        var selectGroup = $('.isSelected')
        var domChoices = $('.choicesPanel');
        var domSelected = dom.find('.isSelected')
        domChoices.each(function () {

            $(this).removeClass('isSelectedThumbnail');

        })
        selectGroup.each(function () {

            $(this).removeClass('is-checked');
      
        })
        dom.addClass('isSelectedThumbnail');
        domSelected.addClass('is-checked').change();

        //console.log(dom);
        //console.log(domSelected);
    });

    $(document).on('change', '.isSelected', function () {
        var dom = $(this)
        var intDom = dom.prop('for')
        var questionId = dom.data('quesid')
        //console.log(intDom)
        //console.log(questionId)

        var option = {
            url: baseUrl + 'updateSelected',
            data: { ansId: intDom, quesId: questionId },
            method: 'POST'
        }
        $.ajax(option)
        //$('.isSelected').each(function () {
        //    console.log('change')
        //})
    })
    

    $(document).on('change', '.isChoosen', function () {

        var dom = $(this)
        var panel = dom.parent().parent().parent().parent();
        var intDom = dom.prop('for')
        var questionId = dom.data('quesid')
        if (dom.hasClass('is-checked')) {

            var option = {
                url: baseUrl + 'updateChoosen',
                data: { ansId: intDom, quesId: questionId, value: true },
                method: 'POST'
            }
            panel.addClass('isSelectedThumbnail');
            $.ajax(option)

        } else {
                var option = {
                    url: baseUrl + 'updateChoosen',
                    data: { ansId: intDom, quesId: questionId , value: false },
                    method: 'POST'
                }
                panel.removeClass('isSelectedThumbnail');
                $.ajax(option)
            }






        //$('.isChoosen').each(function () {
        //    var dom = $(this)
        //    var panel = dom.parent().parent().parent().parent();
        //    var intDom = dom.prop('for')
        //    var questionId = dom.data('quesid')


       
        //    if (dom.hasClass('is-checked')) {
                
        //        var option = {
        //            url: baseUrl + 'updateChoosen',
        //            data: { ansId: intDom, quesId: questionId , value: true },
        //            method: 'POST'
        //        }
        //        panel.addClass('isSelectedThumbnail');
        //        $.ajax(option)
            
        //    } else {
        //        var option = {
        //            url: baseUrl + 'updateChoosen',
        //            data: { ansId: intDom, quesId: questionId , value: false },
        //            method: 'POST'
        //        }
        //        panel.removeClass('isSelectedThumbnail');
        //        $.ajax(option)
        //    }
        // })
    })



    $('.btnquestion').on('click', function () {
        var dom = $(this)
        var isNew = false;
        var questionId = dom.data('question')
        var currIndex = $('#currentIndex')
        var index = dom.data('index')
        var examId = $('#scheduleId').val()
        var container = $('.question-content')
        var noItems = $('.btnquestion').length
        var nextBtn = $('#navigateNext')
        var prevBtn = $('#navigatePrev')    
        currIndex.val(index)
        var BrowseViewOptions = {
            url: baseUrl + 'QuestionView',
            datatype: 'HTML',
            data: { schedId: examId, QuestionId: questionId },
            method: 'POST',
         
            success: function (data) {
                container.html('')
                container.html(data)
                componentHandler.upgradeDom()              
                if (parseInt(noItems) === parseInt(index)) {
                    nextBtn.attr('disabled', true)
                } else {                
                    nextBtn.attr('disabled', false)
                }


                if (parseInt(index) === parseInt(1)) {
                    prevBtn.attr('disabled',true)
                } else {
                    prevBtn.attr('disabled', false)
                }
            }
        }
        $.ajax(BrowseViewOptions)
        $('#examinationTimer').TimeCircles().start()
    })




    $(document).on('click', '#navigateNext', function () {
        var currIndex = $('#currentIndex')
        var goto = parseInt(currIndex.val())
        currIndex.val(goto)
        navigateTo(goto)



    })
    



    $(document).on('click', '#navigateStart', function () {
        navigateTo(0)
        $(this).attr('disabled', true)

    })

    $(document).on('click', '#navigatePrev', function () {
        var currIndex = $('#currentIndex')
        var goto = parseInt(currIndex.val()) -2
        currIndex.val(goto)
        navigateTo(goto)
    })


})()