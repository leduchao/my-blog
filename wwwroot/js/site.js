$(document).ready(function () {
    $("#eye").on('click', (event) => {
        event.preventDefault();

        if ($('#show-hide-password input').attr("type") == "password") {
            $('#show-hide-password input').attr("type", "text");

            $('#eye i').removeClass("bi-eye-fill");
            $('#eye i').addClass("bi-eye-slash-fill");
        } else if ($('#show-hide-password input').attr("type") == "text") {
            $('#show-hide-password input').attr("type", "password");

            $('#eye i').removeClass("bi-eye-slash-fill");
            $('#eye i').addClass("bi-eye-fill");
        }
    });

    $("#eye2").on('click', (event) => {
        event.preventDefault();

        if ($('#show-hide-password2 input').attr("type") == "password") {
            $('#show-hide-password2 input').attr("type", "text");

            $('#eye2 i').removeClass("bi-eye-fill");
            $('#eye2 i').addClass("bi-eye-slash-fill");
        } else if ($('#show-hide-password2 input').attr("type") == "text") {
            $('#show-hide-password2 input').attr("type", "password");

            $('#eye2 i').removeClass("bi-eye-slash-fill");
            $('#eye2 i').addClass("bi-eye-fill");
        }
    });
});
