$(document).on("click", ".btn-modal-share", function () {
    var isShared = $(this).data('shared');
    var fileId = $(this).data('id');
    $("#myModal #BtnShareFile").attr("href", "/File/Share/" + fileId);
    $("#myModal #BtnShareFile").html((isShared) ? "Unshare" : "Share");
    var urlInput = $("#myModal #SharedUrl");
    urlInput.val("http://localhost:30920/File/GetShared/" + fileId);
    if (isShared) urlInput.show();
    else urlInput.hide();

});