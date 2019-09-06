<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="user.id == 0" class="modal-title">Add User</h5>
                    <h5 v-else class="modal-title">Edit User</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="form-group">

                            <label for="email">Email</label>
                            <input v-model="user.email"
                                   v-on:input="validateEmail"
                                   v-on:blur="validateEmail"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="this.user.emailValidClass"
                                   id="email"
                                   placeholder="E.g.: popalin33@yahoo.com" />
                            <div class="invalid-feedback">{{this.user.emailError}}</div>

                            <div v-if="user.id == 0">
                                <label for="password">Password</label>
                                <input v-model="user.password"
                                       v-on:input="validatePassword"
                                       v-on:blur="validatePassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="this.user.passswordValidClass"
                                       id="password" />
                                <div class="invalid-feedback">{{this.user.passwordError}}</div>
                            </div>
                            <div v-else>
                                <label for="password">Password</label>
                                <input v-model="user.editPassword"
                                       v-on:input="validatePassword"
                                       v-on:blur="validatePassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="this.user.passswordValidClass"
                                       id="password"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.user.passwordError}}</div>
                            </div>

                            <div v-if="user.id != 0">
                                <label for="password">Confirm Password</label>
                                <input v-model="user.editConfirmPassword"
                                       v-on:input="validateConfirmationPassword"
                                       v-on:blur="validateConfirmationPassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="this.user.confirmationValidClass"
                                       id="password"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.user.confirmationPasswordError}}</div>
                            </div>

                            <label for="displayName">Display Name</label>
                            <input v-model="user.displayName"
                                   v-on:input="validateDisplayName"
                                   v-on:blur="validateDisplayName"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="this.user.displayNameValidClass"
                                   id="displayName" />
                            <div class="invalid-feedback">{{this.user.displayNameError}}</div>
                            <br />

                            <div>
                                <label for="userRole">Please select User Role</label>
                                <select class="browser-default custom-select" v-model="user.userRole" v-bind:key="user.userRole">
                                    <option selected disabled>Choose your option</option>
                                    <option>Customer</option>
                                    <option>Moderator</option>
                                    <option>Shop</option>
                                    <option>Admin</option>
                                </select>
                            </div>

                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button :disabled="isValid"
                            type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync">
                        Save
                    </button>

                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import $ from "jquery";
    import Api from "../../Api";

    export default {

        computed: {
            isValid() {
                if (this.user.id == 0) {
                    if ((this.user.emailError == null && this.user.passwordError == null && this.user.displayNameError == null) ||
                        (this.user.emailError == "" && this.user.passwordError == "" && this.user.displayNameError == "")) {
                        return false;
                    } else return true;
                }
            }
        },

        data() {
            return {

                isLoading: false,
                user: {
                    id: 0,
                    email: "",
                    password: "",
                    confirmationPassword: "",
                    displayName: "",
                    userRole: "",

                    editPassword: "",
                    editConfirmPassword: "",

                    emailError: null,
                    passwordError: null,
                    confirmationPasswordError: null,
                    displayNameError: null,

                    emailValidClass: null,
                    passswordValidClass: null,
                    confirmationValidClass: null,
                    displayNameValidClass: null,
                    isValid: null
                }
            };
        },

        methods: {

            showAdd() {

                this.user = {
                    id: 0,
                    email: "",
                    password: "",
                    confirmationPassword: "",
                    displayName: "",
                    userRole: "",

                    editPassword: "",
                    editConfirmPassword: "",

                    emailError: null,
                    passwordError: null,
                    confirmationPasswordError: null,
                    displayNameError: null,

                    emailValidClass: null,
                    passswordValidClass: null,
                    confirmationValidClass: null,
                    displayNameValidClass: null,
                    isValid: null
                };
                $(this.$el).modal("show");
            },

            showEdit(user) {
                this.user = user;
                this.user.editPassword = "";
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validateEmail() {
                this.user.emailError = null;
                if (this.user.email.trim().length === 0)
                    this.user.emailError = "Please provide a email";
                else if (this.user.email.length > 80)
                    this.user.emailError = "Email too long (max 80 characters)";
                else if (!this.validEmail(this.user.email))
                    this.user.emailError = "Valid email is required";

                if (this.user.emailError === null) {
                    this.user.emailValidClass = "is-valid";
                } else {
                    this.user.emailValidClass = "is-invalid";
                }
            },

            validEmail: function (email) {
                var regex = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                return regex.test(email);
            },

            validatePassword() {
                this.user.passwordError = null;
                if (this.user.password.trim().length === 0)
                    this.user.passwordError = "Please provide a password";
                else if (this.user.password.length > 64)
                    this.user.passwordError = "Password too long (max 64 characters)";

                if (this.user.passwordError === null) {
                    this.user.passswordValidClass = "is-valid";
                } else {
                    this.user.passswordValidClass = "is-invalid";
                }
            },

            validateConfirmationPassword() {
                this.user.confirmationPasswordError = null;

                if (this.user.editConfirmPassword != this.user.editPassword)
                    this.user.confirmationPasswordError = "Confirmation Password and Password short be the same !";

                if (this.user.confirmationPasswordError === null) {
                    this.user.confirmationValidClass = "is-valid";
                } else {
                    this.user.confirmationValidClass = "is-invalid";
                }
            },

            validateDisplayName() {
                this.user.displayNameError = null;
                if (this.user.displayName.trim().length === 0)
                    this.user.displayNameError = "Please provide a displayName";
                else if (this.user.displayName.length > 50)
                    this.user.displayNameError = "DisplayName too long (max 50 characters)";

                if (this.user.displayNameError === null) {
                    this.user.displayNameValidClass = "is-valid";
                } else {
                    this.user.displayNameValidClass = "is-invalid";
                }
            },

            async saveAsync() {
                if (this.user.id == 0) {
                    await this.addAsync();
                } else {
                    await this.editAsync();
                }
            },

            async addAsync() {
                try {
                    this.isLoading = true;
                    await Api.users.addAsync({
                        email: this.user.email,
                        password: this.user.password,
                        displayName: this.user.displayName,
                        userRole: this.user.userRole
                    });
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }, 

            async editAsync() {
                try {
                    this.isLoading = true;
                    var pass = this.user.password == this.user.editPassword ? this.user.password : this.user.editPassword;
                    pass = this.user.editPassword === "" ? this.user.password : this.user.editPassword;

                    await Api.users.editAsync({
                        id: this.user.id,
                        email: this.user.email,
                        password: pass,
                        displayName: this.user.displayName,
                        userRole: this.user.userRole
                    });
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }
        }
    };
</script>