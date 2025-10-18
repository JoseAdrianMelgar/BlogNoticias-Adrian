// Scripts generales para el sistema de inventario
$(function () {
    $('.alert').each(function () {
        var alert = $(this);
        setTimeout(function () {
            alert.alert('close');
        }, 5000);
    });
});
