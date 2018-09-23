$().ready(function () {

    try {
        demo.checkFullPageBackgroundImage();

        $('.dataTable').DataTable({
            language: {
                url: "/Control/DatatablesLanguage"
            },
            pagingType: "full_numbers",
            lengthMenu: [
                [10, 25, 50, -1],
                [10, 25, 50, "All"]
            ],
            responsive: true,
            columnDefs: [
                { targets: "no-sort", orderable: false },
                { targets: "hiddenCols", visible: false }
            ]
        });

        $(".nav-item.active").closest("div").before().collapse('show');

        $('[data-toggle="tooltip"]').tooltip();

        if ($.validator) {
            $.validator.methods.range = function (value, element, param) {
                var globalizedValue = value.replace(",", ".");
                return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
            }

            $.validator.methods.number = function (value, element) {
                return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
            }
        }
    }
    catch (ex) {
    }
    finally {
        $("body").fadeIn(200);

        window.onbeforeunload = function () {
            $("body").fadeOut(100);
        };
    }
});

var demo = {
    checkFullPageBackgroundImage: function () {
        var $page = $('.full-page');
        var image_src = $page.data('image');

        if (image_src !== undefined) {
            var image_container = '<div class="full-page-background" style="background-image: url(' + image_src + ') "/>';
            $page.append(image_container);
        }
    }
};
