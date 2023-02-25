//{
//    $('document').ready(function () {
//        $('input[type="text"], input[type="email"], textarea').focus(function () {
//            var background = $(this).attr('id');
//            $('#' + background + '-form').addClass('form-group-active');
//            $('#' + background + '-form').removeClass('form-group-error');
//        });
//        $('input[type="text"], input[type="email"], textarea').blur(function () {
//            var background = $(this).attr('id');
//            $('#' + background + '-form').removeClass('form-group-active');
//        });

//        function errorfield(field) {
//            $(field).addClass('form-group-error');
//            console.log(field);
//        }

//        $("#waterform").submit(function () {
//            var stopsubmit = false;

//            if ($('#name').val() == "") {
//                errorfield('#name-form');
//                stopsubmit = true;
//            }
//            if ($('#email').val() == "") {
//                errorfield('#email-form');
//                stopsubmit = true;
//            }
//            if (stopsubmit) return false;
//        });

//    });
//}