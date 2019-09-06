<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="productCategory.id === 0" class="modal-title">Add Product Category</h5>
                    <h5 v-else class="modal-title">Edit Product Category</h5>
                </div>
                <div class="modal-body">
                    <div v-if="isLoading" class="d-flex justify-content-center">
                        <div class="spinner-border text-info">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>

                    <form v-else>
                        <div class="form-group">
                            <label for="name">Name</label>
                            <input v-model="productCategory.name"
                                   v-on:input="validateName"
                                   v-on:blur="validateName"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="productCategory.nameValidClass"
                                   id="name"
                                   placeholder="E.g.: pizza, salads, appliances, etc" />
                            <div class="invalid-feedback">{{this.productCategory.nameError}}</div>
                        </div>

                        <div class="form-group">
                            <label for="description">Description</label>
                            <textarea v-model="productCategory.description"
                                      class="form-control"
                                      id="description"
                                      rows="3" />
                        </div>
                        <div v-if="!isLoadingShops && productCategory.id === 0" class="d-flex justify-content-around">
                            <select v-if="this.currentUser != null && this.currentUser.userRole === 'Admin'" id="selector" v-model="productCategory.shopId"
                                    v-on:click="validateShop"
                                    class="custom-select"
                                    v-bind:class="productCategory.shopValidClass">
                                <option selected disabled value="Shop Name">Shop</option>
                                <option v-for="shop in shops" v-bind:value="shop.id">{{shop.userDisplayName}}</option>
                            </select>
                            <div class="invalid-feedback">{{this.productCategory.shopIdError}}</div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading">
                        Save
                    </button>
                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide"
                            v-bind:disabled="isLoading">
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
    import UserStore from "../../UserStore";

    export default {

        data() {
            return {
                shops: [],
                currentUser: null,
                isLoadingShops: false,
                isLoading: false,
                productCategory: {
                    id: 0,
                    name: "",
                    description: "",
                    nameError: null,
                    validShopId: null,
                    nameValidClass: "",
                    shopIdValidClass: ""
                }
            };
        },

        mounted() {
           
        },

        methods: {
            async show1() {
                await this.loadShops();
                this.productCategory = {
                    id: 0,
                    name: "",
                    description: "",
                    shopId: 0,
                    nameError: null,
                    shopIdError: null

                };
                $(this.$el).modal("show");
                 this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
            },
            async show2(category) {
                await this.loadShops();
                this.productCategory = {
                    id: category.id,
                    name: category.name,
                    description: category.description,
                    shopId: category.shopId,
                    nameError: null,
                    shopIdError: null

                };
                $(this.$el).modal("show");
                 this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validateName() {
                this.productCategory.nameError = null;
                if (this.productCategory.name.trim().length === 0)
                    this.productCategory.nameError = "Please provide a name";
                else if (this.productCategory.name.length > 50)
                    this.productCategory.nameError = "Name too long (max 50 characters)";

                if (this.productCategory.nameError === null) {
                    this.productCategory.nameValidClass = "is-valid";
                } else {
                    this.productCategory.nameValidClass = "is-invalid";
                }

            },
            validateShop() {
                this.productCategory.shopIdError = null;
                if (this.productCategory.shopId === 0)
                    this.productCategory.shopIdError = "Please select a Shop";

                if (this.productCategory.shopIdError === null) {
                    this.productCategory.shopIdValidClass = "is-valid";
                } else {
                    this.productCategory.shopIdValidClass = "is-invalid";
                }
            },
            async loadShops() {
                this.isLoadingShops = true;
                var result = await Api.shops.getAllShopsAsync();
                this.shops = result.data;
                this.isLoadingShops = false;
            },

            async saveAsync() {
                try {
                    this.isLoading = true;
                    if (this.productCategory.id === 0) {
                        await Api.productCategories.addAsync({
                            name: this.productCategory.name,
                            description: this.productCategory.description,
                            shopId:  this.currentUser.userRole ==='Admin'?  this.productCategory.shopId : 0
                        });
                    }
                    else {
                        await Api.productCategories.editAsync({
                            id: this.productCategory.id,
                            name: this.productCategory.name,
                            description: this.productCategory.description
                        });
                    }
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }
        }
    };</script>
