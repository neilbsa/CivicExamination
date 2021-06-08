
var SystemVariables = (function () {

    var baseUrl = $('#SystemBaseURL').val()



    return {
        baseUrl: baseUrl
    }
})();






var globalMethods = (function () {
    var myModal = $('#SystemModal')
    var staticModal = $('#staticModal')
    var saveBtn = $('#modalSaveBtn')
    var modalTitle = myModal.find('.modal-title')



    let snackBar = function(str) {
        var snackbarContainer = document.querySelector('#toastNotif');
        var data = { message: str, timeout: 2000 };
        snackbarContainer.MaterialSnackbar.showSnackbar(data);
    }



    var btnDom = ' <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                 '<button type="button" id="modalSaveBtn" class="btn btn-primary" >Save</button>'


    let openStaticModal = function (bodyHtml) {
        var modalBody = staticModal.find('.staticModalBody')
        modalBody.html(bodyHtml)
        componentHandler.upgradeDom()
        staticModal.modal('show')
    }




    let openModal = function (titleHtml, bodyHtml, functionSave, btnText) {
        var finalFunction = null
        var footerDom = $('.modal-footer')
        saveBtn.off('click')

 
        if (btnText === null) {
            footerDom.html(btnDom)
        } else {
            saveBtn.text(btnText)
        }


      
        if (functionSave === null) {
            saveBtn.hide()
        } else {
            saveBtn.show()
        }
        var functionSave = functionSave || defaultFunction
        modalTitle.html(titleHtml)
        createModalBody(bodyHtml);
        saveBtn.on('click', functionSave)
        componentHandler.upgradeDom()
        myModal.modal('show')
    }


    let defaultFunction = function () {
        alert('No Function')
    }


    let createModalBody = function (body) {
        var modalBody = myModal.find('.modal-body')
        modalBody.html(body)
        componentHandler.upgradeDom()
    }

    let closedModal = function () {
        myModal.modal('hide')

    }
    let closedStaticModal = function () {
        staticModal.modal('hide')

    }

    return {
        OpenStaticModal: openStaticModal,
        CloseStaticModal: closedStaticModal,
        OpenModal: openModal,
        UpdateModalBody: createModalBody,
        CloseModal: closedModal,
        AddSnackBar: snackBar
     
    }
})();





