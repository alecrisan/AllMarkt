<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Product Categories</h2>
            <button class="btn btn-primary" v-on:click="showAdd()">Add</button>

        </div>
        <table class="table">
            <thead>
                <tr>
                    <th class="w-25">Name</th>
                    <th class="w-50">Description</th>
                    <th class="w-50">Shop Name</th>
                    <th class="w-50">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="productCategory in productCategories" v-bind:key="productCategory.id">
                    <td>{{productCategory.name}}</td>
                    <td>{{productCategory.description}}</td>
                    <td>{{productCategory.shopName}}</td>
                    <td>
                        <button class="btn btn-primary" v-on:click="() => showEdit(productCategory)"> Edit </button>
                        <button class="btn btn-danger" v-on:click="() => showDelete(productCategory)">Delete</button>
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
                productCategories: [],
            };
        },

        methods: {
            showDelete(productCategory) {
                this.$refs.deleteModal.show(productCategory)
            },
            showAdd() {
                this.$refs.formModal.show1();
            },
            showEdit(productCategory) {
                this.$refs.formModal.show2(productCategory);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.productCategories.getAllAsync();
                    this.productCategories = result.data;
                } finally {
                    this.isLoading = false;
                }
            }
        },

        mounted() {
            this.loadAsync();
              var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    };
</script>