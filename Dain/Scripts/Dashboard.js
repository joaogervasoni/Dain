function Show(formNumber) {

    var form; 
    var name;

    if (formNumber == 1) {
        form = $("#nameForm").css("display");
        name = "#nameForm";
    }
    else if (formNumber == 2){
        form = $("#addressForm").css("display");
        name = "#addressForm";
    }
    else if (formNumber == 3) {
        form = $("#passForm").css("display");
        name = "#passForm";
    }
    else if (formNumber == 4) {
        form = $("#emailForm").css("display");
        name = "#emailForm";
    }
    else if (formNumber == 5) {
        form = $("#colorForm").css("display");
        name = "#colorForm";
    }
    

    if (form == 'inline') {
        $(name).css("display", "none");
    } else {
        $(name).css("display", "initial");
    }
}

function Preview_image(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('output_image');
        output.src = reader.result;
    }
    reader.readAsDataURL(event.target.files[0]);
}

function ShowProduct(formNumber) {

    if (formNumber == 1) {
        form1 = "#showForm";
        form2 = "#registerForm";

        $(form1).css("display", "initial");
        $(form2).css("display", "none");

        $("#nav1").attr('class', 'nav-link ');
        $("#nav2").attr('class', 'nav-link active');
    }
    else
    if (formNumber == 2) {
        form1 = "#showForm";
        form2 = "#registerForm";

        $(form2).css("display", "initial");
        $(form1).css("display", "none");

        $("#nav1").attr('class', 'nav-link active');
        $("#nav2").attr('class', 'nav-link ');
    }
}