function GenPromoCode() {
    $.ajax({
        url: "/Client/GenPromoCode",
        type: "POST", //метод отправки
        success: function (response) { //Данные отправлены успешно
            $('#Code').val(response);
        },
        error: function () { // Данные не отправлены
            $('#Code').val("Произошла ошибка, попробуйте вновь");
        }
    });
}

$(document).ready(function() {
    CartSumm();
});

$(document).on("click", ".btn-addtocart.btn.btn-block.btn-primary",
    function addToCart() {
        $(this).closest("tr").find('.count').hide();
        $(this).closest("tr").find('.btn-addtocart').html("Убрать из корзины");
        $(this).closest("tr").find('.btn-addtocart').prop('class', "btn-remove btn btn-block btn-primary");
        $(".tbl-cart").append($(this).closest("tr"));
        CartSumm();
    });

$(document).on("click", ".btn-remove.btn.btn-block.btn-primary",
    function addToCart() {
        $(this).closest("tr").find('.count').show();
        $(this).closest("tr").find('.btn-remove').html("Добавить в корзину");
        $(this).closest("tr").find('.btn-remove').prop('class', "btn-addtocart btn btn-block btn-primary");
        $(".tbl-catalog").append($(this).closest("tr"));
        CartSumm();
    });

function CartSumm() {
    var sum = 0;
    $('.summ').html(sum);
    $('.tbl-cart').find('[name="Price"]').each(function() {
        sum += parseInt($(this).html());
        $('.summ').html(sum);
    });
    if (sum<2000) {
        $('.btn-checkout').prop('disabled','true');
    } else {
        $('.btn-checkout').removeAttr('disabled');
    }
}

$(document).on("click",
    ".btn-checkout.btn.btn-block.btn-primary",
    function () {
        var bookIds = [];

        $('.tbl-cart').find('[name="Id"]').each(function () {
            bookIds.push($(this).html());
        });
        $.ajax({
            url: "/Home/CheckOrder",
            type: "POST", //метод отправки
            datatype: "json",
            data: { 'bookIds': bookIds },
            success: function (response) { //Данные отправлены успешно
                alert(response);
            },
            error: function (response) { // Данные не отправлены
                alert(response);
            }
        });
        $(".btn").remove();
    });
