$(function () {


    let baseUrl = SystemVariables.baseUrl + '/EvaluationAnswerSheet/'

    computeKraIndexScores()
    recomputeOverallKraScore()
    computeCompScore()

    computeOverAllWeightedScore()


    //$('#acceptAppraisal').on('click', function () {
    //    var id = $('#appraisalModelId').val()
    //    var option = {
    //        url: baseUrl + "AcceptanceCodeView",
    //        datatype: 'HTML',
    //        data: { id: }
    //    }
    //    console.log(id);

    //})

    $('#acceptAppraisal').on('click', function () {

        var Ansid = $('#appraisalModelId').val()
        var option = {
            url: baseUrl + "AcceptanceCodeView",
            datatype: 'HTML',
            data: { id: Ansid },
            success: function (data) {


                globalMethods.OpenModal("Accept", data, acceptCode,"Accept")
            }, error: function () {

                globalMethods.AddSnackBar('Error : please contact administrator or 911 :D')

            }
            
        }
        $.ajax(option)

    })


    function acceptCode() {
        var dom = $('#acceptanceCodeView')
        var option = {
            url: baseUrl + "UserAccept",
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {

                if (data.success) {
                    globalMethods.CloseModal();
                    globalMethods.AddSnackBar('Acceptance Saved')
                    window.location.href = SystemVariables.baseUrl + "/" + "Evaluation/EvaluatorIndex";
                } else {
                    globalMethods.CloseModal();
                    globalMethods.AddSnackBar('ERROR: Acceptance Code not match : please Check Acceptance Code')
                }
                
            }, error: function () {

                globalMethods.AddSnackBar('Error : please contact administrator or 911 :D')

            }

        }
        dom.ajaxSubmit(option);
    }

    function computeOverAllWeightedScore() {

        $('#overallScoresTable > tbody > tr').each(function () {
            var dom = $(this);
            var indexScore = dom.find('.overAllIndexScore').text()
            var percentage = dom.find('.overAllPercentageScore').text()
            var totalWeightScore = dom.find('.overallWeightedScore')


            var factor = parseFloat(percentage) / 100;
            var res = parseFloat(indexScore) * parseFloat(factor);

            totalWeightScore.text(res)

        })

        computeTotalScore()
    }

    function computeTotalScore() {
        var totalWeightScore = $('.overallWeightedScore');
        var domTotal = $('#overAllScore');
        var totalValue = 0;

        totalWeightScore.each(function () {

            totalValue += parseFloat($(this).text())
        })

        //console.log(totalValue)
        domTotal.text(totalValue);
    }

    function computeCompScore() {
        $('.compModelGroupId').each(function () {
            var dom = $(this)

            computeCompentencyGroup(parseFloat(dom.val()))
            //console.log(dom.val())
        })
    }



    $('.compRatingScore').on('focusout', function () {

        computeCompScore()

    })

    function computeCompDomCount(id) {
        var totalDom = $('.ratingId' + id)
        var totalCount = 0

        totalDom.each(function () {
            var dom = $(this)
            if ($.isNumeric(dom.val()) && dom.val() > 0) {
                totalCount += 1
            }

        })

        return totalCount;
    }


    function computeCompentencyGroup(id) {
        var totalScore = 0
        $('.compGrpItem' + id).each(function () {
            var dom = $(this)

            var id = dom.parent().parent().parent().prev().val()
            var overAllScore = $('#overAll' + id);
            var totalCount = computeCompDomCount(id)
            var res = totalScore += parseFloat(dom.val());
            var avg = res / parseFloat(totalCount)
            overAllScore.text(avg)
        })
        computeOverAllWeightedScore()
    }
    function computeKraIndexScores() {
        var totalKRA = 0
        $('#KRAtbl > tbody > tr').each(function () {

            var dom = $(this);
            var indexScoreDom = dom.find('.kraIndexScore')
            var rawScore = dom.find('.kraRawScoreInput')
            var weight = dom.find('.kraWeightInput')
            var res = parseFloat(rawScore.val()) * (parseFloat(weight.val()) / 100)
            indexScoreDom.val(res)
            totalKRA += res
        })


        var kraIndexScore = $('#kraIndexScore');
        kraIndexScore.text(totalKRA)
        computeOverAllWeightedScore()

    }
    function recomputeOverallKraScore() {
        var kraIndexScore = $('#kraIndexScore');
        var totalKRAscore = 0
        $('.kraIndexScore').each(function () {
            var score = $(this).val();
            totalKRAscore += parseFloat(score);

        })
        kraIndexScore.text(totalKRAscore)
        computeOverAllWeightedScore()
    }




    $('#KRAtbl').on('focusout', '.kraInputs', function () {
        var dom = $(this);
        var domParentTr = dom.parent().parent()
        var indexScoreDom = domParentTr.find('.kraIndexScore')
        var rawScore = domParentTr.find('.kraRawScoreInput')
        var weight = domParentTr.find('.kraWeightInput')
        //console.log(rawScore.val())
        //console.log(weight.val())
        var res = parseFloat(rawScore.val()) * (parseFloat(weight.val()) / 100)


        indexScoreDom.val(res)

        computeKraIndexScores()
        recomputeOverallKraScore()
    })






    $('#saveDraftBtn').on('click', function () {
        var frm = $('#appraisalSheetFrm')

        var options = {
            url: baseUrl + "Update",
            method: 'POST',
            datatype: 'JSON',
            beforeSend: function () {
                globalMethods.OpenStaticModal("Currently Processing Request");

            },
            success: function (data) {
                if (data) {
                    globalMethods.AddSnackBar("Saving Evaluation Succeed Request");
                } else {
                    globalMethods.CloseStaticModal();
                    globalMethods.AddSnackBar("Error: Processing request please contact Administrator")
                }


            },
            complete: function () {

                globalMethods.CloseStaticModal();
            },
            error: function () {
                globalMethods.CloseStaticModal();
                globalMethods.AddSnackBar("Error: Processing request please contact Administrator")
            }

        }

        if (ValidateKRA(false)) {
            frm.ajaxSubmit(options)
        }

    })


    $('#KRAtbl').on('click', '.removeKraBtn', function () {

        var btn = $(this)
        var parent = btn.parent().parent()
        parent.remove()
        computeKraIndexScores()
    })


    $('#recommendedTrainingTable').on('click', '.removeTraining', function () {

        var btn = $(this)
        var parent = btn.parent().parent()
        parent.remove()

    })


    $('#addKRABtn').on('click', function () {
        var tblBody = $('#KRAtbl > tbody');
        var options = {
            url: baseUrl + "KRAItemView",
            datatype: 'HTML',
            success: function (data) {
                tblBody.append(data)
                componentHandler.upgradeDom()

            }
        }
        if (ValidateKRA(true)) {
            $.ajax(options);
        }





    })


    function ValidateKRA(acceptEqual) {
        var dom = $('.kraWeightInput');

        var totalWeigth = 0;
        dom.each(function () {
            var wei = parseFloat($(this).val())
            totalWeigth += wei

        })
        var isValid = true;



        if (acceptEqual) {
            if (totalWeigth >= 100) {
                globalMethods.AddSnackBar("Error: Total KRA weight exceeded to 100%")
                isValid = false;
            }
        } else {
            if (totalWeigth > 100) {
                globalMethods.AddSnackBar("Error: Total KRA weight exceeded to 100%")
                isValid = false;
            }
        }
        return isValid;
    }

    $('#recomendedTrainingSection').on('click', '#addTraining', function () {
        var tblBody = $('#recommendedTrainingTable > tbody');
        var options = {
            url: baseUrl + "RecomendedTrainings",
            datatype: 'HTML',
            success: function (data) {
                tblBody.append(data)
                componentHandler.upgradeDom()

            }
        }
        $.ajax(options);

    })

    $('.proceedEvaluationBtn').on('click', function () {

        var btn = $(this)
        var id = btn.data("appraisalid")
        //console.log(id)
        var options = {
            url: baseUrl + "PrepareAnswerSheets",
            method: "POST",
            dataType: "JSON",
            data: { EmpAppId: id },
            beforeSend: function () {

                globalMethods.OpenStaticModal("Currently preparing Answer sheet");
            },
            success: function (data) {

                if (data.success) {
                    globalMethods.CloseStaticModal();
                    globalMethods.AddSnackBar("Preparing success")
                    window.location.href = baseUrl + "GenerateAnswerSheet?EmpAppId=" + data.id
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




});