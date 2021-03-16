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

class MyFormValidator {
    // The constructor method is a built-in js function for creating and initializing an object created with a class.
    constructor(form, fields) {
        this.form = form;
        this.fields = fields;
    }
    initialize() {
        this.validateOnEntry();
        this.validateOnSubmit();
    }

    validateOnSubmit() {
        let self = this;
        this.form.addEventListener('submit', event => {
            event.preventDefault();
            self.fields.forEach(field => {
                const input = document.querySelector(`#${field}`);
                self.validateFields(input);
            });
        });
    }

    validateOnEntry() {
        let self = this;
        this.fields.forEach(field => {
            const inputFromUser = document.querySelector(`#${field}`);

            inputFromUser.addEventListener('input', event => {
                self.validateFields(inputFromUser);
            });
        });
    }

    validateFields(field) {
        // Check for the presence of values
        if (field.value.trim() === '') {
            this.setStatus(
                field,
                `${field.previousElementSibling.innerText} cannot be blank`,
                'error'
            );
        } else {
            this.setStatus(field, null, 'success');
        }

        // Check for a valid email address
        if (field.type === 'email') {
            const re = /\S+@\S+\.\S+/;
            if (re.test(field.value)) {
                this.setStatus(field, null, 'success');
            } else {
                this.setStatus(
                    field,
                    'Please enter a valid email address',
                    'error'
                );
            }
        }
    }

    setStatus(field, message, status) {
        const errorMessage = field.parentElement.querySelector(
            '.error-message'
        );

        if (status === 'success') {
            if (errorMessage) {
                errorMessage.innerText = '';
            }
        }

        if (status === 'error') {
            field.parentElement.querySelector(
                '.error-message'
            ).innerText = message;
        }
    }
}

const form = document.querySelector('#login');
const fields = ['Login_Email', 'Login_Password'];

const myValidator = new MyFormValidator(form, fields);
myValidator.initialize();
