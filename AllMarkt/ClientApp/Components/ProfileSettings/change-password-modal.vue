<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Change Password</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="form-group">
                            <div>
                                <label for="password">Old Password</label>
                                <input v-model="oldPassword"
                                       v-on:input="validateOldPassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="this.user.oldPasswordValidClass"
                                       id="oldPassword"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.user.oldPasswordError}}</div>
                            </div>
                            <div>
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

                            <div>
                                <label for="password">Confirm Password</label>
                                <input v-model="user.editConfirmPassword"
                                       v-on:input="validateConfirmationPassword"
                                       v-on:blur="validateConfirmationPassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="this.user.confirmationValidClass"
                                       id="passwordConfirm"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.user.confirmationPasswordError}}</div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button id="btn" disabled type="button"
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
        data() {
            return {
                isLoading: false,
                user: {
                    id: 0,
                    email: "",
                    userRole: "",
                    displayName: "",
                    password: "",

                    editPassword: "",
                    editConfirmPassword: "",

                    passwordError: null,
                    confirmationPasswordError: null,
                    oldPasswordError: null,

                    oldPasswordValidClass: null,
                    passswordValidClass: null,
                    confirmationValidClass: null,
                    isValid: null
                },
                oldPassword: "",
                shop: {
                    id: 0,
                    userId: 0,
                    userDisplayName: "",
                    phoneNumber: "",
                    address: "",
                    iban: ""
                }
            };
        },

        methods: {
            check() {
                if ((this.user.passwordError == null && this.user.confirmationPasswordError == null && this.user.oldPasswordError == null) ||
                    (this.user.passwordError == "" && this.user.confirmationPasswordError == "" && this.user.oldPasswordError == ""))
                    document.getElementById("btn").disabled = false;
            },
            showEdit(user, shop) {
                this.user = {
                    id: user.id,
                    email: user.email,
                    userRole: user.userRole,
                    displayName: user.displayName,
                    password: user.oldPassword,

                    editPassword: "",
                    editConfirmPassword: "",

                    passwordError: null,
                    confirmationPasswordError: null,
                    oldPasswordError: null,

                    oldPasswordValidClass: null,
                    passswordValidClass: null,
                    confirmationValidClass: null,
                    isValid: null
                },
                this.shop = {
                    id: shop.id,
                    userId: shop.userId,
                    userDisplayName: shop.userDisplayName,
                    phoneNumber: shop.phoneNumber,
                    address: shop.address,
                    iban: shop.iban
                }

                this.user.editPassword = "";
                this.oldPassword = "";
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validatePassword() {
                this.user.passwordError = null;
                if (this.user.editPassword.trim().length === 0)
                    this.user.passwordError = "Please provide a password";
                else if (this.user.editPassword.length > 64)
                    this.user.passwordError = "Password too long (max 64 characters)";

                if (this.user.passwordError === null) {
                    this.user.passswordValidClass = "is-valid";
                } else {
                    this.user.passswordValidClass = "is-invalid";
                }
                this.check();
            },

            validateConfirmationPassword() {
                this.user.confirmationPasswordError = null;

                if (this.user.editConfirmPassword != this.user.editPassword)
                    this.user.confirmationPasswordError = "Confirmation Password and Password should be the same!";

                if (this.user.confirmationPasswordError === null) {
                    this.user.confirmationValidClass = "is-valid";
                } else {
                    this.user.confirmationValidClass = "is-invalid";
                }

                this.check();
            },

            validateOldPassword() {
                this.user.oldPasswordError = null;

                if (this.oldPassword === this.user.editPassword)
                    this.user.oldPasswordError = "You entered the same password!";

                if (this.user.oldPasswordError === null) {
                    this.user.oldPasswordValidClass = "is-valid";
                } else {
                    this.user.oldPasswordValidClass = "is-invalid";
                }

                this.check();
            },

            async saveAsync() {
                await this.editAsync();
            },

            async editAsync() {
                try {
                    this.isLoading = true;
                    await Api.users.editAsync({
                        id: this.user.id,
                        email: this.user.email,
                        oldPassword: this.oldPassword,
                        newPassword: this.user.editPassword,
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