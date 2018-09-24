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


        var label = document.getElementById('label_image');
        var path = String(document.getElementById('upImage').value).split('\\').pop();

        if (path.length > 26) {
            label.textContent = path.substr(0, 13) + "..." + path.substring(path.length - 10);

        }
        else
            label.textContent = path;
    }
    reader.readAsDataURL(event.target.files[0]);
}