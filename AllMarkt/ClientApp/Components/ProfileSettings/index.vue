<template>
    <layout>
        <div class="mx-5 my-5">
            <div class="d-flex align-items-center">
                <h2 class="flex-grow-1">Profile Settings</h2>
            </div>
            <form>
                <div class="form-group">

                    <label for="userDisplayName">Display Name</label>
                    <input v-model="user.displayName"
                           v-on:input="validateDisplayName"
                           v-on:blur="validateDisplayName"
                           type="text"
                           class="form-control"
                           v-bind:class="this.user.displayNameValidClass"
                           id="userDisplayName" />
                    <div class="invalid-feedback">{{this.user.displayNameError}}</div>

                    <label for="email">Email</label>
                    <input disabled v-model="user.email"
                           type="text"
                           class="form-control"
                           id="email"
                           placeholder="E.g.: popalin33@yahoo.com" />

                    <label for="address">Address</label>
                    <textarea v-model="user.address"
                              v-on:input="validateAddress"
                              v-on:blur="validateAddress"
                              type="text"
                              class="form-control"
                              v-bind:class="this.user.addressValidClass"
                              id="address"
                              rows="4" />
                    <div class="invalid-feedback">{{this.user.addressError}}</div>

                    <label for="phoneNumber">Phone Number</label>
                    <input v-model="user.phoneNumber"
                           v-on:input="validatePhoneNumber"
                           v-on:blur="validatePhoneNumber"
                           type="text"
                           class="form-control"
                           v-bind:class="this.user.phoneNumberValidClass"
                           id="phoneNumber"
                           placeholder="E.g.: 0756317278" />
                    <div class="invalid-feedback">{{this.user.phoneNumberError}}</div>

                    <div v-if="!isCustomer">
                        <label for="iban">IBAN</label>
                        <input v-model="user.iban"
                               v-on:input="validateIBAN"
                               v-on:blur="validateIBAN"
                               type="text"
                               class="form-control"
                               v-bind:class="this.user.IBANValidClass"
                               id="iban" />
                        <div class="invalid-feedback">{{this.user.ibanError}}</div>
                    </div>

                    <br />

                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="showChangePassword">
                        Change Password
                    </button>
                </div>
            </form>
            <div class="modal-footer">
                <button type="button"
                        class="btn btn-primary"
                        v-on:click="editAsync">
                    Save
                </button>

                <a href="/" class="btn btn-primary" role="button">Cancel</a>

            </div>
            <div class="d-flex justify-content-end text-success">
                {{this.okMessage}}
            </div>
            <change-password-modal ref="changePasswordModal" v-on:submitted="load" />
        </div>
    </layout>



</template>

