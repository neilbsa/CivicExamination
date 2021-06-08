(function () {





    $('.removeChoiceBtn').click(function (evt) {
        evt.stopImmediatePropagation()

        var thisTr = $(this).parent().parent();
        thisTr.remove();
    })


    $('.choiceImageUpload').change(function (evt) {
        evt.stopImmediatePropagation()

        var file = $(this)[0]
        var parentContainer = $(this).parent()
        var imageContainer = parentContainer.next();
   
        var reader = new FileReader();
        reader.onloadend = function () {

            imageContainer.html(createChoiceImage(reader.result))
        }
        if (file) {
            reader.readAsDataURL(file.files[0]); //reads the data as a URL

        } else {
            imageContainer.html('')
        }
    })

    function createChoiceImage(src) {
        return '<img id="mainImage" height="200" width="100%" src="' + src + '"/>';
    }

  

})()