$(function () {
    let baseUrl = SystemVariables.baseUrl + '/Communication/'
    var hubCon = $.connection.systemComm;

    updateChatUser()

    function updateChatUser() {
        var option = {
            url: baseUrl + 'GetUserUpdate',
            datatype: 'JSON',
            method: 'POST',
            success: function (data) {
             
                var tableDom = $('#popChatMemberTbl > tbody');
                $.each(data, function (index, value) {   
      
                    if (value.IsVisible) {
                        tableDom.prepend('<tr data-userid = ' + value.Id + '><td> <div class="row"><div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 onlineIcon"><div class="onlineNotification"></div></div><div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 chatMemberOnline"><div>' + value.Firstname + ' ' + value.Lastname + '</div></div></div>  </td></tr>')
                    }
        
                })
            }
        }
        $.ajax(option);
    }


    $('#sidePanelList > .panel > .panel-heading').on('click', function () {
        var dom = $(this).parent().parent();
  
        if (dom.hasClass('inactiveSidePanel')) {

            dom.removeClass('inactiveSidePanel');
            dom.addClass('activeSidePanel');
        } else {

            dom.addClass('inactiveSidePanel');
            dom.removeClass('activeSidePanel');
        }
    })

    $(document).on('click', '.removeCurrent', function () {
        var dom = $(this);
        removeChat(dom);
    });


    function removeChat(dom) {
        var chats = dom.parent().parent().parent().parent().nextAll('.inactiveChatHead , .activeChatHead')
        dom.parent().parent().parent().parent().remove();

        chats.each(function () {
            var thisDom = $(this);
            var right = thisDom.css('right');
            var totalRight = right.slice(0, -2);
            if (parseInt(totalRight) == 310) {

            } else {
                thisDom.css('right', (parseInt(totalRight) - 310))
            }


        })
    }


    $('#popChatMemberTbl').on('click', 'tr', function () {
        var dom = $(this);
        var content = $('.pageGrouperDom');
        var activeChat = $('.activeChatHead')
        var chats = $('.inactiveChatHead , .activeChatHead')
        var totalLen = ((chats.length) * 310) + 310
        var userId = dom.data('userid');
        var windowSize = window.innerWidth;
       

        var option = {
            url: baseUrl + "GetPopChatContainer",
            datatype: 'HTML',
            method: 'POST',
            data: { userFrom :userId },
            success: function (data) {
                var currentOpenDomUser = $('#' + userId + '.chatDetailsVer')
            
                if (currentOpenDomUser.length >= 1) {
                    if (currentOpenDomUser.hasClass('inactiveChatHead')) {                     
                        toggleChatHeader(currentOpenDomUser)                        
                    }                  
                } else {
                 
                    content.append(data);
                    var cont = $('.activeChatHead').last()
                    cont.css('right', totalLen);
                    animateChatMessages(cont)           
                    
                    var totalChatLen = totalLen + 310;


                    if (windowSize < totalChatLen) {
                        //hide first chat
                        var firstChat = chats.first();
                        var removeCurrentBtn = firstChat.find('.panel').find('.panelHead').find('.removeCurrent');
            
                        removeCurrentBtn.click();
                    
                    }
                }
            },
            error: function () {

                alert('Somethings go wrong please call administrator')
            }
        }
        $.ajax(option);

        


    })

    $(document).on('focus', '.chatMessageDetails', function () {
        var dom = $(this);
        var parent = dom.parent().parent().parent().prev();        
        parent.css("background-color", "dodgerblue");     
    });



    $(document).on('focusout', '.chatMessageDetails', function () {
        var dom = $(this);
        var parent = dom.parent().parent().parent().prev();
        parent.css("background-color", "#f5f5f5");
    });

    function animateChatMessages(dom) {
        var currentChat = dom.find('.chatListMessages');
        var lastMessage = currentChat.find('.messageContainer:last-child');
        if(lastMessage.length > 0)
        {
            currentChat.animate({
                scrollTop: lastMessage.position().top * 4
            }, 'slow');
        }
       
    }


    function toggleChatHeader(dom) {
  
        if (dom.hasClass('inactiveChatHead')) {
            dom.removeClass('inactiveChatHead');
            dom.addClass('activeChatHead');
            animateChatMessages(dom)
        } else {

            dom.addClass('inactiveChatHead');
            dom.removeClass('activeChatHead');
            animateChatMessages(dom)
        }
    }

    $('.chatMemberCard').mouseenter(function () {


        $('.chatMemberCard').each(function () {
            var currentDom = $(this);

            if (!currentDom.hasClass('selectedChat')) {
                $(this).css("background-color", "white");
            }

        })
        if ($(this).hasClass('selectedChat') === false) {
            $(this).css("cursor", "pointer").css("background-color", "paleturquoise");
        }
    });

    $(document).on('click', '.chatDetailsVer > .panel > .panel-heading', function () {

        var dom = $(this).parent().parent();   
      
        toggleChatHeader(dom)
        //updateChatTray()

    })

    $('.chatMemberCard').on("click", function () {
        var btnDom = $('.SendMessage');
        var name = $('#recieverName');
        var selectedName = $(this).attr('data-userName')
        var chatDetailsDom = $('#messageDetails')
        var messageCount = $(this).find('.mdl-list__item-sub-title');



        $('.chatMemberCard').each(function () {
            var currentDom = $(this);
            currentDom.removeClass('selectedChat')
            $(this).css("background-color", "white");
        })


        $(this).addClass('selectedChat');
        name.val(selectedName);
        

        var options = {
            url: baseUrl + 'ChatDetails',
            data: { messageFrom: selectedName },
            method: 'GET',
            datatype: 'HTML',
            success: function (data) {
                chatDetailsDom.html(data);
                var chatLen = $('.messageContainer').length

                if (chatLen > 0) {
                    $('.chatDetailList').animate({
                        scrollTop: $('.chatDetailList .messageContainer:last-child').position().top * 10
                    }, 'slow');
                }
                messageCount.text('0 New Message');
            },
            error: function () {
                globalMethods.AddSnackBar("Error fetching chats, please contact Administrator")

            }

        }

        $.ajax(options);
        $(this).css("background-color", "dodgerblue");
    });


    hubCon.client.sendChat = function (chat, senderName) {

        var openChat = $('.pageGrouperDom').find("[data-userconid='" + senderName + "']");


        if (openChat.length === 0) {
            $('#popChatMemberTbl > tbody').find("[data-userid='" + senderName + "']").click()
        } else {
            var chatList = openChat.find('.chatListMessages')



            var option = {
                url: baseUrl + "GetMessageContainer",
                datatype: "HTML",
                method: "GET",
                data: { type: "Recieved", message: chat },
                success: function (data) {
                    chatList.append(data);
                   

                    var messageContLast = chatList.children().last();
                    chatList.animate({
                        scrollTop: messageContLast.position().top * 100
                    }, 'slow');
                }
            }
            $.ajax(option);
        }


        console.log(openChat);
           
  

    };


    $.connection.hub.start().done(function () {
        console.log('connected');
        $(document).on('keypress', '.chatMessageDetails', function (e) {
      
            if (e.which === 13 && !e.shiftKey) {
                var currentDom = $(this);
                var btn = currentDom.parent().next().children().first();               
                btn.click();
                return false;
            }
        })

        $(document).on('click', '.sendMessageBtn', function () {
       
            var dom = $(this);
            var sentmessage = dom.parent().prev().children().first();
            var chatHead = dom.parent().parent().parent().parent().parent();
            var chatHeadList = dom.parent().parent().prev().children().first();
            var recieverId = chatHead.data('userconid');

      

            sendMessageToUserId(recieverId, sentmessage.val());

            var option = {
                url: baseUrl + "GetMessageContainer",
                datatype: "HTML",
                method: "GET",
                data: { type: "Sent", message: sentmessage.val() },
                success: function (data) {
                    chatHeadList.append(data);
                    sentmessage.val('');

                    var messageContLast = chatHeadList.children().last();
                    chatHeadList.animate({
                        scrollTop: messageContLast.position().top * 100
                    }, 'slow');
                }
            }
            $.ajax(option);
        });


        function sendMessageToUserId(userId, message)
        {
          hubCon.server.sendMessage(userId, message)
        }



    });






});