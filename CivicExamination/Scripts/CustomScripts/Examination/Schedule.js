$(function () {
    var currentYear = new Date().getFullYear();

    let baseUrl = SystemVariables.baseUrl + '/Schedule/'
    var newItems = []
    var currentItems = []

    getDataSourceDB()




    getJobPostingList()
    getExaminationList()
    getuserDetails()
    let userCache = []
    let userNames = []
    let JobCache = []
    let JobTitles = []


    let examCache = []
    let examTitles = []


    function getFormattedDate(date) {
        var year = date.getFullYear();

        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;

        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return year + '-' + month + '-' + day
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



    let createScheduleFunc = function () {
        var frm = $('Form:first')
        var options = {
            url: baseUrl + '/Create/',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {
                if (!data.success) {
                    addErrorList(data.errors)
                } else {
                    globalMethods.CloseModal()

                    getDataSourceDB()

                    globalMethods.AddSnackBar('New Schedule created')
                }
            }
        }
        frm.ajaxSubmit(options)
    }







    function getUserDetails(str) {
        var item = $.grep(userCache, function (n, i) {
            return n.FullName === str
        })
        return item[0]
    }



    function getExamDetails(str) {
        var item = $.grep(examCache, function (n, i) {
            return n.examName === str.toString()
        })
        return item[0]
    }



    function getJobDetails(str) {
        var item = $.grep(JobCache, function (n, i) {
            return n.jobName === str.toString()
        })    
        return item[0]
    }


    function getuserDetails() {
        var options = {
            url: SystemVariables.baseUrl + '/Examiner/getUserFullnames',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {
                userCache = data
                userNames = data.map(function (item) { return item.FullName })
            }
        }
        $.ajax(options)

    }



    function getExaminationList() {
        var options = {
            url: SystemVariables.baseUrl + '/Examination/jsonGetExamList',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {

                examCache = data
                examTitles = data.map(function (item) { return item.examName })
            }
        }
        $.ajax(options)

    }


    function getJobPostingList() {
        var options = {
            url: SystemVariables.baseUrl + '/JobPosting/jsonGetJobPostingList',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {

                JobCache = data
                JobTitles = data.map(function (item) { return item.jobName })
            }
        }
        $.ajax(options)

    }



    $(document).on('click', '.jobItem', function () {
        var dom = $(this)
        var itemData = dom.data('itemdata')
        var details = getJobDetails(itemData)
        var search = $('#jobName')
        var searchId = search.prev()


        search.val(details.jobName)
        searchId.val(details.jobId)
        var list = $('#jobpostingList')

        list.toggle('fade')
     
    })

    $(document).on('click', '.examinationItem', function () {
        var dom = $(this)
        var itemData = dom.data('itemdata')
        var details = getExamDetails(itemData)
        var search = $('#examName')
        var searchId = search.prev()

        search.val(details.examName)
        searchId.val(details.examId)
        var list = $('#examinationList')

        list.toggle('fade')

    })





    $('#SystemModal').on('focusin', '#examName', function () {
        $(this).autocomplete({
            source: examTitles
        })
    })


    $('#SystemModal').on('focusin', '#jobName', function () {
        $(this).autocomplete({
            source: JobTitles
        })
    })




    $('#SystemModal').on('click', '#addScheduleMember', function () {
        var tblRow = $('#memberTable > tbody')
        var option = {
            url: baseUrl + 'GetView',
            datatype: 'HTML',
            method: 'GET',
            data: { viewName: 'ScheduledMember' },
            success: function (html) {
                tblRow.append(html)
                componentHandler.upgradeDom()
            }
        }
        $.ajax(option)
    })



    $('#SystemModal').on('click', '.removeMember', function () {
        var thisItem = $(this).parent().parent()
        thisItem.remove()
    })

    $('#SystemModal').on('focusin', '.userNames', function () {

        $(this).autocomplete({
            source: userNames
        })
    })

    $('#SystemModal').on('focusout', '.userNames', function () {
        var dom = $(this)
        var errorCode = dom.next()
        var userId = dom.prev()
        var search = dom.val()
        var itemLookUp
        search = $.trim(search)

        userId.val(null)

        if (search.length > 0) {
            itemLookUp = getUserDetails(search)
        }

        if (search.length <= 0) {

            errorCode.text('Fullname Required').fadeIn().fadeOut(6000)

        } else if (itemLookUp === undefined) {

            errorCode.text('Fullname not in the list').fadeIn().fadeOut(6000)

        } else {
            userId.val(itemLookUp.userId)
        }


    })


    $('#SystemModal').on('focusout', '#examName', function () {
        var search = $(this).val()
        var errorCode = $(this).parent().next()
        var examId = $(this).prev()
        search = $.trim(search)
        var itemLookUp
        examId.val(null)
        if (search.length > 0) {
            itemLookUp = getExamDetails(search)
        }



        if (search.length <= 0) {
            errorCode.text('')
            errorCode.text('Examination Required').fadeIn().fadeOut(6000)
        } else if (itemLookUp === undefined) {

            errorCode.text('Examination not in the list').fadeIn().fadeOut(6000)

        } else {
            examId.val(itemLookUp.examId)
        }
    })

    $('#SystemModal').on('focusout', '#jobName', function () {
        var search = $(this).val()
        var errorCode = $(this).parent().next()
        var jobId = $(this).prev()
        search = $.trim(search)
        var itemLookUp
        jobId.val(null)
        if (search.length > 0) {
            itemLookUp = getJobDetails(search)
        }
        if (search.length <= 0) {
            errorCode.text('')
            errorCode.text('Job Required').fadeIn().fadeOut(6000)
        } else if (itemLookUp === undefined) {
            errorCode.text('Job not in the list').fadeIn().fadeOut(6000)
        } else {
            console.log(itemLookUp)
            jobId.val(itemLookUp.jobId)
        }
    })

    $(document).on('click','#examinationLookup', function () {
        var list = $('#examinationList')
 
        list.toggle('fade')


    })


    $(document).on('click', '#openJobList', function () {
        var list = $('#jobpostingList')
        list.toggle('fade')
    })

        function updateSchedule(event) {      
            var options = {
                url: baseUrl + 'Update',
                datatype: 'HTML',
                data: { Id: event.id },
                success: function (data) {
                    console.log(event)
                    globalMethods.OpenModal('Update Schedule', data, updateScheduleFunc)
                }
            }
            $.ajax(options).done(function () {
                var startDate = $('#SystemModal input[id="startDate"]')
            })
        }


        let showSchedule = function (event) {
            var url = baseUrl + 'ViewResult?' + 'schedId=' + event.id      
            window.location.replace(url)
        }


        let updateScheduleFunc = function () {
            var frm = $('Form:first')
            var options = {
                success: function (data) {

                    if (!data.success) {
                        addErrorList(data.errors)
                    } else {
                        globalMethods.CloseModal()

                        getDataSourceDB()

                        globalMethods.AddSnackBar('New Schedule Updated')
                    }

                }
            }
            frm.ajaxSubmit(options)
        }




        function deleteSchedule(event) {
            //var dataSource = $('#calendar').data('calendar').getDataSource();

            //for (var i in dataSource) {
            //    if (dataSource[i].id == event.id) {
            //        dataSource.splice(i, 1);
            //        break;
            //    }
            //}

            //$('#calendar').data('calendar').setDataSource(dataSource);

            var options = {
                url: baseUrl + 'Delete',
                datatype: 'HTML',
                data: { id : event.id },
                success: function (data) {

                    globalMethods.OpenModal('Delete Schedule', data, deleteConfirmed)
                }
            }

            $.ajax(options)
        }


        let deleteConfirmed = function () {
            var frm = $('Form:first')
            var options = {
                url: baseUrl + 'DeleteConfirmed',
                datatype: 'JSON',
                success: function (data) {
                    if (data.deleteSuccess) {

                        globalMethods.CloseModal()
                        getDataSourceDB()
                    }
                }
            }
            frm.ajaxSubmit(options)


        }





        let onCreateSchedule = function (evt) {

            var option = {
                url: baseUrl + 'Create',
                datatype: 'HTML',
                method: 'GET',
                success: function (data) {

                    globalMethods.OpenModal('Create Schedule', data, createScheduleFunc)
                    componentHandler.upgradeDom()
                }
            }
            $.ajax(option).done(function (x) {
                var startDate = $('#SystemModal input[id="startDate"]')
                var endDate = $('#SystemModal input[id="endDate"]')
                startDate.val(getFormattedDate(evt.startDate))
                endDate.val(getFormattedDate(evt.endDate))

            })

        }




        function getDataSourceDB() {
            var options = {
                url: baseUrl + 'getScheduleList',
                method: 'POST',
                datatype: 'JSON'
            }

            $.ajax(options).done(function (data) {
                var items = []
             
                items = data
                currentItems=[]
                for (var i = 0; i < data.length; i++) {
                    var current = {
                        "id": items[i].schedId,
                        "color": items[i].color,
                        "name": items[i].examName,
                        "memberCount": items[i].memberCount,
                        "startDate": new Date(items[i].startDate),
                        "endDate": new Date(items[i].endDate)
                    }
      
                    currentItems.push(current)
                    
                }
           
                updateDataSource()
            })
        }


        function updateDataSource() {
            $('#calendar').data('calendar').setDataSource(currentItems);
        }



       



        $('#calendar').calendar({
            enableContextMenu: true,
            enableRangeSelection: true,

            contextMenuItems: [
                {
                    text: 'Update',
                    click: updateSchedule
                },
                {
                    text: 'Delete',
                    click: deleteSchedule
                },
                {
                    text: 'Results',
                    click: showSchedule
                }
            ],

            selectRange: onCreateSchedule,
            mouseOnDay: function (e) {

                if (e.events.length > 0) {
                    var content = '';

                    for (var i in e.events) {
                 
                        content += '<div class="event-tooltip-content">'
                            + '<div class="event-name" style="color:' + e.events[i].color + '">' + e.events[i].name + '</div>'
                            + '<div class="event-location">' + e.events[i].memberCount + ' member</div>'
                            + '</div>';
                    }

                    $(e.element).popover({
                        trigger: 'manual',
                        container: 'body',
                        html: true,
                        content: content
                    });

                    $(e.element).popover('show');
                }
            },
            mouseOutDay: function (e) {
                if (e.events.length > 0) {
                    $(e.element).popover('hide');
                }
            },
            dayContextMenu: function (e) {

                $(e.element).popover('hide');
            }
 
        });



        $('#save-event').click(function () {
            saveEvent();
        });
    });

