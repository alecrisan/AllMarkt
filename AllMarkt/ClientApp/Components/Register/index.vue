<template>
    <layout>
        <div class="row">
            <div class="col-4">
            </div>
            <div class="col-4">

                <h3>Register</h3>
                <div v-if="this.currentUser === null || this.currentUser.token === null">
                    <form v-on:submit.prevent="registerAsync">
                        <div class="form-group">

                            <label for="email">Email</label>
                            <input v-model="user.email"
                                   v-on:input="validateEmail"
                                   v-on:blur="validateEmail"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="emailValidClass"
                                   id="email"
                                   placeholder="E.g.: popalin33@yahoo.com" />
                            <div class="invalid-feedback">{{this.errors.email}}</div>

                            <div>
                                <label for="password">Password</label>
                                <input v-model="user.password"
                                       v-on:input="validatePassword"
                                       v-on:blur="validatePassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="passwordValidClass"
                                       id="password"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.errors.password}}</div>
                            </div>

                            <div>
                                <label for="confirmPassword">Confirm Password</label>
                                <input v-model="confirmPassword"
                                       v-on:input="validateConfirmPassword"
                                       v-on:blur="validateConfirmPassword"
                                       type="password"
                                       class="form-control"
                                       v-bind:class="confirmPasswordValidClass"
                                       id="confirmPassword"
                                       placeholder="******" />
                                <div class="invalid-feedback">{{this.errors.confirmPassword}}</div>
                            </div>

                            <div>
                                <label for="displayName">Display Name</label>
                                <input v-model="user.displayName"
                                       v-on:input="validateDisplayName"
                                       v-on:blur="validateDisplayName"
                                       type="text"
                                       class="form-control"
                                       v-bind:class="displayNameValidClass"
                                       id="displayName"
                                       placeholder="E.g.: Pop Alin" />
                                <div class="invalid-feedback">{{this.errors.displayName}}</div>
                            </div>
                            <br />
                            <div class="custom-control custom-radio">
                                <input type="radio"
                                       class="custom-control-input"
                                       id="customer"
                                       v-model="user.userRole"
                                       v-on:click="validateUserRole"
                                       name="roleRadioButtons"
                                       value="Customer" checked />
                                <label class="custom-control-label" for="customer">Customer</label>
                            </div>
                            <div class="custom-control custom-radio">
                                <input type="radio"
                                       class="custom-control-input"
                                       id="shop"
                                       v-model="user.userRole"
                                       v-on:click="validateUserRole"
                                       name="roleRadioButtons"
                                       value="Shop" />
                                <label class="custom-control-label" for="shop">Shop</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <button type="submit"
                                    class="btn btn-primary"
                                    v-bind:disabled="!errors.isValid">
                                Register
                            </button>
                            <div><font color="red">{{this.errors.invalidData}}</font></div>
                            <br />
                            <div class="text-success">
                                {{this.okMessage}}
                            </div>
                        </div>
                    </form>
                </div>
                <div v-else>
                    Already logged in.
                </div>
            </div>
        </div>

        <div class="col-4">
        </div>
    </layout>
</template>
<script>
    import Layout from "../layout";
    import Api from "../../Api";

    export default {
        components: {
            Api,
            Layout
        },

        computed: {
            emailValidClass() {
                if (this.errors.email === null)
                    return "";
                else if (this.errors.email === "")
                    return "is-valid";
                else
                    return "is-invalid";
            },

            passwordValidClass() {
                if (this.errors.password === null)
                    return "";
                else if (this.errors.password === "")
                    return "is-valid";
                else
                    return "is-invalid";
            },

            confirmPasswordValidClass() {
                if (this.errors.confirmPassword === null)
                    return "";
                else if (this.errors.confirmPassword === "")
                    return "is-valid";
                else
                    return "is-invalid";
            },

            displayNameValidClass() {
                if (this.errors.displayName === null)
                    return "";
                else if (this.errors.displayName === "")
                    return "is-valid";
                else
                    return "is-invalid";
            }
        },

        data() {
            return {
                currentUser: null,
                isLoading: false,
                user: {
                    email: "",
                    password: "",
                    displayName: "",
                    userRole: "Customer",
                    isEnabled: "true"
                },

                confirmPassword: "",
                okMessage: "",

                errors: {
                    displayName: null,
                    isValid: null,
                    email: null,
                    password: null,
                    isValid: null,
                    confirmPassword: null,
                    invalidData: null,
                    userRole: ""
                }
            };
        },

        mounted() {
            this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
        },

        methods: {
            validEmail(email) {
                const emailRegex = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                return emailRegex.test(email);
            },

            validateEmail() {
                this.errors.email = "";

                if (this.user.email.trim().length === 0)
                    this.errors.email = "Please provide an email";
                else if (this.user.email.length > 80)
                    this.errors.email = "Email too long (max 80 characters)";
                else if (!this.validEmail(this.user.email))
                    this.errors.email = "Valid email is required";

                this.checkValid();
            },

            validatePassword() {
                this.errors.password = "";

                if (this.user.password.trim().length === 0)
                    this.errors.password = "Please provide a password";
                else if (this.user.password.trim().length < 6)
                    this.errors.password = "Please provide a password longer than 6 characters";
                else if (this.user.password.length > 64)
                    this.errors.password = "Password too long (max 64 characters)";
                this.validateConfirmPassword();
                this.checkValid();
            },

            validateConfirmPassword() {
                this.errors.confirmPassword = "";
                if (this.errors.password !== "")
                    this.errors.confirmPassword = "Invalid password";
                else if (this.user.password != this.confirmPassword)
                    this.errors.confirmPassword = "Confirm Password and Password should be the same!";

                this.checkValid();
            },

            validateDisplayName() {
                this.errors.displayName = "";

                if (this.user.displayName.trim().length === 0)
                    this.errors.displayName = "Please provide a displayName";
                else if (this.user.displayName.length > 50)
                    this.errors.displayName = "DisplayName too long (max 50 characters)";

                this.checkValid();
            },

            validateUserRole() {
                this.errors.userRole = "";

                if (this.user.userRole === "")
                    this.errors.userRole = "Please choose a role";

                this.checkValid();
            },

            checkValid() {
                this.errors.isValid =
                    this.errors.email === "" &&
                    this.errors.password === "" &&
                    this.errors.confirmPassword === "" &&
                    this.errors.displayName === "" &&
                    this.errors.userRole === "";
            },

            async registerAsync() {
                try {
                    this.isLoading = true;
                    await Api.users.registerAsync(this.user);
                    this.okMessage = "Success! Redirecting to login";
                    setTimeout(() => this.$router.push({ path: '/' }), 2000);
                }
                catch (exception) {
                }
                finally {
                    this.isLoading = false;
                }
            },
        }
    };
</script>