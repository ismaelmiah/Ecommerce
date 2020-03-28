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
        },

        messages: {
            FirstName: "Minimum two character required",
            LastName: "Minimum two character required",
            UserName: "Username must be unique required",
            PasswordHash: "As strong as Possible required"
        }
    });
});