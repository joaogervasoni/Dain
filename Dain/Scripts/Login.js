function RegisterJS() {
    $("#loginForm").css("display", "none");
    $("#registerForm").css("display", "initial");
}

function LoginJS() {
    $("#loginForm").css("display", "initial");
    $("#registerForm").css("display", "none");
}

function Preview_image(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('output_image');
        output.src = reader.result;
    }
    reader.readAsDataURL(event.target.files[0]);
}