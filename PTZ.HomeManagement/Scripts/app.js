$().ready(function () {
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
