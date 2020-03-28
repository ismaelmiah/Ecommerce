$(document).ready(function () {
    $("#basic-form").validate({
        rules: {
            FirstName: {
                required: true  
            },
            LastName: {
                required: true
            },
            UserName: {
                required: true
            },
            PasswordHash: {
                required: true
            }
        }
    });
});