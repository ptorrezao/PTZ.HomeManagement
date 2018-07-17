﻿$().ready(function () {
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
    });
});

demo = {
    checkFullPageBackgroundImage: function () {
        $page = $('.full-page');
        image_src = $page.data('image');

        if (image_src !== undefined) {
            image_container = '<div class="full-page-background" style="background-image: url(' + image_src + ') "/>'
            $page.append(image_container);
        }
    }
}