<script>
    import Api from "../../Api";
    import Layout from "../Layout";
    import ChangePasswordModal from "./change-password-modal";
    import UserStore from "../../UserStore";

    export default {
        components: {
            ChangePasswordModal,
            Layout
        },

        data() {
            return {
                isLoading: false,
                isCustomer: true,
                okMessage: "",
                user: {
                    id: 0,
                    email: "",
                    oldPassword: "",
                    newPassword: "",
                    displayName: "",
                    userRole: "",
                    userId: 0,
                    address: "",
                    phoneNumber: "",
                    iban: "",

                    emailError: null,
                    emailValidClass: null,
                    displayNameError: null,
                    displayNameValidClass: null,
                    addressError: null,
                    phoneNumberError: null,
                    ibanError: null,
                    addressValidClass: null,
                    phoneNumberValidClass: null,
                    IBANValidClass: null,

                    isValid: null
                }
            };
        },

        methods: {

            async load() {
                var shop = null;
                var customer = null;
                var userShop = await Api.users.getMyDataAsUserAsync();

                if (userShop.data.userRole === "Shop") {
                    shop = await Api.shops.getMyDataAsShopAsync();
                    this.isCustomer = false;
                } else {
                    customer = await Api.customers.getMyDataAsCustomerAsync();
                    this.isCustomer = true;
                }

                console.log(shop);

                if (this.isCustomer == true) {
                    this.user = {
                        id: customer.data.id,
                        displayName: userShop.data.displayName,
                        oldPassword: userShop.data.password,
                        newPassword: userShop.data.password,
                        email: userShop.data.email,
                        userId: customer.data.userId,
                        address: customer.data.address,
                        phoneNumber: customer.data.phoneNumber,
                        userRole: userShop.data.userRole,

                        addressError: null,
                        phoneNumberError: null,
                        ibanError: null,
                        displayNameError: null,

                        addressValidClass: null,
                        phoneNumberValidClass: null,
                        IBANValidClass: null,
                        displayNameValidClass: null,
                        isValid: null
                    };
                } else {
                    this.user = {
                        id: shop.data.id,
                        displayName: userShop.data.displayName,
                        oldPassword: userShop.data.password,
                        newPassword: userShop.data.password,
                        email: userShop.data.email,
                        userId: shop.data.userId,
                        address: shop.data.address,
                        phoneNumber: shop.data.phoneNumber,
                        userRole: userShop.data.userRole,
                        iban: shop.data.iban,

                        addressError: null,
                        phoneNumberError: null,
                        ibanError: null,
                        displayNameError: null,

                        addressValidClass: null,
                        phoneNumberValidClass: null,
                        IBANValidClass: null,
                        displayNameValidClass: null,
                        isValid: null
                    };
                }
            },
            async showChangePassword() {
                var user = await Api.users.getMyDataAsUserAsync();

                var customer = null;
                var shop = null;

                if (this.isCustomer == true) {
                    customer = await Api.customers.getMyDataAsCustomerAsync();
                    this.$refs.changePasswordModal.showEdit(user.data, customer.data);
                } else {
                    shop = await Api.shops.getMyDataAsShopAsync();
                    this.$refs.changePasswordModal.showEdit(user.data, shop.data);
                }
            },

            validatePhoneNumber() {
                this.user.phoneNumberError = null;
                if (this.user.phoneNumber[0] != '0')
                    this.user.phoneNumberError = "Phone number must start with a 0";
                if (this.user.phoneNumber.length != 10)
                    this.user.phoneNumberError = "Phone number must contain 10 digits";

                if (this.user.phoneNumberError === null) {
                    this.user.phoneNumberValidClass = "is-valid";
                } else {
                    this.user.phoneNumberValidClass = "is-invalid";
                }
            },

            validateAddress() {
                this.user.addressError = null;
                if (this.user.address.length > 255)
                    this.user.addressError = "Address too long (max 255 characters)";

                if (this.user.addressError === null) {
                    this.user.addressValidClass = "is-valid";
                } else {
                    this.user.addressValidClass = "is-invalid";
                }
            },

            validateIBAN() {
                this.user.ibanError = null;
                if (this.user.iban.length != 24)
                    this.user.ibanError = "IBAN must contain 24 digits";

                if (this.user.ibanError === null) {
                    this.user.IBANValidClass = "is-valid";
                } else {
                    this.user.IBANValidClass = "is-invalid";
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
                await this.editAsync();
            },

            async editAsync() {
                try {
                    this.isLoading = true;

                    await Api.users.editAsync({
                        id: this.user.userId,
                        email: this.user.email,
                        oldPassword: this.user.oldPassword,
                        newPassword: this.user.newPassword,
                        displayName: this.user.displayName,
                        userRole: this.user.userRole
                    });

                    if (this.isCustomer) {
                        await Api.customers.editCustomerAsync({
                            id: this.user.id,
                            address: this.user.address,
                            phoneNumber: this.user.phoneNumber
                        });
                    }
                    else {
                        await Api.shops.editShopAsync({
                            id: this.user.id,
                            address: this.user.address,
                            phoneNumber: this.user.phoneNumber,
                            iban: this.user.iban
                        });
                    }

                    this.okMessage = "Profile Updated !"
                    setTimeout(() => this.$router.go({ path: '#' }), 1000);

                } finally {
                    this.isLoading = false;
                }
            }
        },
        mounted() {
            this.load();
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    };
</script>