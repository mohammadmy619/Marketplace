
function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 6000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}
function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}
function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    console.log("mmmm");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}


function removeProductFromOrder(detailId) {
    $.get('/user/remove-order-item/' + detailId).then(res => {
        location.reload();
    });
}

function changeOpenOrderDetailCount(event, detailId) {
    open_waiting();
    $.get('/user/change-detail-count/' + detailId + '/' + event.target.value).then(res => {
        $('#user-open-order-wrapper').html(res);
        close_waiting();
    });
}

function checkDetailCount() {
    $('input[order-detail-count]').on('change', function (event) {
        open_waiting();
        var detailId = $(this).attr('order-detail-count');
        console.log(detailId);
        console.log(event);
        $.get('/user/change-detail-count/' + detailId + '/' + event.target.value).then(res => {
            $('#user-open-order-wrapper').html(res);
            close_waiting();
            checkDetailCount();
        });
    });
}

checkDetailCount();
