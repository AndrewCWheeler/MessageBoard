// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('#register').validate({
        rules: {
            'Register.Username': {
                required: true,
                minlength: 2,
            },
            'Register.Email': {
                required: true,
                email: true,
            },
            'Register.Password': {
                required: true,
                minlength: 8,
                pattern: /^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$/,
            },
            'Register.Confirm': {
                minlength: 8,
                equalTo: '#Register_Password',
            },
        },
        messages: {
            'Register.Username': {
                required: 'Username is required.',
                // minlength: 'Must be more than 2 characters.',
            },
            // validate() includes built-in error messages. We're using them on the email field.
            'Register.Password': {
                required: 'Please provide a password.',
                minlength: 'Must be at least 8 characters.',
                pattern:
                    'Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and a special character.',
            },
            'Register.Confirm': {
                required: 'Please provide a matching password.',
                minlenth: 'Must be at least 8 characters.',
                equalTo: 'Your passwords do not match.',
            },
        },
    });
});
