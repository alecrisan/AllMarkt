<template>
    <layout>
    <div class="row">
        <div class="col-4">
        </div>
        <div class="col-4">
            <h3>Login</h3>
            <form v-on:submit.prevent="loginAsync">
                <div class="form-group">
                    <label for="email">Email</label>
                    <input type="text"
                           class="form-control"
                           v-model="user.email"
                           v-on:input="validateEmail"
                           v-on:blur="validateEmail"
                           v-bind:class="emailValidClass"
                           id="email" />
                    <div class="invalid-feedback">{{this.errors.email}}</div>
                </div>
                <div class="form-group">
                    <label for="password">Password</label>
                    <input type="password"
                           class="form-control"
                           v-model="user.password"
                           v-on:input="validatePassword"
                           v-on:blur="validatePassword"
                           v-bind:class="passwordValidClass"
                           id="password" />
                    <div class="invalid-feedback">{{this.errors.password}}</div>
                </div>
                <div class="form-group">
                    <button type="submit"
                            class="btn btn-primary"
                            v-bind:disabled="!errors.isValid">
                        Login
                    </button>
                    <div><font color="red">{{this.errors.invalidData}}</font></div>
                </div>
            </form>
            <br />
            <div class="text-black-50">Don't have an account? You can register <a href="#/register">here</a></div>
        </div>
        <div class="col-4">
        </div>
    </div>
    </layout>
</template>
<script>
    import Layout from "../Layout";
    import Api from "../../Api";

    export default {
        components: {
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
                if (this.errors.password === null) return ""
                else if (this.errors.password === "")
                    return "is-valid";
                else
                    return "is-invalid";
            }
        },

        data() {
            return {
                user: {
                    email: "",
                    password: ""
                },
                errors: {
                    email: null,
                    password: null,
                    isValid: null,
                    invalidData: null
                }
            };
        },

        methods: {
            validEmail: function (email) {
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
                this.checkValid();
            },

            async loginAsync() {
                try {
                    var output = await Api.login.loginAsync(this.user);
                    localStorage.setItem("currentUser", JSON.stringify(output.data));
                    this.errors.invalidData = null;
                    if (output.data.userRole === "Shop") {
                        this.$router.push('/shopView');
                    }
                    if (output.data.userRole === "Admin") {
                        this.$router.go('/');
                    }
                    if (output.data.userRole === "Customer") {
                        this.$router.push('/customer');
                    }
                }
                catch (exception) {
                    this.errors.isValid = false;
                    this.errors.invalidData = "User or password incorrect";
                }
            },

            checkValid() {
                this.errors.isValid =
                    this.errors.email === "" &&
                    this.errors.password === "";
            }
        }
    };
</script>