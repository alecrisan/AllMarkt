<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Products</h2>
            <button class="btn btn-primary" v-on:click="showAdd">Add</button>
        </div>

        <table class="table">
            <thead>
                <tr>
                    <th class="w-10">Product Category</th>
                    <th class="w-20">Name</th>
                    <th class="w-30">Description</th>
                    <th class="w-10">Price</th>
                    <th class="w-10">State</th>
                    <th class="w-20">Actions</th>
                </tr>
            </thead>

            <tbody v-if="!isLoading">
                <tr v-for="product in products" v-bind:key="product.id">
                    <td>{{product.productCategoryName}}</td>
                    <td>{{product.name}}</td>
                    <td>{{product.description}}</td>
                    <td>{{product.price}}</td>
                    <td v-if="product.state === true">Available</td>
                    <td v-else>Unavailable</td>

                    <td>
                        <button type="button" class="btn btn-primary" v-on:click="() => showEdit(product)">Edit</button>
                        <button class="btn btn-danger" v-on:click="() => showDelete(product)">Delete</button>
                    </td>

                </tr>
            </tbody>

        </table>

        <div v-if="isLoading" class="d-flex justify-content-center">
            <div class="spinner-border text-info">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

        <form-modal ref="formModal" v-on:submitted="loadAsync" />
        <delete-modal ref="deleteModal" v-on:submitted="loadAsync" />

    </layout>
</template>
<script>
    import Api from "../../Api";
    import Layout from "../Layout";
    import FormModal from "./form-modal";
    import DeleteModal from "./delete-modal";
    import UserStore from "../../UserStore";

    export default {
        components: {
            Layout,
            FormModal,
            DeleteModal
        },

        data() {
            return {
                isLoading: false,
                products: []
            };
        },

        methods: {
            showAdd() {
                this.$refs.formModal.showAdd();
            },

            showDelete(product) {
                this.$refs.deleteModal.show(product);
            },

            showEdit(product) {
                this.$refs.formModal.showEdit(product);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    var result = null;
                    if (UserStore.userRole === "Admin") {
                        result = await Api.products.getAllAsync();
                    } else if (UserStore.userRole === "Shop") {
                        result = await Api.products.getAllByShopAsync();

                    }
                    this.products = result.data;
                } finally {
                    this.isLoading = false;
                }
            }
        },
        beforeMount() {
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        },
        mounted() {
            this.loadAsync();
        }
    };
</script>
